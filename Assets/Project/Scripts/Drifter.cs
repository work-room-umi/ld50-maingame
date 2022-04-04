using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifter : MonoBehaviour
{
	Transform _child;
	GameObject _childObject;
	bool _visible = true;

	public float _freq;
	public float _amp;
	//public Vector3 _frontDir;
	public float _noiseMoveAmp=0;
	//public bool _noiseRotate=false;
	public float _noiseScale=1;
	public float _noiseTimeScale=1;
	private Waves _wave;

	// Start is called before the first frame update
	void Start()
	{
		_child = transform.GetChild(0);
		_childObject = _child.gameObject;
    	_wave = (Waves)FindObjectOfType(typeof(Waves));
	}

	public Transform GetChild()
	{
		return _child;
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
		// var transform = this.transform;
		// transform.rotation = Quaternion.identity;
		Vector3 moveDir = GetNoiseDir(_child.position);
		// Quaternion targetRorationNoise = Quaternion.FromToRotation(_frontDir, moveDir * (_noiseRotate? 1f : 0f));
		// transform.rotation = targetRorationNoise;
		transform.position = new Vector3(transform.position.x + moveDir.x * _noiseMoveAmp, transform.position.y,  transform.position.z + moveDir.z * _noiseMoveAmp);

		try
		{
			_child.rotation = Quaternion.identity;
			float height = _wave.GetHeight(_child.position);
			Vector3 normal = _wave.GetNormal(_child.position);
			Quaternion targetRotationXZ = Quaternion.FromToRotation(_child.forward, transform.forward);
			Quaternion targetRotation = Quaternion.FromToRotation(_child.up, normal);
			_child.rotation = targetRotationXZ*targetRotation;
			_child.position = new Vector3(_child.position.x, height + Mathf.Sin(Time.time*_freq)*_amp, _child.position.z);

			if (!_visible)
			{
				_childObject.SetActive(true);
				_visible = true;
			}
		}
		catch(NullReferenceException)
		{
			_childObject.SetActive(false);
			_visible = false;
		}
	}
}
