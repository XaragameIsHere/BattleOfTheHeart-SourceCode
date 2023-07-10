using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;




public class enemyScripting : MonoBehaviour
{
    ParticleSystem.NoiseModule noice;
    ParticleSystem.ShapeModule shape;
    [SerializeField] GameObject fallingObjectArea;
    [SerializeField] playerMovement playerScript;
    [SerializeField] playerUIController Controller;
    [SerializeField] AudioClip FightMusic;
    [SerializeField] AudioClip dialogueMusic;
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] Canvas death;
    dialogueParsing.Dialogue dialogueRoot;

    [SerializeField] GameObject attack1Sprite; 

    public TextAsset jsonFile;
    public int test;

    private ParticleSystem shooter;
    public BoxCollider2D boxTrigger;
    public PolygonCollider2D playerCollider;
    public GameObject player;
    private AudioSource audioSystem;
    private SpriteRenderer spriteComponent;

    

    // Start is called before the first frame update
    void Start()
    {
        shooter = GetComponent<ParticleSystem>();
        noice = shooter.noise;
        shape = shooter.shape;
        spriteComponent = GetComponent<SpriteRenderer>();
        playerCollider = player.GetComponent<PolygonCollider2D>();
        audioSystem = GetComponent<AudioSource>();
    }

    

    

    public void initializeFight()
    {
        player.GetComponent<AudioSource>().Stop();
        audioSystem.Play();
        playerScript.inDialogue = true;
        player.transform.DOMove(new Vector3(4.51f, 11, 0), .5f);
       
        dialogueRoot = JsonUtility.FromJson<dialogueParsing.Dialogue>(jsonFile.text);
        Controller.startDialogue(dialogueRoot);



    }
    
    IEnumerator drop()
    {
        List<GameObject> falling = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {

            GameObject bullet = Instantiate(attack1Sprite);
            falling.Add(bullet);

        }

        for (int v = 0; v < 4; v++)
        {
            foreach (GameObject fallingObject in falling)
            {
                fallingObject.transform.position = fallingObjectArea.transform.GetChild(Mathf.RoundToInt(Random.Range(1, 11))).position;
            }

            yield return new WaitForSeconds(3);
            

            foreach (GameObject fallingObject in falling)
            {
                fallingObject.GetComponent<Rigidbody2D>().simulated = true;
            }
            yield return new WaitForSeconds(2f);

            foreach (GameObject fallingObject in falling)
            {
                fallingObject.GetComponent<Rigidbody2D>().simulated = false;
            }

        }
        falling =null;
    }
    

    IEnumerator timer(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shootyshooty();
    }

    private void spray()
    {
        shape.angle = 45;
        shape.arc = 360;
        noice.enabled = true;
        noice.frequency = 1.59f;
        noice.scrollSpeed = 1.6f;
        noice.strength = 0.09f;
        shootyState = 2;
        StartCoroutine(timer(8));

    }

    private void narrow()
    {
        DOTween.To(() => shape.angle, x => shape.angle = x, 10, 2);
        transform.Rotate(new Vector3(0, 0, -20));
        //transform.DORotate(new Vector3(0, 0, -15), 1).WaitForCompletion();
        shootyState = 1;
        transform.DORotate(new Vector3(0, 0, 40), 3).SetLoops(4, LoopType.Yoyo).OnComplete(shootyshooty);

    }

    int shootyState = 1;

    private void shootyshooty()
    {
        
        shooter.Play();

        if (shootyState == 1)
        {
            spray();
        }
        else if (shootyState == 2) 
        {
            narrow();
        }
    }

    public void continualizeFight()
    {
        print("start");

        audioSystem.clip = FightMusic;
        audioSystem.Play();
        transform.DOMove(new Vector3(-7, 11), 1);
        playerScript.playerCamera.orthographicSize = 7;
        playerScript.inFight = true;
        //shootyshooty();
        StartCoroutine(drop());

    }

    

    private void Update()
    {
        if (playerCollider.IsTouchingLayers(bulletLayer) && !playerScript.invincibility)
        {
            playerScript.invincibility = true;
            playerScript.playerLives -= 1;
            playerScript.rBody.AddForce(new Vector2(20, 4));
        }

        if (playerScript.playerLives <= 0)
        {
            death.enabled = true;
            playerScript.alive = false;
        }
    }



}
