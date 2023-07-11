using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DentedPixel;

public class playerUIController : MonoBehaviour
{
<<<<<<< Updated upstream
	public playerMovement playerStuff;

	public Image dialoguePlayer;
	public Image dialogueEnemy;

	public TMP_Text playerText;
	public TMP_Text enemyText;

	private IEnumerator dialogue(dialogueParsing.dialogueLine[] lines)
	{
		foreach (dialogueParsing.dialogueLine line in lines)
		{
=======
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
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
			enemyText.text = "";
			playerText.text = "";
		}
		stopDialogue();
	}

	


	public void startDialogue(dialogueParsing.Dialogue dialogueRoot)
	{
		print("f");
		LeanTween.moveLocal(dialoguePlayer.gameObject, new Vector3(664, -190, 0), 1);
		LeanTween.moveLocal(dialogueEnemy.gameObject, new Vector3(-621, -190, 0), 1);

		
		foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		{
			
			

			StartCoroutine(dialogue(data.Start));
			
		}
	}

	private void stopDialogue()
	{
		playerStuff.inDialogue = false;
		LeanTween.moveLocal(dialoguePlayer.gameObject, new Vector3(1431, -190, 0), 1);
		LeanTween.moveLocal(dialogueEnemy.gameObject, new Vector3(-1712, -190, 0), 1);
		
	}

=======
            enemyText.text = "";
            playerText.text = "";
        }
        stopDialogue();
    }

    public void startDialogue(dialogueParsing.Dialogue dialogueRoot)
    {
        print("f");
        DOTween
            .To(
                () => playerText.transform.position.x,
                x =>
                    playerText.transform.position = new Vector3(x, playerText.transform.position.y),
                0,
                1
            )
            .SetEase(Ease.Linear);
        DOTween
            .To(
                () => enemyText.transform.position.x,
                x => enemyText.transform.position = new Vector3(x, enemyText.transform.position.y),
                0,
                1
            )
            .SetEase(Ease.Linear);

        foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
        {
            StartCoroutine(dialogue(data.Start));
        }

        //stopDialogue();
    }

    private void stopDialogue()
    {
        playerStuff.inDialogue = false;
        DOTween
            .To(
                () => playerText.transform.position.x,
                x =>
                    playerText.transform.position = new Vector3(x, playerText.transform.position.y),
                400,
                1
            )
            .SetEase(Ease.Linear);
        DOTween
            .To(
                () => enemyText.transform.position.x,
                x => enemyText.transform.position = new Vector3(x, enemyText.transform.position.y),
                -400,
                1
            )
            .SetEase(Ease.Linear);

        enemyStuff.continualizeFight();
    }
>>>>>>> Stashed changes
}
