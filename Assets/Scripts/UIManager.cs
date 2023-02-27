using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AudioSource[] audios;
    [Header("@@ MAIN MENU")]
    public RectTransform knifeText;
    public RectTransform knifeImage;
    public RectTransform hitText;
    public RectTransform playButton;
    public RectTransform currentKnifeImg;
    public TextMeshProUGUI appleCount, scoreText;
    public GameObject mainPanel;
    public TextMeshProUGUI stageText;
    [Header("@@ SETTINGS")]
    public GameObject[] icons;
    public Sprite onButtonImage, offButtonImage;
    public GameObject settingsPanel;
    public TextMeshProUGUI settingsAppleCountText;

    [Header("@@ SELECT KNIFE")]
    public GameObject selectKnifePanel;

    public TextMeshProUGUI skAppleCount;
    public SpriteRenderer selectedKnifeSpriteRenderer;
    public SpriteRenderer currentKnifeSpriteRenderer;
    public RawImage selectedKnifeImage;
    public RawImage currentKnifeImage;
    
    public RawImage[] knifeRawImages;
    public SpriteRenderer[] knifeSpritesRenderers;

    

    void Start()
    {
        MainMenuAnims();

        selectedKnifeImage.transform.DOShakePosition(1f, 5,10,5).SetLoops(-1, LoopType.Yoyo);
        skAppleCount.text = PlayerPrefs.GetInt("AppleCount").ToString();
    }

    private void MainMenuAnims()
    {
        appleCount.text = PlayerPrefs.GetInt("AppleCount").ToString();
        scoreText.text = "SCORE " + PlayerPrefs.GetInt("Score");
        
        
        stageText.text = "STAGE " + (
            SceneManager.GetActiveScene().buildIndex == 0 
            ? PlayerPrefs.GetInt("Level") + 1
            : PlayerPrefs.GetInt("Level") - 1
            );
        settingsAppleCountText.text = (PlayerPrefs.GetInt("AppleCount")).ToString();
        
        playButton.DOScale(Vector3.zero, 1f).From();
        knifeText.DOAnchorPosX(-900f, 0.5f).From();
        hitText.DOAnchorPosX(900f, 0.5f).From();
        currentKnifeImg.DOAnchorPosY(-1200, 1f).From();

        knifeImage.DOAnchorPosX(-2000f, 1f).From().OnComplete(() =>
        {
            knifeImage.DOShakePosition(1f, 10f, 5, 50).SetLoops(-1, LoopType.Incremental);
        });

        var knifeIndex = PlayerPrefs.GetInt("KnifeIndex");
        currentKnifeImage.texture = knifeRawImages[knifeIndex].texture;
        currentKnifeSpriteRenderer.sprite = knifeSpritesRenderers[knifeIndex].sprite;
    }
    
    private void FixedUpdate()
    {
        OnOffIconsAttrs();
    }

    private void OnOffIconsAttrs()
    {
        foreach (var icon in icons)
        {
            switch (icon.name)
            {
                case "OnOff1":
                    if (icon.GetComponent<Image>().sprite == offButtonImage)
                        AudioListener.volume = 0;
                    else AudioListener.volume = 1;
                    break;
                case "OnOff2":
                    if (icon.GetComponent<Image>().sprite == offButtonImage)
                        return;
                    else Handheld.Vibrate();
                    break;
                // farklı sahneden nasıl obje alabilirim bilemedim? 
                // case "OnOff3":
                //     if (icon.GetComponent<Image>().sprite == offButtonImage)
                //         Debug.Log("damn");
                //     else{
                //         Debug.Log(GameManager.instance.score);};
                //     break;
            }
        }
    }

    #region Button Clicks
    public void OnClickButton(RectTransform rt) //play button
    {
        rt.DOScale(rt.localScale * 1.15f, .3f).SetLoops(2, LoopType.Yoyo);
        SceneManager.LoadScene(
             SceneManager.GetActiveScene().buildIndex == 0 
                 ? PlayerPrefs.GetInt("Level") + 1
                 : PlayerPrefs.GetInt("Level") - 1
            );
    }

    public void OnClickSettingsButton(bool isOpen)
    {
        switch (isOpen)
        {
            case true:
                settingsPanel.SetActive(true);
                mainPanel.SetActive(false);
                audios[0].Play();
                break;
            case false: 
                settingsPanel.SetActive(false);
                mainPanel.SetActive(true);
                audios[0].Play();
                break;
        }
    }
    
    public void OnClickKnifeSelectButtons(bool isOpen)
    {
        switch (isOpen)
        {
            case true:
                selectKnifePanel.SetActive(true);
                settingsPanel.SetActive(false);
                mainPanel.SetActive(false);
                audios[0].Play();
                break;
            case false: 
                selectKnifePanel.SetActive(false);
                mainPanel.SetActive(true);
                audios[0].Play();
                break;
        }
    }
    
    public void OnClickOnOffButton(Image i)
    {
        if (i.sprite == offButtonImage)
            i.transform.DOShakeScale(0.1f, 0.2f, 1, 2).OnComplete(() =>
            {
                audios[1].Play();
                i.sprite = onButtonImage;
            });
        else
            i.transform.DOShakeScale(0.1f, 0.2f, 1, 2).OnComplete(() =>
            {
                audios[1].Play();
                i.sprite = offButtonImage;
            });
    }


    public void OnClickSelectKnifeImage(RawImage ri)
    {
       
        
        audios[2].Play();
        for (int i = 0; i < knifeSpritesRenderers.Length; i++)
        {
            if (knifeSpritesRenderers[i].sprite.name == ri.texture.name)
            {
                PlayerPrefs.SetInt("KnifeIndex", i);
            }
        }

        var knifeIndex = PlayerPrefs.GetInt("KnifeIndex");
        
        var knifeRawImage = knifeRawImages[knifeIndex];
        var knifeSpriteRenderer = knifeSpritesRenderers[knifeIndex];

        selectedKnifeImage.texture = knifeRawImage.texture;
        selectedKnifeSpriteRenderer.sprite = knifeSpriteRenderer.sprite;
        currentKnifeImage.texture = knifeRawImage.texture;
        currentKnifeSpriteRenderer.sprite = knifeSpriteRenderer.sprite;


    }
    
    
    
    #endregion
}
