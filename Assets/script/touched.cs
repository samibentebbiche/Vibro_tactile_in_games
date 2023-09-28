 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class touched : MonoBehaviour
{

    Controller controller;


    public enum Direction { Amplitude , Fréquence , Amplitude_Fréquence, Amplitude_Fréquence_continue, SoundOnEnterAndExit, SoundOnEnter, Audio};
    [SerializeField] public Direction Mode;



    [HideInInspector]
    public AudioClip _clip1;
    [HideInInspector]
    public AudioClip _clip2;
    //private AudioSource _audio;


    private float vib_min = 200;
    private float vib_max = 400;

    private float time_on;
    private float time_off;

    PointageSuivi pointage;
    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(touched)), CanEditMultipleObjects]
    public class ModeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            touched tou = (touched)target;
            
            Direction list = tou.Mode;

            if (list == Direction.SoundOnEnter || list == Direction.SoundOnEnterAndExit) DrawFreq(tou);

            if (list == Direction.Audio) DrawAudio(tou);

            if (list == Direction.Fréquence || list == Direction.Amplitude || list == Direction.Amplitude_Fréquence)EditSchema(tou);
        }

        static void EditSchema(touched tou)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Schema de vibration ");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("On ", GUILayout.MaxWidth(40));
            tou.time_on = EditorGUILayout.FloatField(tou.time_on);

            EditorGUILayout.LabelField("Off ", GUILayout.MaxWidth(40));
            tou.time_off = EditorGUILayout.FloatField(tou.time_off);

            EditorGUILayout.EndHorizontal();
        }

        static void DrawAudio(touched tou)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Audio Clip");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Clip 1 ", GUILayout.MaxWidth(35));
            tou._clip1 = EditorGUILayout.ObjectField(tou._clip1,typeof(AudioClip),true) as AudioClip;

            //EditorGUILayout.LabelField("Clip 2 ", GUILayout.MaxWidth(35));
            //tou._clip2 = EditorGUILayout.ObjectField(tou._clip2, typeof(AudioClip), true) as AudioClip;

            EditorGUILayout.EndHorizontal();
        }

        static void DrawFreq(touched tou)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("frequence Min & Max ");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Min ", GUILayout.MaxWidth(40));
            tou.vib_min = EditorGUILayout.FloatField(tou.vib_min);

            EditorGUILayout.LabelField("Max ", GUILayout.MaxWidth(40));
            tou.vib_max = EditorGUILayout.FloatField(tou.vib_max);

            EditorGUILayout.EndHorizontal();
        }
        
    }
