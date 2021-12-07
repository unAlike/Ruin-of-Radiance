using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Quest
{
    public string Title, Description;
    public bool completed;
    public Quest(string Title, string Description){
        this.Title = Title;
        this.Description = Description;
        completed = false;
    }
}
