using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health, maxHealth, mindEnergy, maxMindEnergy;
    
    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        maxHealth = 30;
        mindEnergy = 20;
        maxMindEnergy = 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
