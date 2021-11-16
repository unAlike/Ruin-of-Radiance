using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;


/* to do list 

Functions
- COMBAT LOGIC psuedocode functions
- highlight tiles
- load in enemies
- attack logic

- spawnCreature
- recallCreature
- combatTile - snapUnit


-get Select tile (unit) working by clicking on unit
-determine if slected tile is ocupied and friendly
-display stats
-move by clicking
-apply movement to all units not just character
- figure out end turn

-Disable grid box collider after combat started.

-attack animation

Enemy Combat AI
-Health for enemy Creatures is displayed?
-Spawn Friendly creatures
-Health for friendly creatures

*/

public class CombatLogic : MonoBehaviour {
    [SerializeField]
    List<CombatTile> units;
    Movement moveScript;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0, 1);
    CombatUnit Character;
    public CombatTile selectedTile = new CombatTile(0, 0);
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


        //grid.tiles[0, 1].createUnit(Character);
        //grid.tiles[0, 1].setIsOccupied(true);
        Character.setIsFriendly(true);
        // Character.setActionPoints(3);
        // Debug.Log("Action Points:" + Character.getActionPoints());
        Debug.Log("Finished Start");
    }
    void Update() {
        if (selectedTile != null) {
            //Debug.Log("X: " + selectedTile.xCoord + " Y: " + selectedTile.yCoord);
        }

    }
    public void createPlayer() {
        Character = new CombatUnit(GameObject.Find("Character"),stats.maxHealth,stats.health,stats.damage,1,stats.critRate,true,false);
        grid.getTiles()[0,1].setTileUnit(Character);
        grid.getTiles()[0,1].setIsOccupied(true);
        // Character = new Unit();
        // Character.unitSprite = GameObject.Find("Dynamic Sprite");
        // Debug.Log("Player Created "); 
    }
    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Collision in logic");
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
        
    }
    public void endTurn() {

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

            Debug.Log("X:" + x + " Y:" + y); 

            if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].GetTileUnit().getIsFriendly()) { // select
                grid.selectedTile = grid.getTiles()[x,y];
                grid.selectedTile.setHighlight(4);
                Debug.Log("Select Tile");
            }
            else if (!grid.getTiles()[x,y].getIsOccupied()){ // move
                // stats.actionPoints;
                
                grid.moveTile(grid.selectedTile, grid.getTiles()[x,y]);
                grid.selectedTile = grid.getTiles()[x,y];
                Debug.Log("Move To Tile");
                
            }
            else if (grid.getTiles()[x,y].getIsOccupied() && !grid.getTiles()[x,y].GetTileUnit().getIsFriendly()){ // attack
                Debug.Log("Attack Tile");
            }
            else if (grid.getTiles()[x,y].getIsOccupied() && grid.getTiles()[x,y].GetTileUnit().getIsDefeated()){ // recall
            Debug.Log("Recall Tile"); 
            }
        }
    }

    public void REEEDebug(){
        

        // for tests
        
    }
    



}
