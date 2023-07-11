using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UIElements;

public class playerUIController : MonoBehaviour
{
	public playerMovement playerStuff;
	public enemyScripting enemyStuff;



	public Label playerText;
	public Label enemyText;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

		playerText = root.Q<Label>("PlayerDialogue");
		enemyText = root.Q<Label>("EnemyDialogue");
    }

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
		DOTween.To(() => playerText.transform.position.x, x => playerText.transform.position = new Vector3(x, playerText.transform.position.y), 0, 1).SetEase(Ease.Linear);
        DOTween.To(() => enemyText.transform.position.x, x => enemyText.transform.position = new Vector3(x, enemyText.transform.position.y), 0, 1).SetEase(Ease.Linear);


        foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		{
			
			StartCoroutine(dialogue(data.Start));
			
		}

        //stopDialogue();
    }

	private void stopDialogue()
	{
		playerStuff.inDialogue = false;
        DOTween.To(() => playerText.transform.position.x, x => playerText.transform.position = new Vector3(x, playerText.transform.position.y), 113, 1).SetEase(Ease.Linear);
        DOTween.To(() => enemyText.transform.position.x, x => enemyText.transform.position = new Vector3(x, enemyText.transform.position.y), -113, 1).SetEase(Ease.Linear);

        enemyStuff.continualizeFight();
	}

}
