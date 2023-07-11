using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
//using UnityEngine.UIElements;

public class playerUIController : MonoBehaviour
{
	public playerMovement playerStuff;
	public enemyScripting enemyStuff;

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
			
			StartCoroutine(dialogue(data.Start));
			
		}

        //stopDialogue();
    }

	private void stopDialogue()
	{
		playerStuff.inDialogue = false;
        dialoguePlayer.transform.DOLocalMoveX(1431, 1);
        dialogueEnemy.transform.DOLocalMoveX(-1712, 1);
        enemyStuff.continualizeFight();
	}

}
