using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePooler : MonoBehaviour
{
    public GameObject knife;
    public List<GameObject> knifes = new List<GameObject>();
    public int knifeCount;

    public static KnifePooler instance;

    private void Awake()
    {
        instance = this;
        
        
        for (int i = 0; i < knifeCount; i++)
        {
            var k = Instantiate(knife, transform);
            k.SetActive(false);
            knifes.Add(k);
        }
        
    }
}
