using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio clip sound files


public class RatBehavior : MonoBehaviour 
{
public AudioClip rat1;
public AudioClip rat2;
public AudioClip rat3;
public AudioClip rat4;
public AudioClip ratAttack;

// public AudioClip ratDialouge1;
// public AudioClip ratDialouge2;

//bool inCombat = false;
AudioSource ratSound;

    // Start is called before the first frame update
    void Start()
    {
        // plays sounds
        ratSound = GetComponent<AudioSource>();
         StartCoroutine(Countdown());
 }
 
 private IEnumerator Countdown()
 {
     while(true)
     {
        
         // Debug.Log("Sound Played");
         playRatSound();
         yield return new WaitForSeconds(10);
     }
 }
    void playRatSound() {
        bool inCombat = false;
       if(inCombat) {
        int num1 = Random.Range(0,1);
            if (num1 == 0) {
                ratSound.clip = ratAttack;
            }
       } 
       else {
            int num= Random.Range(0,6);
            if (num == 0) {
                ratSound.clip = rat1;
            }
            else if(num == 1 ) {
                ratSound.clip = rat2;
            }
            else if(num == 2 ) {
                ratSound.clip = rat3;            
            }
            else if(num == 3 ) {
                ratSound.clip = rat4;     
            }
            else {
                // do nothing
            }
       }
       // Debug.Log("Sound should be Played");
        ratSound.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
