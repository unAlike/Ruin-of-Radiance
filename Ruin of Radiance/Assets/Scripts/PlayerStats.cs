using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public int health, maxHealth, mindEnergy, maxMindEnergy, skillPoints, healPower, megaHealPower, damage, slashDamage, sporeDamage, spawnCostReduction, 
    recallCostReduction, boostedSpawnLevel, flipLevel, actionPoints = 5;
    public float critRate, creatureCritRate, shieldRate, lifestealRate;
    public bool hasHeal, hasMegaHeal, hasSlash, hasSporeBomb, hasBoostedSpawn, hasFlip;
    
    // Start is called before the first frame update
    void Start()
    {
        skillPoints = 3;
        health = 25;
        maxHealth = 25;
        mindEnergy = 20;
        maxMindEnergy = 20;
        damage=0;
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

}
