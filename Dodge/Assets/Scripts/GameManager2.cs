using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
	public Text timeText;
	public GameObject bulletSpawnwerPrefab;
	public GameObject itemPrefab;
	public GameObject level;

	List<GameObject> itemList = new List<GameObject>();
	List<GameObject> spawnerList = new List<GameObject>();

	int spawnCounter = 0;
	float surviveTime;
	bool isGameover = false;

	void Start()
	{
		surviveTime = 0f;
		isGameover = false;
		StartCoroutine(CreateMobAndItem());
		StartCoroutine(DeleteItems());
	}

	void Update()
	{
		if (!isGameover)
		{
			surviveTime += Time.deltaTime;
			timeText.text = "Time: " + (int)surviveTime;
		}
	}

	void CreateMob()
	{
		Vector3 randPos = new Vector3(Random.Range(-19f, 19f), 1f, Random.Range(-7f, 7f));
		GameObject bulletSpawner = Instantiate(bulletSpawnwerPrefab, randPos, Quaternion.identity);
		bulletSpawner.transform.parent = level.transform;
		bulletSpawner.transform.localPosition = randPos;

		spawnerList.Add(bulletSpawner);
	}

	void CreateItem()
	{
		Vector3 randPos = new Vector3(Random.Range(-19f, 19f), 1f, Random.Range(-7f, 7f));
		GameObject item = Instantiate(itemPrefab, randPos, Quaternion.identity);
		item.transform.parent = level.transform;
		item.transform.localPosition = randPos;

		itemList.Add(item);
	}

	IEnumerator CreateMobAndItem()
	{
		int i = 0;
		CreateMob();
		CreateItem();
		while (!isGameover)
		{
			yield return new WaitForSeconds(5f);
			i++;

			CreateMob();

			if (i != 2)
			{
				CreateItem();
			}
			else
			{
				i = 0;
			}
		}
	}

	IEnumerator DeleteItems()
	{
		while (!isGameover)
		{
			yield return new WaitForSeconds(10f);
			foreach (GameObject item in itemList)
			{
				Destroy(item);
			}
			itemList.Clear();
		}
	}
}
