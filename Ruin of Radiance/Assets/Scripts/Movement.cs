using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    GameObject staminaBar;
    private Animator anim;
    Tilemap tilemap;
    GridLayout grid;
    private AudioSource audioSource;
    private Rigidbody2D character;
    
    private Vector2 movement;
    [SerializeField]
    float moveSpeed = 1;
    Camera mainCamera;
    [SerializeField]
    public bool inCombat = false;
    bool enteringCombat = false;
    bool exitingCombat = false;
    float stamina = 100;
    float mag =0;
    // Start is called before the first frame update
    [SerializeField]
    public AudioClip[] walkDirt, walkConcrete;
    enum Material {DIRT,CONCRETE,CRYSTAL};
    Material groundMaterial;
    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        audioSource = GameObject.Find("CharacterAudioSource").GetComponent<AudioSource>();
        character = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        staminaBar = GameObject.FindGameObjectWithTag("Stamina");
        groundMaterial = Material.DIRT;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inCombat){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mag = Mathf.Sqrt(Mathf.Pow(movement.x,2) + Mathf.Pow(movement.y,2));
            if(Input.GetKey(KeyCode.LeftShift)){
                if(stamina>0){
                    stamina-=.1f;
                    movement*=2f;
                    Debug.Log("sprinting");
                }
            }
            else{
                if(stamina<100){
                    stamina+=.1f;
                }
            }
        }
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.magnitude);
    
    }

    void FixedUpdate(){
        staminaBar.transform.localScale = new Vector3(stamina/75,.1f,.1f);
        if(mag!=0) character.MovePosition(character.position + (movement * (1/mag)) * moveSpeed * Time.fixedDeltaTime);
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
        //Debug.Log("The Tile Name is '" + tilemap.GetTile(new Vector3Int((int)Mathf.Floor(character.position.x), (int)Mathf.Floor(character.position.y), 0)).ToString() + "' done");
        string str = tilemap.GetTile(new Vector3Int((int)Mathf.Floor(character.position.x), (int)Mathf.Floor(character.position.y), 0)).ToString();
        str = str.Substring(16,str.IndexOf(" ")-16);
        switch(str){
            case "0": case "1": case "2": case "3": case "4": case "5":case "6":  case "8": case "9": case "14": case "15": case "16":case "17": case "18": case "19":
                groundMaterial = Material.CONCRETE;
                break;
            case "11": case "12": case "13":
                groundMaterial = Material.DIRT;
                break;
            default:
                Debug.Log("AHH");
                break;
        }
        switch(groundMaterial){
            case Material.DIRT:
                audioSource.PlayOneShot(walkDirt[Random.Range(0,2)], .5f);
                break;
            case Material.CONCRETE:
                audioSource.PlayOneShot(walkConcrete[Random.Range(0,3)], .5f);
                break;
        }
    }
}
