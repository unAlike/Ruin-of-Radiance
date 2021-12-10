using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DefaultCreatures{
    private CombatUnit rat = new CombatUnit(null,5,5,3,1,.25f,true,false,2,2,Enums.Enemy.Rat);
    private CombatUnit pigeon = new CombatUnit(null,8,8,4,1,.25f,true,false,3,3,Enums.Enemy.Pigeon);
    private CombatUnit raccoon = new CombatUnit(null,12,12,6,1,.25f,true,false,5,5,Enums.Enemy.Raccoon);
    private CombatUnit boar = new CombatUnit(null,25,25,10,1,.1f,true,false,10,10,Enums.Enemy.Boar);
    private CombatUnit falcon = new CombatUnit(null,15,15,20,1,.5f,true,false,10,10,Enums.Enemy.Falcon);


    public CombatUnit getFromEnum(Enums.Enemy e){
        CombatUnit send;
        GameObject pref;
        switch(e){
            case Enums.Enemy.Rat:
                send = (CombatUnit)rat.Clone();
                pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/rat"));
                send.setUnitSprite(pref);
                return send;
            case Enums.Enemy.Boar:
                send = (CombatUnit)boar.Clone();
                pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/boar"));
                send.setUnitSprite(pref);
                return send;
            case Enums.Enemy.Falcon:
                send = (CombatUnit)falcon.Clone();
                pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/falcon"));
                send.setUnitSprite(pref);
                return send;
            case Enums.Enemy.Pigeon:
                send = (CombatUnit)pigeon.Clone();
                pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/pigeon"));
                send.setUnitSprite(pref);
                return send;
            case Enums.Enemy.Raccoon:
                send = (CombatUnit)raccoon.Clone();
                pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/raccoon"));
                send.setUnitSprite(pref);
                return send;
        }
        return null;
    }
    public CombatUnit getRat(){
        CombatUnit send = (CombatUnit)rat.Clone();
        GameObject pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/rat"));
        send.setUnitSprite(pref);
        return send;
    }
    public CombatUnit getPigeon(){
        CombatUnit send = (CombatUnit)pigeon.Clone();
        GameObject pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/pigeon"));
        send.setUnitSprite(pref);
        return send;
    }
    public CombatUnit getBoar(){
        CombatUnit send = (CombatUnit)boar.Clone();
        GameObject pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/boar"));
        send.setUnitSprite(pref);
        return send;
    }
    public CombatUnit getRaccoon(){
        CombatUnit send = (CombatUnit)raccoon.Clone();
        GameObject pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/raccoon"));
        send.setUnitSprite(pref);
        return send;
    }
    public CombatUnit getFalcon(){
        CombatUnit send = (CombatUnit)falcon.Clone();
        GameObject pref = Object.Instantiate(Resources.Load<GameObject>("Prefabs/falcon"));
        send.setUnitSprite(pref);
        return send;
    }


}