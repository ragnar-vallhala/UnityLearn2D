using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]

public class CustomShape : MonoBehaviour
{

    [SerializeField] private GameObject car;

    private UInt64 sec = 0;


    MeshFilter meshFilter;
    Mesh mesh;
    PolygonCollider2D polyCollider;

    [SerializeField]private int nVert = 700;
    [SerializeField]private float h = 0.3f;

    [SerializeField] private float f3 = 0.3f;
    [SerializeField] private float f2 = 0.1f;
    [SerializeField] private float f1 = 2;

    [SerializeField] private float a1 = 0.5f;
    [SerializeField] private float a2 = 2;
    [SerializeField] private float a3 = 2;


    private static Vector3[] boxVertices;
    private static int[] boxTris;
    public void Start()
    {
        
        meshFilter = gameObject.GetComponent<MeshFilter>();

        mesh = new Mesh();
        
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();

        renderTriangle(0);

    }


    private void FixedUpdate()
    {
        Vector3 position = car.transform.position;

        gameObject.transform.position = new Vector3(position.x, gameObject.transform.position.y, gameObject.transform.position.z);


        if ((uint)Time.time - sec > 0)
        {
             


            sec++;
 
        }

        
        renderTriangle(position.x);
        
    }

    

    private void renderTriangle(float pos)
    {
        Vector3[] vertices = new Vector3[nVert];
        int[] tris = new int[3 * (nVert - 2)];


        {
            float xCoord = 0;
            for (int i = vertices.Length / 2; i >= 0; i--)
            {
                vertices[i] = new Vector3(xCoord, F(pos + xCoord), 0);
                xCoord -= h;
            }

            xCoord = 0;
            for (int i = vertices.Length / 2; i < nVert; i++)
            {
                vertices[i] = new Vector3(xCoord, F(pos + xCoord), 0);
                xCoord += h;
            }

        }

       
        

        {
            float thickness_grass = 1.0f;
            for (uint i = 0; i < vertices.Length; i += 2)
            {
                vertices[i] = new Vector3(vertices[i].x, vertices[i].y-thickness_grass, 0);
            }
            mesh.vertices = vertices;
            
        }
        
        

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        for (int i = 1; i < (nVert - 2)/2; i++)
        {
            int p = 6 * (i - 1) + 3;

            tris[p] = 2 * i;
            tris[p + 1] = 2 * i - 1;
            tris[p + 2] = 2 * i + 1;
            tris[p + 3] = 2 * i;
            tris[p + 4] = 2 * i + 1;
            tris[p + 5] = 2 * i + 2;
        }
        {
            int lenTris = tris.Length;
            tris[lenTris - 3] = nVert - 3;
            tris[lenTris - 2] = nVert - 2;
            tris[lenTris - 1] = nVert - 1;
        }

        mesh.triangles = tris;

        

        
           
        meshFilter.mesh = mesh;
       

        {
            Vector2[] colliderPathVert = new Vector2[vertices.Length];
            int counter = 0;
            for (int i = 0; i < vertices.Length; i += 2)
            {
                colliderPathVert[counter] = new Vector2(vertices[i].x, vertices[i].y);
                counter++;
            }

            for (int i = vertices.Length -1 ; i > 0 ; i -= 2)
            {
                colliderPathVert[counter] = new Vector2(vertices[i].x, vertices[i].y);
                counter++;
            }


            polyCollider.SetPath(0, colliderPathVert);
        }

        {
            Vector3[] baseBox;
            baseBox = vertices;
            float thickness_grass = 0.8f;
            for (uint i = 0; i < vertices.Length; i += 2)
            {
                baseBox [i] = new Vector3(vertices[i].x, -5.0f, 0);
                baseBox [i+1] = new Vector3(vertices[i+1].x, vertices[i+1].y-thickness_grass, 0);
                
            }
            boxVertices = baseBox;
            boxTris = tris;
        }
        

    }


    public static Vector3[] ReturnGroundVert(){

        return boxVertices;
    }
    
    public static int[] ReturnGroundTris(){

        return boxTris;
    }

    public float F(float x)
    {

        float ret = (float)(a1 * Math.Sin(f1 * x) + a2 * Math.Sin(Math.PI * f2 * x) + a3 * Math.Sin(Math.E * f3 * x));
        return ret;
    }


}
