using System.Collections.Generic;

public class Sequence : BTNode
{
    private List<BTNode> Nodes = new List<BTNode>();

    public Sequence(List<BTNode> nodes)
    {
        Nodes = nodes;
    }

    public override BTNodeStates Evaluate()
    {
        bool childRunning = false;
        foreach (BTNode node in Nodes)
        {
            switch (node.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    currentNodeState = BTNodeStates.FAILURE;
                    return currentNodeState;
                case BTNodeStates.SUCCESS:
                    continue;
                case BTNodeStates.RUNNING:
                    childRunning = true;
                    continue;
                default:
                    currentNodeState = BTNodeStates.SUCCESS;
                    return currentNodeState;
            }
        }
        currentNodeState = childRunning ? BTNodeStates.RUNNING : BTNodeStates.SUCCESS;
        return currentNodeState;
    }

}
