using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouve : MonoBehaviour
{
    public HealthBar healthBar;

    public float vitesse = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(healthBar.GetHealth() >= healthBar.slider.maxValue / 2)
        {
            vitesse = 3.0f;
        }
        else
        {
            if(healthBar.GetHealth() == 0 )vitesse = 1.0f;
            else vitesse = 2.0f;
        }

        
        if (name == "Canister(Clone)")
        {
            transform.Translate(Vector3.back * vitesse * Time.deltaTime);

            if (transform.position.z < -7f) Destroy(this.gameObject);
        }
        foreach (Transform eachChild in transform)
        {
            if(name == "RoadLines") eachChild.transform.Translate(Vector3.left * vitesse * Time.deltaTime);
            else eachChild.transform.Translate(Vector3.back * vitesse * Time.deltaTime);

            if (eachChild.transform.position.z < -8f)
            {
                //Debug.Log(eachChild.transform.position.z);
                //Debug.Log(eachChild.name);
                eachChild.transform.position = new Vector3(eachChild.transform.position.x, 0.22f , 7.5f);

            }
        }
    }
}
