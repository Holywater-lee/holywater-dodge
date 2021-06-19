﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public static UI instance;

	//public GameObject BossAlarmOBJ;

	Animator anim;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void BossAlarmON()
	{
		//BossAlarmOBJ.SetActive(true);
		anim.SetBool("isBossSpawned", true);
		StartCoroutine(BossAlarmOFF(4f));
	}

	IEnumerator BossAlarmOFF(float time)
	{
		yield return new WaitForSeconds(time);
		anim.SetBool("isBossSpawned", false);
		//BossAlarmOBJ.SetActive(false);
	}
}