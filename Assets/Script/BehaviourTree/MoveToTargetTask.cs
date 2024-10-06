using UnityEngine;

public class MoveToTargetTask : BTNode
{
    IBehaviorAI AI;
    float range;
    event InputEventFloat ForwardEvent;

    public MoveToTargetTask(IBehaviorAI _ai, float _range, InputEventFloat _forwardEvent)
    {
        AI = _ai;
        range = _range;
        ForwardEvent = _forwardEvent;
    }

    public override BTNodeStates Evaluate()
    {
        Vector3 agentPosition = AI.GetAgentTransform().position;
        Vector3 targetPosition = AI.GetMoveTargetPosition();
        float distance = Vector3.Distance(agentPosition, targetPosition);
        float thrust = Mathf.Clamp(distance / range, 30f, 50f);
        if (ForwardEvent != null)
        {
            ForwardEvent(thrust);
        }

        return BTNodeStates.SUCCESS;
    }
}
