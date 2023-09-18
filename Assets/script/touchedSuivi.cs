using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;
public class touchedSuivi : MonoBehaviour
{

    Controller controller;

   
    public enum Direction { Amplitude , Fréquence , Amplitude_Fréquence, Amplitude_Fréquence_continue, SoundOnEnterAndExit, SoundOnEnter, Audio };
    [SerializeField]
    public Direction Mode;

    //public AudioClip audio;
    [SerializeField]
    private AudioClip _clip;
    private AudioSource _audio;
    

    private bool move = false;

    public float vib_min = 200;
    public float vib_max = 400;
    

    sound s;
    PointageSuivi pointage;

    public GameObject cube;
    public GameObject interaction;
    float distance;
    public float distance_max;
    private float time = 0.0f;
    private float interpolationPeriod = 0.2f;
    private float frequency_;
    private bool enter  = false;
    private bool exit = false;

    // Start is called before the first frame update
    void Start()
    {
        
        distance_max = 0.05f;
        controller = new Controller();
        s = GetComponent<sound>();
  
        pointage = transform.parent.gameObject.GetComponent<PointageSuivi>();
        frequency_ = vib_min;
        _audio = GetComponent<AudioSource>();
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
        distance = Vector3.Distance(cube.transform.position, interaction.transform.position);

        
        //s = GetComponent<sound>();

        // if the distance mode is activated
        if (s != null /*&& s.mode == sound.Mode.Touched*/)
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

       
        if (Mode == Direction.Amplitude) // Changement d'amplitude
        {
            if (distance > distance_max)
            {
                pointage.toucher = false;
                s.setFrequency((float)0, GetType().Name);
                s.setIntensity((float)0, GetType().Name);

            }
            else
            {
                pointage.toucher = true;
                time += Time.deltaTime;
                s.setToFrequenceBase(GetType().Name);
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
        else
        if (Mode == Direction.Amplitude_Fréquence) // Changement de l'amplitude et Fréquence
        {
            if (distance > distance_max)
            {
                pointage.toucher = false;
                s.setIntensity((float)0, GetType().Name);
                s.setFrequency((float)0, GetType().Name);
            }
            else
            {
                pointage.toucher = true;
                time += Time.deltaTime;
                if (time >= interpolationPeriod)
                {
                    s.setFrequency((float)0, GetType().Name);
                    s.setIntensity(distance, GetType().Name);
                    if (time >= interpolationPeriod * 2)
                    {
                        s.setToFrequenceBase(GetType().Name);
                        s.setIntensity((float)1, GetType().Name);
                        time = 0.0f;
                    }
                }
            }
        }
        else
        if (Mode == Direction.Amplitude_Fréquence_continue) // Changement d'amplitude et fréquence de manière continue
        {
            if (distance > distance_max)
            {
                pointage.toucher = false;
                s.setIntensity((float)0, GetType().Name);
                s.setFrequency((float)0, GetType().Name);
            }
            else
            {
                pointage.toucher = true;
                s.setToFrequenceBase(GetType().Name);
                s.setIntensity((float)1, GetType().Name);
            }
        }
        else
        if (Mode == Direction.Fréquence) // Changement de fréquence
        {
            if (!move)
            {
                if (distance > distance_max)
                {
                    pointage.toucher = false;
                    s.setFrequency((float)0, GetType().Name);
                    s.setIntensity((float)0, GetType().Name);
                }
                else
                {
                    move = true;
                }
            }

            if (move)
            {
                if (transform.position.y < 1)
                {

                    transform.Translate(Vector3.up * Time.deltaTime );
                    pointage.toucher = true;
                    time += Time.deltaTime;
                    s.setIntensity((float)1, GetType().Name);
                    if (time >= interpolationPeriod)
                    {
                        s.setFrequency((float)0, GetType().Name);
                        if (time >= interpolationPeriod * 2)
                        {
                            s.setToFrequenceBase(GetType().Name);
                            //pointage.cube += 1;
                            time = 0.0f;
                        }
                    }

                }
                else
                {
                    pointage.toucher = true;
                    pointage.cube += 1;
                    move = false;
                }
            }


        } 
        
        else if (enter)
        {
            transform.Translate(Vector3.up * Time.deltaTime *3);
            time += Time.deltaTime;
            s.setIntensity((float)1, GetType().Name);


            
            if ((frequency_ / 7) * (pointage.cube) <= (vib_max / 7) * pointage.cube)
            {

                //Debug.Log(vib_min + (pointage.cube) * (200/7));
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
                enter = false;
            }

        }
        else if (exit)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 3);


            time += Time.deltaTime;
            s.setIntensity((float)1, GetType().Name);
            if (frequency_ > vib_min)
            {
                s.setFrequency((float)frequency_, GetType().Name);
                if (time >= 0.01)
                {
                    frequency_ -= 10;
                    time = 0.0f;
                }
            }
            else
            {
                pointage.cube += 1;


                s.setIntensity((float)0, GetType().Name);
                exit = false;
                pointage.toucher = true;
            }
        }
        else if(Mode == Direction.Audio)
        {
            pointage.interpolationPeriod = 0.5f;
            s.enabled = false;

            if (_audio == null)
            {
                Debug.LogError("Error");
            }
            else
            {
                _audio.clip = _clip;
            }


            if(!move)
            {
                if (distance > distance_max)
                {
                    pointage.toucher = false;

                }
                else
                {
                    move = true;

                }
            }

            if(move)
            {
                if (transform.position.y < 1)
                {
                    
                    transform.Translate(Vector3.up * Time.deltaTime * 2);

                }else
                {
                    _audio.Play();
                    pointage.toucher = true;
                    pointage.cube += 1;
                    move = false;
                }

            }
            
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.name[0] != 't')
        {
            if (Mode == Direction.SoundOnEnterAndExit || Mode == Direction.SoundOnEnter)
            {

                if (pointage.cube.ToString().Equals(this.name))
                {
                    exit = false;

                    if (!enter)
                    {
                        frequency_ = vib_min;

                        enter = true;

                    }

                    pointage.interpolationPeriod = 0.5f;
                    if (Mode == Direction.SoundOnEnter) pointage.toucher = true;
                }
            }
        }

    }    
    void OnTriggerExit(Collider other)
    {
        //
        if (Mode == Direction.SoundOnEnterAndExit)
        {
            frequency_ = vib_max;
            enter = false;
            exit = true;    
        }
    }
}
