using UnityEngine;

public interface IBehaviorAI
{
    public Transform GetTransform();
    public Transform GetAgentTransform();
    public Vector3 SetMoveTargetPosition(Vector3 targetPosition);
    public Vector3 GetMoveTargetPosition();
    public GameObject SetMoveTarget(GameObject newTarget);
    public GameObject GetMoveTarget();
    public bool GetAvoidingFlag();
    public Vector3 SetTempTarget(Vector3 postion);
    public Vector3 ReturnToSaveTarget();
    public Vector3 SetFireTargetPosition(Vector3 targetPosition);
    public Vector3 GetFireTargetPosition();
}
