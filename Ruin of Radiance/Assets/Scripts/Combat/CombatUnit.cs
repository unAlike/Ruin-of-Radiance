using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CombatUnit : ICloneable {
<<<<<<< HEAD

=======
>>>>>>> origin/betterPrefabs
    private GameObject unitSprite;
    private int maxHealth, currentHealth, damage, summonCost, recallCost;
    private float scalingNum, critRate;
    private bool isFriendly, isDefeated;
    private Enums.Enemy type = Enums.Enemy.None;
    
    public CombatUnit(GameObject unitSprite, int maxHealth, int currentHealth, int damage, float scalingNum, float critRate, bool isFriendly, bool isDefeated, int summonCost, int recallCost, Enums.Enemy type){
        this.unitSprite = unitSprite;
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.damage = damage;
        this.scalingNum = scalingNum;
        this.critRate = critRate;
        this.isFriendly = isFriendly;
        this.isDefeated = isDefeated;
        this.summonCost = summonCost;
        this.recallCost = recallCost;
        this.type = type;
        Debug.Log("Created combatUnit");
    }
    public void setHealth(int health){
        currentHealth=health;
    }
    public int getHealth(){
        return currentHealth;
    }
    public void setCritRate(float critRate1){
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
    public void setSummonCost(int summonCost){
        this.summonCost = summonCost;
    }
    public int getSummonCost(){
        return summonCost;
    }
    public void setRecallCost(int recallCost){
        this.recallCost = recallCost;
    }
    public int getRecallCost(){
        return recallCost;
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
    public Enums.Enemy getCreatureType(){
        return type;
    }

    public object Clone(){
        return new CombatUnit(this.unitSprite,this.maxHealth,this.currentHealth,this.damage,this.scalingNum,this.critRate,this.isDefeated,this.isDefeated,this.summonCost,this.recallCost,this.type);
    }

}