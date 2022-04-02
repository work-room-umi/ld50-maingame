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
    void Update()
    {
		transform.position = _playerShipAgent.transform.position + _playerShipAgent.velocity*-0.2f;
		transform.rotation = _playerShipAgent.transform.rotation;
    }
}
