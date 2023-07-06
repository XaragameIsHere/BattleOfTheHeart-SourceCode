using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;
/// <summary>
/// this is the componenent of the UI that controls the dialogue in game
/// ye
/// </summary>
/// 
[System.Serializable]
public class dialogueParsing
{
    [System.Serializable]
    public class dialogueLine
    {
        public string Sprite;
        public string dialogueText;
        public bool Subject;
    }


    [System.Serializable]
    public class dialogueData
    {
        public dialogueLine[] Start;
    }

    [System.Serializable]
    public class Dialogue
    {
        public dialogueData[] cutscene_Dialogue;

    }
}


public class UIelements
{
    
    
}

//public class 
