 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Random=UnityEngine.Random;


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

public class CombatLogic : MonoBehaviour
{
    [SerializeField]
    List<CombatTile> units;
    Movement moveScript;
    public CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0,1);
    Unit Character;
    public CombatTile selectedTile = new CombatTile(0,0);
    void Start(){
        // makes grid invisible on start
        Color tmp = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = tmp;

        // defines movement script
        moveScript = GameObject.Find("Dynamic Sprite").GetComponent<Movement>();
        // moveScript.inCombat = true;
        
        createPlayer();
        
        
        grid.tiles[0,1].createUnit(Character);
        grid.tiles[0,1].setIsOccupied(true);
        Character.setIsFriendly(true);
        Character.setActionPoints(3);
        Debug.Log("Action Points:" + Character.getActionPoints());
    }    
    void Update()
    {
        if(selectedTile!=null){
            Debug.Log("X: " + selectedTile.xCoord + " Y: " + selectedTile.yCoord);
        }


        if (moveScript.inCombat) {
            // Debug.Log("Started Highlight");
            // selectedTile.highlightTiles(grid.findUnit(Character).xCoord, grid.findUnit(Character).yCoord);
            

            if (Input.GetKeyDown(KeyCode.Escape)) {
                endCombat();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveRight = new Vector3(.5f,0,0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character),xCoord+1,yCoord,moveRight);
                Debug.Log("Unit moved right");
                Debug.Log("Action Points:" + Character.getActionPoints());
            
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveDown = new Vector3(0,-0.5f,0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character),xCoord,yCoord-1,moveDown);
                Debug.Log("Unit moved down");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveUp = new Vector3(0,.5f,0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character),xCoord,yCoord+1,moveUp);
                Debug.Log("Unit moved up");
        
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && Character.getActionPoints() > 0) {
                grid.clearHighlight();
                Vector3 moveLeft = new Vector3(-0.5f,0,0);
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                grid.moveUnitTo(grid.findUnit(Character),xCoord-1,yCoord,moveLeft);
                Debug.Log("Unit moved left");
                
        
            }
            else if (Input.GetKeyDown(KeyCode.RightAlt)) {
                //int xCoord = grid.findUnit(Character).xCoord;
                //int yCoord = grid.findUnit(Character).yCoord;
                // grid.moveUnitTo(grid.findUnit(Character),xCoord-1,yCoord);
                //                Change to selected Unit instead of character
                int xCoord = grid.findUnit(Character).xCoord;
                int yCoord = grid.findUnit(Character).yCoord;
                int damage = grid.findUnit(Character).tileUnit.getDamage();
                grid.basicAttack(grid.findUnit(Character),xCoord,yCoord,damage);
                Debug.Log("Unit Attacked");
            }
        }
        
        // Debug.Log("character Position: " + GameObject.Find("DynamicSprite").transform.position);


    }
    public void createPlayer(){
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
        GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);

        //Offset for Sprites
        Vector3 standardOffset = new Vector3(0.5f,-1.7f,0);  
        Vector3 gridPos = new Vector3(0,0,0);
        GameObject charSprite = GameObject.Find("Dynamic Sprite");
        gridPos = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().transform.position + standardOffset;
        charSprite.transform.position = gridPos;
    
        // Unit Character = new Unit();
        // grid.tiles[0,1].createUnit(Character); // places character on [0,1]
        grid.tiles[6,2].createUnit(units[2].tileUnit); // places enemies 1,2,3 on their locations
        grid.tiles[6,1].createUnit(units[1].tileUnit);
        grid.tiles[6,0].createUnit(units[0].tileUnit);
        grid.tiles[5,2].createUnit(units[3].tileUnit); // places enemies 4,5,6 on their locations
        grid.tiles[5,1].createUnit(units[4].tileUnit);
        grid.tiles[5,0].createUnit(units[5].tileUnit);

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
public class CombatGrid{
    public CombatTile[,] tiles;
    

