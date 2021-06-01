using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRigidbody;
	Transform tr;
	public HPBar hpBar;
	public GameObject playerBulletPrefab;

	float timerAfterSpawn = 0f;
	float spawnRate = 0.2f;

	public int hp = 100;
	public float speed = 8f;

	void Start()
	{
		// GetComponent 내 게임 오브젝트에서 원하는 타입의 컴포넌트를 찾아오는 메서드
		playerRigidbody = GetComponent<Rigidbody>();
		tr = GetComponent<Transform>();
	}

	void Update()
	{
		float xInput = Input.GetAxis("Horizontal");
		float zInput = Input.GetAxis("Vertical");

		float xSpeed = xInput * speed;
		float zSpeed = zInput * speed;

		Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
		playerRigidbody.velocity = newVelocity;

		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray.origin, ray.direction, out hit))
		{
			Vector3 projectedPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
			Vector3 currentPos = transform.position;
			Vector3 rotation = projectedPos - currentPos;
			tr.forward = rotation;
		}

		//gameObject.transform.LookAt(transform.position + newVelocity);

		timerAfterSpawn += Time.deltaTime;
		if (Input.GetButton("Fire1") && timerAfterSpawn >= spawnRate)
		{
			timerAfterSpawn = 0f;
			var bullet = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
		}
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

	public void GetHeal(int heal)
	{
		hp += heal;
		if (hp >= 100)
		{
			hp = 100;
		}
		hpBar.setHP(hp);
	}

	void Die()
	{
		gameObject.SetActive(false);

		GameManager gameManager = FindObjectOfType<GameManager>();

		gameManager.EndGame();
	}
}
