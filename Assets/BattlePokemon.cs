using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePokemon : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0.2f * Mathf.Sin(Time.time * 4.0f), transform.position.y);
	}
}
