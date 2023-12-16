using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private int _diamonds = 0;
    [SerializeField] private float _timePerFuel = 50;
    [SerializeField] private float _fuelSpawnMaxDistance = 0;
    [SerializeField] private float _noFuelZoneLeftRelative = 0f;
    [SerializeField] private float _noFuelZoneRightRelative = 0.25f;
    [SerializeField] private float _lastFuelPosition = 0;
    [SerializeField] private float _lastFuelTime;
    [SerializeField] private GameObject _player;

    [SerializeField] private GameObject _fuelPrefab;
    [SerializeField] private GameObject _terrain;
    [SerializeField] private GameObject _UI;
    GameObject _fuelTankObject;

    [SerializeField] private float _fuelVerticalOffset = -2f;

    CustomShape _customShape;
    CarController _carController;
    UiHandler _uiHandler;

    void Start(){
        _customShape = _terrain.GetComponent<CustomShape>();
        _carController = _player.GetComponent<CarController>();
        _uiHandler = _UI.GetComponent<UiHandler>();
        _lastFuelTime = Time.time;
        _lastFuelPosition = 0;
    }

    void Update()
    {
        _lastFuelPosition = _carController.GetLastFuelPosition();
        _lastFuelTime = _carController.GetLastFuelTime();
        
        if(_fuelTankObject==null){
            _lastFuelPosition+= Random.Range(_noFuelZoneLeftRelative*_fuelSpawnMaxDistance,_noFuelZoneRightRelative*_fuelSpawnMaxDistance );
            _fuelTankObject = Instantiate(_fuelPrefab,new Vector3(_lastFuelPosition,_customShape.F(_lastFuelPosition) + _fuelVerticalOffset,0),Quaternion.identity);

        }
    

    }


    public float GetLeftFuel(){
        float x = Time.time - _lastFuelTime;
        return -Mathf.Pow(x,2)/Mathf.Pow(_timePerFuel,2) + 1;
    }


}
