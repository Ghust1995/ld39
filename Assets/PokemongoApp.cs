using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                Debug.Log("Added new pokemon to bag " + maxDistributedRarity);
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
            return 1/(value*value);
        }

        public void SpawnRandomPokemon()
        {
            if (PokemonBag.Count == 0) return;
            var position = new Vector2(
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2),
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2));
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

    IEnumerator SpawnPokemonCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            pokemonContainer.SpawnRandomPokemon();
            //Debug.Log("spawned new pokemon");
        }
    }

    private void OnDrawGizmos()
    {
        if(pokemonContainer == null) return;
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

    // Update is called once per frame
    void Update()
    {
        // Remove
        pokemonContainer.GameMap = FindObjectOfType<MapsApp>().map;
    }
}