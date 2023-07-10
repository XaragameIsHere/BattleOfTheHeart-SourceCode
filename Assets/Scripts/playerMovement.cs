using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
	[SerializeField] Vector2 velocity = new Vector2(1, 0);
	[SerializeField] LayerMask groundLayers;
	[SerializeField] LayerMask wallLayers;
	public Camera playerCamera;
	[SerializeField] float jumpStrength = 20;
	[SerializeField] float friction = 2;
	[SerializeField] float walkSpeed = 5;

	[SerializeField] Sprite badheart;
	[SerializeField] List<Image> hearts = new List<Image>();
	[SerializeField] List<Image> badHearts = new List<Image>();

	public int playerLives = 4;
	public bool invincibility = false;
	public Vector2 velocityUp;
	public Vector2 playerSpeed;

	private CharacterController playerMover;
	//private Animator animationController;
	private bool isGrounded;
	private bool wallRun;
	private ParticleSystem rocketBoots;
	public bool inFight = false;
	public bool inDialogue = false;
	public bool alive = true;
	public Rigidbody2D rBody;
	Collider2D collisionBaybe;
	Collider2D[] hits;
	ParticleSystem.MainModule psMain;
	ParticleSystem.ShapeModule shape;
	Animator playerAnimator;
	SpriteRenderer playerSprite;

	// Start is called before the first frame update
	void Start()
	{
		playerSprite = GetComponent<SpriteRenderer>();
		playerAnimator = GetComponent<Animator>();
		rocketBoots = GetComponent<ParticleSystem>();
		psMain = rocketBoots.main;
		shape = rocketBoots.shape;
		rBody = GetComponent<Rigidbody2D>();
		collisionBaybe = GetComponent<PolygonCollider2D>();
	}

	IEnumerator waitforit()
	{
		yield return new WaitForSeconds(3);
		invincibility = false;
	}

	private void hurt()
	{
		if (alive && !invincibility)
		{
			print("herwo");
			StartCoroutine(waitforit());
			invincibility = true;
			badHearts.Add(hearts[0]);

			hearts.RemoveAt(0);
		}
		
	}

	private void OnParticleCollision(GameObject particleSystem)
	{
		if (particleSystem.layer == 8)
		{
			hurt();
		}
		
		
	}

    private void OnCollisionStay2D(Collision2D collision)
	{
		print(collision.gameObject.layer);
		if (collision.gameObject.layer == 8)
			hurt();
	}
	// Update is called once per frame

	void FixedUpdate()
	{
		isGrounded = rBody.IsTouchingLayers(groundLayers); //checks if the player is on a platform
		//calculates vertical velocity

		if (alive)
		{
			foreach (Image heart in badHearts)
			{
				heart.sprite = badheart;
			}
		}
		
		if (hearts.Count <= 0) 
		{
			alive = false;
		}

		//character controls
		if (inFight)
		{
			rBody.gravityScale = 0;
		}
		else
		{
			rBody.gravityScale = 50f;
		}

		if (Input.GetAxis("walk") > .1)
			playerSprite.flipX = false;
		else if (Input.GetAxis("walk") < .1)
			playerSprite.flipX = true;


		if (Input.GetButton("Jump") && alive && !inDialogue && inFight)
			rBody.velocity = new Vector2(rBody.velocity.x, walkSpeed * Input.GetAxis("Jump"));
		else if (!Input.GetButton("Jump") && Mathf.Abs(rBody.velocity.y) > 1 && inFight)

			rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Lerp(rBody.velocity.y, 0, friction * Time.deltaTime));
		else
			rBody.velocity = new Vector2(rBody.velocity.x, 0);


		playerAnimator.SetFloat("playerDirection", Input.GetAxis("walk"));

		if (Input.GetButton("walk") && alive && !inDialogue)
			rBody.velocity = new Vector2(walkSpeed * Input.GetAxis("walk"), rBody.velocity.y);
		else if (!Input.GetButton("walk") && Mathf.Abs(rBody.velocity.x) > 1)
		
			rBody.velocity = new Vector2(Mathf.Lerp(rBody.velocity.x, 0, friction * Time.deltaTime), rBody.velocity.y);
		else
			rBody.velocity = new Vector2(0, rBody.velocity.y);

		//for when you enter a cutscene
		shape.rotation = new Vector3(90+(45 * Input.GetAxis("walk")), 90, 0);

		if (isGrounded)
		{
			rocketBoots.Stop();
		}
		else
		{
			rocketBoots.Play();
		}



		if (collisionBaybe.IsTouchingLayers(wallLayers))
		{
			//walk = false;
			rBody.AddForce(new Vector2(0, 750));
		}
		/*
		else
		{
			walk = true;
		}*/
	}


		//actually executes moving the player forward

	
	
}
