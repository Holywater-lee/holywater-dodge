using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] HPBar hpBar;
	[SerializeField] GameObject playerBulletPrefab;

	Rigidbody playerRigidbody;
	Transform tr;
	Renderer color;

	[SerializeField] float maxLife = 100;
	[SerializeField] float life = 100;
	[SerializeField] float speed = 8f;
	[SerializeField] float bulletDamage = 30f;
	public float increasedDamage = 0f;
	[SerializeField] float atkRate = 0.2f;
	[SerializeField] float bulletSpeed = 30f;
	[SerializeField] float invincibleTime = 1f;

	float timerAfterAtk = 0f;
	bool bIsInvincible = false;

	void Start()
	{
		playerRigidbody = GetComponent<Rigidbody>();
		tr = GetComponent<Transform>();
		color = GetComponent<Renderer>();
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
		
		timerAfterAtk += Time.deltaTime;

		if ((Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1")) && timerAfterAtk >= atkRate)
		{
			timerAfterAtk = 0f;
			var bullet = Instantiate(playerBulletPrefab, transform.position, transform.rotation);
			var bulletDmg = bullet.GetComponent<PlayerBullet>();
			bulletDmg.damage = bulletDamage + increasedDamage;
			bulletDmg.speed = bulletSpeed;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			getDamage(30); // 고정 피해
		}
	}

	public void getDamage(float damage)
	{
		if (!bIsInvincible)
		{
			life -= damage;
			hpBar.setHP(life, maxLife);

			StartCoroutine(SetInvincible(invincibleTime));

			StartCoroutine(InvincibleColor());
		}
		
		if (life <= 0)
		{
			Die();
		}
	}

	IEnumerator InvincibleColor()
	{
		int i = 0;
		while (i < 10 || bIsInvincible)
		{
			if (i % 2 == 1)
				color.material.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
			else
				color.material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
			yield return new WaitForSeconds(0.1f);
			i++;
		}
	}

	IEnumerator SetInvincible(float invTime)
	{
		bIsInvincible = true;
		yield return new WaitForSeconds(invTime);
		bIsInvincible = false;
		color.material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
	}

	public void GetHeal(int heal)
	{
		life += heal;
		if (life >= maxLife)
		{
			life = maxLife;
		}
		hpBar.setHP(life, maxLife);
	}

	void Die()
	{
		gameObject.SetActive(false);

		GameManager gameManager = FindObjectOfType<GameManager>();

		gameManager.EndGame();
	}
}
