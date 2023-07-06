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
    public bool inDialogue = false;
    public bool alive = true;
    public Rigidbody2D rBody;
    Collider2D collisionBaybe;
    Collider2D[] hits;


    // Start is called before the first frame update
    void Start()
    {
        
        rBody = GetComponent<Rigidbody2D>();
        collisionBaybe = GetComponent<Collider2D>();
    }


    // Update is called once per frame


    void FixedUpdate()
    {
        isGrounded = rBody.IsTouchingLayers(groundLayers); //checks if the player is on a platform
        //calculates vertical velocity

        //character controls
        if (isGrounded && Input.GetButtonDown("Jump") && alive && !inDialogue)
            rBody.velocity = new Vector2(rBody.velocity.x, jumpStrength);   
        
        

        if (Input.GetButton("walk") && alive && !inDialogue)
            rBody.velocity = new Vector2(walkSpeed * Input.GetAxis("walk"), rBody.velocity.y);
        else if (!Input.GetButton("walk") && Mathf.Abs(rBody.velocity.x) > 1)
        
            rBody.velocity = new Vector2(Mathf.Lerp(rBody.velocity.x, 0, friction * Time.deltaTime), rBody.velocity.y);
        else
            rBody.velocity = new Vector2(0, rBody.velocity.y);

        //for when you enter a cutscene
        



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
