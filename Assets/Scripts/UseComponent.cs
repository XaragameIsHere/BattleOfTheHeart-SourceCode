using System;
using UnityEngine;
using UnityEngine.Events;

public class UseComponent : MonoBehaviour
{
    public UnityEvent eventDemo;
    [SerializeField] GameObject player;
    [SerializeField] bool use;


    private void Update()
    {
        if (Input.GetButton("Use") && Vector3.Distance(player.transform.position, transform.position) < 1 && use)
        {
            eventDemo.Invoke();
            Destroy(gameObject);
        }

        if (Vector3.Distance(player.transform.position, transform.position) < 1 && !use)
        {
            eventDemo.Invoke();
            Destroy(gameObject);
        }
    }

    

}