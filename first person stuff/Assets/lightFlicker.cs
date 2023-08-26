using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minFlickerInterval = 0.1f;
    public float maxFlickerInterval = 0.5f;

    private bool isLightOn = true;
    private float timer = 0f;
    private float flickerInterval;

    private void Start()
    {
        GenerateRandomInterval();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flickerInterval)
        {
            isLightOn = !isLightOn;
            flickeringLight.enabled = isLightOn;
            timer = 0f;

            GenerateRandomInterval();
        }
    }

    private void GenerateRandomInterval()
    {
        flickerInterval = Random.Range(minFlickerInterval, maxFlickerInterval);
    }
}

