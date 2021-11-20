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
    
        list.onAddCallback = addItem;
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Enemies");
        };
        list.onChangedCallback = OnChange;

    }
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused){
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
    void addItem(ReorderableList l){
        var index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        l.index = index;
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
                Debug.Log("Worked for "+(element.FindPropertyRelative("type").ToString()));
                CombatUnit unit = df.getFromEnum((Enums.Enemy)element.FindPropertyRelative("type").enumValueIndex);
                element.FindPropertyRelative("obj").objectReferenceValue = (UnityEngine.Object)unit.getUnitSprite();
                unit.getUnitSprite().transform.parent = logic.transform.Find("CombatGrid");
                unit.getUnitSprite().transform.position = unit.getUnitSprite().transform.parent.GetComponent<SpriteRenderer>().transform.position + new Vector3(element.FindPropertyRelative("x").intValue+.5f,element.FindPropertyRelative("y").intValue-2.75f,0);
            }
            
        }
        serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
    }
}
[Serializable]
public struct Unit{
    public Enums.Enemy type;
    public int x,y;
    public float scale;
    public GameObject obj;

}