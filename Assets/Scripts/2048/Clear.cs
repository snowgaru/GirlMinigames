using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    public void DestroyImage()
    {
        for (int i = 0; i < transform.childCount; i++) { Destroy(transform.GetChild(i).gameObject); }
    }

    public void Size()
    {
        for (int i = 0; i < transform.childCount; i++) { transform.GetChild(i).localScale = new Vector3(192, 192, 1); }
    }
    
}
