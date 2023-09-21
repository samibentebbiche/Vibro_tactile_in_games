using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class Pointage : MonoBehaviour
{

    [SerializeField, Range(0, 2)] public int cube; // block that we need to point to
    private float time_spent = 0.0f; // Time spent to complete the pointing into a block
    private float time = 0.0f; // Time spent to change the target block
    public float interpolationPeriod = 5.0f; // Time to change the target block

    [HideInInspector]
    public bool toucher; // true if the hand (index) is touching the block.


    private int old_cube; // temporary buffer to store the old block pointed to
    public TMP_Text text_color;
    public TMP_Text text_time;

    // Start is called before the first frame update

    void Start()
    {
        toucher = false;
        cube = 1;
        old_cube = cube;
        if(text_color != null)
        {
            text_color.text = "green";
            text_color.color = Color.green;
            
        }
        if (text_time != null) text_time.text = "0";

    }

    // Update is called once per frame
    void Update()
    {
        time_spent += Time.deltaTime;
       
        foreach (Transform eachChild in transform)
        {
            if (toucher)
            {
                time += Time.deltaTime;

                if (time >= interpolationPeriod )
                {
                    text_time.text = Math.Round(time_spent,2).ToString();
                    time_spent = 0;
                    time = 0;

                    do
                    {
                        cube = Random.Range(0, 3);
                        
                    }while(cube == old_cube);

                    old_cube = cube;

                    switch (cube)
                    {
                        case 0:
                            text_color.text = "Red";
                            text_color.color = Color.red;
                            toucher = false;
                            break;
                        case 1:
                            text_color.text = "Green";
                            text_color.color = Color.green;
                            toucher = false;
                            break;
                        case 2:
                            text_color.text = "Blue";
                            text_color.color = Color.blue;
                            toucher = false;
                            break;
                        default:

                            break;
                    }
                }

            }
            else
            {
                time = 0;
            }

        }
    }
}
