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

}
