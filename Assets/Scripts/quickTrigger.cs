using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickTrigger : MonoBehaviour
{
    
    public enemyScripting enemy;
    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision.gameObject.layer);
        if (collision.gameObject.layer == 7)
        {
            print("collide");
            GetComponent<Collider2D>().isTrigger = false;
            enemy.initializeFight();
        }
    }
    
    
}
