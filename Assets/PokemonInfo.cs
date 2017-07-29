using UnityEngine;

public class PokemonInfo
{
    public MapInfo position;
    public float GetDistance()
    {
        return 10.0f;
    }
    public PokemonData data;
}

public class PokemonData
{
    public string name;
    public int rarity;
}

public class MapInfo
{
    public string streetName1;
    public string streetName2;
    public string nearestPOI;
}

public class MapApp
{

    public static MapInfo GetMapInfo(Vector2 position)
    {
        return new MapInfo()
        {
            streetName1 = "st1 name",
            streetName2 = "st2 name",
            nearestPOI = "poi"
        };
    }
}
