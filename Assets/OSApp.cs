using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class OSApp : MonoBehaviour {

    public Text batteryText;
    public Text date;
    public Text time;
    public Text smalltime;
    public Image batteryFill;
    public Image batteryBG;
    public Sprite batteryRegularSprite;
    public Sprite batteryChargingSprite;
    public Image signalFill;
    public Cellphone cellphone;

	// Update is called once per frame
	void Update () {
        batteryText.text = string.Format("{0:0}%", cellphone.battery * 100);
        batteryFill.fillAmount = cellphone.battery;
        signalFill.fillAmount = cellphone.signal;
        time.text = cellphone.datetime.ToString("HH:mm");
        smalltime.text = cellphone.datetime.ToString("HH:mm");
        date.text = cellphone.datetime.ToString("ddd, MMM dd");
        batteryBG.sprite = cellphone.isCharging ? batteryChargingSprite : batteryRegularSprite;
        batteryFill.sprite = cellphone.isCharging ? batteryChargingSprite : batteryRegularSprite;
    }
}
