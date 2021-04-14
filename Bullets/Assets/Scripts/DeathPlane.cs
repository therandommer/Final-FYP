using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<EnemyGameplay>())
		{
			col.gameObject.SendMessage("Despawn");
		}
		else
		{
			col.gameObject.SendMessage("Die");
		}
	}
}
