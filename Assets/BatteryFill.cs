using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryFill : MonoBehaviour {

    public Gradient colorGradient;
    public Image image;
    [Range(0, 1)]
    public float fill;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        image.color = colorGradient.Evaluate(fill);
        image.fillAmount = fill;
	}
}
