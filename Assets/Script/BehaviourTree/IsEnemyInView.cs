using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsEnemyInView : BTNode
{
    IBehaviorAI AI;
    float enemyDistance = 250f;
    LayerMask enemyFactionLayerMask;
    Transform agentTransform;

    public IsEnemyInView(IBehaviorAI _ai, LayerMask _enemyFactionLayerMask)
    {
        AI = _ai;
        enemyFactionLayerMask = _enemyFactionLayerMask;
    }

    public override BTNodeStates Evaluate()
    {
        agentTransform = AI.GetAgentTransform();
        List<Collider> spottedEnemies = Physics.OverlapSphere(agentTransform.position, enemyDistance, enemyFactionLayerMask).ToList();
        foreach (Collider collider in spottedEnemies)
        {
            Vector3 target = collider.transform.position - agentTransform.position;
            if (Vector3.Angle(agentTransform.forward, target) <= 30f)
            {
                RaycastHit hit;
                if (Physics.Raycast(agentTransform.position, target, out hit, enemyDistance)
                    && (enemyFactionLayerMask & (1 << hit.transform.root.gameObject.layer)) != 0)
                {
                    AI.SetFireTargetPosition(hit.point);
                    return BTNodeStates.SUCCESS;
                }
            }
        }
        return BTNodeStates.FAILURE;
    }
}
