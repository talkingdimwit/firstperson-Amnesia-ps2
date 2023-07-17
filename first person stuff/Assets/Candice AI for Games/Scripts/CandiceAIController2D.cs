using CandiceAIforGames.AI;
using CandiceAIforGames.AI.Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandiceAIController2D : CandiceAIController
{

    private BoxCollider2D col2D;
    CandiceAIManager candice2D;
    CandiceBehaviorTree BehaviorTree;
    [SerializeField]
    private bool hasAttackAnimation2D = false;
    [SerializeField]
    private List<string> enemyTags2D = new List<string>();
    public List<string> EnemyTags2D { get => enemyTags2D; set => enemyTags2D = value; }
    [SerializeField]
    private float obstacleAvoidanceAOE2D = 0.5f;
    [SerializeField]
    private SensorType sensorType2D = SensorType.Sphere;
    [SerializeField]
    private List<string> objectTags2D = new List<string>();
    [SerializeField]
    private float detectionRadius2D = 3f;
    [SerializeField]
    private float lineOfSight2D = 3f;
    [SerializeField]
    private float detectionHeight2D = 3f;
    [SerializeField]
    private bool Is3D = false;
    [SerializeField]
    private float maxHitPoints2D = 100f;
    [SerializeField]
    private float hitPoints2D = 100f;
    /*
     * Modules
     */
    public CandiceModuleMovement2D movementModule;
    public CandiceModuleDetection detectionModule;
    public CandiceModuleCombat combatModule;

    // Start is called before the first frame update
    void Start()
    {
        //Check if there is a Candice AI Manager Component in the scene.
        candice2D = FindObjectOfType<CandiceAIManager>();
        if (candice2D == null)
        {
            Debug.LogError("You need to attach a Candice AI Manager Component to an Empty GameObject.");
        }
        else
        {
            CandiceAIManager.getInstance().RegisterAgent(gameObject, onRegistration2DComplete);
        }
        col2D = GetComponent<BoxCollider2D>();
        Debug.Log(col2D);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Helper Methods 
    /// <summary>
    /// Callback function when the agent is successfully registered with Candice.
    /// </summary>
    /// <param name="isRegistered">Whether or not the registration was successful.</param>
    /// <param name="agentId">The unique agent ID returned from Candice.</param>
    void onRegistration2DComplete(bool isRegistered, int agentId)
    {

        if (isRegistered)
        {
            AgentID = agentId;
            combatModule = new CandiceModuleCombat(transform, onAttackComplete, "Agent" + AgentID + "-CandiceModuleCombat");
            movementModule = new CandiceModuleMovement2D("Agent" + AgentID + "-CandiceModuleMovement");
            detectionModule = new CandiceModuleDetection(gameObject.transform, onObjectFound, "Agent" + AgentID + "-CandiceModuleDetection");
            HalfHeight = col2D.bounds.extents.y;
            //BehaviorTree = GetComponent<CandiceBehaviorTree>();
            //if (BehaviorTree != null && BehaviorTree.behaviorTree != null)
            if (BehaviorTree != null && BehaviorTree.nodes != null)
            {
                BehaviorTree.Initialise();
                BehaviorTree.CreateBehaviorTree(this);
            }
            Debug.Log("Agent " + AgentID + " successfully registered with Candice.");
        }

    }
    #endregion

    void onObjectFound(CandiceDetectionResults results)
    {
        /*This is where you put your detection logic. 
         * The code below is only a sample to get you started.
         */
        AllyDetected = false;
        EnemyDetected = false;
        PlayerDetected = false;
        foreach (string key in results.objects.Keys)
        {
            if (EnemyTags2D.Contains(key))
            {
                EnemyDetected = true;
                Enemies.AddRange(results.objects[key]);
                MainTarget = Enemies[0];
                MovePoint = MainTarget.transform.position;
                LookPoint = MainTarget.transform.position;
                AttackTarget = Enemies[0];
            }
            if (AllyTags.Contains(key))
            {
                AllyDetected = true;
                Allies.AddRange(results.objects[key]);
            }
            if (key == "Player")
            {
                PlayerDetected = true;
                Players.AddRange(results.objects[key]);
                Player = Players[0];

            }
        }
    }

    public bool WithinAttackRange()
    {
        float distance = float.MaxValue;
        try
        {
            distance = Vector3.Distance(transform.position, AttackTarget.transform.position);
        }
        catch (Exception e)
        {
            Debug.LogError("DefaultBehaviors.WithinAttackRange: No target within attack range: " + e.Message);
        }
        if (distance <= AttackRange)
        {
            LookPoint = AttackTarget.transform.position;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Use the detection module's obstacle avoidance to evade nearby obstacles.
    /// </summary>
    public void AvoidObstacles2D()
    {

        if (col2D != null)
        {
            HalfHeight = col2D.bounds.extents.x * 2;
        }
        else
        {
            HalfHeight = gameObject.transform.localScale.x * 2;
        }
        detectionModule.AvoidObstacles(MainTarget.transform, MovePoint, transform, HalfHeight + obstacleAvoidanceAOE2D, RotationSpeed, true, ObstacleAvoidaceDistance, DetectionLines, PerceptionMask);
    }

    private void FindTarget(GameObject target)
    {
        do
        {
            target.transform.position = new Vector3(UnityEngine.Random.Range(transform.position.x - DetectionRadius, transform.position.x + DetectionRadius), 1, UnityEngine.Random.Range(transform.position.z - DetectionRadius, transform.position.z + DetectionRadius));
        }
        while (!VerifyPoint(target.transform.position));
    }

    private bool VerifyPoint(Vector3 point)
    {
        //This method verifies if the chosen wander/flee point is within the game map and is on a walkable region
        bool isValid = false;
        CandiceGrid grid = CandiceAIManager.getInstance().grid;
        Vector3 worldBottomLeft = grid.worldBottomLeft;

        if (point.x > worldBottomLeft.x && point.x < worldBottomLeft.x + grid.gridWorldSize.x)
        {
            if (point.z > worldBottomLeft.z && point.z < worldBottomLeft.z + grid.gridWorldSize.x)
            {
                isValid = true;
            }
        }

        if (!CandiceAIManager.getInstance().IsPointWalkable(point))
        {
            isValid = false;
        }


        return isValid;
    }

    public void ScanForObjects2DB()
    {
        CandiceDetectionRequest req = new CandiceDetectionRequest(sensorType2D, objectTags2D, detectionRadius2D, detectionHeight2D, lineOfSight2D, Is3D);
        detectionModule.ScanForObjects2D(req);
    }

    public void AttackMelee()
    {
        if (hasAttackAnimation2D && !IsAttacking)
        {
            //Play attack animation which will call the Damage() function
            IsAttacking = true;
        }
        else if (!IsAttacking)
        {
            IsAttacking = true;
            //StartCoroutine(combatModule.DealTimedDamage2D(AttackSpeed, AttackDamage, AttackRange, DamageAngle, enemyTags2D, onAttackComplete));
            //StartCoroutine(combatModule.DealTimedDamage2D(AttackSpeed, AttackDamage, AttackRange, DamageAngle, enemyTags2D, onAttackComplete));
            StartCoroutine(combatModule.DealTimedDamage2D(AttackSpeed, AttackDamage, AttackRange, DamageAngle, enemyTags));
        }
    }

    public void ReceiveDamage(float damage)
    {
        hitPoints2D = combatModule.ReceiveDamage(damage, hitPoints2D);
    }

    void onAttackComplete(bool success)
    {
        IsAttacking = false;
    }

}
