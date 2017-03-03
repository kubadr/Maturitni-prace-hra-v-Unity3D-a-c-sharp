	using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {
	//Float
	public float maxSpeed  = 5;
	public float jumpForce;
	public float checkRadius = 0.2f;
	public float attackCd;
	float attackTimer;

	//Int
	public int maxHealth = 20;
	public static int currHealth;

	//Bool
	bool facingRight = true;
	public bool grounded = false;
	public bool doubleJump = true;
	bool crouch = false;
	bool above = false;
	public bool attacking = false;

	//Vector2
	private Vector2 origBoxSize = new Vector2 (1.14f, 3.53f);
	private Vector2 boxCrouchSize = new Vector2 (1.14f, 1.765f);

	//Transform
	public Transform groundCheck;
	public Transform aboveCheck;

	//Others
	public GameObject DeathUI;
	public BoxCollider2D attackTrigger;
	private BoxCollider2D bc2d;
	public Rigidbody2D rb2d;
	public LayerMask whatIsGround;
	private GameMaster gm;
    private GameObject orc;
	Animator anim;



	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		bc2d = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		currHealth = maxHealth;
		DeathUI.SetActive(false);
		attackTrigger.enabled = false;
		gm = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster>();
        orc = GameObject.Find("Orc");
	
	}

	// FixeUpdate is used for physics actions
	void FixedUpdate ()
	{
		//--------Kontrola, zda-li je jednotka na zemi a zda-li se hlavou dotyka nejakeho objektu (nemuze tudiz vyskocit)--------
		above = Physics2D.OverlapBox (aboveCheck.position, new Vector2 (1.14f, 1.765f), 0f, whatIsGround, 0f, 0.01f);
		grounded = Physics2D.OverlapCircle (groundCheck.position, checkRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);

		if (grounded)
			doubleJump = false;

		anim.SetFloat ("vSpeed", rb2d.velocity.y);

		float move = Input.GetAxis ("Horizontal");

		// --------Pohyb--------
		if(crouch)
			rb2d.velocity = new Vector2 (move * maxSpeed/2, rb2d.velocity.y);
		else
			rb2d.velocity = new Vector2 (move * maxSpeed, rb2d.velocity.y);

		anim.SetFloat ("speed", Mathf.Abs(move));

		// --------Otaceni postavy--------
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 charPos = rb2d.position;

		if ((charPos.x - mousePos.x > 0) && facingRight)
			Flip ();
		if ((charPos.x - mousePos.x < 0) && !facingRight)
			Flip ();

		// --------Utok--------
		if (Input.GetKeyDown (KeyCode.Mouse0) && grounded && !attacking) {
			anim.SetTrigger ("Attack");
			attackTimer = attackCd;
			attacking = true;
		}
		if (!grounded) 
			anim.ResetTrigger ("Attack");

		if (attacking) {
			if (attackTimer > 0.2f) {
				attackTimer -= Time.deltaTime;

				if (attackTimer <0.4f)
					attackTrigger.enabled = true;
				
			} else {
				attacking = false;
				attackTrigger.enabled = false;
			}
		}

		//Debug.Log (attackTimer);

		// --------Plizeni--------
		if(grounded && Input.GetKey(KeyCode.LeftShift))
			Crouch ();

		if(crouch && !above && !Input.GetKey(KeyCode.LeftShift))
			StandUp ();
		
	}

	void Update ()
	{
		// --------Skok--------
		if((grounded || !doubleJump)  && Input.GetKeyDown(KeyCode.Space)){
			if (crouch && above) {
			} else {
				Jump ();
			}
		}

		//--------Smrt--------
		if (currHealth < 1){
			Die();
		}
	}



	void Flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Jump (){
		anim.SetBool ("Ground", false);

		if (!grounded && !doubleJump)
			doubleJump = true;
		if (!grounded && doubleJump) {
			rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
			rb2d.AddForce (new Vector2 (0, jumpForce));
		} else {
			rb2d.AddForce (new Vector2 (0, jumpForce));
		}
	}

	void Crouch (){
		anim.SetBool ("Crouch", true);
		bc2d.size = boxCrouchSize;
		bc2d.offset = new Vector2(0f, -1.7025f);
		crouch = true;
		attackTrigger.size = new Vector2 (1.706f, 1.756f);
		attackTrigger.offset = new Vector2 (0.39f, -1.7025f);
		attackCd = 0.6f;
	}

	void StandUp (){
		anim.SetBool("Crouch", false);
		bc2d.offset = new Vector2(0, -0.82f);
		bc2d.size = origBoxSize;
		crouch = false;
		attackTrigger.size = new Vector2 (1.706f, 3.53f);
		attackTrigger.offset = new Vector2 (0.39f, -0.82f);
		attackCd = 0.9f;
	}

	public void Damage (int dmg){
		currHealth -= dmg;
	}

	void Die(){
		
		Time.timeScale = 0;
		DeathUI.SetActive(true);

	}

	public IEnumerator Knockback(float knockDuration, float knockbackPower, Vector3 knockbackDir){

		float timer = 0;

		while (knockDuration > timer) {
			timer += Time.deltaTime;

			rb2d.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y* knockbackPower,transform.position.z));
				

		}

		yield return 0;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Coin")
        {
            Destroy(col.gameObject);
            gm.StartCoroutine("addPoints", 5);
        }
		if (col.gameObject.tag == "attackRange")
		{
			//orc.GetComponent<OrcAI>().Attack();
			col.transform.parent.GetComponent<OrcAI> ().Attack ();
		}
    }
}
