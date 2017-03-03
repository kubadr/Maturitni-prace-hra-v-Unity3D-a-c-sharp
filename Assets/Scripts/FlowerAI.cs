using UnityEngine;
using System.Collections;

public class FlowerAI : MonoBehaviour {

	Animator anim;
	private PlayerController player;
	private GameObject fHead;
	private GameObject targetPoint;
	public GameObject bullet;
	public GameObject shootPoint;
	private GameObject bulletClone;
	private GameMaster gm;
	public GameObject playerContainer;


	//Integer
	public int currHealth;
	public int maxHealth;

	//Float
	private float distance;
	float angle;
	public float wakeRange;
	public float shootRange;
	public float rotationSpeed;
	public float shootInterval;
	public float bulletSpeed = 100;
	public float bulletTimer;

	//boolean
	public bool awake = false;
	public bool shooting = false;

	void Start () {
		currHealth = maxHealth;
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("PlayerChar").GetComponent<PlayerController> ();
		fHead = this.gameObject.transform.GetChild (0).gameObject;
		targetPoint = GameObject.Find ("TargetPoint");
		gm = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster>();

	}

	void Update () {

		Vector3 targetDistance = player.transform.position - transform.position;
		float xDistance = targetDistance.x;
		float yDistance = targetDistance.y;
		distance = Mathf.Sqrt (xDistance*xDistance + yDistance*yDistance);

		if (distance < wakeRange) {
			anim.SetBool ("Awake", true);
			awake = true;

		} else {
			anim.SetBool ("Awake", false);
			awake = false;
		}

		if (distance < shootRange) {
			shooting = true;
		} else {
			shooting = false;
		}

		if (shooting) {
			Vector3 lookAtVector = targetPoint.transform.position - fHead.transform.position;
			angle = Mathf.Atan2 (lookAtVector.y, lookAtVector.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis (angle - 90, Vector3.forward);
			fHead.transform.rotation = Quaternion.Slerp (fHead.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			Attack();

		} else {
			fHead.transform.rotation = Quaternion.Slerp (fHead.transform.rotation, Quaternion.AngleAxis (0, Vector3.forward), Time.deltaTime * rotationSpeed * 2);
		
		}

		if (currHealth <= 0) {
			Die ();
		}

		//Debug.Log (distance);

	}

	public void Attack(){

		bulletTimer += Time.deltaTime;

		if (bulletTimer >= shootInterval) {

			Vector2 direction = targetPoint.transform.position - fHead.transform.position;
			direction.Normalize ();

			bulletClone = Instantiate (bullet, shootPoint.transform.position, fHead.transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

			bulletTimer = 0;
		}
	}

	public void Damage (int dmg){

		currHealth -= dmg;

	}

	void Die () {
		gm.StartCoroutine("addPoints",20);
		Destroy (gameObject);
	}
}
