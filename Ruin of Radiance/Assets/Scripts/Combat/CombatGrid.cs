using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

public class CombatGrid{
    private CombatTile[,] tiles;
    private int UUID;
    public CombatTile selectedTile = new CombatTile(0,1);

    public CombatGrid(){
        tiles = new CombatTile[7,3];
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
            tiles[i,j] = new CombatTile(i,j);
            }
        }
       Debug.Log("Created Grid");
    }
    public CombatTile[,] getTiles() {
        return tiles;
    }
    // fix return
    public CombatTile getTileOfUnit(CombatUnit Unit1) {
        // find unit location
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
                if (tiles[i,j].GetTileUnit() == Unit1) {
                    Debug.Log("Got tile of unit");
                    return tiles[i,j];
                }
            }
        }
        Debug.Log("404 Unit not found");
        return null;
    }
    public void moveTile(CombatTile fromTile, CombatTile toTile) {
        //Create Temp Unit for Swap
        CombatUnit Temp = toTile.GetTileUnit();
        
        //Swap Units
        toTile.setTileUnit(fromTile.GetTileUnit());
        fromTile.setTileUnit(Temp);

        Debug.Log("Moved:  [" + fromTile.getXCoord() + ", " + fromTile.getYCoord() + "]");

        bool ocuTemp = toTile.getIsOccupied();
        toTile.setIsOccupied(fromTile.getIsOccupied());
        fromTile.setIsOccupied(ocuTemp);
        
        //Snap Units
        fromTile.snapUnit();
        toTile.snapUnit();
    }
    public void attack(int damage, float critRate, int xCoord, int yCoord) {
        // determines damage being delt, critRate, and attacked square location
        if (Random.value < critRate) {
            damage = (int) (damage*(1.5));
        }
        tiles[xCoord,yCoord].takeDamage(damage);
        Debug.Log("Attacked:  [" + xCoord + ", " + yCoord + "]");

    }

    public void summonCreature() {
        // create new unit
        // highlight where you want to put the unit
        // subtract from inventory total depending on the unit type
        // subtract MC energy
        Debug.Log("Summoned Creature to [" + ", " + "]");
    }
    public void recallCreature() {
        // remove unit
        // add to inventory total depending on the unit type
        // subtract MC energy
        Debug.Log("Recalled creature from [" + ", " + "]");
    }
    public void clearHighlights() {
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
                tiles[i,j].setHighlight(0);
            }
        }
        Debug.Log("Cleared Highlights");
    }

    /*
    public void basicAttack(CombatTile unit1, int xCoord, int yCoord,int dmg) {
        if(tiles[xCoord,yCoord].getIsOccupied()) {
            if (tiles[xCoord,yCoord].tileUnit.getIsFriendly() == false){ // unit in space is enemy

            tiles[xCoord,yCoord].takeDamage(dmg);
            Debug.Log(""+ xCoord + ", "+ yCoord + " should have taken damage");
            // unit1.tileUnit.setActionPoints(-1);
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
            Debug.Log("Selected Unit: " + xCoord + yCoord);
            // grid.highlightTiles(selectedTile.xCoord,selectedTile.yCoord,stats.actionPoints);
            
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
    */
}
