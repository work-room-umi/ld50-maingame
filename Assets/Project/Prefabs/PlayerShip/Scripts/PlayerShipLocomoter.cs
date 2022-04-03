using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerShipLocomoter : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField]
    Transform _camera;
    [SerializeField]
    float _speedIntensity;

    Transform _child;

    void Start()
    {
        _child= transform.GetChild(0);
        _agent = GetComponent<NavMeshAgent>();
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    void Update()
    {
        var cameraPosOnPlane = _camera.position; cameraPosOnPlane.y = 0f;
        var forwardDirFromCam = (transform.position - cameraPosOnPlane).normalized;
        var leftDirFromCam    = Vector3.Cross(Vector3.up, forwardDirFromCam).normalized;
        var forwardDirSignFromCam = (Keyboard.current.wKey.IsPressed()?1:0) + (Keyboard.current.sKey.IsPressed()?-1:0);
        var leftDirSignFromCam = (Keyboard.current.dKey.IsPressed()?1:0) + (Keyboard.current.aKey.IsPressed()?-1:0);
        // var nextPoint = _agent.steeringTarget + (forwardDirFromCam*forwardDirSignFromCam + leftDirFromCam * leftDirSignFromCam).normalized*_speedIntensity;
        var nextPoint = transform.position + (forwardDirFromCam*forwardDirSignFromCam + leftDirFromCam * leftDirSignFromCam).normalized*_speedIntensity;
        Vector3 targetDir = (nextPoint - transform.position).normalized;
        // Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

        _agent.SetDestination(nextPoint);
        _agent.nextPosition = transform.position;
    }

    public float CurrentSpeedRate(){
        return _agent.velocity.magnitude/_agent.speed;
    }

    public float CurrentTiltingRate(){
        return Mathf.Clamp01(1f-_child.transform.up.y);
    }
}
