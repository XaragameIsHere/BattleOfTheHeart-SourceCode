using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] Vector2 velocity = new Vector2(1, 0);
    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask wallLayers;
    public Camera playerCamera;
    [SerializeField] float jumpStrength = 20;
    [SerializeField] float friction = 2;
    [SerializeField] float walkSpeed = 5;

    public int playerLives = 4;
    public bool invincibility = false;
    public int playerHealth = 100;
    public Vector2 velocityUp;
    public Vector2 playerSpeed;

    private CharacterController playerMover;
    //private Animator animationController;
    private bool isGrounded;
    private bool wallRun;
    private ParticleSystem rocketBoots;
    public bool inDialogue = false;
    public bool alive = true;
    public Rigidbody2D rBody;
    Collider2D collisionBaybe;
    Collider2D[] hits;
    ParticleSystem.MainModule psMain;
    ParticleSystem.ShapeModule shape;

    // Start is called before the first frame update
    void Start()
    {
        rocketBoots = GetComponent<ParticleSystem>();
        psMain = rocketBoots.main;
        shape = rocketBoots.shape;
        rBody = GetComponent<Rigidbody2D>();
        collisionBaybe = GetComponent<Collider2D>();
    }


    // Update is called once per frame


    void FixedUpdate()
    {
        isGrounded = rBody.IsTouchingLayers(groundLayers); //checks if the player is on a platform
        //calculates vertical velocity

        //character controls

        if (Input.GetButton("Jump") && alive && !inDialogue)
            rBody.velocity = new Vector2(rBody.velocity.x, walkSpeed * Input.GetAxis("Jump"));
        else if (!Input.GetButton("Jump") && Mathf.Abs(rBody.velocity.y) > 1)

            rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Lerp(rBody.velocity.y, 0, friction * Time.deltaTime));
        else
            rBody.velocity = new Vector2(rBody.velocity.x, 0);


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
            rBody.AddForce(new Vector2(-500 * Input.GetAxis("walk"), 500));
        }
        /*
        else
        {
            walk = true;
        }*/
    }


        //actually executes moving the player forward

    
    
}
