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
            var position = new Vector2(
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2),
                Random.Range(-GameMap.CitySize / 2, +GameMap.CitySize / 2));
            float selectedValue = Random.Range(0, maxDistributedRarity);
            PokemonData selectedPokemon = PokemonBag.First((kvp) => kvp.Key > selectedValue).Value;
            Debug.Log("selected pokemon " + selectedPokemon.pokemonName);
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
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var poke in pokemonContainer.SpawnedPokemon)
        {
            Gizmos.color = poke.data.color;
            Gizmos.DrawSphere(new Vector3(poke.position.x, poke.position.y, 0.0f), 0.1f);
            Gizmos.DrawWireSphere
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Remove
        pokemonContainer.GameMap = FindObjectOfType<MapsApp>().map;
    }
}