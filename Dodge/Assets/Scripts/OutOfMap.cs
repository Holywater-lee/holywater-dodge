using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfMap : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "OutZone")
		{
			Destroy(gameObject);
		}
	}
}
