using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

		Vector2 direction=Vector2.zero;
		if (!m_UseNewInputSystem)
		{
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			//huong di chuyen
			direction = new Vector2(horizontal, vertical);
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
			direction = m_MovementInputValue;
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Wall"))
		{

		}
	}
}
