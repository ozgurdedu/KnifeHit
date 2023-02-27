using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject levelFailPanel, topBar;
    public TextMeshProUGUI stageText;
    public static PanelManager instance;
    public SpriteRenderer background;

    public RectTransform levelFailMidBar, levelFailRestartButton;
    public TextMeshProUGUI scoreText, appleScoreText;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stageText.text = "STAGE " + SceneManager.GetActiveScene().name;
    }


    public void LevelFail()
    {
        levelFailPanel.SetActive(true);
        levelFailMidBar.DOAnchorPosY(1500f, 1f).From();
        levelFailRestartButton.DOAnchorPosY(-1500f, 1f).From();
        
        scoreText.text = GameManager.instance.score.ToString();
        appleScoreText.text = PlayerPrefs.GetInt("AppleCount").ToString();

    }
    
    
    
    #region ButtonClicks
    public void BackToHome()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
