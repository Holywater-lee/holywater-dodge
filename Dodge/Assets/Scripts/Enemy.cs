using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] float life;
	[SerializeField] float maxLife;
	[SerializeField] string enemyType;
	[SerializeField] HPBar hpBar;

	Rigidbody rigid;
	Renderer color;

	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		color = GetComponent<Renderer>();
	}

	void Start()
	{
		rigid.velocity = Vector3.forward * -1 * speed;
	}

	public void getDamage(float damage)
	{
		life -= damage;
		hpBar.setHP(life, maxLife);

		color.material.color = new Color(1f, 150 / 255f, 150 / 255f);
		Invoke("ReturnColor", 0.08f);

		if (life <= 0)
		{
			Destroy(gameObject);
		}
	}

	void ReturnColor()
	{
		color.material.color = new Color(1f, 52 / 255f, 52 / 255f);
	}
}
