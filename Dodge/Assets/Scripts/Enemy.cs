using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;
	[SerializeField] float life;
	[SerializeField] float maxLife;
	[SerializeField] float score;
	[SerializeField] string enemyType;
	[SerializeField] HPBar hpBar;

	Renderer color;

	void Awake()
	{
		color = GetComponent<Renderer>();
	}

	public void getDamage(float damage)
	{
		life -= damage;
		hpBar.setHP(life, maxLife);

		//color.material.color = new Color(1f, 150 / 255f, 150 / 255f);
		//Invoke("ReturnColor", 0.08f);
		StartCoroutine(onDamageColor(0.09f));

		if (life <= 0)
		{
			GameManager.instance.currentScore += score;
			GameManager.instance.RefreshScore();
			Destroy(gameObject);
		}
	}

	//void ReturnColor()
	//{
	//	color.material.color = new Color(1f, 52 / 255f, 52 / 255f);
	//}

	IEnumerator onDamageColor(float sec)
	{
		color.material.color = new Color(1f, 150 / 255f, 150 / 255f);
		yield return new WaitForSeconds(sec);
		color.material.color = new Color(1f, 52 / 255f, 52 / 255f);
	}
}
