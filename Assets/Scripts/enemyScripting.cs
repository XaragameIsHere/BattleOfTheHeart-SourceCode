using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;




public class enemyScripting : MonoBehaviour
{

    [SerializeField] playerMovement playerScript;
    [SerializeField] playerUIController Controller;
    dialogueParsing.Dialogue dialogueRoot;

    public TextAsset jsonFile;
    public int test;


    public BoxCollider2D boxTrigger;
    public BoxCollider2D playerCollider;
    public GameObject player;
    private SpriteRenderer spriteComponent;



    // Start is called before the first frame update
    void Start()
    {
        spriteComponent = GetComponent<SpriteRenderer>();
        playerCollider = player.GetComponent<BoxCollider2D>();
    }

    

    

    public void initializeFight()
    {
        playerScript.inDialogue = true;
        LeanTween.move(player, new Vector3( 4.51f, 11, 0), .5f);
        print(jsonFile.text);
        dialogueRoot = JsonUtility.FromJson<dialogueParsing.Dialogue>(jsonFile.text);
        Controller.startDialogue(dialogueRoot);
        print(dialogueRoot.cutscene_Dialogue);



    }





     

    // Update is called once per frame
    void Update()
    {
       
    }
}
