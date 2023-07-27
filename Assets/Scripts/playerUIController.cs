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
    private dialogueParsing.dialogueData mainData;

	public RawImage dialogueChoiceBox;
    public Slider patienceMeter;
    public List<TMP_Text> choiceButtons;

    public Image dialoguePlayer;
	public Image dialogueEnemy;

	public TMP_Text playerText;
	public TMP_Text enemyText;
    [SerializeField] float dialogueSpeed = .02f;
    [SerializeField] Image playerTextEnter;
    [SerializeField] Image enemyTextEnter;
    [SerializeField] Image zoomIn;
    

    private IEnumerator dialogue(dialogueParsing.dialogueLine[] lines)
	{
		foreach (dialogueParsing.dialogueLine line in lines)
		{
            if (line.Subject)
			{
                for (int i = 1; i <= line.dialogueText.Length; i++)
                {
                    playerText.text = line.dialogueText.Substring(0, i);
                    

                    yield return new WaitForSeconds(dialogueSpeed);
                }
                playerTextEnter.enabled = true;
                playerText.text = line.dialogueText;
            }
			else
			{
                for (int i = 1; i <= line.dialogueText.Length; i++)
                {
                    enemyText.text = line.dialogueText.Substring(0, i);
					

                    yield return new WaitForSeconds(dialogueSpeed);
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
		dialoguePlayer.transform.DOMoveX(50, 1);
        dialogueEnemy.transform.DOMoveX(1080, 1);
        zoomIn.transform.DOLocalMoveY(0, 1);
        playerStuff.playerCamera.transform.DOMove(playerStuff.tweenPos.transform.position, 1);
        playerStuff.playerCamera.DOOrthoSize(10, 1);

        if (!dev_SkipCutscene)
        {
            foreach (dialogueParsing.dialogueData data in dialogueRoot.cutscene_Dialogue)
		    {
                mainData = data;
                print(data.Start);
                StartCoroutine(dialogue(data.Start));
			
		    }
        }
        else
        {
            stopDialogue();
        }
        
        
    }
    
    private bool isClicked = false;
    int clickedButton;
    public void click(int s) 
    {
        clickedButton = s;
        isClicked = true;
    }

    private Coroutine dialogueRoutine;
    private IEnumerator typeWrite(dialogueParsing.Dialogue dialogueRoot, dialogueParsing.selection line)
    {

        //print("in new selection");
        for (int i = 1; i <= line.enemy_Text.Length; i++)
        {
            enemyText.text = line.enemy_Text.Substring(0, i);


            yield return new WaitForSeconds(dialogueSpeed);
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
        patience =+ line.choices[clickedButton].Reaction;
        //print(isClicked);
        isClicked = false;
        //print("player has chosen");
        dialogueChoiceBox.transform.DOLocalMoveY(-817, 1);

        for (int i = 1; i <= line.choices[clickedButton].dialogueLine.Length; i++)
        {
            playerText.text = line.choices[clickedButton].dialogueLine.Substring(0, i);


            yield return new WaitForSeconds(dialogueSpeed);
        }

        //print("finished typewriting " + line.choices[clickedButton].dialogueLine);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        //print("moving on");

        if (line.choices[clickedButton].next_Dialogue_Selection == "end")
        {
            patienceMeter.transform.DOLocalMoveX(-1309, 1);
            playerStuff.inFight = false; 
            StartCoroutine( dialogue(mainData.End));
        }
        else
        {
            navigateToSelection(dialogueRoot, line.choices[clickedButton].next_Dialogue_Selection);
        }
            
        
    }

    public void navigateToSelection(dialogueParsing.Dialogue dialogueRoot, string nameOfSelection)
    {
        print(nameOfSelection);
        patienceMeter.transform.DOLocalMoveX(-714, 1);
        dialoguePlayer.transform.DOMoveX(50, 1);
        dialogueEnemy.transform.DOMoveX(1080, 1);
        zoomIn.transform.DOLocalMoveY(0, 1);



        foreach (dialogueParsing.selection data in dialogueRoot.combat_Selections)
        {

			if (data.Name == nameOfSelection)
            {
                //print(data.Name);
                
                dialogueRoutine = StartCoroutine(typeWrite(dialogueRoot, data));
            }

        }

    }

    private float patience = 5;
    public IEnumerator moveMeter()
    {
        patienceMeter.value = patience / 10;
        while (patience > 0)
        {
            yield return new WaitForSeconds(3);
            patience -= 1;
            patienceMeter.value = patience/10;
        }
        StopCoroutine(dialogueRoutine);

        enemyStuff.FirstLevel = false;
        stopDialogue();
    }

    private void stopDialogue()
	{       
        playerStuff.inDialogue = false;
        patienceMeter.transform.DOLocalMoveX(-1309, 1);
        dialoguePlayer.transform.DOMoveX(1431, 1);
        dialogueEnemy.transform.DOMoveX(-1712, 1);
        dialogueChoiceBox.transform.DOLocalMoveY(-781, 1);
        zoomIn.transform.DOLocalMoveY(1635, 1);

        if (playerStuff.inFight)
        {
            enemyStuff.enemyHealth = enemyStuff.enemyMax;
            enemyStuff.continualizeFight();
        }
        else
        {
            enemyStuff.endFight();
        }
	}

}
