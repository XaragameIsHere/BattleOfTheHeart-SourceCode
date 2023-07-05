using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyData
{
    //Sprites Data
    [System.Serializable]
    public class bulletSprites
    {
        public string Attack1;
        public string Attack2;
        public string Attack3;

    }

    [System.Serializable]
    public class spriteData
    {
        public string enemy_Sprite;
        public string close_Up_Sprite;
        public string animator_Path;
        public string close_Up_Animator_Path;
        public bulletSprites[] bullet_Sprites;

    }



    //particle data
    [System.Serializable]
    public class attackParticles
    {
        public string start_Particles;
        public string trail_Particles;
        public string end_Particles;
    }

    [System.Serializable]
    public class particleData
    {
        public attackParticles[] Attack1;
        public attackParticles[] Attack2;
        public attackParticles[] Attack3;
    }



    //Audio 
    [System.Serializable]
    public class music
    {
        public string main_Music;
        public string dialogue_Music;
        public string walk;
    }

    [System.Serializable]
    public class SFX
    {
        public string speaking_SFX_Angry;
        public string speaking_SFX_Happy;
        public string annoyed_SFX;
        public string happy_SFX;
    }

    [System.Serializable]
    public class audioData
    {
        public music[] enemy_Music;
        public SFX[] SFX;
    }

    [System.Serializable]
    public class dialogueLine
    {
        public string Sprite;
        public string dialogueText;
        public string Name;
    }

    //Dialogue
    [System.Serializable]
    public class cutsceneData
    {
        public dialogueLine[] Start;
        public dialogueLine[] End;
    }

    [System.Serializable]
    public class dialogueData
    {
        public cutsceneData[] cutscene_Dialogue;
    }

    [System.Serializable]
    public class enemy 
    {
        public dialogueData[] Dialogue;
        public spriteData[] Sprites;
        public particleData[] Particles;
        public audioData[] Audio;
    }
}

