public class FireWeaponTask : BTNode
{
    IBehaviorAI AI;
    InputEventVector3 fireWeaponEvent;

    public FireWeaponTask(IBehaviorAI _ai, InputEventVector3 _fireWeaponEvent)
    {
        AI = _ai;
        fireWeaponEvent = _fireWeaponEvent;
    }

    public override BTNodeStates Evaluate()
    {
        if (fireWeaponEvent != null)
        {
            fireWeaponEvent(AI.GetFireTargetPosition());
            return BTNodeStates.SUCCESS;
        }
        return BTNodeStates.FAILURE;
    }
}
