using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit{
    public GameObject unitSprite;
    public int maxHealth, currentHealth, damage, maxActionPoints, actionPoints;
    public double scalingNum;
    public bool isFriendly;
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
    public void setActionPoints(int ap){ // for removing action points
       this.actionPoints = ap;
    }
    public int getActionPoints(){
        return this.actionPoints;
    }
    public void fillActionPoints() { // end of turn fills actions points
        this.actionPoints = maxActionPoints;
    }
    public void setIsFriendly(bool friendly){
        isFriendly = friendly;
    }
    public bool getIsFriendly(){
        return isFriendly;
    }

}