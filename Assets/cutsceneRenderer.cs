using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cutsceneRenderer : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Transform returnKey;
    private Image imageToRender;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        imageToRender = GetComponent<Image>();

        foreach (Sprite spriteToRender in sprites)
        {
            imageToRender.sprite = spriteToRender;
            returnKey.localPosition = new Vector3(800, -1200);
            imageToRender.color = new Color(255, 255, 255, 0);
            yield return imageToRender.DOColor(new Color(255, 255, 255, 255), 1.5f).WaitForCompletion();

            returnKey.DOLocalMoveY(-440, 1).SetEase(Ease.OutElastic);

            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
            returnKey.DOLocalMoveY(-1200, 1);
            yield return imageToRender.DOColor(new Color(255, 255, 255, 0), 1.5f).WaitForCompletion();
        }

        SceneManager.LoadScene("lvl_gunslinger");
    }
}
