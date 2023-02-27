using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("@KNIFE")] 
    [HideInInspector]
    public int activeKnifeIndex;
    public KnifePooler knifePooler; 
    public KnifeManager knifeManager;
    [HideInInspector]
    public GameObject activeKnife;
    [HideInInspector]
    public GameObject beforeActiveKnife;
    public GameObject[] playKnives;
    public int playKnivesCount;
    public bool isFire = true;
    [HideInInspector]
    public int activePlayKnifeIndex = 0;
    [Header("@ENEMY")] 
    public GameObject enemy;
    public AudioSource[] enemyAudios;
    public float enemyRotateSpeed;
    public GameObject applePs;
    public GameObject[] enemyParts;
    [Header("@UI")]
    public Texture greenPlayKnife;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI appleCountText;
    public int score;
    public int appleCount;
    public SpriteRenderer[] knifeSpriteRenderers;
    public static GameManager instance;
    public int levelIndex = 2;
    private void Awake()
    {
        instance = this;
    }

    
    private void Start()
    {
        
        if(!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", levelIndex);
        
        scoreText.text = score.ToString();
        appleCountText.text = PlayerPrefs.GetInt("AppleCount").ToString();
        
        var knife = knifePooler.knifes[activeKnifeIndex]; 
        knife.SetActive(true);
      
        
        for (int i = 0; i < playKnivesCount; i++)
        {
            playKnives[i].SetActive(true);
        }
        
        
       
            
    }

    
    private void Update()
    {
        FireKnife();
        enemy.transform.Rotate(new Vector3(0,0,1f) * (Time.deltaTime * enemyRotateSpeed));
        
        
    }

    

    public void FireKnife()
    {
        GetBeforeActiveKnife();
        
        activeKnife = knifePooler.knifes[activeKnifeIndex];
        activeKnife.SetActive(true);
        
            if (!knifeManager.inStoppingArea)
            {
                activeKnife.GetComponent<TrailRenderer>().enabled = false;
                if (isFire)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        activeKnife.GetComponent<Rigidbody2D>().velocity = Vector2.up * 30f;
                        activeKnife.GetComponent<TrailRenderer>().enabled = true;
                        enemyAudios[3].Play();
                        activeKnifeIndex++;
                
                    }
                }            
            }
        
        
    }

  

    public void ChangePlayKnifeColor()
    {
        playKnives[activePlayKnifeIndex].GetComponent<RawImage>().texture = greenPlayKnife;
        activePlayKnifeIndex++;
    }
    private void GetBeforeActiveKnife()
    {
        if (activeKnifeIndex > 0)
        {
            var beforeActifeKnifeIndex = activeKnifeIndex - 1;
            beforeActiveKnife = knifePooler.knifes[beforeActifeKnifeIndex];
        }
    }

    // public void GetAppleParticle(Transform t)
    // {
    //     Instantiate(applePs, t.position, Quaternion.identity);
    // }
}
