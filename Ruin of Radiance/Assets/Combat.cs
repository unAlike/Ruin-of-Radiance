using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim; 
    [SerializeField]
    public AudioClip plantAttack1;
    AudioSource plantSounds;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        plantSounds = GetComponent<AudioSource>();
        plantSounds.clip = plantAttack1;
    }

    // Update is called once per frame
    void Update()
    {
         {
               if(Input.GetKey("space"))
               {
                       anim.Play("characterFrontAttack");
                       plantSounds.Play();
               }
     }
    }
}
