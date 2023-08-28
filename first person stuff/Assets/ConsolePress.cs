using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ConsolePress : MonoBehaviour
{
    public GameObject interactionText; // Use TMP_Text for TextMesh Pro
    public string sceneToLoad; // The name of the scene to load after interaction
    private bool isInRange = false;
    private bool canInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            interactionText.gameObject.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            interactionText.gameObject.SetActive(false);
            canInteract = false;
        }
    }

    private void Update()
    {
        if (isInRange && canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Load the specified scene
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}

