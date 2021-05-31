using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	Rigidbody bulletRigidbody;
	public float speed;
	public int damage = 20;

	void Start()
	{
		bulletRigidbody = GetComponent<Rigidbody>();

		bulletRigidbody.velocity = transform.forward * speed;

		Destroy(gameObject, 3f); // 3초후에 파괴
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bullet")
		{
			Bullet bullet = other.GetComponent<Bullet>();

			if (bullet != null)
			{
				Destroy(bullet.gameObject);
			}
			Destroy(gameObject);
		}
		else if (other.tag == "BulletSpawner")
		{
			BulletSpawner spawner = other.GetComponent<BulletSpawner>();

			if (spawner != null)
			{
				spawner.getDamage(damage);
				Destroy(gameObject);
			}
		}
	}
}
