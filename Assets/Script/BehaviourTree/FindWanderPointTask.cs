using UnityEngine;

public class FindWanderPointTask : BTNode
{
    float range;
    IBehaviorAI AI;

    public FindWanderPointTask(IBehaviorAI _AI, float _range)
    {
        AI = _AI;
        range = _range;
    }

    public override BTNodeStates Evaluate()
    {
        AI.SetMoveTarget(null);
        AI.SetMoveTargetPosition(GameManager.GetPlayerPosition() + Random.insideUnitSphere * range);
        return BTNodeStates.SUCCESS;
    }
}
