using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/* to do list 
function to get tile space to world space

implement move tile to delete and create tiles

enemy combat decisions

trigger combat encounter
turns on grid visuals and overlays

*/

public class CombatLogic : MonoBehaviour
{
    [SerializeField]
    List<CombatTile> units;
    Movement moveScript;
    CombatGrid grid = new CombatGrid();
    CombatTile activeTile = new CombatTile(0,1);
    Unit Character;
    void Start(){
        moveScript = GameObject.Find("DynamicSprite").GetComponent<Movement>();
        // moveScript.inCombat = true;
        createPlayer();
        
        
        grid.tiles[0,1].createUnit(Character);
    }    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            endCombat();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            int xCoord = grid.findUnit(Character).xCoord;
            int yCoord = grid.findUnit(Character).yCoord;
            grid.moveUnitTo(grid.findUnit(Character),xCoord+1,yCoord);
            Debug.Log("Unit moved right");
    
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            int xCoord = grid.findUnit(Character).xCoord;
            int yCoord = grid.findUnit(Character).yCoord;
            grid.moveUnitTo(grid.findUnit(Character),xCoord,yCoord-1);
            Debug.Log("Unit moved down");
    
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            int xCoord = grid.findUnit(Character).xCoord;
            int yCoord = grid.findUnit(Character).yCoord;
            grid.moveUnitTo(grid.findUnit(Character),xCoord,yCoord+1);
            Debug.Log("Unit moved up");
    
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            int xCoord = grid.findUnit(Character).xCoord;
            int yCoord = grid.findUnit(Character).yCoord;
            grid.moveUnitTo(grid.findUnit(Character),xCoord-1,yCoord);
            Debug.Log("Unit moved left");
    
        }
        
        // Debug.Log("character Position: " + GameObject.Find("DynamicSprite").transform.position);


    }
    public void createPlayer(){
        Character = new Unit();
        Character.unitSprite = GameObject.Find("DynamicSprite"); 

    }

    void OnTriggerEnter2D(Collider2D collision) {
        startCombat();
    }
    public void startCombat() {
        // puts player into the combat scene
        Vector3 charStart = new Vector3(0.5f,-1.7f,0);    
        Vector3 gridPos = new Vector3(0,0,0); 
        GameObject charSprite = GameObject.Find("DynamicSprite");
        gridPos = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().transform.position + charStart;
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

    public void endTurn(Unit character) {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Debug.Log("Your turn has been ended");
        }
        // activate the enemy movements
        // refresh action points once enemy movements are made
        character.fillActionPoints();
    }
    public void endCombat() {
        GameObject.Find("CombatGrid").SetActive(false);
        Debug.Log("You have ended the battle");
        moveScript.inCombat = false;

        // allow for collecting creatures
        // turn grid opacity off
        // toggle movement on 
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

    public void moveUnitTo(CombatTile unit1, int xCoord, int yCoord) {
        // play animation to move sprite
        // moving distance, limits player to that distance
        // highlightTiles(unit1.xCoord, unit1.yCoord);
        if(!tiles[xCoord,yCoord].getIsOccupied() && xCoord < 7 && xCoord > -1 && yCoord < 3 && yCoord > -1 ){ // if not occupied - originally .tileUnit.getIsOccupied() n
            tiles[xCoord,yCoord].tileUnit = unit1.tileUnit; // copy unit over then DELETE OLD SPOT
            unit1.deleteUnit(); // deletes the prev unit
            Debug.Log("Moved Unit to " + unit1.xCoord + ", " + unit1.yCoord);
        }
        else { 
             Debug.Log(" space already taken or out of bounds");
        }
         
    }
    public CombatTile findUnit(Unit unit) {
        for (int i = 0; i < 7;++i) {

            for (int j = 0; j < 3;++j) {
            // tiles[i,j];
                if (tiles[i,j].tileUnit == unit) {
                    Debug.Log("Unit Found");
                    return tiles[i,j];
                }
            }
        }
        Debug.Log("Unit not Found");
        return tiles[-1,-1];

    }
    public void highlightTiles(int xCoord, int yCoord) {
        // highlights gridUnit (xCoord + ActionPoints, yCoord)
        // highlights gridUnit (xCoord, yCoord + ActionPoints)
        // highlights gridUnit (xCoord - ActionPoints, yCoord)
        // highlights gridUnit (xCoord, yCoord - ActionPoints)

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
    public void setactionPoints(int ap){ // for removing action points
       this.actionPoints = ap;
    }
    public int getactionPoints(){
        return this.actionPoints;
    }
    public void fillActionPoints() { // end of turn fills actions points
        this.actionPoints = maxActionPoints;
    }

}