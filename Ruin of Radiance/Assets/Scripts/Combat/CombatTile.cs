using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CombatTile{
    private CombatUnit tileUnit = null;
    private int xCoord, yCoord;
    private bool isOccupied;
    private Enums.highlight highlight;

// Check this
    public CombatTile(int x, int y){
        isOccupied = false;
        highlight = 0; // 0-na
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
    public CombatUnit getTileUnit() {
        return tileUnit;
    }
    public void setHighlight(Enums.highlight highlight1) {
        highlight = highlight1;
        // 0 - none, 1 - move, 2 - damage, 3 - MC, 4 - SelectedTile
        // Image  
    
    }
    public Enums.highlight getHighlight() {
        return highlight; 
    }
    public void takeDamage(int damage) {
        
        tileUnit.setHealth(tileUnit.getHealth() - damage);
        Debug.Log("Damage Taken at [" + xCoord + "," + yCoord + "]");
        
        if(tileUnit.getHealth() > 0 ) {
            Debug.Log("Unit still standing");
        }
        else { // tileUnit.getHealth() <= 0 
            tileUnit.setIsDefeated(true);
            Debug.Log("He definitely dead");
        }
        
        
    }
    public void healUnit(int heal) {
        if(tileUnit.getHealth() + heal <= tileUnit.getMaxHealth()){
            tileUnit.setHealth(tileUnit.getHealth()+heal);
        }
        else{
            tileUnit.setHealth(tileUnit.getMaxHealth());
        }
    }
    public void snapUnit() {
        // snaps unit to the grid
        if(tileUnit != null){
            tileUnit.getUnitSprite().transform.position = tileUnit.getUnitSprite().transform.parent.GetComponent<SpriteRenderer>().transform.position + new Vector3(xCoord+.5f,yCoord-2.75f,0);

        }
        
        Debug.Log("Unit Snapped to grid");
    }

}