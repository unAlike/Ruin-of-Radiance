using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatUnit{
    private GameObject unitSprite;
    private int maxHealth, currentHealth, damage;
    private float scalingNum;
    private bool isFriendly;
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
    public void setIsFriendly(bool friendly){
        isFriendly = friendly;
    }
    public bool getIsFriendly(){
        return isFriendly;
    }
    public void setUnitSprite(GameObject unitSprite1) {
        unitSprite = unitSprite1;
    }
    public GameObject getUnitSprite() {
        return unitSprite;
    }

}