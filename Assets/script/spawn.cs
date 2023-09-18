using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{

    public GameObject Canister;
    public float interval = 50;
    private float counter = 0;
    float randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        counter += 1;

        if (counter >= interval)
        {
            randomNumber = Random.Range(0f, 2.1f);

            counter = 0;

            Instantiate(Canister, new Vector3(-randomNumber, 0.3f,5.0f), new Quaternion(0f, 0f, 0f,0f));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
