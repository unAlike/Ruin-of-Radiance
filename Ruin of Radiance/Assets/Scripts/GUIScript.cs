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
    GameObject invPanel, mapPanel, sklPanel, hoverPanel, SkillPointText;
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    PlayerStats stats;
    int healthUpgradePoints, sheildPoints, lifestealPoints, healPoints, megaHealPoints = 0;
    int damagePoints, critPoints, creatureCritPoints, slashPoints, sporeBombPoints = 0;
    int mindEnergyPoints, spawnPoints, recallPoints, boostedSpawnPoints, flipPoints = 0;
    

    void Start()
    {
        EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        stats = GameObject.Find("Character").GetComponent<PlayerStats>();
        invPanel = GameObject.Find("InventoryPanel");
        mapPanel = GameObject.Find("MapPanel");
        sklPanel = GameObject.Find("SkillTreePanel");
        hoverPanel = GameObject.Find("HoverPanel");
        SkillPointText = GameObject.Find("SkillPointText");
        hoverPanel.SetActive(false);
        GameObject.Find("HealthBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("DamageBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("MindBtn1").GetComponent<SkillTreeButton>().unlocked = true;
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
        if(!g.GetComponent<SkillTreeButton>().unlocked){
            hoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            hoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().color = Color.red;
        } 
        else {
            hoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            hoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().color = Color.green;
        }
        hoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().skillName;
        hoverPanel.gameObject.transform.GetChild(1).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().desc;
        hoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().currentPoints + "/" + g.GetComponent<SkillTreeButton>().maxPoints;
    }
    public void updateUIBars(){
        GameObject.Find("HealthValue").GetComponent<Text>().text = (stats.health + "/" + stats.maxHealth);
        GameObject.Find("Health Bar").GetComponent<Slider>().maxValue = stats.maxHealth;
        GameObject.Find("Health Bar").GetComponent<Slider>().value = stats.health;

        if(stats.skillPoints==0) SkillPointText.GetComponent<Text>().color = Color.red;
        else SkillPointText.GetComponent<Text>().color = Color.green;
        SkillPointText.GetComponent<Text>().text = "Skill Points: " + stats.skillPoints;

        GameObject.Find("MindEnergyValue").GetComponent<Text>().text = (stats.mindEnergy + "/" + stats.maxMindEnergy);
        GameObject.Find("Mind Energy Bar").GetComponent<Slider>().maxValue = stats.maxMindEnergy;
        GameObject.Find("Mind Energy Bar").GetComponent<Slider>().value = stats.mindEnergy;
    }
    public void upgradeSkillTree(GameObject obj){
        SkillTreeButton sklBtn = obj.GetComponent<SkillTreeButton>();
        if(GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints>0){
            if(sklBtn.currentPoints<sklBtn.maxPoints && sklBtn.unlocked){
                switch(sklBtn.name){
                    case "HealthBtn1":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("Character").GetComponent<PlayerStats>().maxHealth += 5;
                        GameObject.Find("Character").GetComponent<PlayerStats>().health += 5;
                        GameObject.Find("HealthBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("HealthBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        healthUpgradePoints++;
                        break;
                    case "HealthBtn11":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("HealthBtn12").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        sheildPoints++;
                        break;
                    case "HealthBtn12":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        lifestealPoints++;
                        break;
                    case "HealthBtn21":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("HealthBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        healPoints++;
                        break;
                    case "HealthBtn22":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        megaHealPoints++;
                        break;
                    case "DamageBtn1":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("DamageBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("DamageBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        damagePoints++;
                        break;
                    case "DamageBtn11":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("DamageBtn12").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        critPoints++;
                        break;
                    case "DamageBtn12":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        creatureCritPoints++;
                        break;
                    case "DamageBtn21":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("DamageBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        slashPoints++;
                        break;
                    case "DamageBtn22":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        sporeBombPoints++;
                        break;
                    case "MindBtn1":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("Character").GetComponent<PlayerStats>().maxMindEnergy += 5;
                        GameObject.Find("Character").GetComponent<PlayerStats>().mindEnergy += 5;
                        GameObject.Find("MindBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("MindBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        mindEnergyPoints++;
                        break;
                    case "MindBtn11":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("MindBtn12").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        spawnPoints++;
                        break;
                    case "MindBtn12":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        recallPoints++;
                        break;
                    case "MindBtn21":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        GameObject.Find("MindBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        boostedSpawnPoints++;
                        break;
                    case "MindBtn22":
                        GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints--;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        flipPoints++;
                        break;
                    default:
                        break; 
                }
            }
        }
        
        setPanel(obj);

    }

}
