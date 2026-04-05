using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject HUDView;
    public GameObject UITutorialView;
    public GameObject PauseMenuView;
    public GameObject GameOverView;
    public GameObject LoadingSpinner;
    public GameObject WinView;
    public TextMeshProUGUI respawnCounterText;
    public Button respawnAdButton;
    public Button removeAdsButton;

    public GameManager gameManager;
    public GameObject BGCanvas;

    private bool tutorialState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGCanvas.gameObject.SetActive(true);
        HUDView.gameObject.SetActive(true);
        UITutorialView.gameObject.SetActive(true);
        PauseMenuView.gameObject.SetActive(false);
        GameOverView.gameObject.SetActive(false);
        if (WinView != null)
        {
            WinView.gameObject.SetActive(false);
        }
        LoadingSpinner.gameObject.SetActive(false);
        tutorialState = UITutorialView.gameObject.activeSelf;
        UpdateRespawnCounter(0);
    }

    public void TogglePauseMenu()
    {
        if (gameManager != null && gameManager.IsGameFinished())
        {
            return;
        }

        bool showPauseMenu = !PauseMenuView.gameObject.activeSelf;

        if (showPauseMenu)
        {
            tutorialState = UITutorialView.gameObject.activeSelf;
        }

        PauseMenuView.gameObject.SetActive(showPauseMenu);
        
        if (tutorialState)
        {
            UITutorialView.gameObject.SetActive(!showPauseMenu);
        }

        if (gameManager != null)
        {
            gameManager.SetPaused(showPauseMenu);
        }
    }

    public void ShowGameOver()
    {
        PauseMenuView.gameObject.SetActive(false);
        HUDView.gameObject.SetActive(false);
        UITutorialView.gameObject.SetActive(false);
        GameOverView.gameObject.SetActive(true);
        if (WinView != null)
        {
            WinView.gameObject.SetActive(false);
        }
        LoadingSpinner.gameObject.SetActive(false);
    }

    public void ShowWin()
    {
        PauseMenuView.gameObject.SetActive(false);
        HUDView.gameObject.SetActive(false);
        UITutorialView.gameObject.SetActive(false);
        GameOverView.gameObject.SetActive(false);
        if (WinView != null)
        {
            WinView.gameObject.SetActive(true);
        }
        SetLoadingSpinnerVisible(false);
    }

    public void ShowGameplay()
    {
        HUDView.gameObject.SetActive(true);
        UITutorialView.gameObject.SetActive(false);
        PauseMenuView.gameObject.SetActive(false);
        GameOverView.gameObject.SetActive(false);
        if (WinView != null)
        {
            WinView.gameObject.SetActive(false);
        }
        SetLoadingSpinnerVisible(false);
    }

    public void ShowRespawnLoading()
    {
        HUDView.gameObject.SetActive(false);
        UITutorialView.gameObject.SetActive(false);
        PauseMenuView.gameObject.SetActive(false);
        GameOverView.gameObject.SetActive(false);
        if (WinView != null)
        {
            WinView.gameObject.SetActive(false);
        }
        SetLoadingSpinnerVisible(true);
    }

    public IEnumerator ShowLoadingSpinnerForSeconds(float duration)
    {
        SetLoadingSpinnerVisible(true);
        yield return new WaitForSecondsRealtime(duration);
        SetLoadingSpinnerVisible(false);
    }

    public void SetLoadingSpinnerVisible(bool visible)
    {
        if (LoadingSpinner != null)
        {
            LoadingSpinner.gameObject.SetActive(visible);
        }
    }

    public void UpdateRespawnCounter(int respawnCount)
    {
        if (respawnCounterText != null)
        {
            respawnCounterText.text = respawnCount.ToString();
        }

        if (respawnAdButton != null)
        {
            respawnAdButton.gameObject.SetActive(respawnCount == 0);
        }
    }

    public void UpdateAdRemovalState(bool adsRemoved)
    {
        if (removeAdsButton != null)
        {
            removeAdsButton.gameObject.SetActive(!adsRemoved);
        }
    }
}
