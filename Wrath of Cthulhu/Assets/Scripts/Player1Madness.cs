using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Madness : MonoBehaviour {

	public float MadnessPercentage = 0f;

	// Update is called once per frame
	void Update () {
		GetComponent<Image> ().fillAmount = MadnessPercentage/100f;
	}
}
