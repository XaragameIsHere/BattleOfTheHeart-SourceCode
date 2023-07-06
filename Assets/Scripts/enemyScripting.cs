using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;




public class enemyScripting : MonoBehaviour
{
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


    public BoxCollider2D boxTrigger;
    public BoxCollider2D playerCollider;
    public GameObject player;
    private AudioSource audioSystem;
    private SpriteRenderer spriteComponent;



    // Start is called before the first frame update
    void Start()
    {
        spriteComponent = GetComponent<SpriteRenderer>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        audioSystem = GetComponent<AudioSource>();
    }

    

    

    public void initializeFight()
    {
        player.GetComponent<AudioSource>().Stop();
        audioSystem.Play();
        playerScript.inDialogue = true;
        LeanTween.move(player, new Vector3( 4.51f, 11, 0), .5f);
        print(jsonFile.text);
        dialogueRoot = JsonUtility.FromJson<dialogueParsing.Dialogue>(jsonFile.text);
        Controller.startDialogue(dialogueRoot);
        print(dialogueRoot.cutscene_Dialogue);



    }

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

    public void continualizeFight()
    {
        print("start");
        audioSystem.clip = FightMusic;
        audioSystem.Play();
        LeanTween.move(gameObject, new Vector3(-10, 11), 1);
        playerScript.playerCamera.orthographicSize = 7;
        StartCoroutine(drop());

    }

    IEnumerator waitforit()
    {
        yield return new WaitForSeconds(3);
        playerScript.invincibility = false;
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
