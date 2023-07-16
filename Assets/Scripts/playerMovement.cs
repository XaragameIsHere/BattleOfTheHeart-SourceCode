using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
	[SerializeField] Vector2 velocity = new Vector2(1, 0);
	[SerializeField] LayerMask groundLayers;
	[SerializeField] LayerMask wallLayers;
    [SerializeField] LayerMask enemyLayers;
    public Camera playerCamera;
	[SerializeField] float friction = 2;
	[SerializeField] float walkSpeed = 5;
	[SerializeField] enemyScripting enemyScript;
	[SerializeField] Sprite badheart;
	[SerializeField] List<Image> hearts = new List<Image>();
	[SerializeField] List<Image> badHearts = new List<Image>();
	[SerializeField] GameObject trigger;
	[SerializeField] Slider fuelMeter;

	public int playerLives = 4;
	public bool invincibility = false;
	public Vector2 velocityUp;
	public Vector2 playerSpeed;
	private playerUIController uIController;
	private CharacterController playerMover;
	//private Animator animationController;
	private bool isGrounded;
	private bool wallRun;
	private ParticleSystem rocketBoots;
	public bool inFight = false;
	public bool inDialogue = false;
	public bool alive = true;
	public bool parrying = false;
    public Rigidbody2D rBody;
	Collider2D collisionBaybe;
	Collider2D[] hits;
	ParticleSystem.MainModule psMain;
	ParticleSystem.ShapeModule shape;
	Animator playerAnimator;
	SpriteRenderer playerSprite;
	private bool objectAttained = false;
	private float startDistance;

	// Start is called before the first frame update
	void Start()
	{
		startDistance = Vector3.Distance(transform.position, trigger.transform.position);
		playerSprite = GetComponent<SpriteRenderer>();
		playerAnimator = GetComponent<Animator>();
		rocketBoots = GetComponent<ParticleSystem>();
		psMain = rocketBoots.main;
		shape = rocketBoots.shape;
		collisionBaybe = GetComponent<PolygonCollider2D>();
		uIController = GetComponent<playerUIController>();
	}

	IEnumerator waitforit()
	{
		yield return new WaitForSeconds(3);
		invincibility = false;
        collisionBaybe.excludeLayers = 0;
    }

    public void giveRocketBoots() { fuelMeter.gameObject.SetActive(true); }

    private void hurt()
	{
		if (alive && !invincibility)
		{
			invincibility = true;
			collisionBaybe.excludeLayers = enemyLayers;
			StartCoroutine(waitforit());
			badHearts.Add(hearts[0]);
			hearts.RemoveAt(0);
		}
		
	}

    

    private void OnParticleCollision(GameObject particleSystem)
	{
		if (!parrying)
		{
			if (particleSystem.layer == 8)
			{
				hurt();
			}
		}
		else
		{
            GameObject newObj = new GameObject("Name");
			Instantiate(newObj);
			newObj.transform.position = transform.position;

			SpriteRenderer throwSprite = newObj.AddComponent<SpriteRenderer>();
			throwSprite.sprite = enemyScript.attack2sprite;

			newObj.transform.localScale = new Vector3(.1f, .1f, .1f);
			newObj.transform.DOMove(enemyScript.transform.position, .5f);
		}
		
		
	}

	IEnumerator unParry()
	{
		yield return new WaitForSeconds(.5f);
		parrying = false;
	}

    private void OnCollisionStay2D(Collision2D collision)
	{
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

		playerAnimator.SetBool("Flying", !isGrounded);

		if (Input.GetButton("Jump") && alive && !inDialogue && inFight)
			rBody.velocity = new Vector2(rBody.velocity.x, walkSpeed * Input.GetAxis("Jump"));
		else if (!Input.GetButton("Jump") && Mathf.Abs(rBody.velocity.y) > 1 && inFight)

			rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Lerp(rBody.velocity.y, 0, friction * Time.deltaTime));
		else
			rBody.velocity = new Vector2(rBody.velocity.x, 0);

		playerAnimator.SetFloat("playerSpeed", Mathf.Abs(Input.GetAxis("walk")));
        //playerAnimator.SetBool("walking", Input.GetButton("walk"));

        if (Input.GetButton("walk") && alive && !inDialogue)
			rBody.velocity = new Vector2(walkSpeed * Input.GetAxis("walk"), rBody.velocity.y);
		else if (!Input.GetButton("walk") && Mathf.Abs(rBody.velocity.x) > 1)
		
			rBody.velocity = new Vector2(Mathf.Lerp(rBody.velocity.x, 0, friction * Time.deltaTime), rBody.velocity.y);
		else
			rBody.velocity = new Vector2(0, rBody.velocity.y);

		//for when you enter a cutscene
		shape.rotation = new Vector3(90+(45 * Input.GetAxis("walk")), 90, 0);

		if (Input.GetButton("parry") && !parrying)
		{
			parrying = true;
			playerAnimator.SetTrigger("parry");
			StartCoroutine(unParry());
        }

		if (isGrounded)
		{
			rocketBoots.Stop();
		}
		else
		{
			rocketBoots.Play();
		}

		if (Vector3.Distance(enemyScript.transform.position, transform.position) < 1 && Input.GetButton("Use") && !objectAttained)
		{
			inDialogue = true;
			uIController.navigateToSelection(enemyScript.dialogueRoot, "SelectionStartNOOBJECT1");
			StartCoroutine(uIController.moveMeter());
		}
		else if (Vector3.Distance(enemyScript.transform.position, transform.position) < 1 && Input.GetButton("Use") && objectAttained)
        {
            inDialogue = true;
            uIController.navigateToSelection(enemyScript.dialogueRoot, "SelectionStartOBJECT1");
            StartCoroutine(uIController.moveMeter());
        }


		if (!inFight && !inDialogue)
			fuelMeter.value = Mathf.Abs(Vector3.Distance(transform.position, trigger.transform.position) - startDistance)/ startDistance;
        else 
			fuelMeter.value = 1;


        if (collisionBaybe.IsTouchingLayers(wallLayers))
		{
			//walk = false;
			rBody.AddForce(new Vector2(0, 2500));
		}
		/*
		else
		{
			walk = true;
		}*/
	}


		//actually executes moving the player forward

	
	
}
