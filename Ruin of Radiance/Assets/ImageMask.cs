using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMask : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = .5f;
    }
}
