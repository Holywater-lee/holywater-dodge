using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	public Image hpBar;

	public void setHP(float hp, float maxhp)
	{
		hpBar.fillAmount = hp / maxhp;
	}
}
