using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class PointageSuivi : MonoBehaviour
{
    public int cube = 1;

    private float time_spent = 0.0f;


    public bool off;
    private float time = 0.0f;
    [HideInInspector]
    public float interpolationPeriod = 5.0f;

    sound s;
    private GameObject tableau;
    private GameObject tableau_vide;
    public bool toucher;
    // Start is called before the first frame update


    void Start()
    {
        off = false;
        toucher = false;
        cube = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        time_spent += Time.deltaTime;
        
        foreach (Transform eachChild in transform)
        {
            s = eachChild.GetComponent<sound>();
            if (toucher)
            {
                time += Time.deltaTime;

                if (cube == 8)
                {
                    cube = 1;
                }
            }
            else
            {
                time = 0;
            }
        }
    }
}
