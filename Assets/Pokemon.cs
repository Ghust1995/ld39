using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class Pokemon : MonoBehaviour, IPointerClickHandler
{
    public PokemonInfo pokemonInfo;
    public Image sprite;
    public RectTransform rectTransform
    {
        get
        {
            return GetComponent<RectTransform>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCallback();
    }

    Action clickCallback;

    public void Setup(PokemonInfo info, Action clickCallback)
    {
        pokemonInfo = info;
        sprite.sprite = info.data.sprite;
        this.clickCallback = clickCallback;
    }
}
