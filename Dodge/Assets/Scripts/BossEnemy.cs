using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
	[Header("보스 능력치")]
	[SerializeField] float life;
	[SerializeField] float maxLife;
	[SerializeField] float score;
	[SerializeField] HPBar hpBar;

	Animator anim;

	int stageNum;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		TryAttack();
		stageNum = GameManager.instance.stageNum;
		if (hpBar == null)
		{
			hpBar = FindObjectOfType<findBossHP>().GetComponent<HPBar>();
		}
	}

	public void getDamage(float damage)
	{
		life -= damage;
		hpBar.setHP(life, maxLife);

		if (life <= 0)
		{
			GameManager.instance.currentScore += score;
			GameManager.instance.RefreshScore();

			GameManager.instance.BossClear();
			Destroy(gameObject);
		}
	}

	void TryAttack()
	{
		anim.SetBool("isIdle", false);
		int randomPattern = Random.Range(0, 4);

		switch (randomPattern)
		{
			case 0:
				StartCoroutine(Ptrn_Round());
				break;
			case 1:
				StartCoroutine(Ptrn_Round2());
				break;
			case 2:
				StartCoroutine(Ptrn_Stab());
				break;
			case 3:
				StartCoroutine(Ptrn_Stab2());
				break;
		}
	}

	IEnumerator Ptrn_Round()
	{
		anim.SetInteger("Pattern", 0);
		int bulletSpeed = 11;

		yield return new WaitForSeconds(1f);
		anim.SetBool("isIdle", true);

		for (int i = 0; i < 30; i++)
		{
			var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
			var bulrigid = bullet.GetComponent<Rigidbody>();
			Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 30), 0f, Mathf.Sin(Mathf.PI * 2 * i / 30));
			
			bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
		}

		yield return new WaitForSeconds(1f - stageNum * 0.2f);

		for (int i = 0; i < 40; i++)
		{
			var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
			var bulrigid = bullet.GetComponent<Rigidbody>();
			Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 40), 0f, Mathf.Sin(Mathf.PI * 2 * i / 40));
			
			bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
		}

		yield return new WaitForSeconds(1f - stageNum * 0.2f);

		if (stageNum > 0)
		{
			for (int i = 0; i < 30; i++)
			{
				var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
				var bulrigid = bullet.GetComponent<Rigidbody>();
				Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 30), 0f, Mathf.Sin(Mathf.PI * 2 * i / 30));

				bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
			}
			yield return new WaitForSeconds(1f - stageNum * 0.2f);
		}

		TryAttack();
	}

	IEnumerator Ptrn_Round2()
	{
		anim.SetInteger("Pattern", 1);
		int bulletSpeed = 7;

		yield return new WaitForSeconds(1f);
		anim.SetBool("isIdle", true);

		for (int i = 0; i < 15; i++)
		{
			var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
			var bulrigid = bullet.GetComponent<Rigidbody>();
			Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * i / 15), 0f, -1f);
	
			bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
		}

		yield return new WaitForSeconds(1f - stageNum * 0.2f);

		bulletSpeed = 3;
		if (stageNum > 0)
		{
			for (int i = 0; i < 10; i++)
			{
				var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
				var bulrigid = bullet.GetComponent<Rigidbody>();
				Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * i / 10), 0f, -1f);

				bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
			}
			yield return new WaitForSeconds(1f - stageNum * 0.2f);
			for (int i = 0; i < 15; i++)
			{
				var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
				var bulrigid = bullet.GetComponent<Rigidbody>();
				Vector3 direction = new Vector3(Mathf.Cos(Mathf.PI * i / 15), 0f, -1f);

				bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
			}
			yield return new WaitForSeconds(1f - stageNum * 0.2f);
		}

		TryAttack();
	}

	IEnumerator Ptrn_Stab()
	{
		anim.SetInteger("Pattern", 2);
		int bulletSpeed = 12;

		yield return new WaitForSeconds(1.6f);
		anim.SetBool("isIdle", true);

		Vector3 direction = new Vector3(0, 0f, -1f);

		var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, Quaternion.identity);
		var bulrigid = bullet.GetComponent<Rigidbody>();
		bulrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
		yield return new WaitForSeconds(0.3f);

		for (int i = 1; i < 6; i++)
		{
			var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * 3 * i, Quaternion.identity);
			var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
			bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
			var bulletRight = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.right * 3 * i, Quaternion.identity);
			var bulRrigid = bulletRight.GetComponent<Rigidbody>();
			bulRrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

			yield return new WaitForSeconds(0.3f);
		}

		if (stageNum > 0)
		{
			for (int i = 6; i > 1; i--)
			{
				var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * 3 * i, Quaternion.identity);
				var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
				bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
				var bulletRight = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.right * 3 * i, Quaternion.identity);
				var bulRrigid = bulletRight.GetComponent<Rigidbody>();
				bulRrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

				yield return new WaitForSeconds(0.2f);
			}
		}
		if (stageNum > 1)
		{
			for (int i = 1; i < 6; i++)
			{
				var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * 3 * i, Quaternion.identity);
				var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
				bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
				var bulletRight = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.right * 3 * i, Quaternion.identity);
				var bulRrigid = bulletRight.GetComponent<Rigidbody>();
				bulRrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

				yield return new WaitForSeconds(0.1f);
			}
		}
		
		yield return new WaitForSeconds(1f - stageNum * 0.2f);

		TryAttack();
	}

	IEnumerator Ptrn_Stab2()
	{
		anim.SetInteger("Pattern", 2);
		int bulletSpeed = 14;

		yield return new WaitForSeconds(1.6f);
		anim.SetBool("isIdle", true);

		Vector3 direction = new Vector3(0, 0f, -1f);

		int rand = Random.Range(0, 2);

		if (rand == 0)
		{
			for (int i = 12; i >= 0; i--)
			{
				var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * i, Quaternion.identity);
				var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
				bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

				yield return new WaitForSeconds(0.1f);
			}
			if (stageNum > 1)
			{
				for (int i = 0; i >= -7; i--)
				{
					var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * i, Quaternion.identity);
					var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
					bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

					yield return new WaitForSeconds(0.1f);
				}
			}
		}
		else
		{
			for (int i = 12; i >= 0; i--)
			{
				var bulletRight = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.right * i, Quaternion.identity);
				var bulRrigid = bulletRight.GetComponent<Rigidbody>();
				bulRrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

				yield return new WaitForSeconds(0.1f);
			}
			if (stageNum > 1)
			{
				for (int i = 0; i >= -7; i--)
				{
					var bulletLeft = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position + Vector3.left * i, Quaternion.identity);
					var bulLrigid = bulletLeft.GetComponent<Rigidbody>();
					bulLrigid.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);

					yield return new WaitForSeconds(0.1f);
				}
			}
		}

		yield return new WaitForSeconds(1f - stageNum * 0.2f);

		TryAttack();
	}
}
