using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationLight : MonoBehaviour
{
    public GameObject flashLight;
    public GameObject spotlight;
    bool lightOn = true;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && lightOn == true)
        {
            flashLight.GetComponent<Animator>().Play("lightDown");
            spotlight.gameObject.SetActive(false);
            lightOn = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) && lightOn == false)
        {
            flashLight.GetComponent<Animator>().Play("flashLightUp");
            spotlight.gameObject.SetActive(true);
            lightOn = true;
        }
    }
}
