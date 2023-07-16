using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tutorialScripting : MonoBehaviour
{
    [SerializeField] GameObject player;
    public void dropHint(GameObject hint)
    {
        hint.transform.DOMoveY(player.transform.position.y + 2, 1);
    } 



}
