using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjecTileController : MonoBehaviour
{
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private Vector2 m_Direction;
	[SerializeField] private int damage;
	[SerializeField] private SpawnManager m_SpawnManager;
	private bool m_player;
	private float m_lifeTime;//tg song cua dan

	// Start is called before the first frame update
	void Start()
    {
		m_SpawnManager = FindObjectOfType<SpawnManager>();
	}

    // Update is called once per frame
    void Update()
    {
		transform.Translate(m_Direction * Time.deltaTime * m_MoveSpeed);
		if (m_lifeTime <= 0)
			Release();
		m_lifeTime -= Time.deltaTime;
	}

	private void Release()
	{
		if (m_player == true)
			m_SpawnManager.ReleasePlayerProjectTile(this);
		else
			m_SpawnManager.ReleaseEnemyProjectTile(this);
	}

	public void SetFromPlayer(bool value)
	{
		m_player = value;
	}

	public void Fire()
	{
		//Destroy(gameObject, 10f);
		m_lifeTime = 10f;
	}

	//xu ly va cham trigger giua player va enemy
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
		if (collision.gameObject.CompareTag("Enemy"))
		{
			//Destroy(gameObject);
			Release();
			EnemyController enemy;
			//tim enemy controller
			collision.gameObject.TryGetComponent(out enemy);
			enemy.hit(damage);
			Vector3 hitpos = collision.ClosestPoint(transform.position);
			m_SpawnManager.spawnHitFX(hitpos);
		}
		if (collision.gameObject.CompareTag("Player"))
		{
			//Destroy(gameObject);

			Release();
			PlayerController player;
			//tim enemy controller
			collision.gameObject.TryGetComponent(out player);
			player.hit(damage);
			Vector3 hitpos = collision.ClosestPoint(transform.position);
			m_SpawnManager.spawnHitFX(hitpos);
		}
	}

}
