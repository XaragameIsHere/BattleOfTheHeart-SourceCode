using DG.Tweening;
using System;
using System.Collections;
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
    public BoxCollider2D playerCollider;
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
        playerCollider = player.GetComponent<BoxCollider2D>();
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
    /*
    IEnumerator drop()
    {
        GameObject[] falling = new GameObject[7];
        for (int i = 0; i < 6; i++)
        {
            
            GameObject bullet = Instantiate(attack1Sprite);
            falling.SetValue(bullet, i);
            

        }
        for (int v = 0; v < 4; v++)
        {

            for (int i = 0; i < 6; i++)
            {
                falling[1].transform.position = fallingObjectArea.transform.GetChild(Mathf.RoundToInt(Random.Range(1, 11))).position;
            }

            yield return new WaitForSeconds(Random.Range(1, 3));

            for (int i = 0; i < 6; i++)
            {
                LeanTween.moveY(falling[i], falling[i].transform.position.y - 6, .2f);
            }
            yield return new WaitForSeconds(1.5f);

            
            
            
        }
        falling =null;
    }
    */

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
        StartCoroutine(timer(8));

    }

    private void narrow()
    {
        DOTween.To(() => shape.angle, x => shape.angle = x, 10, 2);
        transform.Rotate(new Vector3(0, 0, -20));
        //transform.DORotate(new Vector3(0, 0, -15), 1).WaitForCompletion();
        transform.DORotate(new Vector3(0, 0, 40), 3).SetLoops(4, LoopType.Yoyo).OnComplete(shootyshooty);

    }

    int shootyState = 2;

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
        shootyshooty();
        //StartCoroutine(drop());

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
