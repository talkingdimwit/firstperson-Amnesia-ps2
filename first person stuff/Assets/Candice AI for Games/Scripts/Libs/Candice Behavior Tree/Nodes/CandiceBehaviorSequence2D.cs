using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;

[Serializable]
public class CandiceBehaviorSequence2D : CandiceBehaviorNode2D
{
    /** Children nodes that belong to this sequence */
    private List<CandiceBehaviorNode2D> m_nodes = new List<CandiceBehaviorNode2D>();

    /** Must provide an initial set of children nodes to work */
    public CandiceBehaviorSequence2D()
    {

    }
    public void SetNodes(List<CandiceBehaviorNode2D> nodes)
    {
        m_nodes = nodes;
    }
    public void AddNode(CandiceBehaviorNode2D node)
    {
        m_nodes.Add(node);
    }
    /* If any child node returns a failure, the entire node fails. Whence all  
     * nodes return a success, the node reports a success. */
    public override CandiceBehaviorStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (CandiceBehaviorNode2D node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case CandiceBehaviorStates.FAILURE:
                    m_nodeState = CandiceBehaviorStates.FAILURE;
                    return m_nodeState;
                case CandiceBehaviorStates.SUCCESS:
                    continue;
                case CandiceBehaviorStates.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = CandiceBehaviorStates.SUCCESS;
                    return m_nodeState;
            }
        }
        m_nodeState = anyChildRunning ? CandiceBehaviorStates.RUNNING : CandiceBehaviorStates.SUCCESS;
        return m_nodeState;
    }
}
