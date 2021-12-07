using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

[CustomEditor(typeof(CombatLogic))]
public class CombatLogicScriptEditor : Editor{
    ReorderableList list;
    CombatLogic logic;
    
    private void OnEnable(){
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("enemies"),false,true,true,true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Enemies");
        };
        list.onChangedCallback = OnChange;

    }
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused){
        list.elementHeight = EditorGUIUtility.singleLineHeight+10;
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        //Type
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 40, EditorGUIUtility.singleLineHeight), "Type");
        EditorGUI.PropertyField(
            new Rect(rect.x+45, rect.y, 70, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("type"),
            GUIContent.none
        ); 
        //X
        EditorGUI.LabelField(new Rect(rect.x + 140, rect.y, 10, EditorGUIUtility.singleLineHeight), "X");
        EditorGUI.PropertyField(
            new Rect(rect.x+155, rect.y, 20, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("x"),
            GUIContent.none
        ); 
        //Y
        EditorGUI.LabelField(new Rect(rect.x + 190, rect.y, 10, EditorGUIUtility.singleLineHeight), "Y");
        EditorGUI.PropertyField(
            new Rect(rect.x+205, rect.y, 20, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("y"),
            GUIContent.none
        ); 
        //Scale
        EditorGUI.LabelField(new Rect(rect.x + 240, rect.y, 40, EditorGUIUtility.singleLineHeight), "Scale");
        EditorGUI.PropertyField(
            new Rect(rect.x+285, rect.y, 20, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("scale"),
            GUIContent.none
        ); 
        //IF CUSTOM
        if(element.FindPropertyRelative("type").enumValueIndex==(int)Enums.Enemy.Custom){
            list.elementHeight = EditorGUIUtility.singleLineHeight*3;
            //Game Object
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight+5, 80, 10), "Game Object");
            EditorGUI.PropertyField(
                new Rect(rect.x+15, rect.y+EditorGUIUtility.singleLineHeight+15, 50, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("obj"),
                GUIContent.none
            ); 
            //Health
            EditorGUI.LabelField(new Rect(rect.x+100, rect.y + EditorGUIUtility.singleLineHeight+5, 60, 10), "Health");
            EditorGUI.PropertyField(
                new Rect(rect.x+105, rect.y+EditorGUIUtility.singleLineHeight+15, 30, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("health"),
                GUIContent.none
            ); 
            //Damage
            EditorGUI.LabelField(new Rect(rect.x+150, rect.y + EditorGUIUtility.singleLineHeight+5, 60, 10), "Damage");
            EditorGUI.PropertyField(
                new Rect(rect.x+160, rect.y+EditorGUIUtility.singleLineHeight+15, 30, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("damage"),
                GUIContent.none
            ); 
            //Crit
            EditorGUI.LabelField(new Rect(rect.x+220, rect.y + EditorGUIUtility.singleLineHeight+5, 60, 10), "Crit");
            EditorGUI.PropertyField(
                new Rect(rect.x+220, rect.y+EditorGUIUtility.singleLineHeight+15, 30, EditorGUIUtility.singleLineHeight), 
                element.FindPropertyRelative("crit"),
                GUIContent.none
            ); 
        }
    }
    void OnChange(ReorderableList l){
        updateScene();
    }
    
    public override void OnInspectorGUI() {
        logic = (CombatLogic)target;
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
        if(EditorGUI.EndChangeCheck()){
            updateScene();
        }
    }
    void updateScene(){
        DefaultCreatures df = new DefaultCreatures();
        Debug.Log(logic.gameObject.transform.Find("CombatGrid").childCount);
        bool deleting = true;
        while(deleting){
            if(logic.gameObject.transform.Find("CombatGrid").childCount>0){
                DestroyImmediate(logic.gameObject.transform.Find("CombatGrid").GetChild(0).gameObject);
            }
            else{
                deleting = false;
            }
        }

        
        Debug.Log("Change");
        
        for(int i=0; i<list.count;i++){
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(i);
            if(element.FindPropertyRelative("type").enumValueIndex!=0){
                if(element.FindPropertyRelative("type").enumValueIndex==7){
                    if((GameObject)element.FindPropertyRelative("obj").objectReferenceValue!=null){
                        Debug.Log("Worked for "+(element.FindPropertyRelative("type").ToString()));
                        CombatUnit unit = new CombatUnit(
                            (GameObject)element.FindPropertyRelative("obj").objectReferenceValue,
                            logic.enemies[i].health,
                            logic.enemies[i].health,
                            logic.enemies[i].damage,
                            element.FindPropertyRelative("scale").floatValue,
                            element.FindPropertyRelative("crit").floatValue,
                            false,
                            false,
                            element.FindPropertyRelative("cost").intValue,
                            element.FindPropertyRelative("cost").intValue,
                            Enums.Enemy.Custom
                        );
                        unit.getUnitSprite().transform.parent = logic.gameObject.transform;
                        unit.getUnitSprite().transform.position = unit.getUnitSprite().transform.parent.GetComponent<SpriteRenderer>().transform.position + new Vector3(element.FindPropertyRelative("x").intValue+.5f,element.FindPropertyRelative("y").intValue-2.75f,0);
                    }
                }
                else{
                    Debug.Log("Worked for "+(element.FindPropertyRelative("type").ToString()));
                    CombatUnit unit = df.getFromEnum((Enums.Enemy)element.FindPropertyRelative("type").enumValueIndex);
                    unit.getUnitSprite().transform.parent = logic.transform.Find("CombatGrid");
                    unit.getUnitSprite().transform.position = unit.getUnitSprite().transform.parent.GetComponent<SpriteRenderer>().transform.position + new Vector3(element.FindPropertyRelative("x").intValue+.5f,element.FindPropertyRelative("y").intValue-2.75f,0);
                }
            }
        }
        serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
    }
    [MenuItem("RoR/Combat/Exit Combat")]
    static void Exit(){
        // logic.endCombat();
        if(GameObject.Find("Character").transform.parent){
            CombatLogic logic = GameObject.Find("Character").transform.parent.parent.GetComponent<CombatLogic>();
            logic.endCombat();
        }
        else{
            Debug.Log("No Combat Running");
        }
    }
    [MenuItem("RoR/Combat/End Turn")]
    static void EndTurn(){
        if(GameObject.Find("Character").transform.parent){
            CombatLogic logic = GameObject.Find("Character").transform.parent.parent.GetComponent<CombatLogic>();
            logic.endTurn();
        }
        else{
            Debug.Log("No Combat Running");
        }
    }
    [MenuItem("RoR/Combat/Debug")]
    static void DebugLogic(){
        if(GameObject.Find("Character").transform.parent){
            CombatLogic logic = GameObject.Find("Character").transform.parent.parent.GetComponent<CombatLogic>();
            logic.ReDebug();
        }
        else{
            Debug.Log("No Combat Running");
        }
    }
    [MenuItem("RoR/Debug")]
    static void GameDebug(){
        GameObject.Find("Canvas").GetComponent<GUIScript>().ReopulateQuests();
    }
    
}
[Serializable]
public struct Unit{
    public Enums.Enemy type;
    public int x,y;
    public float scale, crit;
    public int health, damage;
    public GameObject obj;

}