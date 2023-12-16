using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class UiHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _fuelBar;
    [SerializeField] private float _fuelBarHeight = 1.0f;
    [SerializeField] private float _fuelBarWidth = 1.0f;
    [SerializeField] private float _actualLengthOfBar = 10.0f;
    [SerializeField] private GameObject _collecableHandler;

    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] private Color[] _textColors;

    private Collectables _collectableScript;
    int _score=0;
    int _highScore = 0;
    Material[] _materials;




    void Start()
    {
        _collectableScript = _collecableHandler.GetComponent<Collectables>();
        _highScore = DataSaver.LaodScore();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateFuelBar();

        UpdateScore();

        UpdateHighScore();
        ChangeFontGlow();
        
        
    }

    public int GetScore(){
        return _score;
    }
    void UpdateFuelBar(){
        float _oldFuelBarPosX = _fuelBar.transform.position.x;
        float _oldFuelBarFillStatus = _fuelBar.transform.localScale.x;
        float _fuelLeft = _collectableScript.GetLeftFuel();
        
        if(_fuelLeft<0)
            return;
        _fuelBar.transform.localScale = new Vector3(_fuelBarWidth*_fuelLeft,0,_fuelBarHeight);
        float _newFuelBarFillStatus = _fuelBar.transform.localScale.x;
        float _newFuelBarPosX = _oldFuelBarPosX - _actualLengthOfBar * (_oldFuelBarFillStatus-_newFuelBarFillStatus)/2;
        _fuelBar.transform.position = new Vector3(_newFuelBarPosX, _fuelBar.transform.position.y,_fuelBar.transform.position.z);
        _materials = _fuelBar.GetComponent<MeshRenderer>().materials;
        if(_materials[0])
            _materials[0].SetFloat("_FuelLeft",_fuelLeft);
    }

    void UpdateScore(){
        _score = (int)_playerTransform.position.x / 10;
        _scoreText.SetText(_score.ToString());
        
    }

    void ChangeFontGlow(){
        if(_collectableScript.GetLeftFuel()<0)
            return;
        foreach(var i in _texts){
            float _fuelLeft = _collectableScript.GetLeftFuel();
            Color color = (1-_fuelLeft)*_textColors[0] + _fuelLeft*_textColors[1];
            i.fontMaterial.SetColor("_GlowColor",color);
        }
    }
    public void UpdateHighScore(){
        if(_score>_highScore){
            _highScore = _score;
            DataSaver.SaveScore(_highScore);
        }
        _highScoreText.SetText(_highScore.ToString());
        
    }

    public float GetLeftFuel(){
        return _collectableScript.GetLeftFuel();
    }
}
