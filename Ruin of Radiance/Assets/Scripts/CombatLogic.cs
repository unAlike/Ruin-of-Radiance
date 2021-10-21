using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public void moveTileTo(CombatTile unit1, int xCoord, int yCoord) {
        // play animation to move sprite

         if( !tiles[xCoord,yCoord].tileUnit.getIsOccupied()) // if not occupied
         tiles[xCoord,yCoord].tileUnit = unit1.tileUnit; // copy unit over then DELETE OLD SPOT
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
        tileUnit.setHealth(tileUnit.getHealth()-damage);
    }
    



}
[System.Serializable]
public class Unit{
    public GameObject unitSprite;
    public int maxhp, currHp,dmg;
    public double scalingNum;
    public void setHealth(int health){
        currhp=health;
    }
    public int getHealth(){
        return hp;
    }
    
    public void setDamage(int damage){
        dmg = damage;
    }
    public int getDamage(){

        return dmg;
    }

}