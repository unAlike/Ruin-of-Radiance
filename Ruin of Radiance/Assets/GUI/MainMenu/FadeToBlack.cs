using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour
{
    // Start is called before the first frame update
    bool fade = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fade){
            gameObject.GetComponent<Image>().color = new Vector4(gameObject.GetComponent<Image>().color.r -.01f,gameObject.GetComponent<Image>().color.g -.01f,gameObject.GetComponent<Image>().color.b -.01f,255);
            GameObject.Find("StartButton").GetComponent<Image>().color = new Vector4(gameObject.GetComponent<Image>().color.r -.01f,gameObject.GetComponent<Image>().color.g -.01f,gameObject.GetComponent<Image>().color.b -.01f,255);
        }
        Debug.Log("Hi " + fade);
        if(gameObject.GetComponent<Image>().color.r <= 0){
            SceneManager.LoadScene("01_Tutorial House");

        }
    }
    public void StartFade(){
        fade = true;
    }
}
