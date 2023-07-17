using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;


[Serializable]
public class CandiceBehaviorSelector2D : CandiceBehaviorNode2D
{
    /** The child nodes for this CandiceBehaviorSelector */
    protected List<CandiceBehaviorNode2D> m_nodes = new List<CandiceBehaviorNode2D>();


    /** The constructor requires a lsit of child nodes to be  
        * passed in*/
    public CandiceBehaviorSelector2D()
    {

    }
    public void SetNodes(List<CandiceBehaviorNode2D> nodes)
    {
        m_nodes = nodes;
    }
    public List<CandiceBehaviorNode2D> GetNodes()
    {
        return m_nodes;
    }
    public void AddNode(CandiceBehaviorNode2D node)
    {
        m_nodes.Add(node);
    }
    /* If any of the children reports a success, the CandiceBehaviorSelector will 
        * immediately report a success upwards. If all children fail, 
        * it will report a failure instead.*/
    public override CandiceBehaviorStates Evaluate()
    {
        foreach (CandiceBehaviorNode2D node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case CandiceBehaviorStates.FAILURE:
                    continue;
                case CandiceBehaviorStates.SUCCESS:
                    m_nodeState = CandiceBehaviorStates.SUCCESS;
                    return m_nodeState;
                case CandiceBehaviorStates.RUNNING:
                    m_nodeState = CandiceBehaviorStates.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = CandiceBehaviorStates.FAILURE;
        return m_nodeState;
    }
}

