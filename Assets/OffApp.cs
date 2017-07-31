using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffApp : MonoBehaviour {
    public Cellphone cellphone;
    public BatteryFill batteryfill;
    public Text batteryText;
    public RectTransform offCanvas;

    public IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            offCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            offCanvas.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update () {
		batteryText.text = string.Format("{0:0}%", cellphone.battery * 100);
        batteryfill.fill = cellphone.battery;
    }
}
