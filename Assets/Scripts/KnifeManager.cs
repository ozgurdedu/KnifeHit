
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class KnifeManager : MonoBehaviour
{
     GameManager _gameManager;
     
     public bool inStoppingArea;
     public bool isAppleCollided = false;
    
     
     private void Start()
     {
         _gameManager = GameObject.FindObjectOfType<GameManager>();
         
         var i = PlayerPrefs.GetInt("KnifeIndex");
         GetComponent<SpriteRenderer>().sprite = _gameManager.knifeSpriteRenderers[i].sprite;
     }

     private void Update()
     {
         if(!inStoppingArea) 
             transform.Translate(Vector2.up * (5f * Time.deltaTime));
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
         if (collision.gameObject.CompareTag("Enemy"))
         {
             
             
             _gameManager.enemyAudios[0].Play();
             transform.SetParent(collision.transform);
             GetComponent<TrailRenderer>().enabled = false;
             UpdateScore();
             _gameManager.enemy.transform.DOShakePosition(.3f, .1f, 10, 3);
             gameObject.tag = "ChildKnife";
             _gameManager.ChangePlayKnifeColor();
             GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             if (_gameManager.playKnivesCount == _gameManager.activePlayKnifeIndex)
             {
                 Win();
             }
         }
         if (collision.gameObject.CompareTag("ChildKnife"))
         {
             _gameManager.enemyAudios[2].Play();
             ChangeBackgroundColor();
             _gameManager.enemyRotateSpeed = 0.0f;
             PanelManager.instance.topBar.SetActive(false);
             StopGameAfterAnim();
             PanelManager.instance.LevelFail();
             
         }

     }
     private void OnTriggerEnter2D(Collider2D col)
     {
        
       
         if (col.CompareTag("Stopping"))
         {
             inStoppingArea = true;
         }

         if (col.CompareTag("Apple"))
         {
            
             isAppleCollided = true; //ASLA TRUE OLMUYOR:: ANLAYAMADIM.
             var ps = Instantiate(_gameManager.applePs, transform.position, transform.rotation);
             ps.GetComponent<ParticleSystem>().Play();
             UpdateAppleScore();
             _gameManager.enemyAudios[1].Play();
             col.gameObject.SetActive(false);

         }
        
       
     }

     private void Win()
     {
         PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
         _gameManager.enemyAudios[4].Play();
         _gameManager.enemyRotateSpeed = 0.0f;
         
         var tr = _gameManager.enemy.transform;
         var children = tr.GetComponentsInChildren<Transform>().Where(t => t != tr);

         foreach (var child in children)
         {
             child.DOScale(Vector3.zero, .1f).OnComplete(() =>
             {
                 child.gameObject.SetActive(false);
                 _gameManager.enemy.SetActive(false);
                 foreach (var enemyPart in _gameManager.enemyParts)
                 {
                     enemyPart.SetActive(true);
                     var x = Random.Range(-3f, 3.1f);
                     var y = Random.Range(-7f, -1f);
                     var v = new Vector2(x, y);

                     if (enemyPart.tag == "Knife")
                     {
                         var i = PlayerPrefs.GetInt("KnifeIndex");
                         enemyPart.GetComponent<SpriteRenderer>().sprite = _gameManager.knifeSpriteRenderers[i].sprite;
                     }
                     
                     
                     enemyPart.transform.DOLocalMove(v, .5f).OnComplete(() =>
                     {
                         SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
                     });
                     enemyPart.transform.DOShakeRotation(.5f, 50f, 10, 10f);
                     
                     //enemyPart.GetComponent<Rigidbody2D>().AddForce(v * 50f);
                 }
                 _gameManager.isFire = false;
                 
                 


             });
         }
         
     }

     // public void NextStage()
     // {
     //     _gameManager.isFire = true; 
     //     _gameManager.stages[0].SetActive(false); 
     //     _gameManager.stages[1].SetActive(true);
     // }

   
     
     
     private static void ChangeBackgroundColor()
     {
         PanelManager.instance.background.DOColor(
             Color.black,
             .3f
         ).OnComplete(() =>
         {
             PanelManager.instance.background.DOColor(
                 Color.white,
                 .2f
             );
         });
     }

     private void StopGameAfterAnim()
     {
         
         var v = Random.Range(0, 2); // sağa veya sola savrulması için.
         _gameManager.beforeActiveKnife.transform.DOJump(
             new Vector3(v == 0 ? -0.8f : 0.8f, -3f, 0)
             , -1f, 1, 0.5f).OnComplete(() =>
         {
             _gameManager.isFire = false;
             _gameManager.beforeActiveKnife.transform.SetParent(null);
             _gameManager.beforeActiveKnife.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
             

         });
         _gameManager.beforeActiveKnife.GetComponent<TrailRenderer>().enabled = false;
         

     }

    private void UpdateScore()
    {
        _gameManager.score++;
        _gameManager.scoreText.text = _gameManager.score.ToString();
        PlayerPrefs.SetInt("Score",_gameManager.score + PlayerPrefs.GetInt("Score"));

    }       
    private void UpdateAppleScore()
    {
        _gameManager.appleCount += 2;
        _gameManager.appleCountText.text = PlayerPrefs.GetInt("AppleCount").ToString();
        PlayerPrefs.SetInt("AppleCount", _gameManager.appleCount + PlayerPrefs.GetInt("AppleCount"));
    }
}

