using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;


/* to do list 

Functions
- COMBAT LOGIC
- load in enemies as units, place them into spaces
- When in combat display the available Action points
- disable the stamina bar in combat
- create buttons to have slash and spore bomb functions
- move to tile needs to reduce the action points
- attack if enemy is in range of selected tile + remove action points

- spawnCreature
- recallCreature

- display stats
- 
- Disable grid box collider after combat started.

-attack animation

Enemy Combat AI
-Health for enemy Creatures is displayed?
-Spawn Friendly creatures
-Health for friendly creatures

*/

public class CombatLogic : MonoBehaviour {
    [SerializeField]
    List<GameObject> enemies;
    Movement moveScript;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0, 1);
    CombatUnit Character;
    CombatUnit SpawnUnit;
    PlayerStats stats;
    void Start() {
        Debug.Log("Started Logic");
        stats = GameObject.Find("Character").GetComponent<PlayerStats>();

        // makes grid invisible on start
        Color tmp = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = tmp;

        // defines movement script
        moveScript = GameObject.Find("Character").GetComponent<Movement>();
        // moveScript.inCombat = true;

        createPlayer();
        
        Character.setIsFriendly(true);
        //GetComponent<BoxCollider2D>.delete
        
        Debug.Log("Finished Start");
    }
    void Update() {

    }
    public void createPlayer() {
        Character = new CombatUnit(GameObject.Find("Character"),stats.maxHealth,stats.health,stats.damage,1,stats.critRate,true,false);
        grid.getTiles()[0,1].setTileUnit(Character);
        grid.getTiles()[0,1].setIsOccupied(true);
        // Character = new Unit();
        // Character.unitSprite = GameObject.Find("Dynamic Sprite");
        // Debug.Log("Player Created "); 
    }
    public void spawnEnemies() {
        Debug.Log("Spawning Enemies " + enemies.Count);
        for(int k = 0; k < enemies.Count; k++) {
            CombatUnit unit1 = new CombatUnit(enemies[k], 15, 15, 6, 1f,.25f, false, false);
            
            for(int i = 6; i > 3; i--) {
                for(int j = 2; j >= 0; j--) {

                    if (!grid.getTiles()[i,j].getIsOccupied()) {

                        // unit1.setUnitSprite(prefab1);
                        grid.getTiles()[i,j].setTileUnit(unit1);
                        grid.getTiles()[i,j].setIsOccupied(true);
                        grid.getTiles()[i,j].snapUnit();
                        Debug.Log("Unit Spawned to " + i + ", " + j + "Enemy Snapped");
                        goto LoopEnd;
                    }

                }
                
            }
            LoopEnd:
            Debug.Log("end");
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
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        moveScript.inCombat = true;
        // swap for snapUnit function 
        grid.getTiles()[0,1].snapUnit();
        // snap enemies to grid
        spawnEnemies();
        
    }
    public void endTurn() { // end turn button?
        enemyLogic();
        // character.fillActionPoints();
        // fill creature ap too
    }
    public void endCombat() {
        GameObject.Find("CombatGrid").SetActive(false);
        Debug.Log("You have ended the battle");
        moveScript.inCombat = false;

        // allow for collecting creatures
        // remove dead or captured creatures

    }

    public void clickTile(string name) {
        if(moveScript.inCombat == true) {
            int x = int.Parse(name.Substring(0,1));
            int y = int.Parse(name.Substring(1,1));
            grid.clearHighlights();
            RefreshHighlights();

            Debug.Log("X:" + x + " Y:" + y); 
            
            
                if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getTileUnit().getIsFriendly()) { // select
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
                else if (grid.getTiles()[x,y].getIsOccupied() && !grid.getTiles()[x,y].getTileUnit().getIsFriendly() && stats.actionPoints > 0 ){ // attack
                    Debug.Log("Attack Tile");
                }
                else if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getTileUnit().getIsDefeated()){ // recall
                Debug.Log("Recall Tile"); 
                }
                else if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].getHighlight() == Enums.highlight.Control) {
                    Debug.Log("Summoned Creature"); // if highlight is purple
                    SummonCreature(SpawnUnit, grid.getTiles()[x,y]);
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
                    for(int k = 1; i < 3; ++i) { // checks 6 squares in front of them for friendly units
                        for (int m = -1; i < 2; ++i) {
                            if (grid.getTiles()[enemy.getXCoord()- k, enemy.getYCoord() + m].getIsOccupied() &&
                            grid.getTiles()[enemy.getXCoord()- k, enemy.getYCoord() + m].getTileUnit().getIsFriendly()) {
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

        // highlight character + or - 1 in x & y
        CombatTile charLoc = grid.getTileOfUnit(Character);
        for(int i = 0; i < 5; i++) { // show where you can highlight
            for (int j = 0; j < 3; j++) {
                if (grid.getTiles()[charLoc.getXCoord() -2 , charLoc.getYCoord()-1].getIsOccupied()) {
                    grid.getTiles()[charLoc.getXCoord() -2 , charLoc.getYCoord()-1].setHighlight(Enums.highlight.Control);
                }
            }
        }
        Debug.Log("Summoning Creature");
    }
    public void SummonCreature(CombatUnit unit1, CombatTile unitLoc) {
            
        // CombatUnit cre = new CombatUnit(unit1, 15, 15, 6, 1f,.25f, false, false);

        if (!unitLoc.getIsOccupied()) {
            // unit1.setUnitSprite(prefab1);
            grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].setTileUnit(unit1);
            grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].setIsOccupied(true);
            grid.getTiles()[unitLoc.getXCoord(),unitLoc.getYCoord()].snapUnit();
            Debug.Log("Unit Spawned to " + unitLoc.getXCoord() + ", " + unitLoc.getYCoord() + "Enemy Snapped");
        }
        
        // highlight where you can put the unit (+- 1 in all directions)
        // create unit on selected square

        // subtract MC energy & action points
        // subtract from inventory total depending on the unit type
        Debug.Log("Summoned Creature to [" + ", " + "]");
    }
    public void recallCreature(CombatUnit unit1) {
        // find unit and remove getTileOfUnit(unit1)

        // inventory "x1 Unit"
        // PlayerStats.numOfRats = PlayerStats.numOfRats+1;
        // add to inventory total depending on the unit type 

        // subtract MC energy
        Debug.Log("Recalled creature from [" + ", " + "]");
    }
    public void RefreshHighlights() {
        for(int i = 0; i < 7 ;++i) {
            for (int j = 0; j<3 ;++j) {
                Debug.Log(i + ", " + j);
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
                    }
                    else if(grid.getTiles()[i,j].getIsOccupied() && (grid.getTiles()[i,j].getTileUnit().getIsDefeated()) ) {
                        grid.getTiles()[i,j].setHighlight(Enums.highlight.Control);
                    }
                }
            }
        }
        RefreshHighlights();
    }
    public int GetDistanceBetweenTiles(CombatTile from, CombatTile to) {
        return (Math.Abs((from.getXCoord()-to.getXCoord())))+(Math.Abs(from.getYCoord()-to.getYCoord()));
    }
    public void REEEDebug(){
        
        Debug.Log("Debug function called");
        // for tests
        
    }
    



}
