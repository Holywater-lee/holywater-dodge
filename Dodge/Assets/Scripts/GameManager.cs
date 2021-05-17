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
    }

    void Update()
    {
        if(!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time: " + (int)surviveTime;

            if (surviveTime < 5f && spawnCounter == 0)
            {
                // Quaternion.identity -> 회전 없음
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, bulletSpawners[spawnCounter], Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = bulletSpawners[spawnCounter];
                level.GetComponent<Rotator>().additionalSpeed += 15f;
                spawnCounter++;
            }
            else if (surviveTime >= 5f && surviveTime < 10f && spawnCounter == 1)
            {
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, bulletSpawners[spawnCounter], Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = bulletSpawners[spawnCounter];
                level.GetComponent<Rotator>().additionalSpeed += 15f;
                spawnCounter++;
            }
            else if (surviveTime >= 10f && surviveTime < 15f && spawnCounter == 2)
            {
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, bulletSpawners[spawnCounter], Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = bulletSpawners[spawnCounter];
                level.GetComponent<Rotator>().additionalSpeed += 15f;
                spawnCounter++;
            }
            else if (surviveTime >= 15f && surviveTime < 20f && spawnCounter == 3)
            {
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, bulletSpawners[spawnCounter], Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = bulletSpawners[spawnCounter];
                level.GetComponent<Rotator>().additionalSpeed += 15f;
                spawnCounter++;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
			}
		}
    }

    public void EndGame()
    {
        isGameover = true;
        rotator.isGameover = true;
        gameoverText.SetActive(true);

        // BestTime이 저장되어있지 않다면 0
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if(surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
		}

        recordText.text = "Best Time: " + (int)bestTime;
    }
}
