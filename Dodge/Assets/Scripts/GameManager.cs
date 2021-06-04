using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 게임매니저
 게임시작/종료 관리
 스코어관리/시간
 */

public class GameManager : MonoBehaviour
{
	public GameObject gameoverText;
	public Text timeText;
	public Text recordText;

	public GameObject level; // 불릿 등 레벨 수정할 변수

	float surviveTime;
	bool isGameover;

	void Start()
	{
		surviveTime = 0f;
		isGameover = false;
		float bestTime = PlayerPrefs.GetFloat("BestTime");
		recordText.text = "Best Time: " + (int)bestTime;
	}

	void Update()
	{
		if (!isGameover)
		{
			surviveTime += Time.deltaTime;
			timeText.text = "Time: " + (int)surviveTime;
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	public void EndGame()
	{
		isGameover = true;
		gameoverText.SetActive(true);

		// BestTime이 저장되어있지 않다면 0
		float bestTime = PlayerPrefs.GetFloat("BestTime");

		if (surviveTime > bestTime)
		{
			bestTime = surviveTime;
			PlayerPrefs.SetFloat("BestTime", bestTime);
		}

		recordText.text = "Best Time: " + (int)bestTime;
	}
}
