using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] Vector2 velocity = new Vector2(1, 0);
    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] Camera playerCamera;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpStrength = 20;
    [SerializeField] float friction = 2;
    [SerializeField] float walkSpeed = 5;

    public int playerHealth = 100;
    public bool invincible = false;
    public Vector2 velocityUp;
    public Vector2 playerSpeed;

    private CharacterController playerMover;
    //private Animator animationController;
    private bool isGrounded;
    private bool wallRun;
    private bool alive = true;
    private bool walk = true;
    Rigidbody2D rBody;
    Collider2D collisionBaybe;
    Collider2D[] hits;


    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //animationController = GetComponentInChildren<Animator>();
        //animationController.SetBool("Run", true);
        rBody = GetComponent<Rigidbody2D>();
        collisionBaybe = GetComponent<Collider2D>();
    }

    /*
    public void kill()
    {
        playerCamera.transform.localPosition = new Vector3(0, -0.899999976f, 0.138999999f);
        playerCamera.transform.localEulerAngles = new Vector3(0, 0, -28);


    }
    */
    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = rBody.IsTouchingLayers(groundLayers); //checks if the player is on a platform
        //calculates vertical velocity

        //character controls
        if (isGrounded && Input.GetButtonDown("Jump") && alive)
            rBody.velocity = new Vector2(rBody.velocity.x, jumpStrength);   
        

        

        if (Input.GetButton("walk") && alive && walk)
            rBody.velocity = new Vector2(walkSpeed * Input.GetAxis("walk"), rBody.velocity.y);
        else if (!Input.GetButton("walk") && Mathf.Abs(rBody.velocity.x) > 1)
        
            rBody.velocity = new Vector2(Mathf.Lerp(rBody.velocity.x, 0, friction * Time.deltaTime), rBody.velocity.y);
        else
            rBody.velocity = new Vector2(0, rBody.velocity.y);

        //collisionBaybe.GetContacts(hits);

        if (collisionBaybe.IsTouchingLayers(wallLayers) /*&& Input.GetAxis("walk") != 0 && Input.GetButtonDown("Jump")*/)
        {
            print(rBody.velocity);
            //walk = false;
            rBody.AddForce(new Vector2(-500 * Input.GetAxis("walk"), 500));
            print(rBody.velocity);
        }
        /*
        else
        {
            walk = true;
        }*/
    }


        //actually executes moving the player forward

    
    
}
