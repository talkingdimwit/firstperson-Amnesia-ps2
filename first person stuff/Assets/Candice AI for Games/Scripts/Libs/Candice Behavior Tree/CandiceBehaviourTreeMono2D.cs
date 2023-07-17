using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandiceAIforGames.AI;

public class CandiceBehaviourTreeMono2D : MonoBehaviour
{
    CandiceAIController2D aiController2D;
    private CandiceBehaviorSequence2D rootNode;
    private CandiceBehaviorAction2D ScanForObjectsNode;
    private CandiceBehaviorAction2D AvoidObstaclesNode;
    private CandiceBehaviorAction2D CandicePathfindNode;
    private CandiceBehaviorAction2D canSeeEnemyNode;
    private CandiceBehaviorAction2D lookAtNode;
    private CandiceBehaviorAction2D rotateToNode;
    private CandiceBehaviorSelector2D attackOrChaseSelector;
    private CandiceBehaviorSequence2D attackSequence;
    private CandiceBehaviorAction2D withinAttackRange;
    private CandiceBehaviorAction2D attackNode;


    private CandiceBehaviorSequence2D wanderSequence;
    private CandiceBehaviorSequence2D fleeSequence;

    private CandiceBehaviorAction2D rangeAttackNode;
    private CandiceBehaviorAction2D moveNode;
    private CandiceBehaviorSequence2D followSequence;

    private CandiceBehaviorSequence2D canSeeEnemySequence;
    private CandiceBehaviorSequence2D patrolSequence;
    private CandiceBehaviorSequence2D isDeadSequence;

    private CandiceBehaviorAction2D initNode;

    private CandiceBehaviorAction2D idleNode;
    private CandiceBehaviorAction2D isDeadNode;
    private CandiceBehaviorAction2D dieNode;


    private CandiceBehaviorAction2D setAttack;
    private CandiceBehaviorAction2D setMove;

    private CandiceBehaviorAction2D isPatrollingNode;
    private CandiceBehaviorAction2D patrolNode;

    CandiceBehaviorSelector2D enemyDetctedSelector;
    CandiceBehaviorAction2D fleeNode;
    CandiceBehaviorAction2D wanderNode;
    CandiceDefaultBehaviors2D paladinBehaviours;

    public void Initialise(CandiceAIController2D aiController)
    {
        this.aiController2D = aiController;
    }
    // Start is called before the first frame update
    void Start()
    {
        aiController2D = GetComponent<CandiceAIController2D>();
        rootNode = new CandiceBehaviorSequence2D();
        rootNode.Initialise(transform, aiController2D);

        /*
         * Uncomment to test out the different behaviours.
         * Remember, you can only have one of these functions running at a time.
         * Enjoy, Cheers :-D
         */
        AggressiveAIMelee();
        //AggressiveAIRanged();
        //WanderAI();
        //CowardAI();
    }

    // Update is called once per frame
    void Update()
    {
        Evaluate();
    }

    public void Evaluate()
    {
        rootNode.Evaluate();
    }


