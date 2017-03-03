using UnityEngine;
using System.Collections;

public class FlowerHead : MonoBehaviour {

	private FlowerAI flower;
	private PlayerController player;
	public float rotationSpeed;
	float distance;
	public float fireRange;

	// Use this for initialization
	void Start () {
		
		player = GameObject.Find ("PlayerChar").GetComponent<PlayerController> ();
		flower = GameObject.Find ("Flower").GetComponent<FlowerAI> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetDistance = player.transform.position - transform.position;
		float xDistance = targetDistance.x;
		float yDistance = targetDistance.y;
		distance = Mathf.Sqrt (xDistance*xDistance + yDistance*yDistance);		



	}
}
