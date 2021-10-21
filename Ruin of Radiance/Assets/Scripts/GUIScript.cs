using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    Button invBtn, mapBtn, sklBtn;
    EventSystem EventSystem;
    
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    void Start()
    {
        EventSystem = GameObject.Find("InventoryUI").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch(EventSystem.currentSelectedGameObject.name){
            case "InventoryButton":
                Debug.Log(EventSystem.currentSelectedGameObject.name);
                openGUI=true;
                break;
            case "MapButton":
                Debug.Log(EventSystem.currentSelectedGameObject.name);
                break;
            case "SkillButton":
                Debug.Log(EventSystem.currentSelectedGameObject.name);
                break;
            case null:
                break;
            
        }
        if(openGUI){
            OpenGui();
        }
        else{
            CloseGui();
        }
    }
    public void OpenGui(){
        GameObject.Find("MenuPanel").transform.position = GameObject.Find("MenuPanel").transform.position + new Vector3(0,.01f,0);
    }
    public void CloseGui(){

    }

}
