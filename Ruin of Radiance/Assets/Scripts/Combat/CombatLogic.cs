using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using Cinemachine;


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
    Movement moveScript;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0, 1);
    CombatUnit Character;
    CombatUnit SpawnUnit;
    PlayerStats stats;
    DefaultCreatures dc = new DefaultCreatures();
    GameObject CombatButtonGUI;
    void Start() {
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
        CombatButtonGUI = gameObject.transform.Find("CombatGUICanvas").gameObject;
        CombatButtonGUI.SetActive(false);
        
        Debug.Log("Finished Start");

    }
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space slammed"); 
            endTurn();
        }

    }
    public void createPlayer() {
        Character = new CombatUnit(GameObject.Find("Character"),stats.maxHealth,stats.health,stats.damage,1,stats.critRate,true,false,0,0,Enums.Enemy.Character);
        grid.getTiles()[0,1].setTileUnit(Character);
        grid.getTiles()[0,1].setIsOccupied(true);
        // Character = new Unit();
        // Character.unitSprite = GameObject.Find("Dynamic Sprite");
        // Debug.Log("Player Created "); 
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
        this.enabled = false; // disables the enemy collision
        Debug.Log("Collision now disabled");
        //Starts Combat
        GameObject.Find("Character").transform.parent = gameObject.transform.GetChild(2).transform;
        Debug.Log("SetParent");
        startCombat();
    }
    public void startCombat() {
        // puts player into the combat scene
        gameObject.transform.Find("CombatGrid").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Priority = 100;
        
        moveScript.inCombat = true;
        // swap for snapUnit function 
        grid.getTiles()[0,1].snapUnit();
        // snap enemies to grid
        Destroy(GetComponent<BoxCollider2D>());
        spawnEnemies();
        Debug.Log("End Start");
        CombatButtonGUI.SetActive(true);
    }
    public void endTurn() { // end turn button?
        Debug.Log("END TURN");
        enemyLogic();
        stats.actionPoints = 5;

        HighlightField();
        // Debug.Log("Highlights refreshed end of turn");
    }
    public void captureOp(){
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if(grid.getTiles()[i,j].getTileUnit() != Character) {
                    if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== true) { // if occupied and enemy
                        Debug.Log("Creature Found during check");
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                        grid.getTiles()[i,j].getTileUnit().setIsDefeated(true);
                    }
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
    public void endCombat() {
        if (checkWin()){
            captureOp();
            grid.clearHighlights();
            RefreshHighlights();
            CombatButtonGUI.SetActive(false);
            GameObject.Find("Character").transform.parent = null;
            GameObject.Find("CombatGrid").SetActive(false);
            Debug.Log("You have ended the battle");
            moveScript.inCombat = false;
            gameObject.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Priority = 0;
            
            // allow for collecting creatures
            // remove dead or captured creatures
        }
    }
    public void clickTile(string name) {

        if(moveScript.inCombat == true) {

            int x = int.Parse(name.Substring(0,1));
            int y = int.Parse(name.Substring(1,1));

            if (!grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getHighlight() == Enums.highlight.Control) {
                    Debug.Log("Summoned Creature"); // if highlight is purple
                    SummonCreature(dc.getFromEnum(stats.selectedType), grid.getTiles()[x,y]);
                    stats.decUnit(stats.selectedType);
                    stats.mindEnergy -= dc.getFromEnum(stats.selectedType).getSummonCost();
                }

            grid.clearHighlights();
            RefreshHighlights();

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
                }
                else if (!grid.getTiles()[x,y].getIsOccupied() && stats.actionPoints > 0 ){ // move
                    // stats.actionPoints;
                    if(GetDistanceBetweenTiles(grid.getTiles()[x,y],grid.selectedTile)<=stats.actionPoints){
                        stats.actionPoints -= GetDistanceBetweenTiles(grid.getTiles()[x,y],grid.selectedTile);
                        grid.moveTile(grid.selectedTile, grid.getTiles()[x,y]);
                        grid.selectedTile = grid.getTiles()[x,y];
                        Debug.Log("Move To Tile");
                        RefreshHighlights();
                    }
                    grid.selectedTile.setHighlight(Enums.highlight.Selected);
                    
                    
                    
                }
                else if (grid.getTiles()[x,y].getIsOccupied() && !grid.getTiles()[x,y].getTileUnit().getIsFriendly() && !grid.getTiles()[x,y].getTileUnit().getIsDefeated() && stats.actionPoints > 0 ){ // attack
                    if (GetDistanceBetweenTiles(grid.selectedTile, grid.getTiles()[x,y]) < 2) {
                        Debug.Log("Attack Tile");
                        if(grid.selectedTile.getTileUnit() == Character) {
                            grid.attack(stats.damage,stats.critRate, x, y);
                        }
                        else {
                            grid.attack(grid.selectedTile.getTileUnit().getDamage(),stats.creatureCritRate, x, y);
                        }

                        if (grid.getTiles()[x,y].getTileUnit().getHealth() <= 0) {
                            Debug.Log("Is Defeated, shoulda changed");
                            grid.getTiles()[x,y].setHighlight(Enums.highlight.Control);
                            grid.getTiles()[x,y].getTileUnit().setIsDefeated(true);
                            RefreshHighlights();
                        }
                        stats.actionPoints -= 2;
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
        }
    }

    public void enemyLogic() {
        // find all enemies
        int enemyNum = 0;
        int enemyTrack = 1;
        CombatTile enemy = grid.getTiles()[6,0];
        CombatTile enemy1 = enemy;
        CombatTile enemy2 = enemy;
        CombatTile enemy3 = enemy;
        CombatTile enemy4 = enemy;
        CombatTile enemy5 = enemy;
        CombatTile enemy6 = enemy;
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== false) {
                    switch(enemyNum) {
                        case 1:
                        enemy1.getXCoord() = i;
                        enemy1.getYCoord() = j
                        break;

                        case 2:
                        enemy2=grid.getTiles()[i,j];
                        break;

                        case 3:
                        enemy3=grid.getTiles()[i,j];
                        break;

                        case 4:
                        enemy4=grid.getTiles()[i,j];
                        break;
                        
                        case 5:
                        enemy5=grid.getTiles()[i,j];
                        break;

                        case 6:
                        enemy6=grid.getTiles()[i,j];
                        break;
                    }
                   enemyNum++;
                }
            }
        }
        Debug.Log("enemy Number = " + enemyNum);
        for(enemyTrack = 1; enemyTrack <= enemyNum ;++enemyTrack) {
            switch(enemyTrack) {
                        case 1:
                        enemy=enemy1;
                        break;

                        case 2:
                        enemy=enemy2;
                        break;

                        case 3:
                        enemy=enemy3;
                        break;

                        case 4:
                        enemy=enemy4;
                        break;
                        
                        case 5:
                        enemy=enemy5;
                        break;

                        case 6:
                        enemy=enemy6;
                        break;
                    }
                    
                    // CombatTile enemy = grid.getTiles()[i,j];
                    bool enemyMoved = false;
                    Debug.Log("Enemy" + enemyTrack + " : Y Coord "+ enemy.getYCoord());
                    
                    Debug.Log(enemyNum +" Enemy at" + enemy.getYCoord());
                    if (!enemyMoved) { // in front near
                        for(int k = 1; k <= 3; ++k) { // -occupied and friendly-
                            if (!enemyMoved) {
                                if (enemy.getXCoord() - k >= 0){ // checks for in bounds
                                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getTileUnit().getIsFriendly()) {
                                        // enemy is in front
                                        Debug.Log("Straight ");
                                        if(k==1){
                                            Debug.Log("S 1");
                                            grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord(),enemy.getYCoord());  // attack
                                            enemyMoved = true;
                                            // move back if needs balancing - roll crit? check for in bounds
                                        }
                                        else if (k==2){  // if friendlys are 2 spaces directly infront - move and attack
                                        Debug.Log("S 2");
                                            // move one space and attack
                                            if (!grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()].getIsOccupied()) {
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]);
                                                enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                                grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                enemyMoved = true;
                                            }
                                        }
                                        else if (k==3) {
                                            if (!grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()].getIsOccupied()) {
                                                Debug.Log("S 3 This might be too powerful");
                                                if (checkCrit(enemy.getTileUnit().getCritRate())&& (!grid.getTiles()[enemy.getXCoord()-2, enemy.getYCoord()].getIsOccupied())) { // if crit hits, move 2 and attack
                                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-2, enemy.getYCoord()]); // move 2
                                                        enemy = grid.getTiles()[enemy.getXCoord()-2,enemy.getYCoord()];
                                                        grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord()); // attack
                                                        enemyMoved = true;
                                                }
                                                else { // move only one
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]); // move 1
                                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                                    enemyMoved = true;
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
                                            Debug.Log(" Above");
                                            if(k==1){ 
                                                Debug.Log("A 1");
                                                // check for empty space above 
                                                // move up and attack 
                                                if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                    grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                    enemyMoved = true;
                                                }
                                            }
                                            else if (k==2){
                                                Debug.Log("A 2");  // if friendlys are up 1 over 2 left
                                                if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                    if (checkCrit(enemy.getTileUnit().getCritRate())) { // if crit hits, move up 1, 1 left and attack
                                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()+1]);
                                                        enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()+1];
                                                        grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                        enemyMoved = true;
                                                    }
                                                    else { // else just move up 1
                                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]);
                                                        enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                        enemyMoved = true;
                                                    }
                                                }
                                            }
                                            else if (k==3) {
                                                Debug.Log("A 3");
                                                if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1].getIsOccupied()) { // check for empty space above
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()+1]); // move up one square
                                                    enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()+1];
                                                    enemyMoved = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            if (!enemyMoved) { // below near
                                if (enemy.getYCoord()-1 > 0) {       // check underneath
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
                                                enemyMoved = true;
                                            }
                                        }
                                        else if (k==2){  // if friendlys are up 1 over 2 left 
                                            Debug.Log("B 2");
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1].getIsOccupied()) { // check for empty space above
                                                if (checkCrit(enemy.getTileUnit().getCritRate())) { // if crit hits, move down 1, 1 left and attack
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()-1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()-1];
                                                    grid.attack(enemy.getTileUnit().getDamage(), enemy.getTileUnit().getCritRate(),enemy.getXCoord()-1,enemy.getYCoord());
                                                    enemyMoved = true;
                                                }
                                                else { // else just move up 1
                                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1]);
                                                    enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()-1];
                                                    enemyMoved = true;
                                                }
                                            }
                                        }
                                        else if (k==3) {
                                            Debug.Log("B 3");
                                            if (!grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1].getIsOccupied()) { // check for empty space above
                                                // move up one square
                                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord(), enemy.getYCoord()-1]);
                                                enemy = grid.getTiles()[enemy.getXCoord(),enemy.getYCoord()-1];
                                                enemyMoved = true;
                                            }
                                        }

                                    }    

                                }
                            }
                        }
                    }
                    // checks full board
                    enemyMoved = true; // REMOVE
                    if (!enemyMoved) { // last resort, search full board
                        for(int q = 0; q < 7; ++q) { 
                            for (int s = 0; s < 3; ++s) {
                                if (grid.getTiles()[q,s].getIsOccupied() && grid.getTiles()[q,s].getTileUnit().getIsFriendly()) {
                                    grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()- 1, enemy.getYCoord()]);
                                    enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                    if (checkCrit(0.66f)) {
                                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()-1, enemy.getYCoord()]);
                                        enemy = grid.getTiles()[enemy.getXCoord()-1,enemy.getYCoord()];
                                        Debug.Log("No enemy spotted, moving forward");
                                        enemyMoved = true;
                                    }
                                    else{
                                        Debug.Log("Imma stand here I guess");
                                        enemyMoved = true;
                                    }
                                }
                            }
                        }
                    }
                
        // check row 
                // check row for friendlies, them move up to 2 tiles upon percent chance.
                // check grid for friendlies, if friendlies move foward, w/ 66% chance

        /* in progress
            for(int k = 1; k < 7; k++) {
                if (enemy.getXCoord() - k >= 0 && (enemy.getXCoord() + k <= 6)) {
                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()-1].getTileUnit().getIsFriendly()) {
                    
                    }
                    else if (grid.getTiles()[enemy.getXCoord() + k, enemy.getYCoord()].getIsOccupied() && grid.getTiles()[enemy.getXCoord() + k, enemy.getYCoord()-1].getTileUnit().getIsFriendly()){   
                    
                    }
                }
            }

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
        Debug.Log("Enemy" + enemyTrack + " ended at : Y Coord "+ enemy.getYCoord());
        }
        RefreshHighlights();
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
                    Debug.Log("Base");
                    if (!grid.getTiles()[i,j].getIsOccupied()) {
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                        Debug.Log("Summoning");
                    }
                }
            }
        }
        
        RefreshHighlights();
     
        Debug.Log("Summoning Creature");
    }
    public void SummonCreature(CombatUnit unit1, CombatTile unitLoc) {

        if (!unitLoc.getIsOccupied()) {
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
                if(grid.getTiles()[i,j].getIsOccupied() && (grid.getTiles()[i,j].getTileUnit().getIsDefeated()) ) {
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

    public void ReDebug(){
        
        Debug.Log("Debug function called");
        // for tests
        
    }
    

// made with love <3

}
