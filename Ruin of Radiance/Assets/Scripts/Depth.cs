using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer rend;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.sortingOrder = ((int)Camera.main.WorldToScreenPoint(this.transform.position).y-30) * -1;
    }
}