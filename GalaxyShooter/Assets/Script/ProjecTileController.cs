using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjecTileController : MonoBehaviour
{
	[Header("ProjectTile's variables")]
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private Vector2 m_Direction;
	[SerializeField] private int damage;
	private bool m_fromPlayer;
	private float m_lifeTime;
	private float cur_speedMultiplier;

    // Update is called once per frame
    void Update()
    {
		transform.Translate(m_Direction * Time.deltaTime * cur_speedMultiplier);

		if (m_lifeTime <= 0)
			Release();
		m_lifeTime -= Time.deltaTime;
	}

	private void Release()
	{
		if (m_fromPlayer == true)
			SpawnManager.Instance.ReleasePlayerProjectTile(this);
		else
			SpawnManager.Instance.ReleaseEnemyProjectTile(this);
	}

	public void SetFromPlayer(bool value)
	{
		m_fromPlayer = value;
	}
	
	public void Fire(float speedMultiplier)
	{
		m_lifeTime = 10f/ speedMultiplier;
		cur_speedMultiplier = m_MoveSpeed * speedMultiplier;
	}

	//xu ly va cham trigger giua projectTile với player va enemy
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Release();
			EnemyController enemy;
			//tim enemy controller script
			collision.gameObject.TryGetComponent(out enemy);
			enemy.hit(damage);
			Vector3 hitpos = collision.ClosestPoint(transform.position);
			SpawnManager.Instance.spawnHitFX(hitpos);
		}
		if (collision.gameObject.CompareTag("Player"))
		{
			Release();
			PlayerController player;
			//tim player controller script
			collision.gameObject.TryGetComponent(out player);
			player.hit(damage);
			Vector3 hitpos = collision.ClosestPoint(transform.position);
			SpawnManager.Instance.spawnHitFX(hitpos);
		}
	}

}
