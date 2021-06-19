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
	[Header("UI 텍스트")]
	public GameObject gameoverText;
	public Text scoreText;
	public Text bestScoreText;
	public Text bombText;

	[Header("프리팹 모음")]
	public GameObject[] enemyPrefabs;
	public GameObject[] enemySpawnPositions;
	public GameObject[] bossPrefabs;
	public GameObject bombPos;

	[Header("몬스터가 사용하는 총알")]
	public GameObject enemyBulletPrefab;

	[Header("현재 점수")]
	public float currentScore = 0f;

	[Header("이펙트 프리팹")]
	public GameObject bombEffect;

	float spawnTime = 2f;
	float stageTime = 0f;
	float bossSpawnTime = 0f;
	int stageNum = 0;
	int bombCount = 3;
	
	bool isGameover = false;
	bool isBossSpawned = false;
	[HideInInspector] public bool isBossCleared = false;


	[HideInInspector] public List<GameObject> enemyBulletsList;
	[HideInInspector] public List<GameObject> enemysList;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		float bestScore = PlayerPrefs.GetFloat("BestScore");
		bestScoreText.text = "Best Score: " + (int)bestScore;

		RefreshBombText();
		RefreshScore();

		switch (stageNum)
		{
			case 0: bossSpawnTime = 30f; break;
			case 1: bossSpawnTime = 45f; break;
			case 2: bossSpawnTime = 70f; break;
		}

		StartCoroutine(SpawnEnemyTimer());
		StartCoroutine(BossSpawnTimer());
	}

	void Update()
	{
		if (!isGameover)
		{
			stageTime += Time.deltaTime;
			
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (bombCount > 0)
				{
					Bomb();
				}
			}

			if (isBossCleared)
			{
				//SceneManager.LoadScene(스테이지 if문이나 switch로 어떻게);
				isBossSpawned = false;
				isBossCleared = false;
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

	IEnumerator BossSpawnTimer()
	{
		while(!isBossSpawned && !isGameover)
		{
			if (stageTime > bossSpawnTime)
			{
				SpawnBossEnemy(stageNum);
			}
			yield return new WaitForSeconds(1f);
		}
	}

	void SpawnBossEnemy(int stageNumber)
	{
		isBossSpawned = true;
		if (stageNumber == 0)
		{
			UI.instance.BossAlarmON();
			//var Boss = Instantiate(bossPrefabs[stageNumber], BossSpawnPos.transform.position, Quaternion.identity);
			//sound재생;
		}

		foreach (GameObject enemys in enemysList)
		{
			Destroy(enemys);
		}
		enemysList.Clear();
	}

	void Bomb()
	{
		bombCount--;
		RefreshBombText();
		var BombFX = Instantiate(bombEffect, bombPos.transform.position, Quaternion.identity);
		Destroy(BombFX, 4f);

		foreach (GameObject bullets in enemyBulletsList)
		{
			Destroy(bullets);
		}
		enemyBulletsList.Clear();
	}

	void RefreshBombText()
	{
		bombText.text = "Bomb: " + bombCount;
	}

	public void RefreshScore()
	{
		scoreText.text = "Score: " + (int)currentScore;
	}

	IEnumerator SpawnEnemyTimer()
	{
		float RanSpawnTime;
		while(!isGameover && stageTime < bossSpawnTime)
		{
			SpawnEnemy();
			RanSpawnTime = Random.Range(spawnTime - 1f, spawnTime + 1f);
			yield return new WaitForSeconds(RanSpawnTime);
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
			mob.transform.LookAt(new Vector3(mob.transform.position.x, mob.transform.position.y, mob.transform.position.z - 10f));
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
			mob.transform.LookAt(new Vector3(mob.transform.position.x + 10f * (moveDirection == "Right" ? 1f : -1f), mob.transform.position.y, mob.transform.position.z));

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
