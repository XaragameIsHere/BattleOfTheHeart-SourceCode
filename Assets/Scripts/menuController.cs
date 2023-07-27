using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class menuController : MonoBehaviour
{
    [SerializeField] Button quitButton;
    [SerializeField] Button playButton;
    [SerializeField] Image black;

    // Start is called before the first frame update



    IEnumerator Start()
    {
        yield return black.DOColor(new Color(70, 70, 70, 0), 1).WaitForCompletion();

        Destroy(black.gameObject);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            quitButton.enabled = false;
        } 
        playButton.transform.DOLocalMoveY(-305, 1);
        quitButton.transform.DOLocalMoveY(-413, 1);

    }

    public void playGame()
    {
        if (PlayerPrefs.GetString("Level") == null)
        {
            print("Loading previous save");
            SceneManager.LoadScene("ctscn_opening");
        }
        else
        {
            print("Loading new save");
            SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
