using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;


    void Start(){
        _scoreText.text = DataSaver.LaodTempScore().ToString();
        _highScoreText.text = DataSaver.LaodScore().ToString();
    }

    public void Quit(){
        Application.Quit();
    }

    public void Retry(){
        SceneManager.LoadScene(0);
    }

}