    private void AggressiveAIMelee()
    {
        ScanForObjectsNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.ScanForObjects2D, rootNode);
        AvoidObstaclesNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AvoidObstacles2D, rootNode);
        CandicePathfindNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.CandicePathfind2D, rootNode);
        canSeeEnemyNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.EnemyDetected2D, rootNode);
        lookAtNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.LookAt2D, rootNode);
        rotateToNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.RotateTo2D, rootNode);
        attackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackMelee2D, rootNode);
        rangeAttackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackRange, rootNode);
        moveNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.MoveForward2D, rootNode);
        withinAttackRange = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.WithinAttackRange2D, rootNode);


        attackSequence = new CandiceBehaviorSequence2D();
        attackSequence.SetNodes(new List<CandiceBehaviorNode2D> { withinAttackRange, lookAtNode, attackNode });
        followSequence = new CandiceBehaviorSequence2D();
        followSequence.SetNodes(new List<CandiceBehaviorNode2D> { rotateToNode, AvoidObstaclesNode, moveNode });
        attackOrChaseSelector = new CandiceBehaviorSelector2D();
        attackOrChaseSelector.SetNodes(new List<CandiceBehaviorNode2D> { attackSequence, followSequence });
        rootNode.SetNodes(new List<CandiceBehaviorNode2D> { ScanForObjectsNode, canSeeEnemyNode, attackOrChaseSelector });
    }
    private void AggressiveAIRanged()
    {
        ScanForObjectsNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.ScanForObjects2D, rootNode);
        AvoidObstaclesNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AvoidObstacles2D, rootNode);
        CandicePathfindNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.CandicePathfind2D, rootNode);
        canSeeEnemyNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.EnemyDetected2D, rootNode);
        lookAtNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.LookAt2D, rootNode);
        rotateToNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.RotateTo2D, rootNode);
        attackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackRange, rootNode);
        rangeAttackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackRange, rootNode);
        moveNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.MoveForward2D, rootNode);
        withinAttackRange = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.WithinAttackRange2D, rootNode);


        attackSequence = new CandiceBehaviorSequence2D();
        attackSequence.SetNodes(new List<CandiceBehaviorNode2D> { withinAttackRange, lookAtNode, rangeAttackNode });
        followSequence = new CandiceBehaviorSequence2D();
        followSequence.SetNodes(new List<CandiceBehaviorNode2D> { rotateToNode, AvoidObstaclesNode, moveNode });
        attackOrChaseSelector = new CandiceBehaviorSelector2D();
        attackOrChaseSelector.SetNodes(new List<CandiceBehaviorNode2D> { attackSequence, followSequence });
        rootNode.SetNodes(new List<CandiceBehaviorNode2D> { ScanForObjectsNode, canSeeEnemyNode, attackOrChaseSelector });
    }

    private void CowardAI()
    {
        ScanForObjectsNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.ScanForObjects2D, rootNode);
        AvoidObstaclesNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AvoidObstacles2D, rootNode);
        CandicePathfindNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.CandicePathfind2D, rootNode);
        canSeeEnemyNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.EnemyDetected2D, rootNode);
        lookAtNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.LookAt2D, rootNode);
        attackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackMelee2D, rootNode);
        rangeAttackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackRange, rootNode);
        moveNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.MoveForward2D, rootNode);
        withinAttackRange = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.WithinAttackRange2D, rootNode);
        fleeNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.Flee2D, rootNode);


        fleeSequence = new CandiceBehaviorSequence2D();
        fleeSequence.SetNodes(new List<CandiceBehaviorNode2D> { ScanForObjectsNode, canSeeEnemyNode, fleeNode, lookAtNode, moveNode });
        rootNode.SetNodes(new List<CandiceBehaviorNode2D> { fleeSequence });
    }
    private void WanderAI()
    {
        ScanForObjectsNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.ScanForObjects2D, rootNode);
        AvoidObstaclesNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AvoidObstacles2D, rootNode);
        CandicePathfindNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.CandicePathfind2D, rootNode);
        canSeeEnemyNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.EnemyDetected2D, rootNode);
        lookAtNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.LookAt2D, rootNode);
        attackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackMelee2D, rootNode);
        rangeAttackNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.AttackRange, rootNode);
        moveNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.MoveForward2D, rootNode);
        withinAttackRange = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.WithinAttackRange2D, rootNode);
        wanderNode = new CandiceBehaviorAction2D(CandiceDefaultBehaviors2D.Wander, rootNode);

        wanderSequence = new CandiceBehaviorSequence2D();
        wanderSequence.SetNodes(new List<CandiceBehaviorNode2D> { wanderNode, AvoidObstaclesNode, moveNode });
        rootNode.SetNodes(new List<CandiceBehaviorNode2D> { wanderSequence });
    }
}
