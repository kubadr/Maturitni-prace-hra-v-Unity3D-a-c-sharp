using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

	public int dmg = 20;

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Enemy") {

			col.SendMessageUpwards ("Damage", dmg);
		}
		if (col.gameObject.tag == "Player")
        {

            col.SendMessageUpwards("Damage", dmg);
			Debug.Log ("damage");
        }
    }
}
