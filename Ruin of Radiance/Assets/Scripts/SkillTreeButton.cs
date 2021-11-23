using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool unlocked = false;
    [SerializeField]
    public string skillName = "";
    [SerializeField]
    public string desc = "";
    [SerializeField]
    public int maxPoints = 5;
    [SerializeField]
    public int currentPoints = 0;

    
}
