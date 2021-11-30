using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBackLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator transition;
    public float transitionTime = 1f;
    
    


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) ) {
        //    LoadNextLevel();
        //}
    }


    void OnTriggerEnter2D(Collider2D collision) {
        LoadLastLevel();
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadLastLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2));
    }

    public void LoadBossLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex ) {
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

}
