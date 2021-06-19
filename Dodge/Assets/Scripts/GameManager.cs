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
	public GameObject bossSpawnPos;
	public GameObject[] itemPrefabs;

	[Header("몬스터가 사용하는 총알")]
	public GameObject enemyBulletPrefab;

	[Header("현재 점수")]
	public float currentScore = 0f;

	[Header("이펙트 프리팹")]
	public GameObject bombEffect;
	public GameObject bossFX;
	public GameObject attackFX;

	float spawnTime = 2f;
	float stageTime = 0f;
	float bossSpawnTime = 0f;
	public int stageNum = 0;
	public int bombCount = 2;
	
	bool isGameover = false;
	bool isBossSpawned = false;

	[HideInInspector] public List<GameObject> enemyBulletsList;
	[HideInInspector] public List<GameObject> enemysList;

	AudioSource ado;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		float bestScore = PlayerPrefs.GetFloat("BestScore");
		bestScoreText.text = "Best Score: " + (int)bestScore;

		ado = GetComponent<AudioSource>();

		RefreshBombText();
		RefreshScore();

		switch (stageNum)
		{
			case 0: bossSpawnTime = 30f; break;
			case 1: bossSpawnTime = 45f; break;
			case 2: bossSpawnTime = 70f; break;
		}

		if (SceneManager.GetActiveScene().name == "Level_1")
		{
			stageNum = 0;
		}
		else if (SceneManager.GetActiveScene().name == "Level_2")
		{
			stageNum = 1;
		}
		else if (SceneManager.GetActiveScene().name == "Level_3")
		{
			stageNum = 2;
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

		}
		else
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	public void BossClear()
	{
		isBossSpawned = false;
		Debug.Log("보스클리어!");
		UI.instance.BossHPBarOFF();

		switch(stageNum)
		{
			case 0:
				SceneManager.LoadScene("Level_2");
				break;
			case 1:
				SceneManager.LoadScene("Level_3");
				break;
			case 2:
				//GameClear();
				break;
		}
	}

	IEnumerator BossSpawnTimer()
	{
		while(!isBossSpawned && !isGameover)
		{
			if (stageTime > bossSpawnTime)
			{
				SpawnBossEnemy();
			}
			yield return new WaitForSeconds(1f);
		}
	}

	void SpawnBossEnemy()
	{
		isBossSpawned = true;

		UI.instance.BossAlarmON();
		BGMSwitch.instance.ChangeBGM();
		var SpawnFX = Instantiate(bossFX, bossSpawnPos.transform.position + Vector3.up * 5f, Quaternion.identity);
		Destroy(SpawnFX, 5f);
		Invoke("bossInstantiate", 3f);

		//sound재생;

		foreach (GameObject enemys in enemysList)
		{
			Destroy(enemys);
		}
		enemysList.Clear();
	}

	void bossInstantiate()
	{
		var boss = Instantiate(bossPrefabs[stageNum], bossSpawnPos.transform.position, Quaternion.identity);
		boss.transform.LookAt(boss.transform.position + Vector3.back * 10f);
	}

	void Bomb()
	{
		bombCount--;
		RefreshBombText();
		var BombFX = Instantiate(bombEffect, bombPos.transform.position, Quaternion.identity);
		Destroy(BombFX, 4f);
		ado.Play();

		foreach (GameObject bullets in enemyBulletsList)
		{
			Destroy(bullets);
		}
		enemyBulletsList.Clear();
	}

	public void RefreshBombText()
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
		int randomPos = Random.Range(0, 8);

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
		else if (randomPos == 6)
		{
			StartCoroutine(EnemySpawnPattern0("Right", 6));
		}
		else if (randomPos == 7)
		{
			StartCoroutine(EnemySpawnPattern0("Left", 7));
		}
	}

	IEnumerator EnemySpawnPattern0(string moveDirection, int posIndex)
	{
		int Howmuch = Random.Range(2, 7);
		for (int i = 0; i < Howmuch; i++)
		{
			var mob = Instantiate(enemyPrefabs[0], enemySpawnPositions[posIndex].transform.position, Quaternion.identity);
			var rigid = mob.GetComponent<Rigidbody>();

			rigid.velocity = new Vector3((moveDirection == "Right" ? 1f : -1f) * mob.GetComponent<Enemy>().speed, 0f, 0f);
			mob.transform.LookAt(new Vector3(mob.transform.position.x + 10f * (moveDirection == "Right" ? 1f : -1f), mob.transform.position.y, mob.transform.position.z));

			yield return new WaitForSeconds(0.7f);
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
