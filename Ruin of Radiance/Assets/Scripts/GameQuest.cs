using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public class GameQuest: MonoBehaviour
{
    public string Title, Description;
    public bool completed;
    public GameQuest(string Title, string Description){
        this.Title = Title;
        this.Description = Description;
        completed = false;
    }
}
