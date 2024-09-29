using UnityEngine;

public interface IBehaviorAI
{
    public Vector3 SetTargetPosition(Vector3 targetPosition);
    public Vector3 GetTargetPosition();
    public Transform GetAgentTransform();
    public GameObject SetTarget(GameObject newTarget);
    public GameObject GetTarget();
    public Transform GetTransform();
    public bool GetAvoidingFlag();
    public Vector3 SetTempTarget(Vector3 postion);
    public Vector3 ReturnToSaveTarget();
}
