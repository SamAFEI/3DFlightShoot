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
        AI.SetTarget(null);
        AI.SetTargetPosition(Random.insideUnitSphere * range);
        return BTNodeStates.SUCCESS;
    }
}
