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
        if(s == null )
        {
            try
            {
                s = GetComponent<sound>();
            }
            catch
            {
                Debug.Log("Sound dont't exist !");
            }
        }
        if (s == null) s = gameObject.AddComponent<sound>();

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

    }

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.name == "R_index_end")
        {


            renderer.material.SetColor("_Color", customColor);
        }
        
    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "R_index_end")
        {


            renderer.material.SetColor("_Color", Color.white);
        }

    }
}
 