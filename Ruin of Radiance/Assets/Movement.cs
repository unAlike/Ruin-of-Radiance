using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private Rigidbody2D character;
    private Vector2 movement;
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    public bool inCombat = false;
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
