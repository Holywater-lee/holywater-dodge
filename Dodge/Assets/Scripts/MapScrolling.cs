using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScrolling : MonoBehaviour
{
	public float speed;

	void Update()
	{
		transform.Translate(new Vector3(0f, 0f, -speed));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ScrollZone")
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 46 + transform.position.z);
		}
	}
}
