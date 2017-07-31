using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PokemongoApp : MonoBehaviour
{

    public class PokemonContainer
    {
        public PokemonContainer(Map map, List<PokemonData> availablePokemon)
        {
            GameMap = map;
            AvailablePokemon = availablePokemon;
            maxDistributedRarity = 0.0f;
            AvailablePokemon.ForEach((p) =>
            {
                if (p.rarity == 0) return;
                PokemonBag.Add(maxDistributedRarity, p);
                maxDistributedRarity = maxDistributedRarity + RarityDistribution(p.rarity);
                //Debug.Log("Added new pokemon to bag " + maxDistributedRarity);
            });
        }
        public Map GameMap;
        private float maxDistributedRarity;
        public List<PokemonData> AvailablePokemon;
        public SortedList<float, PokemonData> PokemonBag = new SortedList<float, PokemonData>();
        public List<PokemonInfo> SpawnedPokemon = new List<PokemonInfo>();
        // Use this for initialization

        public float RarityDistribution(float value)
        {
            return 1 / (value * value);
        }
        
        //public float i = 0;
        public PokemonInfo SpawnRandomPokemon()
        {
            if (PokemonBag.Count == 0) return null;
            var position = new Vector2(
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2),
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2));
            //position = new Vector2(0.05f * (i % 20), 0.05f * Mathf.Floor(i/20)) + new Vector2(-0.3f, -0.3f);
            //i++;
            float selectedValue = Random.Range(0, maxDistributedRarity);
            //Debug.Log("Selected: " + selectedValue);
            PokemonData selectedPokemon = PokemonBag.Last((kvp) => kvp.Key < selectedValue).Value;
            //Debug.Log("selected pokemon " + selectedPokemon.name);
            var newPoke = new PokemonInfo()
            {
                position = position,
                mapInfo = GameMap.GetNearestCrossing(position),
                data = selectedPokemon,
            };
            SpawnedPokemon.Add(newPoke);
            return newPoke;
        }
    }

    public PokemonContainer pokemonContainer;
    public List<PokemonData> AvailablePokemon;

    void Start()
    {
        pokemonContainer = new PokemonContainer(FindObjectOfType<MapsApp>().map, AvailablePokemon);
        StartCoroutine(SpawnPokemonCoroutine());
        PopulatePokedex();
        for (int i = 0; i < InitialPokemon; i++)
        {
            var newPoke = pokemonContainer.SpawnRandomPokemon();
            StartCoroutine(DespawnPokemon(newPoke));

        }
    }

    public float DespawnTime;
    public IEnumerator DespawnPokemon(PokemonInfo poke)
    {
        yield return new WaitForSeconds(DespawnTime);
        if (pokemonContainer.SpawnedPokemon.Contains(poke))
        {
            pokemonContainer.SpawnedPokemon.Remove(poke);
        }
    }

    public float spawnDelay;
    public int MaxPokemon = 10;
    IEnumerator SpawnPokemonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            if (pokemonContainer.SpawnedPokemon.Count > MaxPokemon) continue;
            var newPoke = pokemonContainer.SpawnRandomPokemon();
            StartCoroutine(DespawnPokemon(newPoke));
            //Debug.Log("spawned new pokemon");
        }
    }

    private void OnDrawGizmos()
    {
        if (pokemonContainer == null) return;
        foreach (var poke in pokemonContainer.SpawnedPokemon)
        {
            Gizmos.color = poke.data.color;
            //Gizmos.color = Color.red;
            //Debug.Log("showing pokemon " + poke.data.name + "  " + poke.position.x + "  " + poke.position.y + " " + poke.data.color);
            var position1 = new Vector3(poke.position.x, poke.position.y, 0.0f);
            Gizmos.DrawSphere(position1, 0.1f);
            //var position2 = new Vector3(poke.position.x, poke.position.y, 0.0f);
            //Gizmos.DrawWireSphere(position2, 0.1f);
            //Gizmos.DrawLine(position1, position2);
        }
    }

    public Pokemon pokemonImagePrefab;
    public RectTransform pokemonImageContainer;
    public float VisibleRadius;
    public float NearbyRadius;
    public Player player;
    public float visibilityScale;

    public void UpdatePokemonDisplay()
    {
        var children = new List<GameObject>();
        foreach (Transform child in pokemonImageContainer)
        {
            var poke = child.GetComponent<Pokemon>();
            if (Vector2.Distance(poke.pokemonInfo.position, player.position) > VisibleRadius)
            {
                poke.pokemonInfo.isVisible = false;
                children.Add(child.gameObject);
            }
            else
            {
                poke.rectTransform.position = visibilityScale * (poke.pokemonInfo.position - player.position);
            }
            if (!pokemonContainer.SpawnedPokemon.Contains(poke.pokemonInfo))
            {
                children.Add(child.gameObject);
            }
            if (poke.pokemonInfo.isCaptured)
            {
                children.Add(child.gameObject);
                //Debug.Log("Removing pokemon count: " + pokemonContainer.SpawnedPokemon.Count);
                pokemonContainer.SpawnedPokemon.Remove(poke.pokemonInfo);
//                Debug.Log("Removed pokemon count: " + pokemonContainer.SpawnedPokemon.Count);
            }
        }
        children.ForEach(child => Destroy(child));

        foreach (var poke in pokemonContainer.SpawnedPokemon)
        {
            var pokevector = poke.position - player.position;
            if (poke.isVisible || poke.isCaptured) continue;
            if (pokevector.magnitude < VisibleRadius)
            {
                // do visible stuff
                var pokemonImage = Instantiate(pokemonImagePrefab, pokemonImageContainer);
                pokemonImage.Setup(poke, () => StartBattle(poke));
                pokemonImage.rectTransform.position = visibilityScale * pokevector;
                poke.isVisible = true;
            }
            else if (pokevector.magnitude < NearbyRadius)
            {
                //do nearby stuff
            }
        }
    }

    public enum PokemonGOState
    {
        Battle,
        Overworld,
        Pokedex,
    }

    public void ExitBattle()
    {
        state = PokemonGOState.Overworld;
    }

    public void ExitPokedex()
    {
        state = PokemonGOState.Overworld;
    }

    public void OpenPokedex()
    {
        state = PokemonGOState.Pokedex;
    }


    [System.Serializable]
    public struct PokeGoView
    {
        public PokemonGOState state;
        public RectTransform view;
    }
    public List<PokeGoView> views;
    public PokemonGOState state = PokemonGOState.Overworld;

    public Image battlePokemonImage;
    public PokemonInfo battlePokemon;
    public Text battlePokemonText;
    public Button runButton;
    public void StartBattle(PokemonInfo poke)
    {
        state = PokemonGOState.Battle;
        battlePokemon = poke;
        battlePokemonImage.sprite = poke.data.sprite;
        battlePokemonText.text = poke.data.name;
    }

    public Pokeball pokeball;
    public List<PokemonInfo> capturedPokemon = new List<PokemonInfo>();
    public Text CapturedText;
    public int pokemonCount = 0;

    public IEnumerator PokeballCoroutine()
    {

        runButton.gameObject.SetActive(false);
        yield return pokeball.PokeballAnimationCatch(battlePokemonImage.rectTransform.position.y);
        battlePokemonImage.enabled = false;
        yield return pokeball.PokeballAnimationWobble();
        if (Random.value < 1/Mathf.Sqrt(battlePokemon.data.rarity))
        {
            FindObjectOfType<SfxManager>().PlayCapture();
            capturedPokemon.Add(battlePokemon);
            CapturedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4.5f);
            CapturedText.gameObject.SetActive(false);
            state = PokemonGOState.Overworld;
            battlePokemon.isCaptured = true;
        }
        battlePokemonImage.enabled = true;
        pokeball.ResetPosition();
        runButton.gameObject.SetActive(true);
    }

    public void ThrowPokeball()
    {
        StartCoroutine(PokeballCoroutine());
    }

    public PokedexItem pokedexItemPrefab;
    public RectTransform pokedexContainer;
    public void PopulatePokedex()
    {
        AvailablePokemon.ForEach((p) =>
        {
            PokedexItem pi = Instantiate(pokedexItemPrefab, pokedexContainer);
            pi.Setup(p, () =>
            {
                if (capturedPokemon.Count((cp) => cp.data.name == p.name) >= p.SacrificesRequired)
                {
                    int i = 0;
                    capturedPokemon.RemoveAll((cp) =>
                    {
                        return cp.data.name == p.name && i++ < cp.data.SacrificesRequired;
                    });

                    capturedPokemon.Add(new PokemonInfo()
                    {
                        data = p.Evolution,
                        isCaptured = true,
                        isVisible = false,
                    });
                    FindObjectOfType<SfxManager>().PlayEvolution();
                    return true;
                }
                return false;
            });
        });
    }
    public int InitialPokemon;

    // Update is called once per frame
    void Update()
    {
        views.ForEach((v) =>
            {
                v.view.gameObject.SetActive(v.state == state);
            });
        // Remove
        pokemonContainer.GameMap = FindObjectOfType<MapsApp>().map;
        UpdatePokemonDisplay();
        pokemonCount = pokemonContainer.SpawnedPokemon.Count;
        
    }
}