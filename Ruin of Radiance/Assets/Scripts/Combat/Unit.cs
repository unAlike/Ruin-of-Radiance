using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Unit{
    public Enums.Enemy type;
    public int x,y;
    public float scale, crit;
    public int health, damage;
    public GameObject obj;

}