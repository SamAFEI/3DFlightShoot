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

    public Vector3 TargetPos = Vector3.zero;

    //Behaviors
    public Selector rootAI;
    public Sequence CheckArrivalSeqence;
    public Sequence MoveSquence;
    public Sequence DecideToAttack;
    public Selector SelectTargetType;
    public bool avoiding;
    public float avoidDistance = 100f;
    public LayerMask avoidLayerMask;
    public Vector3 temporaryTarget;
    public Vector3 savedTargetPosition;

    GameObject target = null;
    public string enemyFaction = "PlayerFaction";

    private void Start()
    {
        DecideToAttack = new Sequence(new List<BTNode>
        {
            new RandomChanceConditionalTask(1, 100, 10),
            new FindNewTargetTask(this, enemyFaction)
        });
        SelectTargetType = new Selector(new List<BTNode>
        {
            DecideToAttack,
            new FindWanderPointTask(this, 50f)
        });
        CheckArrivalSeqence = new Sequence(new List<BTNode>
        {
            new CheckArrivealTask(this, 20f),
            SelectTargetType
        });
        MoveSquence = new Sequence(new List<BTNode>
        {
            new ObstacleAvoidance(this, avoidDistance, TurnEvent, avoidLayerMask),
            new MoveToTargetTask(this, 1f, ForwardEvent),
            new IsTargetVisible(this),  
            new FireWeaponTask(this, Fire01Event)
        });
        rootAI = new Selector(new List<BTNode>
        {
            CheckArrivalSeqence,
            MoveSquence
        });

        new FindWanderPointTask(this, 50f).Evaluate();
    }

    private void Update()
    {
        rootAI.Evaluate();
    }

    public Vector3 SetTargetPosition(Vector3 targetPosition)
    {
        TargetPos = targetPosition;
        return TargetPos;
    }

    public Vector3 GetTargetPosition()
    {
        return TargetPos;
    }

    public Transform GetAgentTransform()
    {
        return transform;
    }

    public GameObject SetTarget(GameObject newTarget)
    {
        target = newTarget;
        if (target != null) { TargetPos = target.transform.position; }
        return target;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public bool GetAvoidingFlag()
    {
        return avoiding;
    }

    public Vector3 SetTempTarget(Vector3 postion)
    {
        avoiding = true;
        temporaryTarget = postion;
        savedTargetPosition = TargetPos;
        return postion;
    }

    public Vector3 ReturnToSaveTarget()
    {
        avoiding = false;
        temporaryTarget = Vector3.zero;
        TargetPos = savedTargetPosition;
        return TargetPos;
    }
}
