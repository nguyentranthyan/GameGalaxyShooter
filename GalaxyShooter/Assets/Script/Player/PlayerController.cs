using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

	//PowerUp
	public bool canTripleshoot=false;
	public bool canSpeedShooter = false;
	public GameObject shield;
	public bool haveShield = false;
	

	[Header("Player's Health variables")]
	public Action<int, int> onHPChange;
	[SerializeField] private int m_hp;
	[SerializeField] private int m_currentHp;

	//check NewInput
	[SerializeField] private bool m_UseNewInputSystem;
	private PlayerInput m_PlayerInput;
	private Vector2 m_MovementInputValue;
	private bool m_AttackInputValue;
	private PowerUp powerUp;

	private void OnEnable()
	{
		if (m_PlayerInput == null)
		{
			m_PlayerInput = new PlayerInput();
			m_PlayerInput.Player.Movement.started += OnMovement;
			m_PlayerInput.Player.Movement.started += OnMovement;
			m_PlayerInput.Player.Movement.started += OnMovement;
			m_PlayerInput.Player.Attack.started += OnAttack;
			m_PlayerInput.Player.Attack.started += OnAttack;
			m_PlayerInput.Player.Attack.started += OnAttack;
			m_PlayerInput.Enable();
		}
	}

	private void OnDisable()
	{
		m_PlayerInput.Disable();
	}

	private void OnAttack(InputAction.CallbackContext obj)
	{
		if (obj.started)
		{
			m_AttackInputValue = true;
		}
		else if (obj.performed)
		{
			m_AttackInputValue = true;
		}
		else if (obj.canceled)
		{
			m_AttackInputValue = false;
		}
	}

	private void OnMovement(InputAction.CallbackContext obj)
	{
		if (obj.started)
		{
			m_MovementInputValue = obj.ReadValue<Vector2>();
		}
		else if (obj.performed)
		{
			m_MovementInputValue = obj.ReadValue<Vector2>();
		}
		else if (obj.canceled)
		{
			m_MovementInputValue = Vector2.zero;
		}
	}

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

		Vector2 direction=Vector2.zero;
		//UseInputSystemOld
		if (!m_UseNewInputSystem)
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			//huong di chuyen
			direction = new Vector2(horizontal, vertical);
			direction.Normalize();
			if (Input.GetKey(KeyCode.Space))
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
		}
		else
		{
			//UseInputSystemNew
			direction = m_MovementInputValue;
			direction.Normalize();
			if (m_AttackInputValue)
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
		}
	
		transform.Translate(direction * Time.deltaTime * m_MoveSpeed);
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
