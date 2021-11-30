using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public int health, maxHealth, mindEnergy, maxMindEnergy, skillPoints, healPower, megaHealPower, damage, slashDamage, sporeDamage, spawnCostReduction, 
    recallCostReduction, boostedSpawnLevel, flipLevel, actionPoints = 5;
    public int numOfRats = 0,numOfPigeons = 0,numOfRaccoons = 0,numOfBoars = 0,numOfFalcons = 0,numOfWateringCans=0,numOfCrystals=0;
    public float critRate, creatureCritRate, shieldRate, lifestealRate;
    public bool hasHeal, hasMegaHeal, hasSlash, hasSporeBomb, hasBoostedSpawn, hasFlip;
    public Enums.Enemy selectedType;
    public bool placingUnit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        skillPoints = 3;
        health = 25;
        maxHealth = 25;
        mindEnergy = 20;
        maxMindEnergy = 20;
        damage = 5;
        slashDamage=0;
        sporeDamage=0;
        spawnCostReduction  =0;
        recallCostReduction = 0;
        boostedSpawnLevel=0;
        flipLevel=0;
        
        critRate = 0;
        creatureCritRate = 0;
        shieldRate = 0;
        lifestealRate = 0;

        hasHeal = false;
        hasMegaHeal = false;
        hasSlash = false;
        hasSporeBomb = false;
        hasBoostedSpawn = false;
        hasFlip = false;

        if(GameObject.Find("Health Bar")){
            GameObject.Find("Health Bar").GetComponent<Slider>().value = health;
        }
    }
    public bool hasUnits(Enums.Enemy enemy){
        switch((int)enemy){
            case 1:
                if(numOfRats>0)return true;
                break;
            case 2:
                if(numOfPigeons>0)return true;
                break;
            case 3:
                if(numOfBoars>0)return true;
                break;
            case 4:
                if(numOfRaccoons>0)return true;
                break;
            case 5:
                if(numOfFalcons>0)return true;
                break;
        }
        return false;
    }
    public void decUnit(Enums.Enemy e){
        switch((int)e){
            case 1:
                numOfRats--;
                break;
            case 2:
                numOfPigeons--;
                break;
            case 3:
                numOfBoars--;
                break;
            case 4:
                numOfRaccoons--;
                break;
            case 5:
                numOfFalcons--;
                break;
        }
    }

}
