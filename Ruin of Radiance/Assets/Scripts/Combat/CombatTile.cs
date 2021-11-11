using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatTile{
    private CombatUnit tileUnit;
    private int xCoord, yCoord;
    private bool isOccupied;
    private int highlight;

// Check this
    public CombatTile(int x, int y){
        tileUnit = new CombatUnit();
        xCoord = x;
        yCoord = y;
    }
    public void setIsOccupied(bool set){
        isOccupied = set;
    }
    public bool getIsOccupied(){
        return isOccupied;
    }
    public void setXCoord(int x){
        xCoord = x;
    }
    public int getXCoord(){
        return xCoord;
    }
    public void setYCoord(int y){
        yCoord = y;
    }
    public int getYCoord(){
        return yCoord;
    }
    public void setTileUnit(CombatUnit unit1) {
        tileUnit = unit1;
    }
    public CombatUnit GetTileUnit() {
        return tileUnit;
    }
    public void takeDamage(int damage) {
        // randomize damage +- 2
        // int num1 = Random.Range(-1,1);
        
        // int damageDelt = num1 + damage;
        // Debug.Log("Damage output is " + damageDelt);

        // tileUnit.setHealth(tileUnit.getHealth()-damage);
    }
    public void healUnit() {
        // deal damage to a unit
    }
    public void snapUnit() {
        // snaps unit to the grid
    }
    public void setHighlight(int highlight1) {
        highlight = highlight1;
    }
    public int getHighlight() {

        return highlight; 
    }

    /*
    public void deleteUnit() {
        tileUnit = null;
    }
    public void createUnit(CombatUnit unit1){
        tileUnit = unit1;
    }
    */


}