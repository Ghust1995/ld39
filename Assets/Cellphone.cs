using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellphone : MonoBehaviour
{

    [Range(0, 1)]
    public float battery = 1.0f;

    public float batteryConsumptionTuit = 1;
    public float batteryConsumptionOS = 1;
    public float batteryConsumptionMaps = 1;
    public float batteryConsumptionPokemon = 1;

    public Dictionary<App, float> batteryConsumption
    {
        get
        {
            return new Dictionary<App, float>
            {
                {App.Maps, batteryConsumptionMaps },
                {App.OS, batteryConsumptionOS },
                {App.Twitter, batteryConsumptionTuit },
                {App.PokemonGO, batteryConsumptionPokemon },
            };
        }
    }
    public bool isCharging = false;

    [Range(0, 1)]
    public float signal = 1.0f;

    public DateTime datetime;

    public App selectedApp = App.OS;

    public void GoToTwitter()
    {
        selectedApp = App.Twitter;
    }
    public void GoToOS()
    {
        selectedApp = App.OS;
    }
    public void GoToPokemonGO()
    {
        selectedApp = App.PokemonGO;
    }
    public void GoToMaps()
    {
        selectedApp = App.Maps;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        datetime = DateTime.Now;
        battery -= batteryConsumption[selectedApp] * Time.deltaTime / 100.0f;
    }
}
