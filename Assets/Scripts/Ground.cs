using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Ground : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    void Start(){

        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = new Mesh();

    }


    void FixedUpdate(){
        mesh.vertices =  CustomShape.ReturnGroundVert();
        mesh.triangles = CustomShape.ReturnGroundTris();
        meshFilter.mesh = mesh;
    }

}
