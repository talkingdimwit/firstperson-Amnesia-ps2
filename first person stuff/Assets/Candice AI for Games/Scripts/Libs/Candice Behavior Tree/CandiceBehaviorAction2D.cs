using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;

public class CandiceBehaviorAction2D : CandiceBehaviorNode2D
{
    /* Method signature for the action. */
    public delegate CandiceBehaviorStates ActionNodeDelegate(CandiceBehaviorNode2D rootNode);
    CandiceBehaviorNode2D rootNode;
    /* The delegate that is called to evaluate this node */
    private ActionNodeDelegate m_action;

    /* Because this node contains no logic itself, 
     * the logic must be passed in in the form of  
     * a delegate. As the signature states, the action 
     * needs to return a NodeStates enum */
    public CandiceBehaviorAction2D(ActionNodeDelegate action, CandiceBehaviorNode2D rootNode)
    {
        this.rootNode = rootNode;
        m_action = action;
    }

    /* Evaluates the node using the passed in delegate and  
     * reports the resulting state as appropriate */
    public override CandiceBehaviorStates Evaluate()
    {
        switch (m_action(rootNode))
        {
            case CandiceBehaviorStates.SUCCESS:
                m_nodeState = CandiceBehaviorStates.SUCCESS;
                return m_nodeState;
            case CandiceBehaviorStates.FAILURE:
                m_nodeState = CandiceBehaviorStates.FAILURE;
                return m_nodeState;
            case CandiceBehaviorStates.RUNNING:
                m_nodeState = CandiceBehaviorStates.RUNNING;
                return m_nodeState;
            default:
                m_nodeState = CandiceBehaviorStates.FAILURE;
                return m_nodeState;
        }
    }
}
