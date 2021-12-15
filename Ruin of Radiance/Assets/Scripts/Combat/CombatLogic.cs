using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using Cinemachine;
using UnityEngine.SceneManagement;


/* to do list 

Functions
- COMBAT LOGIC
- complete prefabs for all creatures
- animations play for (attacking, being defeated?);

GUI Things
- When in combat display the available Action points
- disable the stamina bar in combat
- specialty buttons for (slash, spore bomb, __ )
- spawnCreature (implemented with GUI)
- display stats for all creatures + enemies


*/

public class CombatLogic : MonoBehaviour {
    [SerializeField]
    public List<Unit> enemies = new List<Unit>();
    AudioClip combatAudio;
    AudioClip prevAudio;
    Movement moveScript;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0, 1);
    CombatUnit Character;
    CombatUnit SpawnUnit;
    PlayerStats stats;
    public bool inCombat = false;
    DefaultCreatures dc = new DefaultCreatures();
    GameObject CombatButtonGUI;
    int enemyTotal = 0;
    CombatTile enemy;
    CombatTile enemy1;
    CombatTile enemy2;
    CombatTile enemy3;
    CombatTile enemy4;
    CombatTile enemy5;
    CombatTile enemy6; 
    GameObject StaminaBar;
    
    bool enemyDelay = false;
    bool usedMajorHeal = false;
    bool usedMinorHeal = false;
    bool usedSporeBomb = false;

    void Start() {
        combatAudio = Resources.Load("BattleMusic") as AudioClip;
        Debug.Log("Started Logic");
        stats = GameObject.Find("Character").GetComponent<PlayerStats>();

        // makes grid invisible on start
        Color tmp = gameObject.transform.Find("CombatGrid").GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        gameObject.transform.Find("CombatGrid").GetComponent<SpriteRenderer>().color = tmp;

        // defines movement script
        moveScript = GameObject.Find("Character").GetComponent<Movement>();
        // moveScript.inCombat = true;

        createPlayer();

        Character.setIsFriendly(true);
        StaminaBar = GameObject.Find("StaminaBar");
        
        CombatButtonGUI = gameObject.transform.Find("CombatGUICanvas").gameObject;
        CombatButtonGUI.SetActive(false);
        Debug.Log("Finished Start");

    }
    void Update() {

    }
    public void createPlayer() {
        Character = new CombatUnit(GameObject.Find("Character"),stats.maxHealth,stats.health,stats.damage,1,stats.critRate,true,false,0,0,Enums.Enemy.Character);
        grid.getTiles()[0,1].setTileUnit(Character);
        grid.getTiles()[0,1].setIsOccupied(true);
    }
    public void spawnEnemies() {
        CombatUnit unit;
        Debug.Log("Spawning Enemies " + enemies.Count);
        int count = gameObject.transform.Find("CombatGrid").childCount;
        for(int i = 0; i<count;i++){
            if(gameObject.transform.Find("CombatGrid").GetChild(i).gameObject){
                if(gameObject.transform.Find("CombatGrid").GetChild(i).name.Contains("Clone")){
                    Destroy(gameObject.transform.Find("CombatGrid").GetChild(i).gameObject);
                }
            }
        }

        for(int i=0; i<enemies.Count; i++){
            if(enemies[i].type!=Enums.Enemy.None && enemies[i].type!=Enums.Enemy.Custom){
                Destroy(enemies[i].obj);
                unit = dc.getFromEnum(enemies[i].type);
                unit.getUnitSprite().gameObject.transform.parent = gameObject.transform.Find("CombatGrid");
                if (!grid.getTiles()[enemies[i].x,enemies[i].y].getIsOccupied()) {
                        grid.getTiles()[enemies[i].x,enemies[i].y].setTileUnit(unit);
                        grid.getTiles()[enemies[i].x,enemies[i].y].setIsOccupied(true);
                        grid.getTiles()[enemies[i].x,enemies[i].y].snapUnit();
                }
            }
            else{
                unit = new CombatUnit(
                            enemies[i].obj,
                            enemies[i].health,
                            enemies[i].health,
                            enemies[i].damage,
                            enemies[i].scale,
                            enemies[i].crit,
                            false,
                            false,
                            0,
                            0,
                            Enums.Enemy.Custom
                        );
                unit.getUnitSprite().gameObject.transform.parent = gameObject.transform.Find("CombatGrid");
                if (!grid.getTiles()[enemies[i].x,enemies[i].y].getIsOccupied()) {
                        grid.getTiles()[enemies[i].x,enemies[i].y].setTileUnit(unit);
                        grid.getTiles()[enemies[i].x,enemies[i].y].setIsOccupied(true);
                        grid.getTiles()[enemies[i].x,enemies[i].y].snapUnit();
                }
            }
            Debug.Log("ENEMIES SPAWNED *********************************************");
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        createPlayer();
        this.enabled = false; // disables the enemy collision
        Debug.Log("Collision now disabled");
        //Starts Combat
        GameObject.Find("Character").transform.Find("Character VCam").parent = null;
        GameObject.Find("Character VCam").GetComponent<CinemachineVirtualCamera>().Follow = null;
        GameObject.Find("Character").transform.parent = gameObject.transform.GetChild(2).transform;
        Debug.Log("SetParent");
        startCombat();
    }
    public void startCombat() {
        prevAudio = GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().clip;
        GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().clip = combatAudio;
        GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().Play();
        inCombat = true;
        moveScript.inCombat = true;

        // puts player into the combat scene
        gameObject.transform.Find("CombatGrid").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Priority = 100;
        
        
        // swap for snapUnit function 
        grid.getTiles()[0,1].snapUnit();
        // snap enemies to grid
        Destroy(GetComponent<BoxCollider2D>());
        spawnEnemies();
        Debug.Log("End Start");
        CombatButtonGUI.SetActive(true);
        StaminaBar.SetActive(false);
        defineEnemies();
        gameObject.transform.Find("CombatGUICanvas").Find("ActionPoints").GetComponent<Text>().text = "Action Points: " + stats.actionPoints;
        // FOR PLAYTEST
        stats.health = Character.getHealth();
        GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
    }
    public void endTurn() { // end turn button?
        Debug.Log("END TURN");
        enemyLogic();
        stats.actionPoints = 5;
        gameObject.transform.Find("CombatGUICanvas").Find("ActionPoints").GetComponent<Text>().text = "Action Points: " + stats.actionPoints;

        HighlightField();
        // Debug.Log("Highlights refreshed end of turn");
    }
    public void captureOp(){
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== true) { // if occupied and enemy
                    Debug.Log("Creature Found during check");
                    grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                    grid.getTiles()[i,j].getTileUnit().setIsDefeated(true);
                    stats.skillPoints++;
                }
                else {
                    Debug.Log("Character Found during check");
                }
            }
        }

    }
    public bool checkWin() {
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== false) { // if occupied and enemy
                return false;
                }
            }
        }
        return true;

    }
    public void lose() {
        ForceEndCombat();
        switch(SceneManager.GetActiveScene().name){
            case "02_Uptown":
            case "03_Alcatrash":
                GameObject.Find("Character").transform.position = new Vector3(-16.5f,29,0);
                break;
            case "04_Downtown":
            case "05_TrashFort":
            case "06_BoarBar":
            case "07_FalconArea":
            case "08_FinalOffice":
                GameObject.Find("Character").transform.position = new Vector3(-6,-13,0);
                break;
            default:
                break;

        }
    }
    public void UseMinorHeal(){
        if(stats.hasHeal){
            if(!usedMinorHeal){
                usedMinorHeal = false;
                grid.getTileOfUnit(Character).healUnit(stats.healPower*2);
                stats.health = grid.getTileOfUnit(Character).getTileUnit().getHealth();
                GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
            }
        }
    }
    public void UseWateringCan(){
        if(stats.numOfWateringCans>0){
            stats.numOfWateringCans--;
            grid.getTileOfUnit(Character).healUnit(100);
            stats.health = grid.getTileOfUnit(Character).getTileUnit().getHealth();
            GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
        }
    }
    public void UseCrystal(){
        if(stats.numOfCrystals >0){
            stats.numOfCrystals--;
            stats.mindEnergy = stats.maxMindEnergy;
            GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
        }
    }
    public void UseMajorHeal(){
        if(stats.hasMegaHeal){
            if(!usedMajorHeal){
                usedMajorHeal = false;
                grid.getTileOfUnit(Character).healUnit(stats.megaHealPower*5);
                stats.health = grid.getTileOfUnit(Character).getTileUnit().getHealth();
                GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
            }
        }
    }
    public void HighlightSlash(){
        if(stats.actionPoints>=3){
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1].setHighlight(Enums.highlight.Damage);
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()].setHighlight(Enums.highlight.Damage);
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1].setHighlight(Enums.highlight.Damage);
            RefreshHighlights();
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1].setHighlight(Enums.highlight.None);
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()].setHighlight(Enums.highlight.None);
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1))
            grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1].setHighlight(Enums.highlight.None);
        }
    }
    public void UseSlash(){
        if(stats.actionPoints>=3){
            stats.actionPoints-=3;
            GameObject.Find("Character").GetComponent<Animator>().Play("characterFrontAttack");
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1)){
                if(grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1].getIsOccupied()){
                    if(!grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1].getTileUnit().getIsFriendly()){
                        grid.attack(stats.slashDamage,1,grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()+1);
                    }
                }
            }
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord())){
                if(grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()].getIsOccupied()){
                    if(!grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()].getTileUnit().getIsFriendly()){
                        grid.attack(stats.slashDamage,1,grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord());
                    }
                }
            }
            if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1)){
                if(grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1].getIsOccupied()){
                    if(!grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1].getTileUnit().getIsFriendly()){
                        grid.attack(stats.slashDamage,1,grid.getTileOfUnit(Character).getXCoord()+1,grid.getTileOfUnit(Character).getYCoord()-1);
                    }
                }
            }
        }
        grid.clearHighlights();
        HighlightField();
    }
    public void HightlightSporeBomb(){
        if(!usedSporeBomb){
            for(int i =-2;i<3;i++){
                for(int j =-2;j<3;j++){
                    if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j)){
                        grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j].setHighlight(Enums.highlight.Damage);
                    }
                }
            }
            RefreshHighlights();
            for(int i =-2;i<3;i++){
                for(int j =-2;j<3;j++){
                    if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j)){
                        grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j].setHighlight(Enums.highlight.None);
                    }
                }
            }
        }
    }
    public void UseSporeBomb(){
        if(!usedSporeBomb){
            usedSporeBomb= true;
            for(int i =-2;i<3;i++){
                for(int j =-2;j<3;j++){
                    if(getInBounds(grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j)){
                        if(grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j].getIsOccupied()){
                            if(!grid.getTiles()[grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j].getTileUnit().getIsFriendly()){
                                grid.attack(stats.sporeDamage,1,grid.getTileOfUnit(Character).getXCoord()+i,grid.getTileOfUnit(Character).getYCoord()+j);
                            }
                        }
                    }
                }
            }
        }
    }
    public void endCombat() {
        if (checkWin()){
            captureOp();
            grid.clearHighlights();
            RefreshHighlights();
            stats.mindEnergy = stats.maxMindEnergy;
            CombatButtonGUI.SetActive(false);
            GameObject.Find("Character").transform.parent = null;
            // GameObject.Find("CombatGrid").SetActive(false);
            gameObject.transform.Find("CombatGrid").gameObject.SetActive(false);
            Debug.Log("You have ended the battle");
            moveScript.inCombat = false;
            GameObject.Find("Character VCam").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Character").transform;
            GameObject.Find("Character VCam").transform.parent = GameObject.Find("Character").transform;
            gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Priority = 0;
            GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().clip = prevAudio;
            GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().Play();
            GameObject.Find("Canvas").GetComponent<GUIScript>().openGUI = false;
            inCombat = false;
            // allow for collecting creatures
            // remove dead or captured creatures
        }
        
    }
    public void ForceEndCombat() {
        grid.clearHighlights();
        RefreshHighlights();
        stats.mindEnergy = stats.maxMindEnergy;
        stats.health = stats.maxHealth;
        GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
        CombatButtonGUI.SetActive(false);
        GameObject.Find("Character").transform.parent = null;
        gameObject.transform.Find("CombatGrid").gameObject.SetActive(false);
        Debug.Log("You have ended the battle");
        moveScript.inCombat = false;
        GameObject.Find("Character VCam").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Character").transform;
        GameObject.Find("Character VCam").transform.parent = GameObject.Find("Character").transform;
        gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Priority = 0;
        GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().clip = prevAudio;
        GameObject.Find("Character").transform.Find("Music").GetComponent<AudioSource>().Play();
        inCombat = false;
        gameObject.SetActive(false);
        // allow for collecting creatures
        // remove dead or captured creatures
    }
    public void clickTile(string name) {
        if(stats.health <= 0) {
            lose();
        }
        if(inCombat) {

            int x = int.Parse(name.Substring(0,1));
            int y = int.Parse(name.Substring(1,1));

            if (!grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getHighlight() == Enums.highlight.Control) {
                
                    Debug.Log("Summoned Creature"); // if highlight is purple
                    //if (stats.mindEnergy - SpawnUnit.getSummonCost() >= 0) {
                    SummonCreature(dc.getFromEnum(stats.selectedType), grid.getTiles()[x,y]);
                    // }
                    
                }
          

            grid.clearHighlights();
            RefreshHighlights();
            grid.updateAnimatorControllers();

            Debug.Log("X:" + x + " Y:" + y); 

            if (checkWin()) {
                captureOp();
            }
            
                if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getTileUnit().getIsFriendly() && !grid.getTiles()[x,y].getTileUnit().getIsDefeated()) { // select
                    grid.selectedTile = grid.getTiles()[x,y];
                    grid.selectedTile.setHighlight(Enums.highlight.Selected);
                    RefreshHighlights();
                    // highlight available squares to move to
                    Debug.Log("Selected Tile: ");
                }// && grid.selectedTile.getTileUnit().getHealth() > 0
                else if (!grid.getTiles()[x,y].getIsOccupied() && stats.actionPoints > 0 ) { // move
                    // stats.actionPoints;
                    if(GetDistanceBetweenTiles(grid.getTiles()[x,y],grid.selectedTile)<=stats.actionPoints){
                        stats.actionPoints -= GetDistanceBetweenTiles(grid.getTiles()[x,y],grid.selectedTile);
                        grid.moveTile(grid.selectedTile, grid.getTiles()[x,y]);
                        grid.selectedTile = grid.getTiles()[x,y];
                        Debug.Log("Move To Tile");
                        RefreshHighlights();
                        gameObject.transform.Find("CombatGUICanvas").Find("ActionPoints").GetComponent<Text>().text = "Action Points: " + stats.actionPoints;
                    }
                    grid.selectedTile.setHighlight(Enums.highlight.Selected);
                    
                    
                    
                } // && grid.selectedTile.getTileUnit().getHealth() > 0
                else if (grid.getTiles()[x,y].getIsOccupied() && !grid.getTiles()[x,y].getTileUnit().getIsFriendly() && !grid.getTiles()[x,y].getTileUnit().getIsDefeated() && stats.actionPoints > 0 ){ // attack
                    if (GetDistanceBetweenTiles(grid.selectedTile, grid.getTiles()[x,y]) < 2) {
                        Debug.Log("Attack Tile");
                        if(grid.selectedTile.getTileUnit() == Character) {
                            grid.attack(stats.damage,stats.critRate, x, y);
                            grid.triggerAttackAnim(grid.selectedTile);
                        }
                        else {
                            grid.attack(grid.selectedTile.getTileUnit().getDamage(),stats.creatureCritRate, x, y);
                            grid.triggerAttackAnim(grid.selectedTile);
                        }

                        if (grid.getTiles()[x,y].getTileUnit().getHealth() <= 0 && grid.getTiles()[x,y].getTileUnit().getCreatureType()!=Enums.Enemy.Character) {
                            Debug.Log("Is Defeated, shoulda changed");
                            grid.getTiles()[x,y].setHighlight(Enums.highlight.Control);
                            grid.getTiles()[x,y].getTileUnit().setIsDefeated(true);
                            RefreshHighlights();
                        }
                        stats.actionPoints -= 2;
                        gameObject.transform.Find("CombatGUICanvas").Find("ActionPoints").GetComponent<Text>().text = "Action Points: " + stats.actionPoints;
                    }
                    else {
                        Debug.Log("Enemy out of Range!");
                    }
                    
                    grid.selectedTile.setHighlight(Enums.highlight.Selected);
                }
                else if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getTileUnit().getIsDefeated()){ // recall
                    recallCreature(grid.getTiles()[x,y].getTileUnit());
                    grid.getTiles()[x,y].setIsOccupied(false);
                    Debug.Log("Recall Tile"); 
                }
                
            HighlightField();
            grid.updateAnimatorControllers();
        }
    }
    

    public void defineEnemies() {
        // int enemyTotal = 0;
        /* // now global access
        CombatTile enemy = grid.getTiles()[6,0];
        CombatTile enemy1 = enemy;
        CombatTile enemy2 = enemy;
        CombatTile enemy3 = enemy;
        CombatTile enemy4 = enemy;
        CombatTile enemy5 = enemy;
        CombatTile enemy6 = enemy;
        */
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== false) {
                    ++enemyTotal;
                    Debug.Log("EnemyDefinedTotal " + enemyTotal);
                    switch(enemyTotal) {
                        case 1:
                        //enemy1.setXCoord() = i;
                        //enemy1.setYCoord() = j;
                        enemy1 = grid.getTiles()[i,j];
                        Debug.Log("Enemy 1 defined");
                        break;

                        case 2:
                        enemy2=grid.getTiles()[i,j];
                        Debug.Log("Enemy 2 defined");
                        break;

                        case 3:
                        enemy3=grid.getTiles()[i,j];
                        Debug.Log("Enemy 3 defined");
                        break;

                        case 4:
                        enemy4=grid.getTiles()[i,j];
                        Debug.Log("Enemy 4 defined");
                        break;
                        
                        case 5:
                        enemy5=grid.getTiles()[i,j];
                        Debug.Log("Enemy 5 defined");
                        break;

                        case 6:
                        enemy6=grid.getTiles()[i,j];
                        Debug.Log("Enemy 6 defined");
                        break;
                    }
                   
                }
            }
        }
        Debug.Log("Enemy Total" + enemyTotal);

    }
    public IEnumerator enemyDelayCoroutine() {
        
        yield return new WaitForSeconds(1);
        enemyDelay = true;
    }
    public void enemyLogic() {
        
        Debug.Log("Enemy Total logic start: " + enemyTotal);
        for(int enemyTrack = 1; enemyTrack <= enemyTotal ; ++enemyTrack) {
            Debug.Log("Start of Loop : Enemy Track: " + enemyTrack);
            switch(enemyTrack) { // decide which enemy moves first
                case 1:
                enemy=enemy1;
                Debug.Log("Enemy 1 logic run");
                break;

                case 2:
                enemy=enemy2;
                Debug.Log("Enemy 2 logic run");
                break;

                case 3:
                enemy=enemy3;
                Debug.Log("Enemy 3 logic run");
                break;

                case 4:
                enemy=enemy4;
                Debug.Log("Enemy 4 logic run");
                break;
                
                case 5:
                enemy=enemy5;
                Debug.Log("Enemy 5 logic run");
                break;

                case 6:
                enemy=enemy6;
                Debug.Log("Enemy 6 logic run");
                break;
            }
            
            
            // CombatTile enemy = grid.getTiles()[i,j];
            if (enemy.getTileUnit() != null){
                bool enemyMoved = false;
                if(!enemy.getTileUnit().getIsDefeated()){
                    
                    Debug.Log("Enemy" + enemyTrack + " : Y Coord "+ enemy.getYCoord());
                    
                    // Debug.Log(enemyTrack +" Enemy at" + enemy.getYCoord());
                    if (!enemyMoved) { // in front near
                        for(int k = 1; k <= 3; ++k) { // -occupied and friendly-
                            if (!enemyMoved) {
                                if (enemy.getXCoord() - k >= 0){ // checks for in bounds
                                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getTileUnit().getIsFriendly()) {
                                        // enemy is in front
                                        Debug.Log("Straight ");
                                        

                                        if(k==1){
                                            Debug.Log("Straight 1");
                                            grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());  // attack
                                            grid.triggerAttackAnim(enemy);
                                            enemyMoved = true;
                                            //StartCoroutine("enemyDelayCoroutine");
                                            //while(!enemyDelay){Debug.Log("1"); }
                                            // enemyDelay = false;
                                            // move back if needs balancing - roll crit? check for in bounds
                                        }
                                        else if (k==2){  // if friendlys are 2 spaces directly infront - move and attack
                                        Debug.Log("Straight 2 - Move and Attack");
                                            // move one space and attack
                                            if (!grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()].getIsOccupied()) {
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]);
                                                enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                                grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                grid.triggerAttackAnim(enemy);
                                                enemyMoved = true;

                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){Debug.Log("1");  }
                                                // enemyDelay = false;
                                            }
                                        }
                                        else if (k==3) {
                                            if (!grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()].getIsOccupied()) {
                                                Debug.Log("Straight 3");
                                                if (checkCrit(enemy.getTileUnit().getCritRate())&& (!grid.getTiles()[enemy.getXCoord()-2, enemy.getYCoord()].getIsOccupied())) { // if crit hits, move 2 and attack
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-2, enemy.getYCoord()]); // move 2
                                                    enemy = grid.getTiles()[enemy.getXCoord()-2,enemy.getYCoord()];
                                                    grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord()); // attack
                                                    enemyMoved = true;

                                                    // StartCoroutine("enemyDelayCoroutine");
                                                    // while(!enemyDelay){ }
                                                    // enemyDelay = false;
                                                    Debug.Log("Crit Hit move 3");
                                                }
                                                else { // move only one
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]); // move 1
                                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                                    enemyMoved = true;

                                                    // StartCoroutine("enemyDelayCoroutine");
                                                    // while(!enemyDelay){ }
                                                    // enemyDelay = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (!enemyMoved) { // above and below near
                    for(int k = 1; k <= 3; ++k) { // -occupied and not friendly-
                        if (!enemyMoved) { // above near
                            if (enemy.getXCoord() - k >= 0){ // checks for in bounds
                                if (enemy.getYCoord() + 1 < 3) { // check underneath too
                                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord() + 1].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()+1].getTileUnit().getIsFriendly()) {
                                        // enemy is above
                                        Debug.Log("Above");
                                        if(k==1){ 
                                            Debug.Log("Above 1");
                                            // check for empty space above 
                                            // move up and attack 
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]);
                                                enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                grid.triggerAttackAnim(enemy);
                                                enemyMoved = true;
                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){ }
                                                // enemyDelay = false;
                                            }
                                        }
                                        else if (k==2){
                                            Debug.Log("Above 2");  // if friendlys are up 1 over 2 left
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                if (checkCrit(enemy.getTileUnit().getCritRate())) { // if crit hits, move up 1, 1 left and attack
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()+1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()+1];
                                                    grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                    grid.triggerAttackAnim(enemy);
                                                    enemyMoved = true;
                                                    // StartCoroutine("enemyDelayCoroutine");
                                                    // while(!enemyDelay){ }
                                                    // enemyDelay = false;
                                                }
                                                else { // else just move up 1
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                    enemyMoved = true;
                                                    // StartCoroutine("enemyDelayCoroutine");
                                                    // while(!enemyDelay){ }
                                                    // enemyDelay = false;
                                                }
                                            }
                                        }
                                        else if (k==3) {
                                            Debug.Log("Above 3");
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]); // move up one square
                                                enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                enemyMoved = true;
                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){ }   
                                                // enemyDelay = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                        if (!enemyMoved) { // below near
                            if (enemy.getXCoord() - k >= 0) {
                                if (enemy.getYCoord()-1 >= 0) {       // check underneath
                                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord() - 1].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()-1].getTileUnit().getIsFriendly()) {
                                        // enemy is in front
                                        Debug.Log("Enemy Option 2 - Below");
                                        if(k==1){
                                            Debug.Log("B 1");
                                            // check for empty space below
                                            // move down and attack
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1].getIsOccupied()) {
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1]);
                                                enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()-1];
                                                grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                grid.triggerAttackAnim(enemy);
                                                enemyMoved = true;
                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){ }
                                                // enemyDelay = false;
                                            }
                                        }
                                        else if (k==2){  // if friendlys are up 1 over 2 left 
                                            Debug.Log("B 2");
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1].getIsOccupied()) { // check for empty space above
                                                if (checkCrit(enemy.getTileUnit().getCritRate())) { // if crit hits, move down 1, 1 left and attack
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()-1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()-1];
                                                    grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                    grid.triggerAttackAnim(enemy);
                                                    enemyMoved = true;
                                                }
                                                else { // else just move up 1
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()-1];
                                                    enemyMoved = true;
                                                }
                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){ }
                                                // enemyDelay = false;
                                            }
                                        }
                                        else if (k==3) {
                                            Debug.Log("B 3");
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1].getIsOccupied()) { // check for empty space above
                                                // move up one square
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1]);
                                                enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()-1];
                                                enemyMoved = true;
                                                // StartCoroutine("enemyDelayCoroutine");
                                                // while(!enemyDelay){ }
                                                // enemyDelay = false;
                                            }
                                        }

                                    }  
                                }  

                            }
                        }
                    }
                }
                // checks full board
                if (!enemyMoved) { // last resort, search full board
                    bool friendlyUnitAlive = false;
                    bool enemyBehind = false;
                    bool enemyFar = false;
                    bool enemyFoward = false;
                    int q = 0, s = 0;
                    Debug.Log("Full Board");
                    for(q = 0; q < 7; ++q) { 
                        for (s = 0; s < 3; ++s) {
                            if (grid.getTiles()[q,s].getIsOccupied() && grid.getTiles()[q,s].getTileUnit().getIsFriendly()) {
                                friendlyUnitAlive = true;
                                if (enemy.getXCoord()-1 > grid.getTiles()[q,s].getXCoord()) {
                                    enemyFoward = true;
                                }
                                if (enemy.getXCoord()+1 < grid.getTiles()[q,s].getXCoord()) {
                                    enemyBehind = true;    
                                }
                                if (enemy.getXCoord()-3 < grid.getTiles()[q,s].getXCoord()) {
                                    enemyFar = true;    
                                }
                            }
                        }
                    }
                    if (friendlyUnitAlive) {
                        if (enemyBehind == false && enemyFoward) {
                            if (enemy.getXCoord() - 2 >= 0) {
                                if (!grid.getTiles()[enemy.getXCoord()-1 , enemy.getYCoord()].getIsOccupied()) {
                                    if (checkCrit(.75f)) {
                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()- 1, enemy.getYCoord()]);
                                        enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                        Debug.Log("Full Board check move 1 : Enemy Number "+ enemyTrack);
                                        Debug.Log("EnemyNumber " + enemyTrack + " p1 : [ "+ enemy.getXCoord() + "," + enemy.getYCoord() + "] ");
                                        

                                        if (enemyFar && checkCrit(0.25f) ) {
                                            grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]);
                                            enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                            Debug.Log("Crit Hit move 1 more forward");
                                            enemyMoved = true;
                                            Debug.Log("EnemyNumber " + enemyTrack + " after crit : [ "+ enemy.getXCoord() + "," + enemy.getYCoord() + "] ");
                                        }
                                        else{
                                            Debug.Log("No Crit");
                                            enemyMoved = true;
                                        }
                                        // StartCoroutine("enemyDelayCoroutine");
                                        //     while(!enemyDelay){ }
                                        //     enemyDelay = false;
                                    }
                                }
                            }
                        }
                        if (enemy.getXCoord() +1 <= 6 && enemyBehind) { 
                                if (enemy.getXCoord() +1 <= 6) {
                                    if (!grid.getTiles()[enemy.getXCoord()+1, enemy.getYCoord()].getIsOccupied()) {
                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()+1, enemy.getYCoord()]);
                                        enemy = grid.getTiles()[enemy.getXCoord()+1,enemy.getYCoord()];
                                        enemyMoved = true;
                                        Debug.Log("Enemy " + enemyTrack + " mvoed backwards to [ "+ enemy.getXCoord() + "," + enemy.getYCoord() + "] ");
                                    }
                                }
                            
                        }
                    }
                }
            }
     
        /* in progress
            
        // check above, then below row
                // else check row 1 up, or 1 down move accordingly if possible
        // danger response
                // low health is getTileUnit().getMaxHealth() / 4 > getTileUnit().getHealth();
                // if low health, attack and move backwards

                // scatter if low health for birds?

                // special logic for "custom enemies" aka bosses

                // wait / coroutine inbetween enemy movements
                // doesn't move if there is creature in that position

        */

        switch(enemyTrack) { // decide which enemy moves first
                case 1:
                enemy1 = enemy;
                Debug.Log("Enemy 1 logic ended");
                break;

                case 2:
                enemy2 = enemy;
                Debug.Log("Enemy 2 logic ended");
                break;

                case 3:
                enemy3 = enemy;
                Debug.Log("Enemy 3 logic ended");
                break;

                case 4:
                enemy4 = enemy;
                Debug.Log("Enemy 4 logic ended");
                break;
                
                case 5:
                enemy5 = enemy;;
                Debug.Log("Enemy 5 logic ended");
                break;

                case 6:
                enemy6 = enemy;
                Debug.Log("Enemy 6 logic ended");
                break;
            }
        Debug.Log("Enemy " + enemyTrack + " ended at : [ "+ enemy.getXCoord() + "," + enemy.getYCoord() + "] ");
        }

        grid.clearHighlights();
        grid.selectedTile.setHighlight(Enums.highlight.Selected);
        HighlightField();
        // RefreshHighlights();
        if (inCombat) {
            stats.health = Character.getHealth();
            // Debug.Log("Character Health: " + stats.health);
            GameObject.Find("Canvas").GetComponent<GUIScript>().updateUIBars();
            // Character.getHealth();
            if(stats.health <= 0) {
                lose();
            }
        }
        if(stats.health <= 0) {
                lose();
        }
    }

    public bool checkCrit(float startCrit) {
        if (Random.value < startCrit) {
            return true;
        }
        return false;
    }
    public void summonCreatureHighlight() {
        
        Debug.Log("Summoning Creature highlights");
        grid.clearHighlights();
        
        // highlight character + or - 1 in x & y
        CombatTile charLoc = grid.getTileOfUnit(Character);

        for(int i = charLoc.getXCoord()-2; i<=charLoc.getXCoord()+2; i++){
            for(int j = charLoc.getYCoord()-2; j<=charLoc.getYCoord()+2; j++){
                if(!(i < 0 || i>6 || j<0 || j>2)){
                   // Debug.Log("Base");
                    if (!grid.getTiles()[i,j].getIsOccupied()) {
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                        // Debug.Log("Summoning");
                    }
                }
            }
        }
        
        RefreshHighlights();
     
        Debug.Log("Summoned Highlight");
        
        
           
    }
    public void SummonCreature(CombatUnit unit1, CombatTile unitLoc) {
        if (stats.mindEnergy - unit1.getSummonCost() >= 0) {
            if (!unitLoc.getIsOccupied()) {
                stats.mindEnergy -= unit1.getSummonCost();
                unit1.getUnitSprite().gameObject.transform.parent = gameObject.transform.Find("CombatGrid");
                unit1.setIsFriendly(true);
                unit1.getUnitSprite().GetComponent<SpriteRenderer>().flipX = true;
                grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].setTileUnit(unit1);
                grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].setIsOccupied(true);
                grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].snapUnit();
                Debug.Log("Unit Spawned to " + unitLoc.getXCoord() + ", " + unitLoc.getYCoord() + "Enemy Snapped");
            }
            
            // highlight where you can put the unit (+- 1 in all directions)
            // create unit on selected square

            // subtract MC energy & action points
            // subtract from inventory total depending on the unit type
            Debug.Log("Summoned Creature to [" + unitLoc.getXCoord()  + ", " + unitLoc.getYCoord() + "]");
            stats.decUnit(stats.selectedType);
        }
        else {
            Debug.Log("Creature not spawned ");
        }
    }
    public void recallCreature(CombatUnit unit1) {
        // inventory "x1 Unit"
        Debug.Log("Creature Recalled");
        
        switch(unit1.getCreatureType()){
            case Enums.Enemy.Rat:
                stats.numOfRats += 1;
                break;
            case Enums.Enemy.Pigeon:
                stats.numOfPigeons += 1;
                break;
            case Enums.Enemy.Boar:
                stats.numOfBoars += 1;
                break;
            case Enums.Enemy.Raccoon:
                stats.numOfRaccoons += 1;
                break;
            case Enums.Enemy.Falcon:
                stats.numOfFalcons += 1;
                break;
        }
        stats.mindEnergy -= unit1.getRecallCost();

        grid.getTileOfUnit(unit1).setIsOccupied(false);
        grid.getTileOfUnit(unit1).setTileUnit(null);
        Destroy(unit1.getUnitSprite());
        
        Debug.Log("Recalled creature from [" + ", " + "]");
    }
    public void RefreshHighlights() {
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                // Debug.Log(i + ", " + j);
                switch(grid.getTiles()[i,j].getHighlight()){ // tile_Overlay.png
                    case Enums.highlight.None: // none
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().color = new Color(1,1,1,0);
                        // Debug.Log("Refresh none");
                        break;
                    case Enums.highlight.Move: // Move
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().sprite = Resources.Load<Sprite>("CombatAssets/tileMoveOverlay");
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().color = new Color(1,1,1,1);
                        // Debug.Log("Refresh move");
                        break;
                    case Enums.highlight.Damage: // Damage
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().sprite = Resources.Load<Sprite>("CombatAssets/tileDamageOverlay");
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().color = new Color(1,1,1,1);
                        // Debug.Log("Refresh damage");
                        break;
                    case Enums.highlight.Control: // Control
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().sprite = Resources.Load<Sprite>("CombatAssets/tileControlOverlay");
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().color = new Color(1,1,1,1);
                        // Debug.Log("Refresh control");
                        break;
                    case Enums.highlight.Selected: // Selected
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().sprite = Resources.Load<Sprite>("CombatAssets/tileSelectedOverlay");
                        gameObject.transform.GetChild(0).gameObject.transform.Find(""+i+""+j).GetComponent<Image>().color = new Color(1,1,1,1);
                        // Debug.Log("Refresh select");
                        break;
                }
            }
        }


    }
    public void HighlightField(){
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {

                int actionsUsed = GetDistanceBetweenTiles(grid.selectedTile, grid.getTiles()[i,j]);
                if(actionsUsed <= stats.actionPoints){
                    if(!grid.getTiles()[i,j].getIsOccupied()){
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Move);
                    }       
                    else if(((actionsUsed) < stats.actionPoints) && grid.getTiles()[i,j].getIsOccupied() && !(grid.getTiles()[i,j].getTileUnit().getIsFriendly()) ) {
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Damage);
                        Debug.Log("determined damage overlay");
                    }
                    else {
                        
                    }
                    
                }
                if(grid.getTiles()[i,j].getIsOccupied() && (grid.getTiles()[i,j].getTileUnit().getIsDefeated()) && grid.getTiles()[i,j].getTileUnit().getCreatureType()!=Enums.Enemy.Character) {
                    grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                    Debug.Log("Should have changed highlight to purple");
                }
            }
        }
        RefreshHighlights();
    }
    public int GetDistanceBetweenTiles(CombatTile from, CombatTile to) {
        return (Math.Abs((from.getXCoord()-to.getXCoord())))+(Math.Abs(from.getYCoord()-to.getYCoord()));
    }
    public bool getInBounds(int x, int y){
        if(x<0 || x>6)return false;
        if(y>2||y<0)return false;
        return true;
    }

    public void ReDebug(){
        
        Debug.Log("Debug function called");
        // for tests
        
    }
    

// made with love <3

}