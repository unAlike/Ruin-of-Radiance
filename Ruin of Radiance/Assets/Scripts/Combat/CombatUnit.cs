using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit{
    private GameObject unitSprite;
    private int maxHealth, currentHealth, damage;
    private float scalingNum;
    private float critRate;
    private bool isFriendly, isDefeated;
    
    public CombatUnit(GameObject unitSprite, int maxHealth, int currentHealth, int damage, float scalingNum, float critRate, bool isFriendly, bool isDefeated){
        this.unitSprite = unitSprite;
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.damage = damage;
        this.scalingNum = scalingNum;
        this.critRate = critRate;
        this.isFriendly = isFriendly;
        this.isDefeated = isDefeated;
        Debug.Log("Created combatUnit");
    }
    public void setHealth(int health){
        currentHealth=health;
    }
    public int getHealth(){
        return currentHealth;
    }
    public void setCritRate(int critRate1){
        critRate=critRate1;
    }
    public float getCritRate(){
        return critRate;
    }
    public void setMaxHealth(int health){
        maxHealth=health;
    }
    public int getMaxHealth(){
        return maxHealth;
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
    public void setIsDefeated(bool defeated){
        isDefeated = defeated;
    }
    public bool getIsDefeated(){
        return isDefeated;
    }
    public void setUnitSprite(GameObject unitSprite1) {
        unitSprite = unitSprite1;
    }
    public GameObject getUnitSprite() {
        return unitSprite;
    }

}