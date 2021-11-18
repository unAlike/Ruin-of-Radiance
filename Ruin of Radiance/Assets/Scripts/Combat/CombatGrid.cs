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
        for (int i = 0; i < 7;i++) {
            for (int j = 0; j < 3;j++) {
                if (tiles[i,j].getTileUnit() == Unit1) {
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
        CombatUnit Temp = toTile.getTileUnit();
        
        //Swap Units
        toTile.setTileUnit(fromTile.getTileUnit());
        fromTile.setTileUnit(Temp);

        Debug.Log("Moved:  [" + fromTile.getXCoord() + ", " + fromTile.getYCoord() + "]");

        bool ocuTemp = toTile.getIsOccupied();
        toTile.setIsOccupied(fromTile.getIsOccupied());
        fromTile.setIsOccupied(ocuTemp);

        Enums.highlight highTemp = fromTile.getHighlight();
        fromTile.setHighlight(toTile.getHighlight());
        toTile.setHighlight(highTemp);
        
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

    
    public void clearHighlights() {
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
                tiles[i,j].setHighlight(0);
            }
        }
        Debug.Log("Cleared Highlights");
    }

    /*
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
   
    
    */
}
