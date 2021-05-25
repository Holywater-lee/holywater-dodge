using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float RotateSpeed;
	float time;
	public float additionalSpeed = 0f;
	public bool isGameover;

	void Start()
	{
		time = 0f;
		//RotateSpeed = Random.Range(-10, 10);
		isGameover = false;
	}

	void Update()
	{
		Rotate();
	}

	// 5초에 한번씩 회전 속도 및 방향 변화
	void RandomSpeed()
	{
		time += Time.deltaTime;
		if (time > 5)
		{
			time = 0;
			int reverse = Random.Range(0, 2);
			if (reverse == 0)
				RotateSpeed = Random.Range(10, 15) + additionalSpeed;
			else
				RotateSpeed = Random.Range(-10, -15) - additionalSpeed;
		}
	}

	void Rotate()
	{
		if (!isGameover)
		{
			//transform.Rotate(0f, RotateSpeed * Time.deltaTime, 0f);
			transform.Rotate(0f, (RotateSpeed + additionalSpeed) * Time.deltaTime, 0f);
		}
	}
}
