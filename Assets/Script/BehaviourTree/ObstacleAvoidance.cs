using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : BTNode
{
    IBehaviorAI AI;
    Transform agentTransform;
    float avoidDistance;
    LayerMask avoidLayerMask;
    event InputEventVector3 TurnEvent;

    public ObstacleAvoidance(IBehaviorAI _ai, float _avoidDistance, InputEventVector3 _turnEvent, LayerMask _avoidLayerMask)
    {
        AI = _ai;
        avoidDistance = _avoidDistance;
        TurnEvent = _turnEvent;
        avoidLayerMask = _avoidLayerMask;
    }

    public override BTNodeStates Evaluate()
    {
        agentTransform = AI.GetAgentTransform();
        List<Vector3> rayDirections = new List<Vector3>()
        {
            agentTransform.forward,
            HelperUtilities.GetDirectionFormAngleInDegrees(10f, agentTransform.forward, agentTransform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(-10f, agentTransform.forward, agentTransform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(10f, agentTransform.forward, agentTransform.up),
            HelperUtilities.GetDirectionFormAngleInDegrees(-10f, agentTransform.forward, agentTransform.up),
            HelperUtilities.GetDirectionFormAngleInDegrees(20f, agentTransform.forward, agentTransform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(-20f, agentTransform.forward, agentTransform.right),
            HelperUtilities.GetDirectionFormAngleInDegrees(20f, agentTransform.forward, agentTransform.up),
            HelperUtilities.GetDirectionFormAngleInDegrees(-20f, agentTransform.forward, agentTransform.up),
            (agentTransform.forward + agentTransform.right).normalized,
            (agentTransform.forward - agentTransform.right).normalized,
            (agentTransform.forward + agentTransform.up).normalized,
            (agentTransform.forward - agentTransform.up).normalized,
            (agentTransform.up).normalized,
            (-agentTransform.up).normalized,
            (agentTransform.right).normalized,
            (-agentTransform.right).normalized,
            (-agentTransform.forward).normalized,
        };
        DrawRay(rayDirections);

        RaycastHit hit;
        if ((Physics.Raycast(agentTransform.position, rayDirections[0], out hit, avoidDistance, avoidLayerMask) ||
            Physics.Raycast(agentTransform.position, rayDirections[1], out hit, avoidDistance, avoidLayerMask) ||
            Physics.Raycast(agentTransform.position, rayDirections[2], out hit, avoidDistance, avoidLayerMask) ||
            Physics.Raycast(agentTransform.position, rayDirections[3], out hit, avoidDistance, avoidLayerMask) ||
            Physics.Raycast(agentTransform.position, rayDirections[4], out hit, avoidDistance, avoidLayerMask)) && 
            hit.transform.root.gameObject != agentTransform.root.gameObject)
        {
            List<Vector3> avoidpaths = RondomList(rayDirections);
            for (int i = 0; i < avoidpaths.Count; i++)
            {
                bool canTurn = CheckTurn(avoidpaths[i]);
                if (canTurn)
                {
                    return BTNodeStates.SUCCESS;
                }
            }
        }
        Vector3 agentPosition = agentTransform.position;
        Vector3 targetPosition = AI.GetTargetPosition();
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

    private void DrawRay(List<Vector3> rayDirections)
    {
        foreach (var vector in rayDirections)
        {
            Debug.DrawRay(agentTransform.position, vector * avoidDistance, Color.blue);
        }
    }

    private List<Vector3> RondomList(List<Vector3> list)
    {
        List<Vector3> originalList = list;
        List<Vector3> newList = new List<Vector3>();
        while (originalList.Count > 0)
        {
            int index = Random.Range(0, originalList.Count - 1);
            newList.Add(originalList[index]);
            originalList.RemoveAt(index);
        }
        return newList;
    }

    private bool CheckTurn(Vector3 rayDirection)
    {
        RaycastHit hit;
        float distance = avoidDistance / 4f; 
        if (!Physics.Raycast(agentTransform.position, rayDirection, out hit, distance, avoidLayerMask))
        {
            Vector3 newHeading = rayDirection;
            Vector3 newTarget = agentTransform.position + (newHeading * distance);
            AI.SetTempTarget(newTarget);
            if (TurnEvent != null)
            {
                TurnEvent(newHeading);
            }
            Debug.DrawRay(agentTransform.position, rayDirection * distance, Color.red);
            return true;
        }
        return false;
    }
}
