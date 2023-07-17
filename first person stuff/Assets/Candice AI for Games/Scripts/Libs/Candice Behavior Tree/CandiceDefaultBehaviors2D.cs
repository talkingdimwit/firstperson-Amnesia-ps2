using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;

public class CandiceDefaultBehaviors2D
{
    public static CandiceBehaviorStates ObjectDetected2D(CandiceBehaviorNode2D rootNode)
    {

        CandiceAIController2D agent = rootNode.aiController2D;
        if (agent.ObjectDetected)
        {
            return CandiceBehaviorStates.SUCCESS;
        }
        else
            return CandiceBehaviorStates.FAILURE;
    }
    public static CandiceBehaviorStates PlayerDetected2D(CandiceBehaviorNode2D rootNode)
    {

        CandiceAIController2D agent = rootNode.aiController2D;
        if (agent.PlayerDetected)
        {
            return CandiceBehaviorStates.SUCCESS;
        }
        else
            return CandiceBehaviorStates.FAILURE;
    }
    public static CandiceBehaviorStates EnemyDetected2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        if (agent.EnemyDetected)
        {
            return CandiceBehaviorStates.SUCCESS;
        }
        else
            return CandiceBehaviorStates.FAILURE;
    }
    public static CandiceBehaviorStates AllyDetected2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        if (agent.AllyDetected)
        {
            return CandiceBehaviorStates.SUCCESS;
        }
        else
            return CandiceBehaviorStates.FAILURE;
    }

    public static CandiceBehaviorStates LookAt2D(CandiceBehaviorNode2D rootNode)
    {
        try
        {
            CandiceAIController2D agent = rootNode.aiController2D;
            agent.movementModule.LookAt2D(agent.transform, agent);
            return CandiceBehaviorStates.SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return CandiceBehaviorStates.FAILURE;
        }

    }
    public static CandiceBehaviorStates RotateTo2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceBehaviorStates state = CandiceBehaviorStates.SUCCESS;
        try
        {
            float desiredAngle = 180;
            int direction = 1;
            float angle = Vector3.Angle((rootNode.aiController2D.MainTarget.transform.position - rootNode.aiController2D.transform.position), rootNode.aiController2D.transform.right);
            if (angle > 90)
                angle = 360 - angle;
            if (angle > desiredAngle)
                direction = -1;

            float rotation = (direction * rootNode.aiController2D.RotationSpeed) * Time.deltaTime;
            rootNode.aiController2D.transform.Rotate(0, rotation, 0);
        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("CandiceDefaultBehaviors.RotateTo: " + e.Message);

        }

        return state;
    }
    public static CandiceBehaviorStates AvoidObstacles2D(CandiceBehaviorNode2D rootNode)
    {
        try
        {
            CandiceAIController2D agent = rootNode.aiController2D;
            agent.AvoidObstacles2D();
            return CandiceBehaviorStates.SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return CandiceBehaviorStates.FAILURE;
        }

    }
    public static CandiceBehaviorStates CandicePathfind2D(CandiceBehaviorNode2D rootNode)
    {
        try
        {
            CandiceAIController2D agent = rootNode.aiController2D;
            if (agent.CandicePathfind())
                return CandiceBehaviorStates.SUCCESS;
            else
                return CandiceBehaviorStates.FAILURE;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return CandiceBehaviorStates.FAILURE;
        }

    }


    public static CandiceBehaviorStates SetMovePoint2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceBehaviorStates state = CandiceBehaviorStates.FAILURE;
        CandiceAIController2D agent = rootNode.aiController2D;
        try
        {
            if (agent.MainTarget != null)
            {
                agent.MovePoint = agent.MainTarget.transform.position;
                state = CandiceBehaviorStates.SUCCESS;
            }
            else
            {
                Debug.LogError("CandiceDefaultBehaviors.SetMoveTarget: Main Target is NULL");
            }
        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("CandiceDefaultBehaviors.SetMovePoint: " + e.Message);

        }
        return state;
    }
    public static CandiceBehaviorStates MoveForward2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.IsMoving = true;
        CandiceBehaviorStates state = CandiceBehaviorStates.SUCCESS;
        try
        {
            agent.movementModule.MoveForward2D(agent.transform, agent);

        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("DefaultBehaviors.MoveForward: " + e.Message);
        }

        return state;
    }
    public static CandiceBehaviorStates MoveForwardWithSlopeAlignment2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.IsMoving = true;
        CandiceBehaviorStates state = CandiceBehaviorStates.SUCCESS;
        try
        {
            agent.movementModule.MoveForwardWithSlopeAlignment2D(agent.transform, agent);

        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("DefaultBehaviors.MoveForwardWithSlopeAlignment: " + e.Message);
        }

        return state;
    }
    public static CandiceBehaviorStates Flee2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.Flee();
        return CandiceBehaviorStates.SUCCESS;
    }

    public static CandiceBehaviorStates Wander(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.Wander();
        return CandiceBehaviorStates.SUCCESS;
    }
    public static CandiceBehaviorStates WaypointPatrol2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.IsMoving = true;
        CandiceBehaviorStates state = CandiceBehaviorStates.SUCCESS;
        try
        {
            agent.WaypointPatrol();

        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("DefaultBehaviors.WaypointPatrol: " + e.Message);
        }

        return state;
    }

    public static CandiceBehaviorStates WithinAttackRange2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;

        if (agent.WithinAttackRange())
            return CandiceBehaviorStates.SUCCESS;
        else
            return CandiceBehaviorStates.FAILURE;
    }
    public static CandiceBehaviorStates SetAttackTarget2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceBehaviorStates state = CandiceBehaviorStates.FAILURE;
        CandiceAIController2D agent = rootNode.aiController2D;
        try
        {
            if (agent.MainTarget != null)
            {
                agent.AttackTarget = agent.MainTarget;
                state = CandiceBehaviorStates.SUCCESS;
            }
            else
            {
                Debug.LogError("CandiceDefaultBehaviors.SetAttackTarget: Main Target is NULL");
            }
        }
        catch (Exception e)
        {
            state = CandiceBehaviorStates.FAILURE;
            Debug.LogError("CandiceDefaultBehaviors.SetAttackTarget: " + e.Message);
        }
        return state;
    }
    public static CandiceBehaviorStates AttackMelee2D(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.AttackMelee();
        return CandiceBehaviorStates.SUCCESS;
    }
    public static CandiceBehaviorStates AttackRange(CandiceBehaviorNode2D rootNode)
    {
        CandiceAIController2D agent = rootNode.aiController2D;
        agent.AttackRanged();
        return CandiceBehaviorStates.SUCCESS;
    }

    public static CandiceBehaviorStates ScanForObjects2D(CandiceBehaviorNode2D rootNode)
    {
        try
        {
            CandiceAIController2D agent = rootNode.aiController2D;
            agent.ScanForObjects2DB();
            return CandiceBehaviorStates.SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return CandiceBehaviorStates.FAILURE;
        }

    }
}
