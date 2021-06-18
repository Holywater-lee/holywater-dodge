﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	Rigidbody bulletRigidbody;
	public float speed;
	public float damage = 30;

	void Start()
	{
		bulletRigidbody = GetComponent<Rigidbody>();

		bulletRigidbody.velocity = transform.forward * speed;

		GameManager.instance.enemyBulletsList.Add(this.gameObject);
		StartCoroutine(DeleteBulletByTime(3f));
	}

	IEnumerator DeleteBulletByTime(float time)
	{
		yield return new WaitForSeconds(time);
		GameManager.instance.enemyBulletsList.Remove(this.gameObject);
		Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			// 상대방 게임오브젝트에서 PlayerController 컴포넌트 가져옴
			PlayerController playerController = other.GetComponent<PlayerController>();

			// PlayerController 컴포넌트를 가져오는데 성공했다면
			if (playerController != null)
			{
				playerController.getDamage(damage);
				Destroy(gameObject);
			}
		}
	}
}
