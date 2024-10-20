using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IControllerInput, IBehaviorAI
{
    public event InputEventFloat ForwardEvent;
    public event InputEventFloat HorizontalStrafeEvent;
    public event InputEventFloat VerticalStrafeEvent;
    public event InputEventFloat YawEvent;
    public event InputEventFloat PitchEvent;
    public event InputEventFloat RollEvent;
    public event InputEventVector3 TurnEvent;
    public event InputEventVector3 Fire01Event;
    public event InputEventVector3 Fire02Event;

    public Vector3 MoveTargetPos = Vector3.zero;
    public Vector3 FireTargetPos = Vector3.zero;

    //Behaviors
    public Selector rootAI;
    public Sequence CheckArrivalSeqence;
    public Sequence MoveSquence;
    public Sequence DecideToAttack;
    public Selector SelectTargetType;
    public Sequence attackAI;
    public bool avoiding;
    public float avoidDistance = 100f;
    public LayerMask avoidLayerMask;
    public Vector3 temporaryTarget;
    public Vector3 savedTargetPosition;

    GameObject target = null;
    public LayerMask enemyFactionLayerMask;
    public string enemyFaction = "PlayerFaction";

    private void Start()
    {
        DecideToAttack = new Sequence(new List<BTNode>
        {
            new RandomChanceConditionalTask(1, 100, 50),
            new FindNewTargetTask(this, enemyFaction)
        });
        SelectTargetType = new Selector(new List<BTNode>
        {
            DecideToAttack,
            new FindWanderPointTask(this, 200f)
        });
        CheckArrivalSeqence = new Sequence(new List<BTNode>
        {
            new CheckArrivealTask(this, 80f),
            SelectTargetType
        });
        MoveSquence = new Sequence(new List<BTNode>
        {
            new ObstacleAvoidance(this, avoidDistance, TurnEvent, avoidLayerMask),
            new MoveToTargetTask(this, 40f, ForwardEvent),
            //new IsTargetVisible(this),  
            //new FireWeaponTask(this, Fire01Event)
        });
        rootAI = new Selector(new List<BTNode>
        {
            CheckArrivalSeqence,
            MoveSquence
        });

        attackAI = new Sequence(new List<BTNode>
        {
            new IsEnemyInView(this, enemyFactionLayerMask),
            new FireWeaponTask(this, Fire01Event)
        });

        new FindWanderPointTask(this, 200f).Evaluate();
    }

    private void Update()
    {
        rootAI.Evaluate();
        attackAI.Evaluate();
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public Transform GetAgentTransform()
    {
        return transform;
    }

    public Vector3 SetMoveTargetPosition(Vector3 targetPosition)
    {
        MoveTargetPos = targetPosition;
        return MoveTargetPos;
    }

    public Vector3 GetMoveTargetPosition()
    {
        return MoveTargetPos;
    }

    public GameObject SetMoveTarget(GameObject newTarget)
    {
        target = newTarget;
        if (target != null) { MoveTargetPos = target.transform.position; }
        return target;
    }

    public GameObject GetMoveTarget()
    {
        return target;
    }

    public bool GetAvoidingFlag()
    {
        return avoiding;
    }

    public Vector3 SetTempTarget(Vector3 postion)
    {
        avoiding = true;
        temporaryTarget = postion;
        savedTargetPosition = MoveTargetPos;
        return postion;
    }

    public Vector3 ReturnToSaveTarget()
    {
        avoiding = false;
        temporaryTarget = Vector3.zero;
        MoveTargetPos = savedTargetPosition;
        return MoveTargetPos;
    }

    public Vector3 GetFireTargetPosition()
    {
        return FireTargetPos;
    }

    public Vector3 SetFireTargetPosition(Vector3 targetPosition)
    {
        FireTargetPos = targetPosition;
        return FireTargetPos;
    }
}
