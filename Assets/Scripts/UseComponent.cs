using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UseComponent : MonoBehaviour
{
    public UnityEvent eventDemo;//Event System Trigger
    private BoxCollider2D triggerCollider;//Collider of object we are interacting with
    [SerializeField] GameObject player;//the player object in the scene
    [SerializeField][Range(1, 5)] int useRange = 1;//range of where the interactible object is able to be interacted with
    
    //Two types of interactible objects
    private enum UseableTypes
    {
        noKey,//no key press required
        Use// key press required
    };

    [SerializeField] bool destroyOnUse; //destroys the so it can'd be interacted with a second time
    [SerializeField] UseableTypes inputType;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    private IEnumerator Use()
    {
        eventDemo.Invoke();//does the defined function
        
        if (destroyOnUse)
        {
            Destroy(gameObject);//destroys object so it can't be used
        }
        else
        {
            yield return new WaitForSeconds(1);//holds for a second
            triggerCollider.isTrigger = false;
        }

    }


    private void Update()
    {
        
        //checks the input types
        switch (inputType)
        {
            //if nokey
            case UseableTypes.noKey:
                if (Vector3.Distance(player.transform.position, transform.position) < useRange)//checks if the distance between the player and the object is less than the use range
                    StartCoroutine(Use());

                break;
            case UseableTypes.Use:
                if (Input.GetButton("Use") && Vector3.Distance(player.transform.position, transform.position) < useRange)//checks if the distance between the player and the object is less than the use range and is pressing the use key
                    StartCoroutine(Use());

                break;
            
        }
    }
}