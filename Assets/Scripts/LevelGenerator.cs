using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] SpriteShapeController _spriteShapeController;

    [SerializeField] float _delX = 0.5f;
    [SerializeField] float _lengthOfLevel = 5.0f;
    [SerializeField] Transform _playerTransform;
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

    private int _lastIntegralPosition = 0;
    private List<int> _positions = new();

    public void Awake(){

        int k = (int)transform.position.x-5;
        for(int i=0;i<10;i++){
            _positions.Add(k+i);
        }
    }
   public void OnValidate(){
       GeneratePath();   
   }

   public void FixedUpdate(){
        Debug.Log(5);
        transform.position = new Vector3(_playerTransform.position.x,transform.position.y,transform.position.z);
        if((int)transform.position.x!=_lastIntegralPosition){
            if((int)transform.position.x < _lastIntegralPosition){
                InsertFront(_positions[0]-1);
            }
            else if((int)transform.position.x > _lastIntegralPosition){
                InsertEnd(_positions[9]+1);
            }
        }
        string s = "Arr";
        for(int i=0;i<10;i++){
            s+=" >> ";
        }
        


        GeneratePath();


   }

    private float GenerateXPos(int index,float x){
        /*
        *   Apply a linear interpolation to find the value of position based on the players location
        *   If player location is x0
        *       length of the path is l
        *       distance between two points is h
        *   then number of points is l/h = n
        *   
        *   Fit a line d = ki + c
        *   here c = x0-l/2
        *        k = l/n
        *   also n = l/delX
        *   k = delX
        */
        float c = x - _lengthOfLevel/2;
        float k = _delX;
        return k*index + c;
    }
    
    private void GeneratePath(){

        _spriteShapeController.spline.Clear();
        int k = (int)transform.position.x-5;

        for(int i=0;i<50;i+=5){
            _pos = new Vector3(k+i,F(k+i),transform.position.z);
            _spriteShapeController.spline.InsertPointAt(i/5,_pos);
            if(i!=0 && i!=45){
                _spriteShapeController.spline.SetTangentMode(i/5,ShapeTangentMode.Continuous);
                _spriteShapeController.spline.SetLeftTangent(i/5,_smoothness*_xMultiplier*Vector3.left);
                _spriteShapeController.spline.SetRightTangent(i/5,_smoothness*_xMultiplier*Vector3.right);
            }
        }
        _spriteShapeController.spline.InsertPointAt(10,new Vector3(_pos.x,_pos.y-_bottom,_pos.z));
        _spriteShapeController.spline.InsertPointAt(11,new Vector3(k,_pos.y-_bottom,_pos.z));
        
    }




    private float F(float x)
    {
        float ret = (float)(a1 * Math.Sin(f1 * x) + a2 * Math.Sin(Math.PI * f2 * x) + a3 * Math.Sin(Math.E * f3 * x));
        return ret;
    }



    private void InsertFront(int num){
        for(int i=0;i<9;i++){
            _positions[i+1] = _positions[i];
        }
        _positions[0] = num;
    }

    private void InsertEnd(int num){
        for(int i=9;i>0;i--){
            _positions[i] = _positions[i-1];
        }
        _positions[9] = num;
    }
}
