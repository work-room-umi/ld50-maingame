using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraLocomoter : MonoBehaviour
{
    // Start is called before the first frame update
	[SerializeField]
	NavMeshAgent _playerShipAgent;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		// var dir = Vector3.ProjectOnPlane(_playerShipAgent.steeringTarget - transform.position, Vector3.up).normalized;
		// var dot = Vector3.Dot(_playerShipAgent.velocity, dir)*dir;
		// var fixedVel = _playerShipAgent.velocity - dot*2f;
		var fixedVel = Vector3.ProjectOnPlane(_playerShipAgent.velocity, transform.forward);
		transform.position = _playerShipAgent.transform.position + fixedVel*1f;
		// transform.position = _playerShipAgent.transform.position + _playerShipAgent.velocity*0.2f;
		transform.rotation = _playerShipAgent.transform.rotation;
    }

}
