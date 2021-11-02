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
    GameObject invPanel, mapPanel, sklPanel;
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        invPanel = GameObject.Find("InventoryPanel");
        mapPanel = GameObject.Find("MapPanel");
        sklPanel = GameObject.Find("SkillTreePanel");
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.currentSelectedGameObject){
            openGUI=true;
            switch(EventSystem.currentSelectedGameObject.name){
                case "InventoryButton":
                    Debug.Log(EventSystem.currentSelectedGameObject.name);
                    invPanel.SetActive(true);
                    mapPanel.SetActive(false);
                    sklPanel.SetActive(false);
                    break;
                case "MapButton":
                    Debug.Log(EventSystem.currentSelectedGameObject.name);
                    invPanel.SetActive(false);
                    mapPanel.SetActive(true);
                    sklPanel.SetActive(false);
                    break;
                case "SkillTreeButton":
                    Debug.Log(EventSystem.currentSelectedGameObject.name);
                    invPanel.SetActive(false);
                    mapPanel.SetActive(false);
                    sklPanel.SetActive(true);
                    break;
                case null:
                    openGUI=false;
                    break;
                default:
                    openGUI = false;
                    break;
                
            }
        }
        else{
            openGUI = false;
        }
        if(openGUI){
            OpenGui();
        }
        else{
            CloseGui();
        }
    }
    public void OpenGui(){
        if(GameObject.Find("MenuPanel").transform.localPosition.y<140){
            GameObject.Find("MenuPanel").transform.localPosition = GameObject.Find("MenuPanel").transform.localPosition + new Vector3(0,1f,0);
        }
        
    }
    public void CloseGui(){
        if(GameObject.Find("MenuPanel").transform.localPosition.y>-35){
            GameObject.Find("MenuPanel").transform.localPosition = GameObject.Find("MenuPanel").transform.localPosition + new Vector3(0,-1f,0);
        }
    }

}
