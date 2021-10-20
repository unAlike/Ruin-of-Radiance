using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio clip sound files


public class RatBehavior : MonoBehaviour {
    [SerializeField]
    private AudioClip[] ratNoises;
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
                ratSound.PlayOneShot(ratAttack);
            }
       } 
       else {
            int num= Random.Range(0,6);
            ratSound.PlayOneShot(ratNoises[num]);
       }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
