using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkLocomoter : MonoBehaviour
{
	NavMeshAgent _agent;

	[SerializeField]
	Transform _target;

	[SerializeField]
	float _speedFreq = 2f;

	[SerializeField]
	float _speedAmp = 1f;

	float _defaultSpeed;

	[SerializeField]
	float _lifeTime = 0f;

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_defaultSpeed = _agent.speed;
	}

	void Update()
	{
		_agent.destination = _target.position;
		_agent.speed = _defaultSpeed + Mathf.Sin(Time.time * _speedFreq) * _speedAmp;
		var targetDir = (_agent.destination - transform.position).normalized;
		_agent.speed *= Mathf.Clamp01(Vector3.Dot(targetDir, transform.forward));

		_lifeTime -= Time.deltaTime;

		if (_lifeTime < 0f)
		{
			Destroy(gameObject);
		}
	}
}
