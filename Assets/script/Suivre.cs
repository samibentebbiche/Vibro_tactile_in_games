using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;

public class Suivre : MonoBehaviour
{
    Controller controller;
    PointageSuivi pointage;
    Renderer renderer;
    Color customColor;
    public GameObject crean;
    sound s;

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();

        s = transform.parent.gameObject.GetComponent<sound>();

        pointage = GameObject.Find("Objects").GetComponent<PointageSuivi>();

        renderer = this.GetComponent<Renderer>();

        customColor = new Color(0.1f, 0.9f, 0.1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = controller.Frame();
        if (frame.Hands.Count == 0)
        {
            s.setFrequency((float)0, GetType().Name);
            s.setIntensity((float)0, GetType().Name);
        }
        //Debug.Log(this.name);
        //Debug.Log(pointage.cube);
        if (pointage.cube == 0)
        {
            // TODO
            //distance = Vector3.Distance(.transform.position, indexTip.transform.position);

        }
        else if(pointage.cube == 1)
        {
            // TODO
        }
        else
        {
            // TODO
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.name == "R_index_end")
        {
            //if (s.mode == sound.Mode.Distance)
            //{
                //Debug.Log(other.name);
                //s.setToFrequenceBase(GetType().Name);
                //s.setIntensity((float)1, GetType().Name);
            //}



            renderer.material.SetColor("_Color", customColor);
        }
        
    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "R_index_end")
        {


            
            //if (s.mode == sound.Mode.Distance)
            //{
                s.setIntensity((float)0, GetType().Name);
                s.setFrequency((float)0, GetType().Name);
            //}


            renderer.material.SetColor("_Color", Color.white);
        }

    }
}
 