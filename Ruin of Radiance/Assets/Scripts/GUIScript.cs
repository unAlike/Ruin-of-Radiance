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
    GameObject invPanel, mapPanel, sklPanel, hoverPanel;
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    PlayerStats stats;
    

    void Start()
    {
        EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        stats = GameObject.Find("Dynamic Sprite").GetComponent<PlayerStats>();
        invPanel = GameObject.Find("InventoryPanel");
        mapPanel = GameObject.Find("MapPanel");
        sklPanel = GameObject.Find("SkillTreePanel");
        hoverPanel = GameObject.Find("HoverPanel");
        hoverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateUIBars();
        if(hoverPanel.activeSelf){
            Vector3 pos = Input.mousePosition + new Vector3(3,3,0);
            pos.z = 20;
            hoverPanel.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
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
                // default:
                //     openGUI = false;
                //     break;
                
            }
        }
        else{
            openGUI = false;
        }
        
    }
    public void FixedUpdate(){
        if(openGUI){
            OpenGui();
        }
        else{
            CloseGui();
        }
    }
    public void OpenGui(){
        if(GameObject.Find("MenuPanel").transform.localPosition.y<140){
            GameObject.Find("MenuPanel").transform.localPosition = GameObject.Find("MenuPanel").transform.localPosition + new Vector3(0,5f,0);
        }
        
    }
    public void CloseGui(){
        if(GameObject.Find("MenuPanel").transform.localPosition.y>-35){
            GameObject.Find("MenuPanel").transform.localPosition = GameObject.Find("MenuPanel").transform.localPosition + new Vector3(0,-5f,0);
        }
    }

    public void enableHoverPanel(){
        hoverPanel.SetActive(true);
    }
    public void disableHoverPanel(){
        hoverPanel.SetActive(false);
    }

    public void setPanel(GameObject g){
        hoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().skillName;
        hoverPanel.gameObject.transform.GetChild(1).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().desc;
        hoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().currentPoints + "/" + g.GetComponent<SkillTreeButton>().maxPoints;
    }
    public void updateUIBars(){
        GameObject.Find("HealthValue").GetComponent<Text>().text = (stats.health + "/" + stats.maxHealth);
        GameObject.Find("MindEnergyValue").GetComponent<Text>().text = (stats.mindEnergy + "/" + stats.maxMindEnergy);
    }

}
