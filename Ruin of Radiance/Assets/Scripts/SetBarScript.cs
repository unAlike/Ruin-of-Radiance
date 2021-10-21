using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBarScript : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void setBarMax(int max){
        slider.maxValue = max;
    }
    public void setBar(int num){
        if(num>=slider.minValue && num <= slider.maxValue){
            slider.value = num;
        }
        else{
            Debug.Log("Invalid Integer for Slider: "+slider.ToString());
        }
    }
}
