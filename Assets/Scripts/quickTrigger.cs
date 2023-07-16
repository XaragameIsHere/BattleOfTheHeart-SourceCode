using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class quickTrigger : MonoBehaviour
{
    public UnityEvent eventDemo;
    [SerializeField] bool keepTrigger;
    

    /// <summary>
    /// This tells the enemy script that you like... exist
    /// nothing else
    /// </summary>
    /// 
    private IEnumerator waitFor()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Collider2D>().enabled = keepTrigger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.layer);
        if (collision.gameObject.layer == 7)
        {
            eventDemo.Invoke();
            print("collide");
            
            GetComponent<Collider2D>().enabled = false;

        }
    }
    
    
}
