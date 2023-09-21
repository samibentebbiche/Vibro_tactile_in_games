using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Distance : MonoBehaviour
{
    Controller controller;

    public enum Direction { Frequency, Intensity };
    [SerializeField]
    public Direction Mode;

    // The script script sound
    sound s;

    [HideInInspector]
    public float frequence_min = 100;
    [HideInInspector]
    public float frequence_max = 400;

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(Distance)), CanEditMultipleObjects]
    public class ModeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Distance dis = (Distance)target;
            
            Direction list = dis.Mode;
            if (list == Direction.Frequency ) DrawFreq(dis);
        }

        static void DrawFreq(Distance dis)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("frequence Min & Max ");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Min ", GUILayout.MaxWidth(40));
            dis.frequence_min = EditorGUILayout.FloatField(dis.frequence_min);

            EditorGUILayout.LabelField("Max ", GUILayout.MaxWidth(40));
            dis.frequence_max = EditorGUILayout.FloatField(dis.frequence_max);

            EditorGUILayout.EndHorizontal();
        }
        
    }
#endif
    #endregion

    public GameObject interaction;

    // Distance between two object
    private float distance;

    Rigidbody rb;


    PointageSuivi pointage;
    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();

        try
        {
            pointage = transform.parent.gameObject.GetComponent<PointageSuivi>();
        }
        catch (Exception e)
        {
            pointage = null;
        }

        

        s = GetComponent<sound>();
        if (GetComponent<Rigidbody>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Frame frame = controller.Frame();


        // verify if there is no hands in the frame then set the frequency and intensity to 0
        if (controller.IsConnected == true)
        {
            if (frame.Hands.Count == 0)
            {
                s.setFrequency((float)0, GetType().Name);
                s.setIntensity((float)0, GetType().Name);
            }
        }
        // calculate the distance between the cube and the tip of the hands
        distance = Vector3.Distance(this.transform.position, interaction.transform.position);


        s = GetComponent<sound>();

        // if the distance mode is activated 

       
        if (s != null)
        {
      
            if (pointage == null) vibrer();
            else
            {
                Debug.Log(pointage.cube);
                if (pointage.cube == int.Parse(this.name))
                {
                    
                    vibrer();
                }
            }

            
        }
    }

    void vibrer()
    {

        
        if (Mode == Direction.Frequency) 
        {

            if (distance > 0.05)
            {
                float scale = transform.parent.gameObject.transform.localScale.x;
                s.setIntensity((float)1, GetType().Name);
                s.UpFrerquency(distance, scale, GetType().Name);

            }
            else
            {

                //if (pointage != null) {
                //    pointage.toucher = true;
                //    s.setFrequency(0, GetType().Name);
                //}
            }

        }
        else if (Mode == Direction.Intensity)
        {
            if (distance > 0.1)
            {
                s.UpIntensity(distance, GetType().Name);
                s.setToFrequenceBase(GetType().Name);
            }

        }
    }
}
