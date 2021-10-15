using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonBehavior : MonoBehaviour
{ public AudioClip raccoon1;

public AudioClip raccoon2;
public AudioClip raccoon3;
public AudioClip raccoon4;
public AudioClip raccoonAttack;

// public AudioClip ratDialouge1;
// public AudioClip ratDialouge2;

//bool inCombat = false;
AudioSource raccoonSound;

    // Start is called before the first frame update
    void Start()
    {
        // plays sounds
        
        // playPlantSound();
        // 
        raccoonSound = GetComponent<AudioSource>();
         StartCoroutine(Countdown());
 }
 
 private IEnumerator Countdown()
 {
     while(true)
     {
        
         Debug.Log("Raccoon sound Played");
         playRaccoonSound();
         yield return new WaitForSeconds(13);
     }
 }


    void playRaccoonSound() {
        bool inCombat = false;
       if(inCombat) {
        int num1 = Random.Range(0,1);
            if (num1 == 0) {
                raccoonSound.clip = raccoonAttack;
            }
       } 
       else {
            int num= Random.Range(0,6);
            if (num == 0) {
                raccoonSound.clip = raccoon1;
            }
            else if(num == 1 ) {
                raccoonSound.clip = raccoon2;
            }
            else if(num == 2 ) {
                raccoonSound.clip = raccoon3;            
            }
            else if(num == 3 ) {
                raccoonSound.clip = raccoon4;     
            }
            else {
                // do nothing
            }
       }
       // Debug.Log("Sound should be Played");
        raccoonSound.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
