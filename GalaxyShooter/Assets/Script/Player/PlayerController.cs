using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Player's variables")]
	[SerializeField] private int m_MoveSpeed;

	[Header("ProjectTile's variables")]
	[SerializeField] private ProjecTileController m_ProjectTile;
	[SerializeField] private Transform m_FiringPoint;//hướng bắn
	[SerializeField] private float m_FiringCoolDown;//tốc độ bắn
	private float m_TempCoolDown;

	[Header("Player's Health variables")]
	public Action<int, int> onHPChange;
	[SerializeField] private int m_hp = 5;
	[SerializeField] private int m_currentHp;

	//check NewInput
	[SerializeField] private bool m_UseNewInputSystem;
	private PlayerInput m_PlayerInput;
	private Vector2 m_MovementInputValue;
	private bool m_AttackInputValue;

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
		if (onHPChange != null)
			onHPChange(m_currentHp, m_hp);
	}

    // Update is called once per frame
    void Update()
    {
		if (!GameManager.Instance.isActive())
			return;
		
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
					Fire();
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
					Fire();
					m_TempCoolDown = m_FiringCoolDown;
				}
			}
		}
	
		transform.Translate(direction * Time.deltaTime * m_MoveSpeed);
		m_TempCoolDown -= Time.deltaTime;
	}

	//khoi tao đạn
	private void Fire()
	{
		ProjecTileController projectile = SpawnManager.Instance.spawnPlayerProjectTile(m_FiringPoint.position);
		projectile.Fire(1);
		AudioManager.Instance.PlayLazerSFXClip();
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
			SpawnManager.Instance.spawnExplosionFXsPlayerPool(transform.position);
			//using singleton
			GameManager.Instance.Gameover(false); //gameover
			AudioManager.Instance.PlayExplosionSFXClip();
		}
		//player bi va cham
		AudioManager.Instance.PlayHitSFXClip();
	}
}
