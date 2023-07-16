
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
    public class choice
    {
        public int Reaction;
        public string dialogueLine;
        public string next_Dialogue_Selection;
    }

    [System.Serializable]
    public class selection
    {
        public string Name;
        public string enemy_Text;
        public choice[] choices;
    }

    [System.Serializable]
    public class Dialogue
    {
        public dialogueData[] cutscene_Dialogue;
        public selection[] combat_Selections;
    }


}

