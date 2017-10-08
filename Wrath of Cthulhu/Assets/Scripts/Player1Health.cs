using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Health : MonoBehaviour {

	public float LifePercentage = 100f;
	
	// Update is called once per frame
	void Update () {
		GetComponent<Image> ().fillAmount = LifePercentage/100f;
	}
}
