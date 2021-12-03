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
         for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                if (grid.getTiles()[i,j].getIsOccupied() && grid.getTiles()[i,j].getTileUnit().getIsFriendly()== false) { // if occupied and enemy
                    CombatTile enemy = grid.getTiles()[i,j]; 
                    // int movementValue = 0;

                     //if ((enemy.getXCoord() + movementValue > 0) || enemy.getXCoord() + movementValue > 0 ) {   }
                     // movement 0-2 or up to 3 for birds, can move 1 and attack same turn

            // before all check that x and y are inside the borders
        
            // checks in front of enemy   
            for(int k = 1; k < 3; k++) { // -occupied and friendly-
                if (enemy.getXCoord() - k >= 0){ // checks for in bounds
                    if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()].getTileUnit().getIsFriendly()) {
                        // enemy is in front
                        Debug.Log("Enemy Option 1");
                        if(k==1){
                            // attack
                            // move back if needs balancing - roll crit? check for in bounds
                        }
                        else if (k==2){  // if friendlys are 2 spaces directly infront - move and attack
                            // move one space and attack
                        }
                        else if (k==3) {
                            // check critVal
                            // if crit hits, move 2 and attack
                            // else move 1 or 2, one if needs balancing
                        }

                    }
                }
            }
            
            for(int k = 1; k < 3; k++) { // -occupied and not friendly-
                if (enemy.getXCoord() - k >= 0){ // checks for in bounds
                    if (enemy.getYCoord() + 1 < 3) { // check underneath too
                    
                        if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord() + 1].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()+1].getTileUnit().getIsFriendly()) {
                            // enemy is above
                            Debug.Log("Enemy Option 2 Above");
                            if(k==1){
                                // check for empty space above 
                                // move up and attack

                            }
                            else if (k==2){  // if friendlys are up 1 over 2 left 
                                // if crit hits, move up 1, 1 left and attack
                                // else just move up 1
                            }
                            else if (k==3) {
                                // check for empty space above
                                // move up one square
                            }

                        }
                            // check underneath
                        if (grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord() - 1].getIsOccupied() && grid.getTiles()[enemy.getXCoord() - k, enemy.getYCoord()-1].getTileUnit().getIsFriendly()) {
                            // enemy is in front
                            Debug.Log("Enemy Option 2 Below");
                            if(k==1){
                                // check for empty space above 
                                // move up and attack

                            }
                            else if (k==2){  // if friendlys are up 1 over 2 left 
                                // if crit hits, move up 1, 1 left and attack
                                // else just move up 1
                            }
                            else if (k==3) {
                                // check for empty space above
                                // move up one square
                            }

                        }    

                    }
                }
            }
                
        // check above near
                // if friendly characters are above or below 1 and 2 left, try to move up if +1 up is unoccupied
        // check row 
                // check row for friendlies, them move up to 2 tiles upon percent chance.
        // check above, then below row
                // else check row 1 up, or 1 down move accordingly if possible
        // danger response
                // low health is getTileUnit().getMaxHealth() / 4 > getTileUnit().getHealth();
                // if low health, attack and move backwards

                // scatter if low health for birds?

                // special logic for "custom enemies" aka bosses

                // wait / coroutine inbetween enemy movements
                // doesn't move if there is creature in that position


                    
                    for(int k = 1; i < 3; ++k) { // checks 6 squares in front of them for friendly units
                        for (int m = -1; i < 2; ++m) {
                            if (grid.getTiles()[enemy.getXCoord()- k, enemy.getYCoord() + m].getIsOccupied() && grid.getTiles()[enemy.getXCoord()- k, enemy.getYCoord() + m].getTileUnit().getIsFriendly()) {
                                grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()- 1, enemy.getYCoord()]);
                            }
                        }
                    }
                    if (Random.value < .66) { // 2/3 chance they will move
                        grid.moveTile(enemy, grid.getTiles()[enemy.getXCoord()- 1, enemy.getYCoord()]);
                        Debug.Log("Enemy moved closer");
                    }
                    else {
                        Debug.Log("Enemy stood there menacingly, play sound?");
                    }
                


                
                }
            }
        }
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
    



}
