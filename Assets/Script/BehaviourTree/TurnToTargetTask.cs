using UnityEngine;

public class TurnToTargetTask : BTNode
{
    IBehaviorAI AI;
    event InputEventVector3 TurnEvent;

    public TurnToTargetTask(IBehaviorAI _ai, InputEventVector3 _trunEvent)
    {
        AI = _ai;
        TurnEvent = _trunEvent;
    }

    public override BTNodeStates Evaluate()
    {
        Vector3 agentPosition = AI.GetAgentTransform().position;
        Vector3 targetPosition = AI.GetMoveTargetPosition();
        Vector3 desiredHeading = (targetPosition - agentPosition);
        if (TurnEvent != null)
        {
            if (Vector3.Angle(agentPosition, desiredHeading) > 10)
            {
                TurnEvent(desiredHeading);
            }
        }
        return BTNodeStates.SUCCESS;
    }
}
