using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;




public class enemyScripting : MonoBehaviour
{
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
        print(jsonFile.text);
        
        interfaceComponent.Dialogue dialogueRoot = JsonUtility.FromJson<interfaceComponent.Dialogue>(jsonFile.text);

        print(dialogueRoot.cutscene_Dialogue);
        
        

        foreach (interfaceComponent.dialogueData data in dialogueRoot.cutscene_Dialogue)
        {
            foreach (interfaceComponent.dialogueLine newData in data.Start)
            {
                print(newData.Name);
            }
        } 
    }

    





     

    // Update is called once per frame
    void Update()
    {
       
    }
}
