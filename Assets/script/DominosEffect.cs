using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;
public class DominosEffect : MonoBehaviour
{

    Controller controller;

   
    //public enum Direction { Amplitude , Fréquence , Amplitude_Fréquence, Amplitude_Fréquence_continue, SoundOnEnterAndExit, SoundOnEnter, Audio };
    //[SerializeField]
    //public Direction Mode;

    //public AudioClip audio;
    //[SerializeField]

    //private AudioClip _clip;
    //private AudioSource _audio;
    

    //private bool move = false;
    

    sound s;
    PointageSuivi pointage;

    
    float distance;

    private float time = 0.0f;
    private float interpolationPeriod = 0.2f;
    private float frequency_;
    private bool enter  = false;
    private float vib_min;
    private float vib_max;

    // Start is called before the first frame update
    void Start()
    {
        
        
        controller = new Controller();
        s = GetComponent<sound>();
  
        pointage = transform.parent.gameObject.GetComponent<PointageSuivi>();

        vib_min = pointage.vib_min;
        vib_max = pointage.vib_max;

        frequency_ = vib_min;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pointage.cube.ToString());
        Frame frame = controller.Frame();

        //verify if there is no hands in the frame then set the frequency and intensity to 0
        if (frame.Hands.Count == 0)
        {
            s.setFrequency((float)0, GetType().Name);
            s.setIntensity((float)0, GetType().Name);
            pointage.toucher = false;
        }
        // calculate the distance between the cube and the tip of the hands
        distance = Vector3.Distance(this.transform.position, pointage.interaction.transform.position);


        // if the distance mode is activated
        if (s != null )
        {

            // look wich cube is suposed to vibrate and vibrate this cube if this one is suposed to vibrate
            if (pointage.cube.ToString() == this.name)
            {
                //Debug.Log(pointage.cube.ToString() + "  '  " + this.name);

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

        if (enter)
        {
            
            pointage.toucher = true;

            time += Time.deltaTime;
            s.setIntensity((float)1, GetType().Name);

            if ((frequency_ / 7) * (pointage.cube) <= (vib_max / 7) * pointage.cube)
            {
                
                s.setFrequency(vib_min + (pointage.cube) * (200 / 7), GetType().Name);

                if (time >= 0.01)
                {
                    frequency_ += (float)10;
                    time = 0.0f;
                }
            }
            else
            {
                pointage.cube += 1;
                s.setIntensity((float)0, GetType().Name);

            }

        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.name[0] != 't')
        {


                if (pointage.cube.ToString().Equals(this.name))
                {
                    
                    if (!enter)
                    {
                        frequency_ = vib_min;

                        enter = true;

                    }

                    pointage.interpolationPeriod = 0.5f;
                    pointage.toucher = true;
                }

        }

    }    

}
