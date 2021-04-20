using System;
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

	public Action<int, int> onHPChange;
	[SerializeField] private int m_hp = 5;
	[SerializeField] private int m_currentHp;

	//private GameManager m_gameManager;
	//private SpawnManager m_SpawnManager;
	private AudioManager m_AudioManager;

    // Start is called before the first frame update
    void Start()
    {
		m_currentHp = m_hp;
		if (onHPChange != null)
			onHPChange(m_currentHp, m_hp);
		//m_SpawnManager = FindObjectOfType<SpawnManager>();
		//m_gameManager = FindObjectOfType<GameManager>();
		m_AudioManager = FindObjectOfType<AudioManager>();
	}

    // Update is called once per frame
    void Update()
    {
		//if (!m_gameManager.isActive())
		if (!GameManager.Instance.isActive())
			return;
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
		ProjecTileController projectile = SpawnManager.Instance.spawnPlayerProjectTile(m_FiringPoint.position);
		SpawnManager.Instance.spawnshooterFX(m_FiringPoint.position);
		projectile.Fire(1);
		m_AudioManager.PlayLazerSFXClip();
	}

	//mat mau khi va cham 
	public void hit(int damage)
	{
		m_currentHp -= damage;
		if (onHPChange != null)
			onHPChange(m_currentHp,m_hp);

		if (m_currentHp <= 0)
		{
			Destroy(gameObject);
			//player bi pha huy
			SpawnManager.Instance.spawnExplosionFX(transform.position);
			//m_gameManager.Gameover(false);
			//using singleton
			GameManager.Instance.Gameover(false); //gameover
			m_AudioManager.PlayExplosionSFXClip();
		}
		//player bi va cham
		m_AudioManager.PlayHitSFXClip();
	}
}
