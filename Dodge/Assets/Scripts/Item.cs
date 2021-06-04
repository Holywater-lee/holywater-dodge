using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[SerializeField] int healAmount = 20;
	void Start()
	{
		StartCoroutine("Rotator");
	}

	IEnumerator Rotator()
	{
		while (true)
		{
			transform.Rotate(0f, 15f * Time.deltaTime, 0f);
			yield return null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if (player != null)
			{
				player.GetHeal(healAmount);
				Destroy(gameObject);
			}
		}
	}
}
