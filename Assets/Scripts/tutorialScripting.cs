using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tutorialScripting : MonoBehaviour
{
    IEnumerator waitToDestroy(GameObject hint)
    {
        yield return new WaitForSeconds(2);
        Destroy(hint);
    }

    [SerializeField] GameObject player;
    public void dropHint(GameObject hint)
    {
        hint.transform.DOMoveY(player.transform.position.y + 2, 1);
        StartCoroutine(waitToDestroy(hint));
    } 



}
