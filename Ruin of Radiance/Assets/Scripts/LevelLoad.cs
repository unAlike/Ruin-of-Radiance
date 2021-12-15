using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    string toSceneName;
    [SerializeField]
    Vector2 pos;
    [SerializeField]
    AudioClip clip;
    public Animator transition;
    bool switching = false;
    [SerializeField]
    AudioClip currentMusic;
    void Start(){

    }
    void OnTriggerEnter2D(Collider2D collision) {
        if(!switching){
            StartCoroutine("LoadLevel");
            switching = true;
        }
    }
    IEnumerator LoadLevel() {
        transition.SetTrigger("StartCrossfade");
        Debug.Log("Loading Scene #$##############");
        yield return new WaitForSeconds(1.5f); 
        GameObject.Find("Character").transform.position = new Vector3(pos.x,pos.y, 0);
        GameObject.Find("Character").transform.Find("Music").gameObject.GetComponent<AudioSource>().clip = currentMusic;
        GameObject.Find("Character").transform.Find("Music").gameObject.GetComponent<AudioSource>().Play();
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(toSceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone){
            yield return null;
        }
        
        
       
        SceneManager.MoveGameObjectToScene(GameObject.Find("Character"),SceneManager.GetSceneByName(toSceneName));
        
        SceneManager.UnloadSceneAsync(currentScene);
    }



}
