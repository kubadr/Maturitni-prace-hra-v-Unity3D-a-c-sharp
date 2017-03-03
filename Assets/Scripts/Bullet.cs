using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private float destroyTimer = 0;
	public float destroyTime;

	void OnTriggerEnter2D(Collider2D col){

		if (col.isTrigger != true) {
			if (col.CompareTag ("Player")) {

				col.GetComponent<PlayerController> ().Damage (2);

			}

			Destroy (gameObject);

		}
			
	}

	void Update () {

		destroyTimer += Time.deltaTime;
		if (destroyTimer >= destroyTime)
			Destroy (gameObject);

	}
}
