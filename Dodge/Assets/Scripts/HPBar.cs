using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	public Image hpBar;
	void Start()
	{

	}

	void Update()
	{

	}

	public void setHP(int hp)
	{
		hpBar.fillAmount = (float)hp / 100f;
	}
}
