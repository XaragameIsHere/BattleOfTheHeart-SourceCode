using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class playerUIController : MonoBehaviour
{

	public playerMovement playerStuff;
	public enemyScripting enemyStuff;
    public bool dev_SkipCutscene = false;

	public RawImage dialogueChoiceBox;
    public Slider patienceMeter;
    public List<TMP_Text> choiceButtons;

    public Image dialoguePlayer;
	public Image dialogueEnemy;

	public TMP_Text playerText;
	public TMP_Text enemyText;
    [SerializeField] TMP_Text playerTextEnter;
    [SerializeField] TMP_Text enemyTextEnter;
    

    private IEnumerator dialogue(dialogueParsing.dialogueLine[] lines)
	{
		foreach (dialogueParsing.dialogueLine line in lines)
		{
            if (line.Subject)
			{
                for (int i = 1; i <= line.dialogueText.Length; i++)
                {
                    playerText.text = line.dialogueText.Substring(0, i);
                    

                    yield return new WaitForSeconds(.02f);
                }
                playerTextEnter.enabled = true;
                playerText.text = line.dialogueText;
            }
			else
			{
                for (int i = 1; i <= line.dialogueText.Length; i++)
                {
                    enemyText.text = line.dialogueText.Substring(0, i);
					

                    yield return new WaitForSeconds(.02f);
                }
                enemyTextEnter.enabled = true;
                enemyText.text = line.dialogueText;
            }

			
			
			yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
            playerTextEnter.enabled = false;
            enemyTextEnter.enabled = false;

            enemyText.text = "";
			playerText.text = "";
		}
		stopDialogue();
	}

    public void startDialogue(dialogueParsing.Dialogue dialogueRoot)
	{
		print("f");
		dialoguePlayer.transform.DOLocalMoveX(-664, 1);
        dialogueEnemy.transform.DOLocalMoveX(621, 1);
        playerStuff.playerCamera.transform.DOMove(playerStuff.tweenPos.transform.position, 1);
        playerStuff.playerCamera.DOOrthoSize(10, 1);

        if (!dev_SkipCutscene)
        {
            foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		    {
			
			    StartCoroutine(dialogue(data.Start));
			
		    }
        }
        else
        {
            stopDialogue();
        }
        
        
    }
    
    public bool isClicked = false;
    int clickedButton;
    public void click(int s) 
    {
        clickedButton = s;
        isClicked = true;
    }
    private IEnumerator typeWrite(dialogueParsing.Dialogue dialogueRoot, dialogueParsing.selection line)
    {

        //print("in new selection");
        for (int i = 1; i <= line.enemy_Text.Length; i++)
        {
            enemyText.text = line.enemy_Text.Substring(0, i);


            yield return new WaitForSeconds(.02f);
        }

        enemyText.text = line.enemy_Text;
        //print("finished typewriting "+ line.enemy_Text);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        //print("finished creating choices " );
        for (int i = 0; i < 4; i++)
        {
            choiceButtons[i].text = line.choices[i].dialogueLine;
        }

        dialogueChoiceBox.transform.DOLocalMoveY(-300, 1);
        isClicked = false;
        //print("moving box of choices" );
        yield return new WaitUntil(() => isClicked);
        //print(isClicked);
        isClicked = false;
        //print("player has chosen");
        dialogueChoiceBox.transform.DOLocalMoveY(-817, 1);

        for (int i = 1; i <= line.choices[clickedButton].dialogueLine.Length; i++)
        {
            playerText.text = line.choices[clickedButton].dialogueLine.Substring(0, i);


            yield return new WaitForSeconds(.02f);
        }

        //print("finished typewriting " + line.choices[clickedButton].dialogueLine);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        //print("moving on");

        navigateToSelection(dialogueRoot, line.choices[clickedButton].next_Dialogue_Selection);
        
    }

    public void navigateToSelection(dialogueParsing.Dialogue dialogueRoot, string nameOfSelection)
    {
        StopCoroutine("typeWrite");
        print(nameOfSelection);
        patienceMeter.transform.DOLocalMoveX(-714, 1);
        dialoguePlayer.transform.DOLocalMoveX(-664, 1);
        dialogueEnemy.transform.DOLocalMoveX(621, 1);



        foreach (dialogueParsing.selection data in dialogueRoot.combat_Selections)
        {

			if (data.Name == nameOfSelection)
            {
                //print(data.Name);
                StartCoroutine(typeWrite(dialogueRoot, data));
            }

        }

    }

    float patience = 5;
    public IEnumerator moveMeter()
    {
        patienceMeter.value = patience / 10;
        while (patienceMeter.value > 0)
        {
            yield return new WaitForSeconds(2);
            patience -= 1;
            patienceMeter.value -= patience/10;
        }

        
    }

    private void stopDialogue()
	{
		playerStuff.inDialogue = false;
        dialoguePlayer.transform.DOLocalMoveX(1431, 1);
        dialogueEnemy.transform.DOLocalMoveX(-1712, 1);
        enemyStuff.continualizeFight();
	}

}
