using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PointageSuivi : MonoBehaviour
{
    [HideInInspector]
    public int cube = 1;
    [HideInInspector]
    public int number_cube;
    //private float time_spent = 0.0f;

    [HideInInspector]
    public bool off;
    private float time = 0.0f;
    [HideInInspector]
    public float interpolationPeriod = 5.0f;

    sound s;
    private GameObject tableau;
    private GameObject tableau_vide;
    [HideInInspector]
    public bool toucher;
    // Start is called before the first frame update
    public float vib_min = 200;
    public float vib_max = 400;
    public GameObject interaction;
    [HideInInspector]
    public float frequency_;
    public bool succecive = false;

    //public float 


    void Start()
    {
        off = false;
        toucher = false;
        cube = 1;
        frequency_ = vib_min;
        number_cube = this.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {

        //time_spent += Time.deltaTime;
        foreach (Transform eachChild in transform)
        {
            s = eachChild.GetComponent<sound>();



            if (toucher)
            {

                if (cube == number_cube + 1)
                {
                   frequency_ = vib_min;
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
