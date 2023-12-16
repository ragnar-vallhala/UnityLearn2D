using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flappy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float velocity;
    [SerializeField] float force;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(velocity*Time.deltaTime,0,0) + gameObject.transform.position;

        if(Input.GetKey(KeyCode.Space)){
            animator.SetBool("Pushed",true);
            rb.AddForce(force*gameObject.transform.up);
        }
        else
            animator.SetBool("Pushed",false);
        
    }
}
