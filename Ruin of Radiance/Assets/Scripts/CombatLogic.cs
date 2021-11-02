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
    void Start(){
        moveScript = GameObject.Find("DynamicSprite").GetComponent<Movement>();
        // moveScript.inCombat = true;
    }    
    void Update()
    {
        
        // Debug.Log("character Position: " + GameObject.Find("DynamicSprite").transform.position);


    }

    void OnTriggerEnter2D(Collider2D collision) {
        startCombat();
    }
    public void startCombat() {
        // puts player into the combat scene
        Vector3 charStart = new Vector3(0.5f,-1.7f,0);    
        Vector3 gridPos = new Vector3(0,0,0); 
        GameObject character = GameObject.Find("DynamicSprite");
        gridPos = GameObject.Find("CombatGrid").GetComponent<SpriteRenderer>().transform.position + charStart;
        character.transform.position = gridPos;

        // place the player on [0,1]
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
    public void endCombat(int currentHealth) {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (currentHealth == 0)) {
            // Destroy(CombatGrid);
            Debug.Log("You have ended the battle");
        }
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

    }
    public void moveUnitTo(CombatTile unit1, int xCoord, int yCoord) {
        // play animation to move sprite
        // moving distance, limits player to that distance
        // highlightTiles(unit1.xCoord, unit1.yCoord);
        if(!tiles[xCoord,yCoord].getIsOccupied()){ // if not occupied - originally .tileUnit.getIsOccupied() n
            tiles[xCoord,yCoord].tileUnit = unit1.tileUnit; // copy unit over then DELETE OLD SPOT
            unit1.deleteUnit(); // deletes the prev unit
        

        }
        else { 
             Debug.Log(" space already taken.");
        }
         
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

    public CombatTile(){
        tileUnit = new Unit();
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