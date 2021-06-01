﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;
    public float hp = 100f;
    public HPBar hpBar;

    private Transform target;
    float spawnRate;
    float timeAfterSpawn;
    void Start()
    {
        // 누적시간 초기화
        timeAfterSpawn = 0f;
        // 생성 간격 랜덤설정
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        // PlayerController 컴포넌트를 가진 게임오브젝트를 찾아 target에 할당
        target = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        CreateBullet();
    }

    public void getDamage(int damage)
    {
        hp -= damage;
        hpBar.setHP((int)hp);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CreateBullet()
    {
        // timeAfterSpawn 갱신
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }

}