    public CombatGrid(){
        tiles = new CombatTile[7,3];
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
            tiles[i,j] = new CombatTile(i,j);
            }
        }
       
    }

    public void moveUnitTo(CombatTile tile1, int xCoord, int yCoord,Vector3 moveVector) {
        try {
            // play animation to move sprite
            // currently limits the distance but throws error when out of bounds FIXME
            // highlightTiles(unit1.xCoord, unit1.yCoord);
            if(!tiles[xCoord,yCoord].getIsOccupied() && xCoord < 8 && xCoord > -1 && yCoord < 3 && yCoord > -1 ){ // if not occupied - originally .tileUnit.getIsOccupied() n
                tile1.tileUnit.actionPoints = tile1.tileUnit.actionPoints -1; 
                Debug.Log("Action Points:" + tile1.tileUnit.getActionPoints());
                tiles[xCoord,yCoord].tileUnit = tile1.tileUnit; // copy unit over then DELETE OLD SPOT
                GameObject.Find("CombatGrid").GetComponent<CombatLogic>().grid.findUnit(tile1.tileUnit).deleteUnit(); // deletes the prev unit
                Debug.Log("Moved Unit to " + tiles[xCoord,yCoord].xCoord + ", " + tiles[xCoord,yCoord].yCoord);
                // move sprite to appropriate tile
                // moveVector = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().transform.position + moveVector;
                GameObject unitSprite = GameObject.Find("Dynamic Sprite");
                unitSprite.transform.Translate(moveVector);

            }
            else { 
                Debug.Log(" space already taken or out of bounds");
            }

        }

        catch(IndexOutOfRangeException) {
           Debug.Log("You can't run away!");
        }


    }

    public void basicAttack(CombatTile unit1, int xCoord, int yCoord,int dmg) {
        if(tiles[xCoord,yCoord].getIsOccupied()) {
            if (tiles[xCoord,yCoord].tileUnit.getIsFriendly() == false){ // unit in space is enemy

            tiles[xCoord,yCoord].takeDamage(dmg);
            Debug.Log(""+ xCoord + ", "+ yCoord + " should have taken damage");
            unit1.tileUnit.setActionPoints(-1);
            tiles[xCoord,yCoord].tileUnit.setHealth(-dmg);
            Debug.Log(tiles[xCoord,yCoord].tileUnit.getHealth() + " is the enemies health");
            }
            else {
                Debug.Log(""+ xCoord + ", "+ yCoord + " is a Friendly Unit");
            }
        }
    }
    public void specialAttack() {
        
    }
    public void rangedAttack() {
        
    }
    public CombatTile findUnit(Unit unit) {
        for (int i = 0; i < 7;++i) {

            for (int j = 0; j < 3;++j) {
            // tiles[i,j];
                if (tiles[i,j].tileUnit == unit) {
                    // Debug.Log("Unit Found");
                    return tiles[i,j];
                }
            }
        }
        Debug.Log("Unit not Found");
        return tiles[-1,-1];

    }
    public void highlightTiles(int xCoord, int yCoord, int range) {
        // highlight right
        
        // Debug.Log("Taken in " + xCoord + ", " + yCoord + ", " + ap + ": Action Points");
        
        SpriteRenderer grid = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>();
        
        for (int i = 0; i < range; ++i) { // highlight to the right 0 - 6
        GameObject moveSquare = GameObject.Find("tileMoveOverlay"+i);
        GameObject damageSquare = GameObject.Find("tileDamageOverlay"+i);
        GameObject square = moveSquare;
        Vector3 orignalPos = square.transform.position;
            if(tiles[xCoord,yCoord].getIsOccupied() && !tiles[xCoord,yCoord].tileUnit.getIsFriendly()) {
                square = damageSquare;
            } //enemy in square
            if(xCoord + i < 6)
            square.transform.position = grid.transform.position + (new Vector3(i,0,0))+ (new Vector3(1,-2,0)) + (new Vector3(xCoord,yCoord,0));

        
            //square.transform.position = grid.transform.position;
        }
        int j = 6;
        for (int i = 0; i < range; ++i) { // highlight to the left 6 - 11
            
            GameObject moveSquare = GameObject.Find("tileMoveOverlay"+j);
            GameObject damageSquare = GameObject.Find("tileDamageOverlay"+j);
            GameObject square = moveSquare;
            // Vector3 orignalPos = square.transform.position;
            if(tiles[xCoord,yCoord].getIsOccupied() && !tiles[xCoord,yCoord].tileUnit.getIsFriendly()) {
                square = damageSquare;
            } //enemy in square
            if(xCoord - i > 0)
            square.transform.position = grid.transform.position + (new Vector3(-i,0,0))+ (new Vector3(-1,-2,0))+ (new Vector3(xCoord,yCoord,0));

            //square.transform.position = grid.transform.position;
        j++;
        }
        j = 12;
        for (int i = 0; i < range; ++i) { // highlight to the up 12-13
            
            GameObject moveSquare = GameObject.Find("tileMoveOverlay"+j);
            GameObject damageSquare = GameObject.Find("tileDamageOverlay"+j);
            GameObject square = moveSquare;
            // Vector3 orignalPos = square.transform.position;
            if(tiles[xCoord,yCoord].getIsOccupied() && !tiles[xCoord,yCoord].tileUnit.getIsFriendly()) {
                square = damageSquare;
            } //enemy in square
            if(yCoord + i < 2)
            square.transform.position = grid.transform.position + (new Vector3(0,i,0))+ (new Vector3(0,-1,0))+ (new Vector3(xCoord,yCoord,0));

            //square.transform.position = grid.transform.position;
        j++;
        }
        j=14;
        for (int i = 0; i < range; ++i) { // highlight to the up 14-15
            
            GameObject moveSquare = GameObject.Find("tileMoveOverlay"+j);
            GameObject damageSquare = GameObject.Find("tileDamageOverlay"+j);
            GameObject square = moveSquare;
            // Vector3 orignalPos = square.transform.position;
            if(tiles[xCoord,yCoord].getIsOccupied() && !tiles[xCoord,yCoord].tileUnit.getIsFriendly()) {
                square = damageSquare;
            } //enemy in square
        
            if(yCoord - i > 0)
            square.transform.position = grid.transform.position + (new Vector3(0,-i,0))+ (new Vector3(0,-3,0))+ (new Vector3(xCoord,yCoord,0));

            //square.transform.position = grid.transform.position;
        j++;
        }
        
        Debug.Log("Drew Highlights");

    }
    public void clearHighlight() {
        SpriteRenderer grid = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>();
        
        for (int i = 0; i < 21; ++i) { // highlight to the right 0 - 6
            GameObject moveSquare = GameObject.Find("tileMoveOverlay"+i);
            moveSquare.transform.position = (new Vector3(10000,-20000,0));
            
            // Vector3 orignalPos = square.transform.position;
            // grid.transform.position + (new Vector3(i,0,0))+
            //square.transform.position = grid.transform.position;
        }

        Debug.Log("Finished Clearing Tiles");

    }

    public void selectTile(int xCoord, int yCoord){ // matters once we can spawn friendly creatures
        
        //Debug.Log("X: "+ xCoord + "  Y: " + yCoord);
        CombatGrid grid = GameObject.Find("CombatGrid").GetComponent<CombatLogic>().grid;
        CombatTile selectedTile = GameObject.Find("CombatGrid").GetComponent<CombatLogic>().selectedTile;
        
        if(tiles[xCoord,yCoord].getIsOccupied() && tiles[xCoord,yCoord].tileUnit.getIsFriendly()){ // if space is a unit
            selectedTile = tiles[xCoord, yCoord];
            Debug.Log("Selected Unit?");
            grid.highlightTiles(selectedTile.xCoord,selectedTile.yCoord,selectedTile.tileUnit.getActionPoints());
            
        }
        else if(tiles[xCoord,yCoord].getIsOccupied() && !tiles[xCoord,yCoord].tileUnit.getIsFriendly()){ // detects enemy
            Debug.Log("Attack Enemy");
            // basicAttack();
        }
        else if(!tiles[xCoord,yCoord].getIsOccupied()) { //empty space
            Vector3 vect = new Vector3(xCoord-selectedTile.xCoord, yCoord-selectedTile.yCoord, 0);
            //moveUnitTo
            //GameObject.Find("CombatGrid").GetComponent<CombatLogic>().selectedUnit
            grid.clearHighlight();
            Debug.Log("Running movement script");
            grid.moveUnitTo(selectedTile,xCoord,yCoord,vect);
            Debug.Log("x: "+ xCoord + " y: "+ yCoord + " fdsdsfwsjdhgfkjhsdagf  x:" + tiles[xCoord,yCoord].xCoord + " y:" + tiles[xCoord,yCoord].yCoord);
            selectedTile = tiles[xCoord,yCoord];

        }
        else{
            Debug.Log("None of the scripts ran");
        }
        Debug.Log("I AM RUNNING");
        GameObject.Find("CombatGrid").GetComponent<CombatLogic>().selectedTile = selectedTile;
    }
}
[System.Serializable]
public class CombatTile{
    public Unit tileUnit;
    public int xCoord, yCoord;
    private bool isOccupied;

