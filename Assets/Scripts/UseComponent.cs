using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UseComponent : MonoBehaviour
{
    public UnityEvent eventDemo;
    private BoxCollider2D triggerCollider;
    [SerializeField] enemyScripting enemyScript;
    [SerializeField] playerMovement playerScript;
    [SerializeField] GameObject player;
    [SerializeField][Range(1, 5)] int useRange = 1;
    private enum UseableTypes
    {
        noKey,
        Use,
        
    };

    [SerializeField] bool destroyOnUse;
    [SerializeField] UseableTypes inputType;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    private IEnumerator Use()
    {
        eventDemo.Invoke();
        print("check1");
        if (destroyOnUse)
        {
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(1);
            print("check2");
            triggerCollider.isTrigger = false;
        }

    }


    private void Update()
    {
        

        switch (inputType)
        {
            case UseableTypes.noKey:
                if (Vector3.Distance(player.transform.position, transform.position) < useRange)
                    StartCoroutine(Use());

                break;
            case UseableTypes.Use:
                if (Input.GetButton("Use") && Vector3.Distance(player.transform.position, transform.position) < useRange)
                    StartCoroutine(Use());

                break;
            
        }
    }
}