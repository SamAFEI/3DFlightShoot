using UnityEngine;

public class CheckArrivealTask : BTNode
{
    IBehaviorAI AI;
    float closeDistance;
    public CheckArrivealTask(IBehaviorAI _ai, float _closeDistance)
    {
        AI = _ai;
        closeDistance = _closeDistance;
    }

    public override BTNodeStates Evaluate()
    {
        Vector3 agentPosition = AI.GetAgentTransform().position;
        Vector3 targetPosition = AI.GetMoveTargetPosition();
        float distance = Vector3.Distance(agentPosition, targetPosition);
        if (distance < closeDistance)
        {
            return BTNodeStates.SUCCESS;
        }
        return BTNodeStates.FAILURE;
    }
}
