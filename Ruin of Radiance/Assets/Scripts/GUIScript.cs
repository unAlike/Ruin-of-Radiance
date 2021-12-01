using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject invBtn, mapBtn, sklBtn;
    EventSystem EventSystem;
    GameObject invPanel, mapPanel, sklPanel, hoverPanel, SkillPointText;
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    PlayerStats stats;
    int healthUpgradePoints, sheildPoints, lifestealPoints, healPoints, megaHealPoints = 0;
    int damagePoints, critPoints, creatureCritPoints, slashPoints, sporeBombPoints = 0;
    int mindEnergyPoints, spawnPoints, recallPoints, boostedSpawnPoints, flipPoints = 0;
    Movement movement;
    

    void Start()
    {
        EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        stats = GameObject.Find("Character").GetComponent<PlayerStats>();
        invPanel = GameObject.Find("InventoryPanel");
        mapPanel = GameObject.Find("MapPanel");
        sklPanel = GameObject.Find("SkillTreePanel");
        sklBtn = GameObject.Find("SkillTreeButton");
        mapBtn = GameObject.Find("MapButton");
        hoverPanel = GameObject.Find("HoverPanel");
        SkillPointText = GameObject.Find("SkillPointText");
        hoverPanel.SetActive(false);
        GameObject.Find("HealthBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("DamageBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("MindBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        movement = GameObject.Find("Character").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.inCombat){
            sklBtn.SetActive(false);
            mapBtn.SetActive(false);
            setInventoryCreatureButtons(true);
        }
        else{
            sklBtn.SetActive(true);
            mapBtn.SetActive(true);
            setInventoryCreatureButtons(false);
        }
        updateUIBars();
        updateCreatureCounts();
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
        PlayerStats stats = GameObject.Find("Character").GetComponent<PlayerStats>();
        if(GameObject.Find("Character").GetComponent<PlayerStats>().skillPoints>0){
            if(sklBtn.currentPoints<sklBtn.maxPoints && sklBtn.unlocked){
                switch(sklBtn.name){
                    case "HealthBtn1":
                        stats.skillPoints--;
                        stats.maxHealth += 5;
                        stats.health += 5;
                        GameObject.Find("HealthBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("HealthBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("HealthBtn11").GetComponent<Button>().interactable = true;
                        GameObject.Find("HealthBtn21").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        healthUpgradePoints++;
                        break;
                    case "HealthBtn11":
                        stats.skillPoints--;
                        stats.shieldRate += 5;
                        GameObject.Find("HealthBtn12").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("HealthBtn12").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        sheildPoints++;
                        break;
                    case "HealthBtn12":
                        stats.skillPoints--;
                        stats.lifestealRate += 5;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        lifestealPoints++;
                        break;
                    case "HealthBtn21":
                        stats.skillPoints--;
                        stats.hasHeal=true;
                        stats.healPower+=1;
                        GameObject.Find("HealthBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("HealthBtn22").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        healPoints++;
                        break;
                    case "HealthBtn22":
                        stats.skillPoints--;
                        stats.hasMegaHeal=true;
                        stats.megaHealPower+=1;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        megaHealPoints++;
                        break;
                    case "DamageBtn1":
                        stats.skillPoints--;
                        stats.damage += 5;
                        GameObject.Find("DamageBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("DamageBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("DamageBtn11").GetComponent<Button>().interactable = true;
                        GameObject.Find("DamageBtn21").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        damagePoints++;
                        break;
                    case "DamageBtn11":
                        stats.skillPoints--;
                        stats.critRate +=.05f;
                        GameObject.Find("DamageBtn12").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("DamageBtn12").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        critPoints++;
                        break;
                    case "DamageBtn12":
                        stats.skillPoints--;
                        stats.creatureCritRate += .05f;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        creatureCritPoints++;
                        break;
                    case "DamageBtn21":
                        stats.skillPoints--;
                        stats.hasSlash = true;
                        stats.slashDamage += 5;
                        GameObject.Find("DamageBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("DamageBtn22").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        slashPoints++;
                        break;
                    case "DamageBtn22":
                        stats.skillPoints--;
                        stats.hasSporeBomb=true;
                        stats.sporeDamage += 5;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        sporeBombPoints++;
                        break;
                    case "MindBtn1":
                        stats.skillPoints--;
                        stats.maxMindEnergy += 5;
                        stats.mindEnergy += 5;
                        GameObject.Find("MindBtn11").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("MindBtn21").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("MindBtn11").GetComponent<Button>().interactable = true;
                        GameObject.Find("MindBtn21").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        mindEnergyPoints++;
                        break;
                    case "MindBtn11":
                        stats.skillPoints--;
                        stats.spawnCostReduction+=1;
                        GameObject.Find("MindBtn12").GetComponent<SkillTreeButton>().unlocked=true;
                        GameObject.Find("MindBtn12").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().unlocked = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        spawnPoints++;
                        break;
                    case "MindBtn12":
                        stats.skillPoints--;
                        stats.recallCostReduction+=1;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        recallPoints++;
                        break;
                    case "MindBtn21":
                        stats.skillPoints--;
                        stats.hasBoostedSpawn=true;
                        stats.boostedSpawnLevel+=1;
                        GameObject.Find("MindBtn22").GetComponent<SkillTreeButton>().unlocked = true;
                        GameObject.Find("MindBtn22").GetComponent<Button>().interactable = true;
                        obj.GetComponent<SkillTreeButton>().currentPoints++;
                        boostedSpawnPoints++;
                        break;
                    case "MindBtn22":
                        stats.skillPoints--;
                        stats.hasFlip=true;
                        stats.flipLevel+=1;
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
    public void setSelectedUnit(int type){
        if(movement.inCombat){
            if(stats.hasUnits((Enums.Enemy)type)) stats.selectedType = (Enums.Enemy)type;
            GameObject.Find("Character").gameObject.transform.parent.transform.parent.gameObject.GetComponent<CombatLogic>().summonCreatureHighlight();
        }
    }
    public void setInventoryCreatureButtons(bool enabled){
        if(invPanel.activeSelf){
            if(stats.numOfRats<=0) GameObject.Find("Rat").GetComponent<Button>().enabled = false;
            else GameObject.Find("Rat").GetComponent<Button>().enabled = enabled;

            if(stats.numOfPigeons<=0) GameObject.Find("Pigeon").GetComponent<Button>().enabled = false;
            else GameObject.Find("Pigeon").GetComponent<Button>().enabled = enabled;

            if(stats.numOfRaccoons<=0) GameObject.Find("Raccoon").GetComponent<Button>().enabled = false;
            else GameObject.Find("Raccoon").GetComponent<Button>().enabled = enabled;

            if(stats.numOfFalcons<=0) GameObject.Find("Falcon").GetComponent<Button>().enabled = false;
            else GameObject.Find("Falcon").GetComponent<Button>().enabled = enabled;

            if(stats.numOfBoars<=0) GameObject.Find("Boar").GetComponent<Button>().enabled = false;
            else GameObject.Find("Boar").GetComponent<Button>().enabled = enabled;

            if(stats.numOfWateringCans<=0) GameObject.Find("Watering Can").GetComponent<Button>().enabled = false;
            else GameObject.Find("Watering Can").GetComponent<Button>().enabled = enabled;

            if(stats.numOfCrystals<=0) GameObject.Find("Crystal").GetComponent<Button>().enabled = false;   
            else GameObject.Find("Crystal").GetComponent<Button>().enabled = enabled;
        }
        
    }
    public void updateCreatureCounts(){
        if(invPanel.activeSelf){
            GameObject.Find("Rat").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfRats;
            GameObject.Find("Pigeon").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfPigeons;
            GameObject.Find("Raccoon").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfRaccoons;
            GameObject.Find("Falcon").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfFalcons;
            GameObject.Find("Boar").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfBoars;
            GameObject.Find("Watering Can").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfWateringCans;
            GameObject.Find("Crystal").transform.GetChild(0).GetComponent<Text>().text = "x" + stats.numOfCrystals;

            if(stats.numOfRats<=0) GameObject.Find("Rat").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Rat").GetComponent<Image>().color = new Color(255,255,255,255);

            if(stats.numOfPigeons<=0) GameObject.Find("Pigeon").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Pigeon").GetComponent<Image>().color = new Color(255,255,255,255);

            if(stats.numOfRaccoons<=0) GameObject.Find("Raccoon").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Raccoon").GetComponent<Image>().color = new Color(255,255,255,255);
            
            if(stats.numOfFalcons<=0) GameObject.Find("Falcon").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Falcon").GetComponent<Image>().color = new Color(255,255,255,255);

            if(stats.numOfBoars<=0) GameObject.Find("Boar").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Boar").GetComponent<Image>().color = new Color(255,255,255,255);

            if(stats.numOfWateringCans<=0) GameObject.Find("Watering Can").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Watering Can").GetComponent<Image>().color = new Color(255,255,255,255);

            if(stats.numOfCrystals<=0) GameObject.Find("Crystal").GetComponent<Image>().color = new Color(255,255,255,0);
            else GameObject.Find("Crystal").GetComponent<Image>().color = new Color(255,255,255,255);
        }
    }

}
