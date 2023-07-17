using CandiceAIforGames.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CandiceBehaviorNode2D
{
    public Transform transform;
    public CandiceAIController2D aiController2D;
    public int patrolCount = 0;
    public int id;
    /* Delegate that returns the state of the node.*/
    public delegate CandiceBehaviorStates NodeReturn();

    /* The current state of the node */
    protected CandiceBehaviorStates m_nodeState;

    public CandiceBehaviorStates nodeState
    {
        get { return m_nodeState; }
    }

    /* The constructor for the node */
    public CandiceBehaviorNode2D() { }
    public void Initialise(Transform transform, CandiceAIController2D aiController2D)
    {
        this.transform = transform;
        this.aiController2D = aiController2D;
    }

    /* Implementing classes use this method to evaluate the desired set of conditions */
    public abstract CandiceBehaviorStates Evaluate();

}
