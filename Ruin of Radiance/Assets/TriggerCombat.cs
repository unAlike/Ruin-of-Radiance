using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCombat : MonoBehaviour
{
    [SerializeField]
    Camera CombatCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("wall")) {

            // does nothing
            return;
        }

        if (collision.gameObject.CompareTag("enemy")) {
            Debug.Log("Combat Collision started yo.");

            
            CombatCam.orthographicSize = 2f;

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
    } }
