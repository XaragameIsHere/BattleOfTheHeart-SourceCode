using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interfaceComponent
{
    [System.Serializable]
    public class dialogueLine
    {
        public string Sprite;
        public string dialogueText;
        public string Name;
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
