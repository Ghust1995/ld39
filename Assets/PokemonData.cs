using UnityEngine;

[CreateAssetMenu(fileName = "new pokemon", menuName = "Pokemon/New", order = 1)]
public class PokemonData : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public int rarity;
    public PokemonData Evolution;
    public int SacrificesRequired;
}


