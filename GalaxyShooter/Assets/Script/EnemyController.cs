using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private Transform[] m_WayPoints; //duong di enemy
	private int m_curentLWayPointIndex; //diem hien tai cua enemy

	[SerializeField] private bool m_Active;

	[SerializeField] private ProjecTileController m_ProjectTile;
																	
	[SerializeField] private Transform m_FiringPoint;//hướng bắn
	[SerializeField] private float m_MinFiringCoolDown;//tốc độ bắn
	[SerializeField] private float m_MaxFiringCoolDown;
	private float m_TempCoolDown;

	[SerializeField] private int m_hp;
	[SerializeField] private int m_currentHp;

	private SpawnManager m_SpawnManager;

	// Start is called before the first frame update
	void Start()
    {
		m_SpawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!m_Active)
			return;

		int nextWaypoint = m_curentLWayPointIndex + 1;
		if (nextWaypoint > m_WayPoints.Length - 1)
			nextWaypoint = 0;
		transform.position = Vector3.MoveTowards(transform.position, m_WayPoints[nextWaypoint].position, m_MoveSpeed * Time.deltaTime);
		if (transform.position == m_WayPoints[nextWaypoint].position)
			m_curentLWayPointIndex = nextWaypoint;

		Vector3 direction = m_WayPoints[nextWaypoint].position - transform.position;//huong di chuyen 
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //goc cua huong di chuyen 
		transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

		if (m_TempCoolDown <= 0)
		{
			Fire();
			m_TempCoolDown = Random.Range(m_MinFiringCoolDown,m_MaxFiringCoolDown);
		}
		m_TempCoolDown -= Time.deltaTime;
	}
	//khoi tao duong di enemy
	public void Init(Transform[] waypoints)
	{
		m_WayPoints = waypoints;
		m_Active = true;
		transform.position = waypoints[0].position;
		m_TempCoolDown = Random.Range(m_MinFiringCoolDown, m_MaxFiringCoolDown);
		m_currentHp = m_hp;
	}
	private void Fire()
	{
		//khoi tao đạn
		//ProjecTileController projectile = Instantiate(m_ProjectTile, m_FiringPoint.position, Quaternion.identity, null);
		ProjecTileController projectile = m_SpawnManager.spawnEnemyProjectTile(m_FiringPoint.position);
		projectile.Fire();
	}
	//mat mau khi va cham 
	public void hit(int damage)
	{
		m_currentHp -= damage;
		if (m_currentHp <= 0)
		{
			//Destroy(gameObject);
			m_SpawnManager.ReleasedEnemy(this);
		}
	}
	
}
