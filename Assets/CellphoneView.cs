using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CellphoneView : MonoBehaviour {

    [Serializable]
    public struct AppView
    {
        public App app;
        public Canvas canvas;
    }

    public Cellphone cellphone;
    public RawImage screen;
    public List<AppView> appViews;

    private Dictionary<App, AppView> apps;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        appViews.ForEach((v) =>
        {
            v.canvas.gameObject.SetActive(v.app == cellphone.selectedApp);
        });
	}
}
