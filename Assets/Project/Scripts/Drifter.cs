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
	Vector3 _frontDir;
	[SerializeField]
	float _moveAmp=0;
	[SerializeField]
	float _rotationAmp=0;
	[SerializeField]
	float _noiseScale=1;
	[SerializeField]
	float _noiseTimeScale=1;
	[SerializeField]
	GameObject _wave;

	// Start is called before the first frame update
	void Start()
	{
		_child = transform.GetChild(0);
	}

	Vector3 GetNoiseDir(Vector3 pos)
	{
		float time = Time.time * _noiseTimeScale;
		float noise = Mathf.PerlinNoise((pos.x + time) * _noiseScale, (pos.z + time) * _noiseScale);
		Vector3 noiseDir = new Vector3(Mathf.Cos(noise * Mathf.PI), 0, Mathf.Sin(noise * Mathf.PI));
		Vector3 dir = Vector3.Normalize(noiseDir);
		return dir;
	}

	// Update is called once per frame
	void Update()
	{
		Waves wave = _wave.GetComponent<Waves>();
		float height = wave.GetHeight(_child.position);
		Vector3 normal = wave.GetNormal(_child.position);
		Vector3 moveDir = GetNoiseDir(_child.position);

		_child.rotation = Quaternion.identity;
		Quaternion upRotation = Quaternion.FromToRotation(_child.up, normal);
		Quaternion frontRotation = Quaternion.FromToRotation(_frontDir, moveDir * _rotationAmp);
		_child.rotation = upRotation * frontRotation;
		_child.position = new Vector3(_child.position.x + moveDir.x * _moveAmp, height + Mathf.Sin(Time.time*_freq)*_amp, _child.position.z + moveDir.z * _moveAmp);

	}
}
