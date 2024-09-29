using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindNewTargetTask : BTNode
{
    IBehaviorAI AI;
    string enemyFaction;
    public FindNewTargetTask(IBehaviorAI _ai, string _enemyFaction)
    {
        AI = _ai;
        enemyFaction = _enemyFaction;
    }
    public override BTNodeStates Evaluate()
    {
        List<GameObject> targets = new List<GameObject>();
        targets = GameObject.FindGameObjectsWithTag(enemyFaction).ToList();

        if (targets.Count > 0)
        {
            int randomChoice = Random.Range(0, targets.Count);
            AI.SetTarget(targets[randomChoice]);
            return BTNodeStates.SUCCESS;
        }
        return BTNodeStates.FAILURE;
    }
}