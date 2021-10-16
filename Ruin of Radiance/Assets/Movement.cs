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
    private AudioSource audio;
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
    // Start is called before the first frame update
    [SerializeField]
    public AudioClip[] walkDirt, walkConcrete;
    enum Material {DIRT,CONCRETE,CRYSTAL};
    Material groundMaterial;
    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        audio = GameObject.Find("CharacterAudioSource").GetComponent<AudioSource>();
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

            if(Input.GetKey(KeyCode.LeftShift)){
                if(stamina>0){
                    stamina-=.1f;
                    movement = movement*2;
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
        staminaBar.transform.localScale = new Vector3(stamina/75,.2f,.2f);


        character.MovePosition(character.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(enteringCombat){
            if(mainCamera.orthographicSize > 2.5) mainCamera.orthographicSize -= .1f;
            else enteringCombat = false;
        }
        if(exitingCombat){
            if(mainCamera.orthographicSize < 5) mainCamera.orthographicSize += .1f;
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
        Debug.Log("The Tile Name is '" + tilemap.GetTile(new Vector3Int((int)Mathf.Floor(character.position.x), (int)Mathf.Floor(character.position.y), 0)).ToString() + "' done");
        switch(tilemap.GetTile(new Vector3Int((int)Mathf.Floor(character.position.x), (int)Mathf.Floor(character.position.y), 0)).ToString()){
            case "UptownTileSheet_0 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_1 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_2 (UnityEngine.Tilemaps.Tile)": 
            case "UptownTileSheet_3 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_4 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_5 (UnityEngine.Tilemaps.Tile)":
            case "UptownTileSheet_6 (UnityEngine.Tilemaps.Tile)":  case "UptownTileSheet_8 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_9 (UnityEngine.Tilemaps.Tile)": 
            case "UptownTileSheet_14 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_15 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_16 (UnityEngine.Tilemaps.Tile)":
            case "UptownTileSheet_17 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_18 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_19 (UnityEngine.Tilemaps.Tile)":
                groundMaterial = Material.CONCRETE;
                break;
            case "UptownTileSheet_11 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_12 (UnityEngine.Tilemaps.Tile)": case "UptownTileSheet_13 (UnityEngine.Tilemaps.Tile)":
                groundMaterial = Material.DIRT;
                break;
            default:
                Debug.Log("AHH");
                break;
        }
        switch(groundMaterial){
            case Material.DIRT:
                audio.PlayOneShot(walkDirt[Random.Range(0,2)], .5f);
                break;
            case Material.CONCRETE:
                audio.PlayOneShot(walkConcrete[Random.Range(0,3)], .5f);
                break;
        }
    }
}
