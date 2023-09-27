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

    Frame frame = null;
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

            if(frame != null)
            {
                try
                {
                    hand = frame.Hands[0];
                    car.transform.position = new Vector3((hand.Rotation.z * -2), car.transform.position.y, car.transform.position.z);
                }
                catch
                {

                }
                
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
