using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifter : MonoBehaviour
{
	Transform _child;

	[SerializeField]
	float _freq;
	[SerializeField]
	float _amp;

	// Start is called before the first frame update
	void Start()
	{
		_child = transform.GetChild(0);
	}

	// Update is called once per frame
	void Update()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.up);
		int layerMask = LayerMask.GetMask(new string[] { "Water"});
		Physics.queriesHitBackfaces = true;
		if(Physics.Raycast(ray, out hit, 10f, layerMask)){
			_child.rotation = Quaternion.identity;
			Quaternion targetRotation = Quaternion.FromToRotation(_child.up, SmoothedNormal(hit));
			_child.rotation = targetRotation;
			_child.position = new Vector3(_child.position.x, hit.point.y + Mathf.Sin(Time.time*_freq)*_amp, _child.position.z);
		}
		Physics.queriesHitBackfaces = false;
	}

	Vector3 SmoothedNormal(RaycastHit aHit)
	{
		var MC = aHit.collider as MeshCollider;
		if (MC == null)
			return aHit.normal;
		var M = MC.sharedMesh;
		var normals = M.normals;
		var indices = M.triangles;
		var N0 = normals[indices[aHit.triangleIndex * 3 + 0]];
		var N1 = normals[indices[aHit.triangleIndex * 3 + 1]];
		var N2 = normals[indices[aHit.triangleIndex * 3 + 2]];
		var B = aHit.barycentricCoordinate;
		var localNormal = (B[0] * N0 + B[1] * N1 + B[2] * N2).normalized;
		return MC.transform.TransformDirection(localNormal);
	}
}
