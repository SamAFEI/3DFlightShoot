using System.Collections.Generic;

public class Selector : BTNode
{
    protected List<BTNode> Nodes = new List<BTNode>();

    public Selector(List<BTNode> nodes)
    {
        Nodes = nodes;
    }

    public override BTNodeStates Evaluate()
    {
        foreach (BTNode node in Nodes)
        {
            switch (node.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    continue;
                case BTNodeStates.SUCCESS:
                    currentNodeState = BTNodeStates.SUCCESS;
                    return currentNodeState;
                default:
                    continue;
            }
        }
        currentNodeState = BTNodeStates.FAILURE;
        return currentNodeState;
    }
}
