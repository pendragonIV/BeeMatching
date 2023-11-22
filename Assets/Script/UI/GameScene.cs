using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button replayButton;

    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text timeLeftText;

    private void Start()
    {
        levelText.text = "Level " + (LevelManager.instance.currentLevelIndex + 1).ToString();
    }

    public void UpdateTimeLeftText(float timeLeft)
    {
        int minutes = (int)timeLeft / 60;
        int seconds = (int)timeLeft % 60;
        timeLeftText.text = minutes + " : " + seconds.ToString("00");
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        StartCoroutine(SetAchive(winPanel));
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        StartCoroutine(SetAchive(losePanel));
    }

    private IEnumerator SetAchive(Transform achiveParent)
    {
        Transform achiveContainer = achiveParent.GetChild(1);
        for (int i = 0; i < achiveContainer.childCount; i++)
        {
            yield return new WaitForSecondsRealtime(.3f);
            if (i < GameManager.instance.achivement)
            {
                Transform star = achiveContainer.GetChild(i);
                SetStar(star);
            }
        }
    }

    private void SetStar(Transform star)
    {
        star.localScale = new Vector3(0, 0, 0);
        star.gameObject.SetActive(true);
        star.DOScale(new Vector3(1, 1, 1), .3f).SetEase(Ease.OutBack).SetUpdate(true);
        Transform starDisabler = star.GetChild(0);
        starDisabler.gameObject.SetActive(false);
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
