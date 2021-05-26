using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
	[Header("Player's variables")]
	[SerializeField] private float m_MoveSpeed = 5.0f;

	[Header("ProjectTile's variables")]
	[SerializeField] private ProjecTileController m_ProjectTile;
	[SerializeField] private Transform m_FiringPoint0;//hướng bắn giữa
	[SerializeField] private Transform m_FiringPoint1;//hướng bắn trái
	[SerializeField] private Transform m_FiringPoint2;//hướng bắn phải
	[SerializeField] private float m_FiringCoolDown;//tốc độ bắn
	private float m_TempCoolDown;
	private Vector2 m_ChangeMove;

	//PowerUp
	public bool canTripleshoot=false;
	public bool canSpeedShooter = false;
	public GameObject shield;
	public bool haveShield = false;
	

	[Header("Player's Health variables")]
	public Action<int, int> onHPChange;
	public int m_hp;
	public int m_currentHp;
	private PowerUp powerUp;

	// Start is called before the first frame update
	void Start()
    {
		m_currentHp = m_hp;
		onHPChange?.Invoke(m_currentHp, m_hp);
	}
	// Update is called once per frame
	void FixedUpdate()
    {
		if (!GameManager.Instance.isActive())
			return;
		if (canSpeedShooter)
		{
			m_MoveSpeed = 10.0f;
		}
		else
		{
			m_MoveSpeed = 5.0f;
		}
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		//huong di chuyen
		m_ChangeMove = new Vector2(horizontal, vertical);

		m_ChangeMove.Normalize();
		//}
		if (Input.GetMouseButton(0))
		{
			if (m_TempCoolDown <= 0)
			{
				if (canTripleshoot == true)
				{
					Fire();
				}
				else
				{
					Fire();
				}
				m_TempCoolDown = m_FiringCoolDown;
			}
		}
		transform.Translate(m_ChangeMove * Time.deltaTime * m_MoveSpeed);
		m_TempCoolDown -= Time.deltaTime;
	}
	//khởi tạo đạn
	private void Fire()
	{
		ProjecTileController projectile0 = SpawnManager.Instance.spawnPlayerProjectTile(m_FiringPoint0.position);
		projectile0.Fire(1);

		if(canTripleshoot == true)
		{
			ProjecTileController projectile1 = SpawnManager.Instance.spawnPlayerProjectTile(m_FiringPoint1.position);
			ProjecTileController projectile2 = SpawnManager.Instance.spawnPlayerProjectTile(m_FiringPoint2.position);
			projectile1.Fire(1);
			projectile2.Fire(1);
		}
		AudioManager.Instance.PlayLazerSFXClip();
	}
	
	public void HealthBonus()
	{
		SpawnManager.Instance.ReleasePowerUp(powerUp);
		if (m_currentHp == 100) return;
		else
		{
			m_currentHp += 20;
			if (m_currentHp > 100) m_currentHp = 100;
			onHPChange?.Invoke(m_currentHp, m_hp);
		}
	}

	public void HaveShield()
	{
		SpawnManager.Instance.ReleasePowerUp(powerUp);
		haveShield = true;
		shield.SetActive(true);
		StartCoroutine(HaveShieldShooter());
	}
	public IEnumerator HaveShieldShooter()
	{
		yield return new WaitForSeconds(5.0f);
		haveShield = false;
		shield.SetActive(false);
	}

	public void SpeedShooter()
	{
		SpawnManager.Instance.ReleasePowerUp(powerUp);
		canSpeedShooter = true;
		StartCoroutine(SpeedShootPowerDown());
	}
	public IEnumerator SpeedShootPowerDown()
	{
		yield return new WaitForSeconds(3.0f);
		canSpeedShooter = false;
	}

	public void TrippleShooter()
	{
		SpawnManager.Instance.ReleasePowerUp(powerUp);
		canTripleshoot = true;
		StartCoroutine(TripleShootPowerDown());
	}

	public IEnumerator TripleShootPowerDown()
	{
		yield return new WaitForSeconds(5.0f);
		canTripleshoot = false;
	}
	//mat mau khi va cham 
	public void hit(int damage)
	{
		m_currentHp -= damage;
		onHPChange?.Invoke(m_currentHp, m_hp);

		if (m_currentHp <= 0)
		{
			Destroy(gameObject);
			//player bi pha huy
			SpawnManager.Instance.spawnExplosionFX(transform.position);
			//using singleton
			GameManager.Instance.Gameover(false); //gameover
			AudioManager.Instance.PlayExplosionSFXClip();
		}
		//player bi va cham
		AudioManager.Instance.PlayHitSFXClip();
	}
}
