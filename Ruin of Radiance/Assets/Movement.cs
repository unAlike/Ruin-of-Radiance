using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private Rigidbody2D character;
    private Vector2 movement;
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    Camera combatCam;
    [SerializeField]
    public bool inCombat = false;
    bool enteringCombat = false;
    bool exitingCombat = false;
    // Start is called before the first frame update
    [SerializeField]
    public AudioClip[] walkDirt, walkConcrete;
    enum Material {DIRT,CONCRETE,CRYSTAL};
    Material groundMaterial;
    void Start()
    {
        groundMaterial = Material.DIRT;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inCombat){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if(Input.GetKey(KeyCode.LeftShift)){
                Debug.Log("Shift");
                movement = movement*2;
            }
        }
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.magnitude);
    
    }

    void FixedUpdate(){
        character.MovePosition(character.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(enteringCombat){
            if(combatCam.orthographicSize > 2.5) combatCam.orthographicSize -= .1f;
            else enteringCombat = false;
        }
        if(exitingCombat){
            if(combatCam.orthographicSize < 5) combatCam.orthographicSize += .1f;
            else exitingCombat = false;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Hit something");
        if (collision.gameObject.CompareTag("wall")) {

            // does nothing
            return;
        }

        if (collision.gameObject.CompareTag("enemy")) {
            Debug.Log("Combat Collision started yo.");

            if(!enteringCombat && !inCombat){
                enteringCombat = true;
                inCombat = true;
                movement.x = 0;
                movement.y=0;
            }

            // toggle movement bool to false

            // run CameraToCombat();

            // play audio file basicFillerMusic.mp3

            // create grid for players to stand on
            // move character to grid location [1,2]
            // move enemy location to [7,2]


            // begin combat orientation

           // GameObject theThingWeHit = collision.gameObject;

           // ghostScriptTTH theScript = theTHingWeHit.getComponent<ghostScriptTTH>();

           // theScript.takeDamage();

        }
    } 
    void OnTriggerExit2D(Collider2D col){
        exitingCombat = true;
    }
    void playWalkSoundEffect(){
        switch(groundMaterial){
            case Material.DIRT:
                audio.PlayOneShot(walkDirt[Random.Range(0,2)], .5f);
                return;
            case Material.CONCRETE:
                audio.PlayOneShot(walkConcrete[Random.Range(0,3)], .5f);
                return;
        }
    }
}
