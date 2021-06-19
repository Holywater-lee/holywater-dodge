using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[SerializeField] string ItemType;
	[SerializeField] int healAmount = 20;

	void Update()
	{
		transform.Rotate(0f, 15f * Time.deltaTime, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if (player != null)
			{
				if (ItemType == "Heal")
				{
					player.GetHeal(healAmount);
					Destroy(gameObject);
				}
				else if (ItemType == "Buff")
				{
					player.PierceBuff();
					Destroy(gameObject);
				}
				else if (ItemType == "Upgrade")
				{
					player.increasedDamage += 10f;
					Destroy(gameObject);
				}
				else if (ItemType == "Bomb")
				{
					GameManager.instance.bombCount += 1;
					GameManager.instance.RefreshBombText();
					if (GameManager.instance.bombCount > 2)
						GameManager.instance.bombCount = 2;
					Destroy(gameObject);
				}
			}
		}
	}
}
