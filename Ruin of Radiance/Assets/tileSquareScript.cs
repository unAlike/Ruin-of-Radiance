using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tileSquareScript : MonoBehaviour, IPointerDownHandler // IPointerUpHandler
{
    // Start is called before the first frame update
        public int xVal = 0;
        public int yVal = 0;
        CombatGrid logic;

    public void onMouseDown() {
        Debug.Log("Calls Funtion with " + xVal + yVal);
    }



    public void callSelectTile() {
        Debug.Log("Calls Funtion with " + xVal + yVal);
        //logic = GameObject.Find("CombatGrid").GetComponent<CombatGrid>();
        //logic.selectTile(xVal,yVal);
    }

public void OnPointerDown(PointerEventData eventData) {
    
         if (Input.GetMouseButtonDown(0)) {
             CastRay();
         }       
     
       // Do stuff when object is touched/clicked
    Debug.Log("Down Calls Funtion with " + xVal + yVal);
     void CastRay() {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
         if (hit.collider !=null) {
             Debug.Log (hit.collider.gameObject.name);
     }
    }


    
 
    /*
    public void OnPointerUp(PointerEventData data) {
        // do stuff when touch/click is released
        Debug.Log("Up Calls Funtion with " + xVal + yVal);
    }
    */
}

}
