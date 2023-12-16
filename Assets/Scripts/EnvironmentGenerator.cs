using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] SpriteShapeController _spriteShapeController;
    [SerializeField] GameObject _player;
    private List<int> _points = new();
    [SerializeField] private int _length = 10;
    [SerializeField] private int _lastIntegralPosition;
    
    [SerializeField] float _smoothness = 1.0f;
    [SerializeField] float _xMultiplier = 1.0f;
    [SerializeField] float _bottom = 1.0f;
    private Vector3 _pos; 
    [SerializeField] private float a1 = 0.5f;
    [SerializeField] private float f1 = 2;
    [SerializeField] private float a2 = 2;
    [SerializeField] private float f2 = 0.1f;
    [SerializeField] private float a3 = 2;
    [SerializeField] private float f3 = 0.3f;

    void Start()
    {
        _lastIntegralPosition = (int)gameObject.transform.position.x;
        for(int i=0; i<_length;i++){
            _points.Add(_lastIntegralPosition - _length/2 + i);
        }
    }

    void OnValidate(){
        Start();
        GeneratePath();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(_player.transform.position.x,transform.position.y,transform.position.z);

        if(_lastIntegralPosition<(int)gameObject.transform.position.x){
            InsertEnd();
            _lastIntegralPosition++;
        }
            
        else if(_lastIntegralPosition>(int)gameObject.transform.position.x){
            InsertFront();
            _lastIntegralPosition--;
        }
            
        GeneratePath();

        
    }

    
    private float F(float x)
    {
        float ret = (float)(a1 * Math.Sin(f1 * x) + a2 * Math.Sin(Math.PI * f2 * x) + a3 * Math.Sin(Math.E * f3 * x));
        return ret;
    }


    private void GeneratePath(){

        _spriteShapeController.spline.Clear();
        for(int i=0;i<_length;i++){
            _pos = new Vector3(_points[i],F(_points[i]),transform.position.z);
            _spriteShapeController.spline.InsertPointAt(i,_pos);
            if(i!=0 && i!=_length-1){
                _spriteShapeController.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                _spriteShapeController.spline.SetLeftTangent(i,_smoothness*_xMultiplier*Vector3.left);
                _spriteShapeController.spline.SetRightTangent(i,_smoothness*_xMultiplier*Vector3.right);
            }
        }
        _spriteShapeController.spline.InsertPointAt(_length,new Vector3(_pos.x,_pos.y-_bottom,_pos.z));
        _spriteShapeController.spline.InsertPointAt(_length+1,new Vector3(_points[0],_pos.y-_bottom,_pos.z));
        
    }


    private void InsertFront(){
        for(int i=_length-1;i>0;i--){
           _points[i] =_points[i-1];
        }
       _points[0] = _points[0]-1;
    }

    private void InsertEnd(){
        for(int i=0;i<_length-1;i++){
           _points[i] =_points[i+1];
        }
       _points[_length-1] = _points[_length-1]+1;
    }
}
