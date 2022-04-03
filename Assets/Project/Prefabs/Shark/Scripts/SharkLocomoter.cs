using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkLocomoter : MonoBehaviour
{
	NavMeshAgent _agent;

	[SerializeField]
	Transform _target;

	public void SetTarget(Transform target){
		_target = target;
	}

    void Start()
    {
		_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
		_agent.destination = _target.position;
    }
}
