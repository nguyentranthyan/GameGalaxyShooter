using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private int m_MoveSpeed;
	[SerializeField] private ProjecTileController m_ProjectTile;
	[SerializeField] private Transform m_FiringPoint;//hướng bắn
	[SerializeField] private float m_FiringCoolDown;//tốc độ bắn

	private float m_TempCoolDown;
	[SerializeField] private int m_hp;
	[SerializeField] private int m_currentHp;
	[SerializeField] private SpawnManager m_SpawnManager;

    // Start is called before the first frame update
    void Start()
    {
		m_currentHp = m_hp;
		m_SpawnManager = FindObjectOfType<SpawnManager>();

	}

    // Update is called once per frame
    void Update()
    {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		//huong di chuyen
		Vector2 direction = new Vector2(horizontal, vertical);
		transform.Translate(direction * Time.deltaTime * m_MoveSpeed);

		if (Input.GetKey(KeyCode.Space))
		{
			if (m_TempCoolDown <= 0)
			{
				Fire();
				m_TempCoolDown = m_FiringCoolDown;
			}
		}
		m_TempCoolDown -= Time.deltaTime;
	}
	private void Fire()
	{
		//khoi tao đạn
		//ProjecTileController projectile =Instantiate(m_ProjectTile, m_FiringPoint.position, Quaternion.identity, null);
		ProjecTileController projectile = m_SpawnManager.spawnPlayerProjectTile(m_FiringPoint.position);
		m_SpawnManager.spawnshooterFX(m_FiringPoint.position);
		projectile.Fire();
	}

	//mat mau khi va cham 
	public void hit(int damage)
	{
		m_currentHp -= damage;
		if (m_currentHp <= 0)
		{
			Destroy(gameObject);
		}
	}
}
