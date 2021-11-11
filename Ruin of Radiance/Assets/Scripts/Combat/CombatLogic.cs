using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;


/* to do list 
-add debugs for actions
Debug.Log(" ");

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
    PlayerStats stats;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0, 1);
    Unit Character;
    public CombatTile selectedTile = new CombatTile(0, 0);
    void Start() {
        // makes grid invisible on start
        stats = GameObject.Find("Dynamic Sprite").GetComponent<PlayerStats>();
        
        Color tmp = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = tmp;

        // defines movement script
        moveScript = GameObject.Find("Dynamic Sprite").GetComponent<Movement>();
        // moveScript.inCombat = true;

        createPlayer();


        grid.tiles[0, 1].createUnit(Character);
        grid.tiles[0, 1].setIsOccupied(true);
        Character.setIsFriendly(true);
        Character.setActionPoints(3);
        Debug.Log("Action Points:" + Character.getActionPoints());
    }
    void Update() {
        if (selectedTile != null) {
            // Debug.Log("X: " + selectedTile.xCoord + " Y: " + selectedTile.yCoord);
        }


        if (moveScript.inCombat) {
            // Debug.Log("Started Highlight");
            // selectedTile.highlightTiles(grid.findUnit(Character).xCoord, grid.findUnit(Character).yCoord);

            /* no need for keyboard input
            if (Input.GetKeyDown(KeyCode.Escape)) {
                endCombat();
            } else if (Input.GetKeyDown(KeyCode.RightArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveRight = new Vector3(.5f, 0, 0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character), xCoord + 1, yCoord, moveRight);
                Debug.Log("Unit moved right");
                Debug.Log("Action Points:" + Character.getActionPoints());

            } else if (Input.GetKeyDown(KeyCode.DownArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveDown = new Vector3(0, -0.5f, 0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character), xCoord, yCoord - 1, moveDown);
                Debug.Log("Unit moved down");
            } else if (Input.GetKeyDown(KeyCode.UpArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveUp = new Vector3(0, .5f, 0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character), xCoord, yCoord + 1, moveUp);
                Debug.Log("Unit moved up");

            } else if (Input.GetKeyDown(KeyCode.LeftArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveLeft = new Vector3(-0.5f, 0, 0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character), xCoord - 1, yCoord, moveLeft);
                Debug.Log("Unit moved left");


            } else if (Input.GetKeyDown(KeyCode.RightAlt)) {
                //int xCoord = grid.findUnit(Character).xCoord;
                //int yCoord = grid.findUnit(Character).yCoord;
                // grid.moveUnitTo(grid.findUnit(Character),xCoord-1,yCoord);
                //                Change to selected Unit instead of character
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                int damage = grid.findUnit(Character).tileUnit.getDamage();
                grid.basicAttack(grid.findUnit(Character), xCoord, yCoord, damage);
                Debug.Log("Unit Attacked");
            }
            */
        }

        // Debug.Log("character Position: " + GameObject.Find("DynamicSprite").transform.position);


    }
    public void createPlayer() {
        Character = new Unit();
        Character.unitSprite = GameObject.Find("Dynamic Sprite");
        // Debug.Log("Player Created "); 
    }
    void OnTriggerEnter2D(Collider2D collision) {
        //Starts Combat
        startCombat();
    }
    public void startCombat() {
        // GameObject.Find("CombatGrid").SetActive(true); // makes the grid visable
        // GameObject.Find("CombatGrid").opacity(1);
        // puts player into the combat scene
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        //Offset for Sprites
        Vector3 standardOffset = new Vector3(0.5f, -1.7f, 0);
        Vector3 gridPos = new Vector3(0, 0, 0);
        GameObject charSprite = GameObject.Find("Dynamic Sprite");
        gridPos = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().transform.position + standardOffset;
        charSprite.transform.position = gridPos;

        // Unit Character = new Unit();
        // grid.tiles[0,1].createUnit(Character); // places character on [0,1]
        grid.tiles[6, 2].createUnit(units[2].tileUnit); // places enemies 1,2,3 on their locations
        grid.tiles[6, 1].createUnit(units[1].tileUnit);
        grid.tiles[6, 0].createUnit(units[0].tileUnit);
        grid.tiles[5, 2].createUnit(units[3].tileUnit); // places enemies 4,5,6 on their locations
        grid.tiles[5, 1].createUnit(units[4].tileUnit);
        grid.tiles[5, 0].createUnit(units[5].tileUnit);

        // place the player on [0,1]
        // units[1].tileUnit.getHealth();

        // toggle movement off
        // Enemies placed in combat grid
        // turns on combat overlay
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
}
