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

        bool sp = false;
        public void SpawnRandomPokemon()
        {
            if (sp) return;
            sp = true;
            if (PokemonBag.Count == 0) return;
            var position = new Vector2(
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2),
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2));
            position = Vector2.zero;
            float selectedValue = Random.Range(0, maxDistributedRarity);
            //Debug.Log("Selected: " + selectedValue);
            PokemonData selectedPokemon = PokemonBag.Last((kvp) => kvp.Key < selectedValue).Value;
            //Debug.Log("selected pokemon " + selectedPokemon.name);
            SpawnedPokemon.Add(new PokemonInfo()
            {
                position = position,
                mapInfo = GameMap.GetNearestCrossing(position),
                data = selectedPokemon,
            });
        }
    }

    public PokemonContainer pokemonContainer;
    public List<PokemonData> AvailablePokemon;

    void Start()
    {
        pokemonContainer = new PokemonContainer(FindObjectOfType<MapsApp>().map, AvailablePokemon);
        StartCoroutine(SpawnPokemonCoroutine());
    }

    public float spawnDelay;
    IEnumerator SpawnPokemonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            pokemonContainer.SpawnRandomPokemon();
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
    private List<PokemonInfo> capturedPokemon = new List<PokemonInfo>();
    public Text CapturedText;

    public IEnumerator PokeballCoroutine()
    {

        runButton.gameObject.SetActive(false);
        yield return pokeball.PokeballAnimationCatch(battlePokemonImage.rectTransform.position.y);
        battlePokemonImage.enabled = false;
        yield return pokeball.PokeballAnimationWobble();
        if (Random.value > 0.5f)
        {
            capturedPokemon.Add(battlePokemon);
            CapturedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
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
    }
}