using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 게임매니저
 게임시작/종료 관리
 스코어관리/스테이지 시간
 */

public class GameManager : MonoBehaviour
{
	public GameObject gameoverText;
	public Text scoreText;
	public Text bestScoreText;

	public GameObject[] enemyPrefabs;
	public GameObject[] enemySpawnPositions;
	public GameObject[] bossPrefabs;

	public float currentScore = 0f;

	float spawnTime = 3f;
	float stageTime = 0f;
	float bossSpawnTime = 0f;
	int stageNum = 0;
	
	bool isGameover = false;
	bool isBossDead = false;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		float bestScore = PlayerPrefs.GetFloat("BestScore");
		bestScoreText.text = "Best Score: " + (int)bestScore;

		switch (stageNum)
		{
			case 0: bossSpawnTime = 30f; break;
			case 1: bossSpawnTime = 45f; break;
			case 2: bossSpawnTime = 70f; break;
		}

		StartCoroutine(SpawnEnemyTimer());
	}

	void Update()
	{
		if (!isGameover)
		{
			stageTime += Time.deltaTime;
			
			if (stageTime > bossSpawnTime)
			{
				//SpawnBoss(stageNum);
				//if(isBossDead) 다음씬 -> isBossDead = false
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	public void RefreshScore()
	{
		scoreText.text = "Score: " + (int)currentScore;
	}

	IEnumerator SpawnEnemyTimer()
	{
		while(!isGameover)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(spawnTime);
		}
	}

	void SpawnEnemy()
	{
		int randomEnemy = Random.Range(0, 3);
		int randomPos = Random.Range(0, 6);

		if (randomPos < 4)
		{
			var mob = Instantiate(enemyPrefabs[randomEnemy], enemySpawnPositions[randomPos].transform.position, Quaternion.identity);
			var rigid = mob.GetComponent<Rigidbody>();
			rigid.velocity = new Vector3(0f, 0f, mob.GetComponent<Enemy>().speed * -1f);
		}
		else if (randomPos == 4)
		{
			StartCoroutine(EnemySpawnPattern0("Right", 4));
		}
		else if (randomPos == 5)
		{
			StartCoroutine(EnemySpawnPattern0("Left", 5));
		}
		// 6,7은 아직 구상중!
	}

	IEnumerator EnemySpawnPattern0(string moveDirection, int posIndex)
	{
		for (int i = 0; i < 5; i++)
		{
			var mob = Instantiate(enemyPrefabs[0], enemySpawnPositions[posIndex].transform.position, Quaternion.identity);
			var rigid = mob.GetComponent<Rigidbody>();

			rigid.velocity = new Vector3((moveDirection == "Right" ? 1f : -1f) * mob.GetComponent<Enemy>().speed, 0f, 0f);

			yield return new WaitForSeconds(0.5f);
		}
	}

	public void EndGame()
	{
		isGameover = true;
		gameoverText.SetActive(true);

		float bestScore = PlayerPrefs.GetFloat("BestScore");

		if (currentScore > bestScore)
		{
			bestScore = currentScore;
			PlayerPrefs.SetFloat("BestScore", bestScore);
		}

		bestScoreText.text = "Best Score: " + (int)bestScore;
	}
}
