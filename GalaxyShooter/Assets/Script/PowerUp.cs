using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public float powerUpSpeed = 3.0f;
	public int powerUpID;

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * Time.deltaTime * powerUpSpeed);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			switch (powerUpID)
			{
				case 0: player.TrippleShooter(); break;
				case 1: player.SpeedShooter(); break;
				case 3: player.HaveShield(); break;
				case 4: player.HealthBonus(); break;
				default:
					break;
			}
			SpawnManager.Instance.ReleasePowerUp(this);
		}
	}
}
