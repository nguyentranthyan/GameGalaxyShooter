using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[Header("Enemy's variables")]
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private Transform[] m_WayPoints; //duong di enemy
	private int m_curentLWayPointIndex; //diem hien tai cua enemy

	[Header("Enemy's ProjecTile variables")]
	[SerializeField] private ProjecTileController m_ProjectTile;														
	[SerializeField] private Transform m_FiringPoint;//hướng bắn
	[SerializeField] private float m_MinFiringCoolDown;//tốc độ bắn
	[SerializeField] private float m_MaxFiringCoolDown;
	private float m_TempCoolDown;

	[Header("Enemy's Health variables")]
	[SerializeField] private int m_hp;
	[SerializeField] private int m_currentHp;

	[SerializeField] private bool m_Active;

	//luu trong wave
	private float curMoveSpeed;
	private float m_speedMultiplier;

	[SerializeField] private int ScoreEnenmy;//score enemy

    // Update is called once per frame
    void Update()
    {
		if (!m_Active)
			return;

		int nextWaypoint = m_curentLWayPointIndex + 1;
		if (nextWaypoint > m_WayPoints.Length - 1)
			nextWaypoint = 0;
		//Di chuyen Enemy den diem den
		transform.position = Vector3.MoveTowards(
								transform.position, 
								m_WayPoints[nextWaypoint].position, 
								curMoveSpeed * Time.deltaTime
							);
		//Enemy den diem den
		if (transform.position == m_WayPoints[nextWaypoint].position)
			m_curentLWayPointIndex = nextWaypoint;
		//Huong di chuyen 
		Vector3 direction = m_WayPoints[nextWaypoint].position - transform.position;
		//Goc cua huong di chuyen 
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
		transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

		//Thời gian bắn đạn 
		if (m_TempCoolDown <= 0)
		{
			Fire();
			m_TempCoolDown = Random.Range(m_MinFiringCoolDown,m_MaxFiringCoolDown);
		}
		
		m_TempCoolDown -= Time.deltaTime;
	}

	//khoi tao duong di enemy
	public void Init(Transform[] waypoints, float speedMultiplier)
	{
		m_Active = true;
		m_WayPoints = waypoints;
		m_speedMultiplier = speedMultiplier;
		curMoveSpeed = m_MoveSpeed * speedMultiplier;
		transform.position = waypoints[0].position;
		m_TempCoolDown = Random.Range(m_MinFiringCoolDown, m_MaxFiringCoolDown)/speedMultiplier;
		m_currentHp = m_hp;
	}

	//khoi tao đạn
	private void Fire()
	{
		//ProjecTileController projectile = Instantiate(m_ProjectTile, m_FiringPoint.position, Quaternion.identity, null);
		ProjecTileController projectile = SpawnManager.Instance.spawnEnemyProjectTile(m_FiringPoint.position);
		projectile.Fire(m_speedMultiplier);
		AudioManager.Instance.PlayPlasmaSFXClip();
	}

	//mat mau khi va cham 
	public void hit(int damage)
	{
		m_currentHp -= damage;
		if (m_currentHp <= 0)
		{
			SpawnManager.Instance.ReleasedEnemyController(this);
			SpawnManager.Instance.spawnExplosionFX(transform.position);

			//using singleton
			GameManager.Instance.AddScore(ScoreEnenmy);
			AudioManager.Instance.PlayExplosionSFXClip();
		}
		AudioManager.Instance.PlayHitSFXClip();
	}
	
}
