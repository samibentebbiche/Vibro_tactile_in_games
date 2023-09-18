using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;

public class DistanceSuivi : MonoBehaviour
{
    Controller controller;

    [SerializeField, Range(0, 3)] private int Mode = 0;


    sound s;
    PointageSuivi pointage;
    public GameObject cube;
    public GameObject interaction;
    float distance;
    private float time = 0.0f;
    private float interpolationPeriod = 0.2f;


    
    public float frequence_min = 100;
    
    public float frequence_max = 400;

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
        s = GetComponent<sound>();
        pointage = transform.parent.gameObject.GetComponent<PointageSuivi>();
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = controller.Frame();
        // verify if there is no hands in the frame then set the frequency and intensity to 0
        if (frame.Hands.Count == 0)
        {
            s.setFrequency((float)0, GetType().Name);
            s.setIntensity((float)0, GetType().Name);
            pointage.toucher = false;
        }

        // calculate the distance between the cube and the tip of the hands
        distance = Vector3.Distance(cube.transform.position, interaction.transform.position);
        

        //s = GetComponent<sound>();

        // if the distance mode is activated 
        if (s != null)
        {

            // look wich cube is suposed to vibrate and vibrate this cube if this one is suposed to vibrate
            if ((pointage.cube.ToString() == this.name))
            {
                //Debug.Log(distance);
                vibrer();
            }
            else
            {
                // if this cube is note supposed to vibrate we set frequency and intensity to 0
                s.setFrequency((float)0, GetType().Name);
                s.setIntensity((float)0, GetType().Name);
            }
        }
    }
    void vibrer()
    {
        if (Mode == 0)  
        {
            Debug.Log(pointage.cube);
            if (distance > 0.1)
            {
                pointage.toucher = false;
                s.setIntensity((float)1, GetType().Name);
                s.UpFrerquency(distance, GetType().Name);
            }
            else
            {
                if(!pointage.toucher)
                {
                    pointage.toucher = true;

                    time += Time.deltaTime;
                    if (time >= interpolationPeriod)
                    {
                        s.setIntensity((float)0, GetType().Name);
                        if (time >= interpolationPeriod * 2)
                        {
                            s.setIntensity((float)1, GetType().Name);
                            time = 0.0f;
                        }
                    }
                }
            }
        }
        
        else if (Mode == 1)
        {

            if (distance > 0.1)
            {
                pointage.toucher = false;
                s.setIntensity((float)1, GetType().Name);
                s.UpFrerquency(distance, GetType().Name);
            }
            else
            {
                pointage.toucher = true;
                time += Time.deltaTime;
                if (time >= interpolationPeriod)
                {
                    s.setFrequency((float)0, GetType().Name);
                    if (time >= interpolationPeriod * 2)
                    {
                        s.setToFrequenceBase(GetType().Name);
                        time = 0.0f;
                    }
                }
            }
        }
        // this
        else if (Mode == 2)
        {
            if (distance > 0.1)
            {
                pointage.toucher = false;
                s.UpIntensity(distance * 10, GetType().Name);
                s.setToFrequenceBase(GetType().Name);
            }
            else
            {
                pointage.toucher = true;
                time += Time.deltaTime;
                if (time >= interpolationPeriod)
                {
                    s.setFrequency((float)0, GetType().Name);
                    if (time >= interpolationPeriod * 2)
                    {
                        s.setToFrequenceBase(GetType().Name);
                        time = 0.0f;
                        
                    }
                }
            }
        }
        else if(Mode == 3)
        {
            pointage.off = true;

        }

    }

    //  oui madare acheter le crb

    //void ontriggerenter()
    //{
    //    //s.setfrequency((float)0);
    //    //s.setintensity((float)0);

    //    pointage.cube += 1;
    //}

    //void ontriggerstay(collider other)
    //{

    //}

    //void ontriggerexit(collider other)
    //{
    //    //s.setfrequency((float)0);
    //    //s.setintensity((float)0);
    //}

}
