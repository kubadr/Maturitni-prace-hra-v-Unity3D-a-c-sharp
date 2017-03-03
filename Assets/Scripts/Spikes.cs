using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	private PlayerController player;

	void Start () {

		player = GameObject.Find ("PlayerChar").GetComponent<PlayerController> ();

	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.CompareTag ("Player")) {
			PlayerController.currHealth -= 8;

			if (!player.doubleJump)
				player.doubleJump = true;
			if (player.grounded)
				player.grounded = false;
			StartCoroutine(player.Knockback(0.005f, 200, player.transform.position));
		}

	}

}
