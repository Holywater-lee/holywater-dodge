using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("몬스터 능력치")]
	public float speed;
	[SerializeField] float life;
	[SerializeField] float maxLife;
	[SerializeField] float score;
	[SerializeField] HPBar hpBar;
	[SerializeField] bool canAttack;
	[SerializeField] float AttackCooldown;

	Renderer color;

	float ranAtkcool;

	void Awake()
	{
		color = GetComponent<Renderer>();
	}

	void Start()
	{
		GameManager.instance.enemysList.Add(this.gameObject);

		if (canAttack)
		{
			ranAtkcool = Random.Range(AttackCooldown - 1f, AttackCooldown + 1f);
			StartCoroutine(Attack());
		}
	}

	public void getDamage(float damage)
	{
		life -= damage;
		hpBar.setHP(life, maxLife);

		StartCoroutine(onDamageColor(0.09f));

		if (life <= 0)
		{
			GameManager.instance.currentScore += score;
			GameManager.instance.RefreshScore();
			SpawnItem();

			GameManager.instance.enemysList.Remove(this.gameObject);
			Destroy(gameObject);
		}
	}

	void SpawnItem()
	{
		int rand = Random.Range(0, 11);
		if (rand < 2)
		{
			int itemindex = Random.Range(0, 4);
			Instantiate(GameManager.instance.itemPrefabs[itemindex], transform.position, Quaternion.identity);
		}
	}

	IEnumerator onDamageColor(float sec)
	{
		color.material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
		yield return new WaitForSeconds(sec);
		color.material.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
	}

	IEnumerator Attack()
	{
		while(canAttack)
		{
			yield return new WaitForSeconds(ranAtkcool);

			ranAtkcool = Random.Range(AttackCooldown - 1f, AttackCooldown + 1f);
			var pos = FindObjectOfType<PlayerController>();
			if (pos != null)
				transform.LookAt(pos.transform.position);
			var bullet = Instantiate(GameManager.instance.enemyBulletPrefab, transform.position, transform.rotation);
			var bulRigid = bullet.GetComponent<Rigidbody>();
			bulRigid.velocity = transform.forward * 7;
		}
	}
}
