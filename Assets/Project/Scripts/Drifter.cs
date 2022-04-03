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
	[SerializeField]
	float _noiseScale=0;
	[SerializeField]
	GameObject _wave;

	// Start is called before the first frame update
	void Start()
	{
		_child = transform.GetChild(0);
	}

	Vector3 GetNoiseDir(Vector3 pos)
	{
		float noise = Mathf.PerlinNoise(pos.x, pos.z);
		Vector3 noiseDir = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI));
		Vector3 dir = Vector3.Normalize(noiseDir);
		return dir;
	}

	// Update is called once per frame
	void Update()
	{
		Waves wave = _wave.GetComponent<Waves>();
		float height = wave.GetHeight(_child.position);
		Vector3 normal = wave.GetNormal(_child.position);
		Vector3 moveDir = GetNoiseDir(_child.position) * _noiseScale;

		_child.rotation = Quaternion.identity;
		Quaternion targetRotation = Quaternion.FromToRotation(_child.up, normal);
		_child.rotation = targetRotation;
		_child.position = new Vector3(_child.position.x + moveDir.x, height + Mathf.Sin(Time.time*_freq)*_amp, _child.position.z + moveDir.z);

	}
}
