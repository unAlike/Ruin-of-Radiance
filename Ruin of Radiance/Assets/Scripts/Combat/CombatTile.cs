using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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