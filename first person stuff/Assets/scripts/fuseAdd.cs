using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuseAdd : MonoBehaviour
{
    public GameObject replacementObject; // Object to activate after collision
    public GameObject animationObject;   // Object with animation to activate

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fuse"))
        {
            // Disable colliders and renderers of both collided objects
            Collider[] colliders = collision.gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            Renderer[] renderers = collision.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.enabled = false;
            }

            colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.enabled = false;
            }

            // Activate the replacement object
            if (replacementObject != null)
            {
                replacementObject.SetActive(true);
                Animation anim = animationObject.GetComponent<Animation>();
                anim.enabled = true;
            }
        }
    }
}