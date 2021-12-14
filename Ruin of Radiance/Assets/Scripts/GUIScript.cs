using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject invBtn, mapBtn, sklBtn, questBtn,abilBtn;
    EventSystem EventSystem;
    GameObject invPanel, mapPanel, sklPanel, SkillTreeHoverPanel, SkillPointText, questPanel, abilitiesPanel, AbilitiesHoverPanel, MapHoverPanel;
    string activeBtn;
    [SerializeField]
    bool openGUI = false;
    PlayerStats stats;
    int healthUpgradePoints, sheildPoints, lifestealPoints, healPoints, megaHealPoints = 0;
    int damagePoints, critPoints, creatureCritPoints, slashPoints, sporeBombPoints = 0;
    int mindEnergyPoints, spawnPoints, recallPoints, boostedSpawnPoints, flipPoints = 0;
    Movement movement;
    [SerializeField]
    List<Quest> quests = new List<Quest>();
    

    void Start()
    {
        EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        stats = GameObject.Find("Character").GetComponent<PlayerStats>();

        invPanel = GameObject.Find("InventoryPanel");
        mapPanel = GameObject.Find("MapPanel");
        sklPanel = GameObject.Find("SkillTreePanel");
        questPanel = GameObject.Find("QuestPanel");
        abilitiesPanel = GameObject.Find("AbilitiesPanel");

        sklBtn = GameObject.Find("SkillTreeButton");
        questBtn = GameObject.Find("QuestButton");
        mapBtn = GameObject.Find("MapButton");
        abilBtn = GameObject.Find("AbilitiesButton");

        SkillTreeHoverPanel = GameObject.Find("SkillTreeHoverPanel");
        AbilitiesHoverPanel = GameObject.Find("AbilitiesHoverPanel");
        MapHoverPanel = GameObject.Find("MapHoverPanel");
        SkillPointText = GameObject.Find("SkillPointText");

        abilitiesPanel.SetActive(false);
        
        abilBtn.SetActive(false);

        SkillTreeHoverPanel.SetActive(false);
        AbilitiesHoverPanel.SetActive(false);
        MapHoverPanel.SetActive(false);

        GameObject.Find("HealthBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("DamageBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        GameObject.Find("MindBtn1").GetComponent<SkillTreeButton>().unlocked = true;
        movement = GameObject.Find("Character").GetComponent<Movement>();
        PopulateQuests();
    }

    // Update is called once per frame
    void Update()
    {
        if(movement){
            if(movement.inCombat){
                sklBtn.SetActive(false);
                mapBtn.SetActive(false);
                questBtn.SetActive(false);
                abilitiesPanel.SetActive(true);
                abilBtn.SetActive(true);
                setInventoryCreatureButtons(true);
            }
            else{
                sklBtn.SetActive(true);
                mapBtn.SetActive(true);
                questBtn.SetActive(true);
                setInventoryCreatureButtons(false);
                abilitiesPanel.SetActive(false);
                abilBtn.SetActive(false);
            }
        }
        else{
            movement = GameObject.Find("Character").GetComponent<Movement>();
        }
        updateUIBars();
        updateCreatureCounts();
        if(SkillTreeHoverPanel.activeSelf){
            Vector3 pos = Input.mousePosition + new Vector3(3,3,0);
            pos.z = 20;
            SkillTreeHoverPanel.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
        if(AbilitiesHoverPanel.activeSelf){
            Vector3 pos = Input.mousePosition + new Vector3(3,3,0);
            pos.z = 20;
            AbilitiesHoverPanel.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
        if(MapHoverPanel.activeSelf){
            Vector3 pos = Input.mousePosition + new Vector3(3,3,0);
            pos.z = 20;
            MapHoverPanel.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
        if(EventSystem.currentSelectedGameObject){
            openGUI=true;
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

    public void enableSkillTreeHoverPanel(){
        SkillTreeHoverPanel.SetActive(true);
    }
    public void disableSkillTreeHoverPanel(){
        SkillTreeHoverPanel.SetActive(false);
    }
    public void enableAbilitiesHoverPanel(){
        AbilitiesHoverPanel.SetActive(true);
    }
    public void disableAbilitiesHoverPanel(){
        AbilitiesHoverPanel.SetActive(false);
    }
    public void enableMapHoverPanel(){
        MapHoverPanel.SetActive(true);
    }
    public void disableMapHoverPanel(){
        MapHoverPanel.SetActive(false);
    }

    public void setSkillTreePanel(GameObject g){
        if(!g.GetComponent<SkillTreeButton>().unlocked){
            SkillTreeHoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            SkillTreeHoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().color = Color.red;
        } 
        else {
            SkillTreeHoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            SkillTreeHoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().color = Color.green;
        }
        SkillTreeHoverPanel.gameObject.transform.GetChild(0).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().skillName;
        SkillTreeHoverPanel.gameObject.transform.GetChild(1).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().desc;
        SkillTreeHoverPanel.gameObject.transform.GetChild(2).GetComponent<Text>().text = g.GetComponent<SkillTreeButton>().currentPoints + "/" + g.GetComponent<SkillTreeButton>().maxPoints;
    }
    public void setMapPanel(GameObject g){
        switch(g.name){
            case "StarterHouse":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Starter House";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Tutorial";
                break;
            case "Uptown":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Uptown";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "The domain of Al Trashino, the mob boss that resides in Alcatrash";
                break;
            case "Alcatrash":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Alcatrash";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Home of Al Trashino... What a dump";
                break;
            case "Downtown":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Downtown";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "A mysterious figure rules this section of the city";
                break;
            case "Office":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Office";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "The home of a plant lover";
                break;
            case "BoarBar":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Boar Bar";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "The Watering Hole";
                break;
            case "FalconTower":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Falcon Tower";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Roost of Jordan BelFalcon";
                break;
            case "TrashFort":
                MapHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Trash Fort";
                MapHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Home of Tank Sinatra";
                break;
            default:
                break;
        }
    }
    public void setAbilitiesPanel(GameObject g){
        switch(g.name){
            case "MinorHeal":
                AbilitiesHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Minor Heal";
                AbilitiesHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Heals 2 Health";
                break;
            case "MajorHeal":
                AbilitiesHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Minor Heal";
                AbilitiesHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Heals 10 Health";
                break;
            case "Slash":
                AbilitiesHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Slash";
                AbilitiesHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Use Slash Attack";
                break;
            case "SporeBomb":
                AbilitiesHoverPanel.transform.GetChild(0).GetComponent<Text>().text = "Spore Bomb";
                AbilitiesHoverPanel.transform.GetChild(1).GetComponent<Text>().text = "Use Spore Bomb";
                break;
            default:
                break;
        }
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
        setSkillTreePanel(obj);
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
    public void OpenInventory(){
        openGUI = true;
        invPanel.SetActive(true);
        mapPanel.SetActive(false);
        sklPanel.SetActive(false);
        questPanel.SetActive(false);
    }
    public void OpenMap(){
        openGUI = true;
        invPanel.SetActive(false);
        mapPanel.SetActive(true);
        sklPanel.SetActive(false);
        questPanel.SetActive(false);
    }
    public void OpenSkillTree(){
        openGUI = true;
        invPanel.SetActive(false);
        mapPanel.SetActive(false);
        sklPanel.SetActive(true);
        questPanel.SetActive(false);
    }
    public void OpenQuests(){
        openGUI = true;
        invPanel.SetActive(false);
        mapPanel.SetActive(false);
        sklPanel.SetActive(false);
        questPanel.SetActive(true);
    }
    public void OpenAbilities(){
        openGUI = true;
        invPanel.SetActive(false);
        mapPanel.SetActive(false);
        sklPanel.SetActive(false);
        questPanel.SetActive(false);

    }
    public void PopulateQuests(){
        foreach(Transform t in GameObject.Find("Content").transform){
            GameObject.Destroy(t.gameObject);
        }
        foreach(Quest q in quests){
            GameObject item = Instantiate(Resources.Load<GameObject>("QuestItem"),Vector3.zero,Quaternion.identity);
            GameQuest gamequest = item.AddComponent<GameQuest>();
            gamequest.Title = q.Title;
            gamequest.Description = q.Description.Replace("\\n", "\n");
            gamequest.completed = q.completed;
            gamequest.available = q.available;
            item.transform.GetChild(0).gameObject.GetComponent<Text>().text = gamequest.Title;

            //item.transform.position = GameObject.Find("Content").transform.position;
            item.transform.position = new Vector3(0,0,0);
            item.transform.parent = GameObject.Find("Content").transform;
            item.transform.localScale = new Vector3(1,1,1);
            item.transform.localPosition = new Vector3(0,-30-(50*quests.IndexOf(q)),0);
            item.GetComponent<Button>().onClick.AddListener(delegate { QuestInfo(gamequest);});
            if(!gamequest.available){
                item.GetComponent<Button>().interactable = false;
            }
            if(gamequest.completed){
                item.GetComponent<Button>().interactable = false;
                Color color;
                ColorUtility.TryParseHtmlString("#3F9044", out color);
                item.transform.GetChild(0).gameObject.GetComponent<Text>().color = color;
            }
        }
    }
    public void ReopulateQuests(){
        Debug.Log("Repop");
        foreach(Quest q in quests){
            Debug.Log(q.Title);
            GameObject item = GameObject.Find("Content").transform.GetChild(quests.IndexOf(q)).gameObject;
            GameQuest gamequest = item.GetComponent<GameQuest>();
            gamequest.Title = q.Title;
            gamequest.Description = q.Description.Replace("\\n", "\n");
            gamequest.completed = q.completed;
            gamequest.available = q.available;
            item.transform.GetChild(0).gameObject.GetComponent<Text>().text = gamequest.Title;
            item.GetComponent<Button>().onClick.AddListener(delegate { QuestInfo(gamequest);});

            if(gamequest.completed){
                item.GetComponent<Button>().interactable = false;
                Color color;
                ColorUtility.TryParseHtmlString("#3F9044", out color);
                item.transform.GetChild(0).gameObject.GetComponent<Text>().color = color;
            }
            else{
                if(!gamequest.available){
                    item.GetComponent<Button>().interactable = false;
                }
                else{
                    item.GetComponent<Button>().interactable = false;
                
                    item.GetComponent<Button>().interactable = true;
                    Color color;
                    ColorUtility.TryParseHtmlString("#00FF06", out color);
                    item.transform.GetChild(0).gameObject.GetComponent<Text>().color = color;
                }
            }

            
            
        }
    }
    public void QuestInfo(GameQuest gq){
        GameObject questInfo = GameObject.Find("QuestInfo");
        questInfo.transform.GetChild(0).gameObject.GetComponent<Text>().text = gq.Title;
        questInfo.transform.GetChild(1).gameObject.GetComponent<Text>().text = gq.Description.Replace("\\n", "\n");
    }
    public Quest GetGameQuest(string s){
        foreach(Quest gq in quests){
            if(gq.Title == s){
                return gq;
            }
        }
        return null;
    }

}
