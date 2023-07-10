using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
		//LeanTween.moveLocal(dialoguePlayer.gameObject, new Vector3(664, -190, 0), 1);
		//LeanTween.moveLocal(dialogueEnemy.gameObject, new Vector3(-621, -190, 0), 1);

		
		foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		{
			
			

			//StartCoroutine(dialogue(data.Start));
			
		}

        stopDialogue();
    }

	private void stopDialogue()
	{
		playerStuff.inDialogue = false;
		//LeanTween.moveLocal(dialoguePlayer.gameObject, new Vector3(1431, -190, 0), 1);
		//LeanTween.moveLocal(dialogueEnemy.gameObject, new Vector3(-1712, -190, 0), 1);
		enemyStuff.continualizeFight();
	}

}
