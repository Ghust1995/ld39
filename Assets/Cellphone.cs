using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellphone : MonoBehaviour
{

    [Range(0, 1)]
    public float battery = 1.0f;
    public bool isCharging {
        get
        {
            return Vector2.Distance(FindObjectOfType<Player>().position, FindObjectOfType<House>().position) < chargeDistance;
        }
    }
    public bool wasCharging = false;
    public float chargeDistance;

    public float batteryConsumptionTuit = 1;
    public float batteryConsumptionOS = 1;
    public float batteryConsumptionMaps = 1;
    public float batteryConsumptionPokemon = 1;
    public float chargeSpeed = 5;
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
                {App.Off, 0 },
            };
        }
    }

    [Range(0, 1)]
    public float signal = 1.0f;

    public DateTime datetime;

    public App selectedApp = App.OS;

    public void GoToTwitter()
    {
            musicPlayer.Pause();
        selectedApp = App.Twitter;
    }
    public void GoToOS()
    {
            musicPlayer.Pause();
        if (selectedApp != App.Off || battery > 0.1f)
        {
            selectedApp = App.OS;
        }
    }
    public void GoToPokemonGO()
    {
        selectedApp = App.PokemonGO;
        musicPlayer.Play();
    }
    public void GoToMaps()
    {
            musicPlayer.Pause();
        selectedApp = App.Maps;
    }

    // Use this for initialization
    void Start()
    {
    }
    public AudioSource musicPlayer;

    // Update is called once per frame
    void Update()
    {
        datetime = DateTime.Now;
        if (isCharging)
        {
            if (!wasCharging)
            {
                FindObjectOfType<SfxManager>().PlayCharging();
            }
            battery += chargeSpeed * Time.deltaTime / 100.0f;
        }
        else
        {
            battery -= batteryConsumption[selectedApp] * Time.deltaTime / 100.0f;
        }
        if (battery >= 1)
        {
            battery = 1;
        }
        if (battery <= 0)
        {
            battery = 0;
            musicPlayer.Pause();
            selectedApp = App.Off;
        }

        wasCharging = isCharging;
    }
}
