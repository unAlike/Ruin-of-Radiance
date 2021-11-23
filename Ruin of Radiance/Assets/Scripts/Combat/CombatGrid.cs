using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

public class CombatGrid {
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
        Debug.Log("Attacked:  [" + xCoord + ", " + yCoord + "]");
        if (Random.value < critRate) {
            damage = (int) (damage*(1.5));
        }
        Debug.Log("Dealt " + damage + " damage");
        tiles[xCoord,yCoord].takeDamage(damage);

    }
    public void clearHighlights() {
        for (int i = 0; i < 7;++i) {
            for (int j = 0; j < 3;++j) {
                tiles[i,j].setHighlight(0);
            }
        }
        Debug.Log("Cleared Highlights");
    }

}
