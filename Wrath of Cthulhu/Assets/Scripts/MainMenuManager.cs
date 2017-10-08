using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		SceneManager.LoadScene("PlayerMovement", LoadSceneMode.Additive);
    }
}
