using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSwitch : MonoBehaviour
{
	public AudioClip BossBGM;

	AudioSource ado;

	public static BGMSwitch instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		ado = GetComponent<AudioSource>();
	}

	public void ChangeBGM()
	{
		ado.clip = BossBGM;
		ado.Play();
	}
}
