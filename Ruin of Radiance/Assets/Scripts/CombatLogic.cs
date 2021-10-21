using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CombatLogic : MonoBehaviour
{
    [SerializeField]
    List<CombatTile> units;

    CombatGrid grid = new CombatGrid();
    void Start(){
    }    
    void Update()
    {
        
    }
}
public class CombatGrid{
    public CombatTile[,] tiles;
    void start(){
        tiles = new CombatTile[7,3];
    }
    
}
[System.Serializable]
public class CombatTile{
    public Unit tileUnit;
    public int x, y;

    void start(){
        tileUnit = new Unit();
    }
}
[System.Serializable]
public class Unit{
    public GameObject unitSprite;
    public float h,d;
    

}