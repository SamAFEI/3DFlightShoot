using UnityEngine;

public class IsTargetVisible : BTNode
{
    IBehaviorAI AI;
    public IsTargetVisible(IBehaviorAI _ai)
    {
        AI = _ai;
    }
    public override BTNodeStates Evaluate()
    {
        if (AI.GetTarget() == null)
        {
            return BTNodeStates.FAILURE;
        }
        RaycastHit hit;
        if (Physics.Raycast(AI.GetTransform().position, AI.GetTransform().forward, out hit, 200f))
        {
            if (hit.collider.transform.root.gameObject == AI.GetTarget())
            {
                return BTNodeStates.SUCCESS;
            }
        }
        return BTNodeStates.FAILURE;
    }
}
