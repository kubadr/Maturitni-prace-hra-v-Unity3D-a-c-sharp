using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public GameObject DeathUI;
	public GameObject PauseUI;
	public bool paused = false;

	// Use this for initialization
	void Start () {
		
		PauseUI.SetActive (false);
		DeathUI.SetActive (false);
	}

	void Update () {
	
		PauseGame ();

		if(PlayerController.currHealth < 1){
		Die ();
		}

	}

	public void PauseGame(){

		if (Input.GetButtonDown("Pause")) {
			paused = !paused;
		}

		if (paused) {
			PauseUI.SetActive(true);
			Time.timeScale = 0;
		}else{
			PauseUI.SetActive(false);
			Time.timeScale = 1;
		}


	}

	public void ResumeOnClick(){

		if (paused) {
			paused = false;
		}

	}

	public void Die(){

		Time.timeScale = 0;
		DeathUI.SetActive (true);

	}
}
