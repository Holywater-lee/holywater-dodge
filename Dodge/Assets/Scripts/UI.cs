using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public static UI instance;

	public GameObject BossHPBar;
	public GameObject GameClearUI;

	Animator anim;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void GameClear()
	{
		anim.SetBool("isGameClear", true);
		FindObjectOfType<PlayerController>().GetHeal(100);
	}

	public void BossAlarmON()
	{
		BossHPBar.SetActive(true);
		anim.SetBool("isBossSpawned", true);
		StartCoroutine(BossAlarmOFF(4f));
	}

	public void BossHPBarOFF()
	{
		BossHPBar.SetActive(false);
	}

	IEnumerator BossAlarmOFF(float time)
	{
		yield return new WaitForSeconds(time);
		anim.SetBool("isBossSpawned", false);
		//BossAlarmOBJ.SetActive(false);
	}
}
