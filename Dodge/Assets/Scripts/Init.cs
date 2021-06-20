using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
	public float score;

	public static Init instance;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		instance = this;
	}
}
