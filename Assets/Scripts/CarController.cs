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

    [SerializeField] AudioSource bgmSound;
    [SerializeField] AudioSource startSound;
    [SerializeField] AudioSource idleSound;
    [SerializeField] AudioSource revForwardSound;
    [SerializeField] AudioSource revBackwardSound;

    [SerializeField] GameObject UIHandler;
    UiHandler uiHandlerScript;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cols = new Collider2D[10];
        lastFuelTime = Time.time;
        deathHandler = deathGameObject.GetComponent<DeathHandler>();
        bgmSound.playOnAwake = true;
        startSound.Play();
        uiHandlerScript = UIHandler.GetComponent<UiHandler>();

    }
    

    void FixedUpdate()
    {
        if(!startSound.isPlaying && !idleSound.isPlaying)
            idleSound.Play();
        
        if(rb != null)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                moveBackward();
            else if(Input.GetKey(KeyCode.RightArrow))
                moveForward();
            else{
                revBackwardSound.mute = true;
                revForwardSound.mute = true;
                SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
                gasingSprite.sprite = noGasSprite;
                SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
                breakingSprite.sprite = noBreakSprite;
            }
        }

        if((Input.GetKey(KeyCode.RightControl)||Input.GetKey(KeyCode.LeftControl))&&Input.GetKeyDown(KeyCode.UpArrow)){
            bgmSound.volume+=0.1f;
            uiHandlerScript.AddMessage("BGM volume: "+bgmSound.volume.ToString());
        }
        else if((Input.GetKey(KeyCode.RightControl)||Input.GetKey(KeyCode.LeftControl))&&Input.GetKeyDown(KeyCode.DownArrow)){
            bgmSound.volume-=0.1f;
            uiHandlerScript.AddMessage("BGM volume: "+bgmSound.volume.ToString());
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            startSound.volume+=0.1f;
            idleSound.volume+=0.1f;
            revBackwardSound.volume+=0.1f;
            revForwardSound.volume+=0.1f;

            uiHandlerScript.AddMessage("Sound Effect volume: "+revBackwardSound.volume.ToString());
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)){
            startSound.volume-=0.1f;
            idleSound.volume-=0.1f;
            revBackwardSound.volume-=0.1f;
            revForwardSound.volume-=0.1f;
            uiHandlerScript.AddMessage("Sound Effect volume: "+startSound.volume.ToString());
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
        if(revBackwardSound.isPlaying)
            revBackwardSound.mute = true;
        if(!revForwardSound.isPlaying){
            revForwardSound.mute = false;
            revForwardSound.Play();
        }
            
        SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
        breakingSprite.sprite = noBreakSprite;
        SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
        gasingSprite.sprite = gasSprite;

        rb.AddRelativeForce(new Vector2(force,0.0f));
    }

    private void moveBackward()
    {
        if(revForwardSound.isPlaying)
            revForwardSound.mute = true;
        if(!revBackwardSound.isPlaying){
            revBackwardSound.mute = false;
            revBackwardSound.Play();
        }
        SpriteRenderer breakingSprite = breakButton.GetComponent<SpriteRenderer>();
        breakingSprite.sprite = breakSprite;
        SpriteRenderer gasingSprite = gasButton.GetComponent<SpriteRenderer>();
        gasingSprite.sprite = noGasSprite;
        
        rb.AddRelativeForce(new Vector2(-force, 0.0f));
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
