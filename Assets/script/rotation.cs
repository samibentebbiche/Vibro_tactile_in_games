using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System;

public class rotation : MonoBehaviour
{
    Controller controller;

    sound s;
    private GameObject car;

    public int maxHealth = 30;
    public int currentHealth;

    public HealthBar healthBar;


    public float min_vib = 200;

    public int numberRoad = 4;
    public float max_vib = 400;

    private bool touch_canister;

    private float time = 0.0f;
    private float interpolationPeriod = 0.2f;
    private int i;

    Frame frame;
    Hand hand;
    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
        car = GameObject.Find("/Objects/car");
        touch_canister = false;
        i = 0;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 0.5)
        {
            if(currentHealth > 0) currentHealth -= 1;
            healthBar.SetHealth(currentHealth);
            time = 0;
        }


        s = car.GetComponent<sound>();
        if (s != null)
        {

                frame = controller.Frame();
                hand = frame.Hands[0];

                //Debug.Log(hand.Rotation.z);

                car.transform.position = new Vector3((hand.Rotation.z * -2), car.transform.position.y, car.transform.position.z);

            if ((hand.Rotation.z > ((float)1 / numberRoad) * 3) && (hand.Rotation.z < ((float)1 / numberRoad) * 4))
            {

                //Debug.Log(" road 4");

                //s.setIntensity((float)1, GetType().Name);
                //s.setFrequency(min_vib + ((max_vib - min_vib) / numberRoad) * 0, GetType().Name);



            }
            else if ((hand.Rotation.z > ((float)1 / numberRoad) * 2) && (hand.Rotation.z < ((float)1 / numberRoad) * 3))
            {
                //Debug.Log(" road 3");
                //s.setIntensity((float)1, GetType().Name);
            
            }
            else if ((hand.Rotation.z > ((float)1 / numberRoad) * 1) && (hand.Rotation.z < ((float)1 / numberRoad) * 2))
            {
                //Debug.Log(" road 2");
                //s.setIntensity((float)1, GetType().Name);
                //s.setFrequency(min_vib + ((max_vib - min_vib) / numberRoad) * 2, GetType().Name);
               
            }
            else if ((hand.Rotation.z > 0) && (hand.Rotation.z < ((float)1 / numberRoad)))
            {
                //Debug.Log(" road 1");
                //s.setIntensity((float)1, GetType().Name);
                //s.setFrequency(min_vib + ((max_vib - min_vib) / numberRoad) * 3, GetType().Name);
            }
            else
            {
                //s.setFrequency((float)0, GetType().Name);
                //s.setIntensity((float)0, GetType().Name);
            }




        }


    }

    void OnTriggerEnter(Collider other)
    {
        touch_canister = true;
        
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.name[0] == 'C')
        {
            touch_canister = false;
            Destroy(other.gameObject);
            if (currentHealth < healthBar.slider.maxValue) currentHealth += 5;
            healthBar.SetHealth(currentHealth);
        }

    }
}