#endif
    #endregion

    sound s;

    //public GameObject cube;
    public GameObject interaction;
    private float distance;
    public float distance_max;

    // private buffer
    private float time = 0.0f;
    private float interpolationPeriod = 0.2f;
    private float frequency_;
    private bool enter  = false;
    private bool exit = false;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            pointage = transform.parent.gameObject.GetComponent<PointageSuivi>();
        }
        catch (Exception e)
        {

            pointage = null;
        }
        controller = new Controller();

        try
        {
            s = transform.parent.gameObject.GetComponent<sound>();
        }
        catch (Exception e)
        {

            if (s == null)
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
        }

        if (s == null) s = gameObject.AddComponent<sound>();
        if (GetComponent<Rigidbody>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        GetComponent<Collider>().isTrigger = true;

        frequency_ = vib_min;

        //if(transform.parent.gameObject.GetComponent<AudioSource>() == null)
        //{
            
        //    if(GetComponent<AudioSource>() == null)
        //    {

        //        _audio = transform.parent.gameObject.AddComponent<AudioSource>();
        //    }
        //    else
        //    {
        //        _audio = GetComponent<AudioSource>();
        //    }
        //}
        //else
        //{
        //    //_audio = transform.parent.gameObject.GetComponent<AudioSource>();
        //}

        time_on = 0.2f;
        time_off = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        

        // verify if there is no hands in the frame then set the frequency and intensity to 0
        if(controller.IsConnected == true)
        {
            Frame frame = controller.Frame();

            if (frame.Hands.Count == 0)
            {
                s.setFrequency((float)0, GetType().Name);
                s.setIntensity((float)0, GetType().Name);

            }
        }

        // calculate the distance between the cube and the tip of the hands
        distance = Vector3.Distance(this.transform.position, interaction.transform.position);

        
        // if the distance mode is activated
        if (s != null)
        {
            //// look wich cube is suposed to vibrate and vibrate this cube if this one is suposed to vibrate

            vibrer();
        }
    }

    void vibrer()
    {

        if (Mode == Direction.Amplitude) // Changement d'amplitude
        {
            s.enabled = true;
            if (distance > distance_max)
            {
                
                s.setFrequency((float)0, GetType().Name);
                s.setIntensity((float)0, GetType().Name);

            }
            else
            {
                
                time += Time.deltaTime;
                s.setToFrequenceBase(GetType().Name);
                if (time >= time_on)
                {
                    s.setIntensity((float)0, GetType().Name);
                    if (time >= time_off + time_on)
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
            s.enabled = true;
            if (distance > distance_max)
            {
                
                s.setIntensity((float)0, GetType().Name);
                s.setFrequency((float)0, GetType().Name);
            }
            else
            {
                
                time += Time.deltaTime;
                if (time >= time_on)
                {
                    s.setFrequency((float)0, GetType().Name);
                    s.setIntensity(distance, GetType().Name);
                    if (time >= time_off + time_on)
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
            s.enabled = true;
            if (distance > distance_max)
            {
               
                s.setIntensity((float)0, GetType().Name);
                s.setFrequency((float)0, GetType().Name);
            }
            else
            {
                s.setToFrequenceBase(GetType().Name);
                s.setIntensity((float)1, GetType().Name);
            }

        }
        else
        if (Mode == Direction.Fréquence) // Changement de fréquence
        {
            s.enabled = true;
            if (distance > distance_max)
            {
                
                s.setFrequency((float)0, GetType().Name);
                

            }
            else
            {     
                s.setIntensity((float)1, GetType().Name);
                time += Time.deltaTime;
                
                if (time >= time_off)
                {
                    
                    s.setToFrequenceBase(GetType().Name);
                    

                    if (time >= time_on + time_off)
                    {
                        s.setFrequency((float)0, GetType().Name);
                        time += Time.deltaTime;
                        time = 0;
                  
                    }

                }


            }
        }
        else if (enter)
        {
            s.enabled = true;
            time += Time.deltaTime;
            
            s.setIntensity((float)1, GetType().Name);

            if (frequency_ <= vib_max)
            {

                s.setFrequency((float)frequency_, GetType().Name);
                if (time >= 0.01)
                {
                    frequency_ += (float)10;

                    time = 0.0f;
                }
            }
            else
            {

                frequency_ = vib_min;
                s.setIntensity((float)0, GetType().Name);
                enter = false;
            }


        }
        else if (exit)
        {
            s.enabled = true;
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
                s.setIntensity((float)0, GetType().Name);
                exit = false;
               
            }
        }
        else if(Mode == Direction.Audio)
        {
           
            s.enabled = false;

            if (s.audio == null)
            {
                Debug.LogError("Error");
            }
            else
            {
                s.audio.clip = _clip1;
            }

            if (distance > distance_max)
            {

            }
            else
            {

                s.audio.Play();
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (Mode == Direction.SoundOnEnterAndExit || Mode == Direction.SoundOnEnter)
        {

            if (interaction.name == other.name )
            {

                exit = false;
                if (!enter)
                {
                    frequency_ = vib_min;
                    enter = true;
                }
            }
            
                
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (Mode == Direction.SoundOnEnterAndExit)
        {
            if (interaction.name == other.name)
            {

                frequency_ = vib_max;
                enter = false;
                exit = true;


            }
        }

    }
}
