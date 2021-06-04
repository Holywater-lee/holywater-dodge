using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	Rigidbody bulletRigidbody;
	[HideInInspector] public float speed;
	[HideInInspector] public float damage;

	// 관통 여부
	bool bIsPierce = false;

	void Start()
	{
		bulletRigidbody = GetComponent<Rigidbody>();

		bulletRigidbody.velocity = transform.forward * speed;

		Destroy(gameObject, 5f);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Enemy enemy = other.GetComponent<Enemy>();

			if (enemy != null)
			{
				enemy.getDamage(damage);
				
				if (!bIsPierce)
					Destroy(gameObject);
			}
		}
	}
}
