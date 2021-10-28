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
    CombatGrid grid = new CombatGrid();
    void Start(){
        
    }    
    void Update()
    {
        
    }
}
public class CombatGrid{
    public CombatTile[,] tiles;
    

    public CombatGrid(){
        tiles = new CombatTile[7,3];

    }
    public void moveUnitTo(CombatTile unit1, int xCoord, int yCoord) {
        // play animation to move sprite

        if(!tiles[xCoord,yCoord].getIsOccupied()){ // if not occupied - originally .tileUnit.getIsOccupied() n
            tiles[xCoord,yCoord].tileUnit = unit1.tileUnit; // copy unit over then DELETE OLD SPOT
            unit1.deleteUnit(); // deletes the prev unit
        }
        else { 
             Debug.Log(" space already taken.");
        }
         
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
    private GameObject unitSprite;
    private int maxHealth, currentHealth, damage;
    private double scalingNum;
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

}