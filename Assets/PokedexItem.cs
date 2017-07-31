using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class PokedexItem : MonoBehaviour {
    public Text count;
    public Text pokename;
    public Image image;
    public Button Sacrifice;
    public Text SacrificeText;

    public PokemonData pokedata;

    public void Setup(PokemonData data)
    {
        pokedata = data;
        image.sprite = pokedata.sprite;
        if (data.Evolution == null)
        {
            Sacrifice.gameObject.SetActive(false);
        }
        else
        {
            SacrificeText.text = "Evolve!";
            //Sacrifice.onClick;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (pokedata == null) return;
        var capturedPokemon = FindObjectOfType<PokemongoApp>().capturedPokemon;
        var intcount = capturedPokemon.Count((p) => p.data.name == pokedata.name);
        count.text = "Count " + intcount;
        pokename.text = intcount > 0 ? pokedata.name : "???";
        image.color = intcount > 0 ? Color.white : Color.black;

	}
}
