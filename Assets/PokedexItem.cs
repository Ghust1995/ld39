using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class PokedexItem : MonoBehaviour {
    public Text count;
    public Text pokename;
    public Image image;
    public Button Sacrifice;
    public Text SacrificeText;

    public PokemonData pokedata;

    public void Setup(PokemonData data, Func<bool> evolvePokemon)
    {
        pokedata = data;
        image.sprite = pokedata.sprite;
        if (data.Evolution == null)
        {
            Sacrifice.gameObject.SetActive(false);
        }
        else
        {
            SacrificeText.text = "evolve!";
            Sacrifice.onClick.AddListener(() =>
            {
                if (!evolvePokemon())
                {
                    StartCoroutine(ShowNecessarySacrifice());
                }
            });
        }
    }

    IEnumerator ShowNecessarySacrifice()
    {
            SacrificeText.text = pokedata.SacrificesRequired + " required";
        yield return new WaitForSeconds(3.0f);
            SacrificeText.text = "evolve!";
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
