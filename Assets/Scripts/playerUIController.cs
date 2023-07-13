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

	public RawImage dialogueChoiceBox;
    public Slider patienceMeter;
    public List<TMP_Text> choiceButtons;

    public Image dialoguePlayer;
	public Image dialogueEnemy;

	public TMP_Text playerText;
	public TMP_Text enemyText;

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

                playerText.text = line.dialogueText;
            }
			else
			{
                for (int i = 1; i <= line.dialogueText.Length; i++)
                {
                    enemyText.text = line.dialogueText.Substring(0, i);
					

                    yield return new WaitForSeconds(.02f);
                }
				enemyText.text = line.dialogueText;
            }

			
			
			yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

			enemyText.text = "";
			playerText.text = "";
		}
		stopDialogue();
	}

    public void startDialogue(dialogueParsing.Dialogue dialogueRoot)
	{
		print("f");
		dialoguePlayer.transform.DOLocalMoveX(664, 1);
        dialogueEnemy.transform.DOLocalMoveX(-621, 1);


        foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		{
			
			//StartCoroutine(dialogue(data.Start));
			
		}

        stopDialogue();
    }

    bool isClicked = false;
    int clickedButton;
    public void click(int s) 
    {
        clickedButton = s;
        isClicked = true;
    }
    private IEnumerator typeWrite(dialogueParsing.Dialogue dialogueRoot, dialogueParsing.selection line)
    {
        print("in new selection");
        for (int i = 1; i <= line.enemy_Text.Length; i++)
        {
            enemyText.text = line.enemy_Text.Substring(0, i);


            yield return new WaitForSeconds(.02f);
        }

        enemyText.text = line.enemy_Text;

        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

        for (int i = 0; i < 4; i++)
        {
            choiceButtons[i].text = line.choices[i].dialogueLine;
        }

        dialogueChoiceBox.transform.DOLocalMoveY(-300, 1);

        yield return new WaitUntil(() => isClicked);
        isClicked = false;

        dialogueChoiceBox.transform.DOLocalMoveY(-817, 1);

        for (int i = 1; i <= line.choices[clickedButton].dialogueLine.Length; i++)
        {
            playerText.text = line.choices[clickedButton].dialogueLine.Substring(0, i);


            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

        navigateToSelection(dialogueRoot, line.choices[clickedButton].next_Dialogue_Selection);
        
    }

    public void navigateToSelection(dialogueParsing.Dialogue dialogueRoot, string nameOfSelection)
    {
        StopCoroutine("typeWrite");
        print(dialogueRoot.combat_Selections);
        patienceMeter.transform.DOLocalMoveX(-714, 1);
        dialoguePlayer.transform.DOLocalMoveX(664, 1);
        dialogueEnemy.transform.DOLocalMoveX(-621, 1);



        foreach (dialogueParsing.selection data in dialogueRoot.combat_Selections)
        {

			if (data.Name == nameOfSelection)
            {
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
