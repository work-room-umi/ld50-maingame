using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerShipLocomoter : MonoBehaviour
{
	NavMeshAgent _agent;

	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (Keyboard.current.wKey.wasPressedThisFrame)
		{
			Debug.Log("w key pressed");
		}
		if (Keyboard.current.sKey.wasPressedThisFrame)
		{
			Debug.Log("s key pressed");
		}
		var nextPoint = _agent.steeringTarget;
		Vector3 targetDir = (nextPoint - transform.position).normalized;
		Quaternion targetRotation = Quaternion.LookRotation(targetDir);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

		_agent.SetDestination(nextPoint + targetDir*0.1f);
		_agent.nextPosition = transform.position;
	}
}
