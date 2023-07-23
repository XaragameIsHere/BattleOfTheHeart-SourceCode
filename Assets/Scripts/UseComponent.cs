using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UseComponent : MonoBehaviour
{
    public UnityEvent eventDemo;
    [SerializeField] enemyScripting enemyScript;
    [SerializeField] playerMovement playerScript;
    [SerializeField] GameObject player;
    private enum UseableTypes
    {
        noKey,
        Use,
        
    };
    [SerializeField] UseableTypes inputType;


    private void Update()
    {
        switch (inputType)
        {
            case UseableTypes.noKey:
                if (Vector3.Distance(player.transform.position, transform.position) < 1)
                {
                    eventDemo.Invoke();
                    Destroy(gameObject);
                }
                break;
            case UseableTypes.Use:
                if (Input.GetButton("Use") && Vector3.Distance(player.transform.position, transform.position) < 1)
                {
                    eventDemo.Invoke();
                    Destroy(gameObject);
                }
                break;
            
        }

        

        
    }

}