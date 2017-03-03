using UnityEngine;
using System.Collections;

public class OrcAI : MonoBehaviour {

	//Integer
	public int currHealth;
	public int maxHealth;

	//Floats
	public float wakeRange;
	public float attackInterval;
	private float distance;
    public float attackTimer;
    public float moveSpeed;
    public float followRange;
    public float attackCD = 3;  

	//Bools
	bool facingRight = false;
	public bool awake = false;
	public bool attacking = false;
	//bool grounded = false;
    bool following = false;

	//others
	Animator anim;
	private PlayerController player;
	public Rigidbody2D rb2d;
	public BoxCollider2D attackRange;
	public BoxCollider2D attackTrigger;
	private GameMaster gm;

	// Use this for initialization
	void Start () {

		currHealth = maxHealth;
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		attackTrigger.enabled = false;
		attackRange.enabled = true;
		player = GameObject.Find ("PlayerChar").GetComponent<PlayerController> ();
		gm = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster>();

	}
	
	// Update is called once per frame
	void Update () {

		distance = Vector2.Distance (player.transform.position, transform.position);
		if (distance < wakeRange) {
			awake = true;
		} else {
			awake = false;
		}

		if (distance < followRange && !attacking){
			following = true;
		} else  {
            following = false;
        }

		if (awake) {

			if (following && !facingRight)
				rb2d.velocity = new Vector2 (-moveSpeed, rb2d.velocity.y);
			if (following && facingRight)
				rb2d.velocity = new Vector2 (moveSpeed, rb2d.velocity.y);
			anim.SetFloat ("Speed", Mathf.Abs(rb2d.velocity.x));
			if (attacking) {
				if (attackTimer > 1f) {
					attackTimer -= Time.deltaTime;

					if (attackTimer < 2f && attackTimer > 1.8f) {
						attackTrigger.enabled = true;
					} else {
						attackTrigger.enabled = false;
					}
				} else {
					attacking = false;
					anim.SetBool ("Attacking", true);
				}
			} else if (!attacking && attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else if (!attacking && attackTimer <= 0) {
				attackRange.enabled = true;
			}
		

			if (rb2d.position.x - player.rb2d.position.x > 0 && facingRight)
				Flip ();
			if (rb2d.position.x - player.rb2d.position.x < 0 && !facingRight)
				Flip ();

		}

		if (currHealth <= 0)
			Die ();

	}
    public void Attack() {
		if (awake && !attacking && attackTimer <= 0){
            anim.SetTrigger("Attack");
			attackRange.enabled = false;
			attacking = true;
			anim.SetBool ("Attacking", true);
			attackTimer = attackCD;
            //Debug.LogError("enter");    
        }		
	}

	void Flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Damage (int dmg){
		currHealth -= dmg;
	}

	void Die () {
		gm.StartCoroutine("addPoints",40);
		Destroy (gameObject);
	}
}
