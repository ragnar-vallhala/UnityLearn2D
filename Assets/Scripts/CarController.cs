using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 10.0f;
    [SerializeField] private float jumpFactor = 0.5f;
    [SerializeField] private float yOffsetFromCam = -2.0f;
    [SerializeField] private Vector2 offsetCamera = new Vector2(0, 0);
    [SerializeField] private GameObject gameCam;

    [SerializeField] private Sprite noGasSprite;
    [SerializeField] private Sprite gasSprite;
    [SerializeField] private Sprite noBreakSprite;
    [SerializeField] private Sprite breakSprite;
    [SerializeField] private GameObject breakButton;
    [SerializeField] private GameObject gasButton;

    private UInt16 objectsInContact = 0;
    private Collider2D[] cols;

    [SerializeField] private float lastFuelPosition = 0;
    [SerializeField] private float lastFuelTime;
    [SerializeField] private GameObject deathGameObject;
    private DeathHandler deathHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cols = new Collider2D[10];
        lastFuelTime = Time.time;
        deathHandler = deathGameObject.GetComponent<DeathHandler>();

    }
    

    void FixedUpdate()
    {
        
        if(rb != null)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                moveBackward();
            else if(Input.GetKey(KeyCode.RightArrow))
                moveForward();
            else{
                SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
                gasingSprite.sprite = noGasSprite;
                SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
                breakingSprite.sprite = noBreakSprite;
            }
            if (Input.GetKey(KeyCode.UpArrow))
                jump();
        }
        if (gameCam != null)
        {
            gameCam.transform.position = new Vector3( gameObject.transform.position.x,
                                                    gameObject.transform.position.y + yOffsetFromCam, 
                                                    gameCam.transform.position.z);
        }
        
        objectsInContact = (ushort)rb.GetContacts(cols);


    }
    private void moveForward()
    {
        SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
        breakingSprite.sprite = noBreakSprite;
        SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
        gasingSprite.sprite = gasSprite;

        rb.AddRelativeForce(new Vector2(force,0.0f));
    }

    private void moveBackward()
    {
        SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
        breakingSprite.sprite = breakSprite;
        SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
        gasingSprite.sprite = noGasSprite;
        
        rb.AddRelativeForce(new Vector2(-force, 0.0f));
    }

    private void jump()
    {
        
        if (objectsInContact > 0)
        {
            bool isJumpable = false;
            for(int i=0;i<objectsInContact; i++)
            {
                if (cols[i].tag.Equals("Ground"))
                {
                    isJumpable = true;
                    break;
                }
 
            }
            if(isJumpable)
                rb.AddForce(jumpFactor * force * gameObject.transform.up, ForceMode2D.Impulse);
            
        }
        
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Fuel")){
            lastFuelPosition = transform.position.x;
            lastFuelTime = Time.time;
            Destroy(collision.gameObject);
        }
    }

    public float GetLastFuelPosition(){
        return lastFuelPosition;
    }

    public float GetLastFuelTime(){
        return lastFuelTime;
    }

    
}
