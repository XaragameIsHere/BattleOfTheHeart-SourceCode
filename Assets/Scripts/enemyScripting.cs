using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;




public class enemyScripting : MonoBehaviour
{
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().sprite = ;
        initializeFight();
    }

    private void initializeFight()
    {
        print(jsonFile.text);
        
        enemyData.enemy enemyRoot = JsonUtility.FromJson<enemyData.enemy>(jsonFile.text);

        Debug.Log(enemyRoot.Dialogue);

        
        foreach (enemyData.dialogueData data in enemyRoot.Dialogue)
        {
            foreach (enemyData.cutsceneData newData in data.cutscene_Dialogue)
            {
                foreach (enemyData.dialogueLine newNewData in newData.Start)
                {
                    Debug.Log(newNewData.Name);
                }
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
