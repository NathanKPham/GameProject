﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

	private Animator menuAnim;

	// Use this for initialization
	void Awake () {
		menuAnim = GetComponent<Animator> ();
	}
	public void MenuFade(){
		menuAnim.SetTrigger ("FadeOut");
	}
	public void LoadScene () {
		Application.LoadLevel ("PlayerMovement");
	}
}
