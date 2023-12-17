using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{

    private int _score = 0;
    private float _fuelLeft = 1;
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _player;

    private UiHandler _uiHandler;
   
    void Start()
    {
        _uiHandler = _gameUI.GetComponent<UiHandler>();
    }

    void Update(){

        _fuelLeft = _uiHandler.GetLeftFuel();
        if(_fuelLeft<=0.01){
            Die(0);
        }
            
    }
    void OnCollisionEnter2D(Collision2D collision){
        
        if(collision.gameObject.tag.CompareTo("Ground")==0){
           Die(1);
        }
    }
    void Die(int killedBy){
        if(!Directory.Exists(Path.Join(Directory.GetCurrentDirectory(),"Assets/Resources"))){
            Directory.CreateDirectory(Path.Join(Directory.GetCurrentDirectory(),"Assets/Resources"));
        }
        ScreenCapture.CaptureScreenshot("Assets/Resources/deathSnap.png");
        _score = _uiHandler.GetScore();
        DataSaver.SaveTempScore(_score);

        SceneManager.LoadScene(1);
    }

}
