using UnityEngine;

public class PokemonInfo
{
    public Map.StreetCrossing mapInfo;
    public Vector2 position;
    public PokemonData data;
}

[CreateAssetMenu(fileName = "new pokemon", menuName = "Pokemon/New", order = 1)]
public class PokemonData : ScriptableObject
{
    public string pokemonName;
    public Sprite sprite;
    public Color color;
    public int rarity;
}

