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
	public GameObject bulletSpawnerPrefab;
	public GameObject itemPrefab;
	int prevItemCheck;

	Vector3[] bulletSpawners = new Vector3[4];
	int spawnCounter;

	Rotator rotator;

	float surviveTime;
	bool isGameover;

	void Start()
	{
		surviveTime = 0f;
		isGameover = false;
		rotator = FindObjectOfType<Rotator>();
		float bestTime = PlayerPrefs.GetFloat("BestTime");
		recordText.text = "Best Time: " + (int)bestTime;

		bulletSpawners[0].x = -8f;
		bulletSpawners[0].y = 1f;
		bulletSpawners[0].z = 8f;

		bulletSpawners[1].x = 8f;
		bulletSpawners[1].y = 1f;
		bulletSpawners[1].z = 8f;

		bulletSpawners[2].x = -8f;
		bulletSpawners[2].y = 1f;
		bulletSpawners[2].z = -8f;

		bulletSpawners[3].x = 8f;
		bulletSpawners[3].y = 1f;
		bulletSpawners[3].z = -8f;

		StartCoroutine("createBulletSpawner");
		StartCoroutine("createItemPrefab");
	}

	void Update()
	{
		if (!isGameover)
		{
			surviveTime += Time.deltaTime;
			timeText.text = "Time: " + (int)surviveTime;
			/*
			if (surviveTime % 5f <= 0.01f && prevItemCheck == 4)
			{
				Vector3 randomPos = new Vector3(Random.Range(-8f, 8f), 1f, Random.Range(-8f, 8f));

				GameObject item = Instantiate(itemPrefab, randomPos, Quaternion.identity);
				item.transform.parent = level.transform;
				item.transform.localPosition = randomPos;
			}
			prevItemCheck = (int)(surviveTime % 5f);
			*/
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene("SampleScene");
			}
		}
	}

	IEnumerator createItemPrefab()
	{
		while (!isGameover)
		{
			yield return new WaitForSeconds(5f);
			Vector3 randomPos = new Vector3(Random.Range(-6f, 6f), 1f, Random.Range(-6f, 6f));

			GameObject item = Instantiate(itemPrefab, randomPos, Quaternion.identity);
			item.transform.parent = level.transform;
			item.transform.localPosition = randomPos; // 로컬 좌표계로 설정
		}
	}

	IEnumerator createBulletSpawner()
	{
		for (int i = 0; i < 4; i++)
		{
			Spawner(i);
			yield return new WaitForSeconds(5f);
		}
	}

	void Spawner(int i)
	{
		// Quaternion.identity -> 회전 없음
		GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, bulletSpawners[i], Quaternion.identity);
		bulletSpawner.transform.parent = level.transform;
		bulletSpawner.transform.localPosition = bulletSpawners[i];
		level.GetComponent<Rotator>().additionalSpeed += 15f;
	}

	public void EndGame()
	{
		isGameover = true;
		rotator.isGameover = true;
		gameoverText.SetActive(true);
		StopCoroutine("createBulletSpawner");

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
