using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    private NavMeshAgent Enemy;
    public Transform PlayerTarget;
    public Transform targetLocation; // Assign the target location in the Inspector


    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy.SetDestination(PlayerTarget.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Move the collided object to the target location
            collision.gameObject.transform.position = targetLocation.position;
        }
    }
}