    public CombatTile(int x, int y){
        tileUnit = new Unit();
        xCoord = x;
        yCoord = y;

    }
    public void setIsOccupied(bool set){
        isOccupied = set;
    }
    public bool getIsOccupied(){
        return isOccupied;
    }
    public void deleteUnit() {
        tileUnit = null;
    }
    public void createUnit(Unit unit1){
        tileUnit = unit1;
        
    }
    public void takeDamage(int damage ) {
        // randomize damage +- 2
        
        int num1 = Random.Range(-1,1);
        
        int damageDelt = num1 + damage;
        Debug.Log("Damage output is " + damageDelt);

        tileUnit.setHealth(tileUnit.getHealth()-damage);
    }
    
    



}
[System.Serializable]
public class Unit{
    public GameObject unitSprite;
    public int maxHealth, currentHealth, damage, maxActionPoints, actionPoints;
    public double scalingNum;
    public bool isFriendly;
    public void setHealth(int health){
        currentHealth=health;
    }
    public int getHealth(){
        return currentHealth;
    }
    public void setDamage(int damage){
       this.damage = damage;
    }
    public int getDamage(){
        return this.damage;
    }
    public void setActionPoints(int ap){ // for removing action points
       this.actionPoints = ap;
    }
    public int getActionPoints(){
        return this.actionPoints;
    }
    public void fillActionPoints() { // end of turn fills actions points
        this.actionPoints = maxActionPoints;
    }
    public void setIsFriendly(bool friendly){
        isFriendly = friendly;
    }
    public bool getIsFriendly(){
        return isFriendly;
    }

}