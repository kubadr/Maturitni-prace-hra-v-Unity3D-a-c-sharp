using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Sprite[] HeartSprites;

	public Image HeartUI;

	private PlayerController player;

	void Start () {
		GameObject playerObj = GameObject.Find ("PlayerChar");
		player = playerObj.GetComponent<PlayerController> ();

			//GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerController>();
	}

	void Update () {

		HeartUI.sprite = HeartSprites [PlayerController.currHealth];
	}

}
