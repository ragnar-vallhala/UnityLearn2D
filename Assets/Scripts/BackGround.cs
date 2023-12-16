using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Material[] materials;
    [SerializeField] private float _speedOfBackGround = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        materials = gameObject.GetComponent<MeshRenderer>().materials;
        
    }

    void Update()
    {
        materials[0].SetColor("_PlayerPosition",new Color(gameObject.transform.position.x*_speedOfBackGround,0,0,0));        
    }
}
