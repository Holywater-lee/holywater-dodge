using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRigidbody;
	public HPBar hpBar;

	public int hp = 100;
	public float speed = 8f;
	
	void Start()
	{
		// GetComponent 내 게임 오브젝트에서 원하는 타입의 컴포넌트를 찾아오는 메서드
		playerRigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		float xInput = Input.GetAxis("Horizontal");
		float zInput = Input.GetAxis("Vertical");

		float xSpeed = xInput * speed;
		float zSpeed = zInput * speed;

		Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);

		playerRigidbody.velocity = newVelocity;
		gameObject.transform.LookAt(transform.position + newVelocity);
	}

	public void getDamage(int damage)
	{
		hp -= damage;
		hpBar.setHP(hp);
		if (hp <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		gameObject.SetActive(false);

		GameManager gameManager = FindObjectOfType<GameManager>();

		gameManager.EndGame();
	}
}
