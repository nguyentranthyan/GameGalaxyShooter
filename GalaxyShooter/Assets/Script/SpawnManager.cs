using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
	[SerializeField] private bool m_Active;

	[Header("Pooling Object variables")]
	[SerializeField] private PlayerController m_PlayerControllerPrefabs;
	[SerializeField] private EnemiesPool m_EnemiesPool;
	[SerializeField] private ProjectTilePool m_EnemyProjectTilePool;
	[SerializeField] private ProjectTilePool m_PlayerProjectTilePool;
	[SerializeField] private PowerPool m_PowerUpPool;
	[SerializeField] private ParticleFXPool m_hitFxPool;
	[SerializeField] private ParticleFXPool m_ExplosionFXsPlayerPool;
	[SerializeField] private ParticleFXPool m_ExplosionFXsPool;

	[Header("Enemy's Path Manager variables")]
	[SerializeField] private EnemyPath[] m_EnemyPath;
	[SerializeField] private int m_MinTotalEnemies;
	[SerializeField] private int m_MaxTotalEnemies;

	[SerializeField] private int m_tolalgroup;
	[SerializeField] private float m_EnemySpawnInterval;

	[Header("Player's Path Manager variables")]
	public PlayerController m_Player;

	//kiem tra spawn Enemy 
	private bool m_IsSpawningEnemies;
	private WaveData m_curWave;

	#region singleton
	[Header("Golbal variables")]
	private static SpawnManager m_Isntance;
	public static SpawnManager Instance
	{
		get
		{
			if (m_Isntance == null)
				m_Isntance = FindObjectOfType<SpawnManager>();
			return m_Isntance;
		}
	}
	private void Awake()
	{
		if (m_Isntance == null)
			m_Isntance = this;
		else if (m_Isntance != this)
			Destroy(gameObject);
	}
	#endregion

	public void StartBatter(WaveData wave, bool resetPosition)
	{
		//luu du lieu voi wave data
		m_curWave = wave;
		m_MinTotalEnemies = m_curWave.minTotalEnemies;
		m_MaxTotalEnemies = m_curWave.maxTotalEnemies;
		m_tolalgroup = m_curWave.totalGroup;

		//check khoi tao player
		if (m_Player == null)
		{
			m_Player = Instantiate(m_PlayerControllerPrefabs);//khoi tao player
			m_Player.shield.SetActive(false);
		}

		if (m_Player != null && resetPosition)
		{
			m_Player.transform.position = Vector3.zero;
			m_Player.shield.SetActive(false);
		}
			
		StartCoroutine(IESpawnEnemyGroup(m_tolalgroup));
		StartCoroutine(IESpawnPowerUp(3f));
	}


	private IEnumerator IESpawnPowerUp(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		spawnPowerUp(transform.position);
	}


	//spawn Enemy Group
	private IEnumerator IESpawnEnemyGroup(int pgroup)
	{
		m_IsSpawningEnemies = true;
		for (int i = 0; i < pgroup; i++){

			int totalEnemies = Random.Range(m_MinTotalEnemies, m_MaxTotalEnemies);

			EnemyPath path = m_EnemyPath[Random.Range(0, m_EnemyPath.Length)];
			yield return StartCoroutine(IESpawnEnemy(totalEnemies,path));

			if (i < pgroup - 1)
				yield return new WaitForSeconds(3f / m_curWave.speedMultiplier);
		}
		m_IsSpawningEnemies = false;
	}

	//spawn Enemy
	private IEnumerator IESpawnEnemy(int m_TotalEnemies, EnemyPath path)
	{
		for (int i = 0; i < m_TotalEnemies; i++)
		{
			yield return new WaitUntil(()=>m_Active);
			yield return new WaitForSeconds(m_EnemySpawnInterval / m_curWave.speedMultiplier);

			//dung Instantiate lam ton dung luong memory
			//EnemyController enemy= Instantiate(m_EnemyPrefabs, null);

			// su dung pool
			EnemyController enemy = m_EnemiesPool.spawn(path.WayPoints[0].position, transform);
			
			//khoi tao duong di enemy
			enemy.Init(path.WayPoints , m_curWave.speedMultiplier);
		}
	}

	//thay cho destroy Enemy
	public void ReleasedEnemy(EnemyController obj)
	{
		m_EnemiesPool.release(obj);
	}
	public void ReleasedEnemyController(EnemyController Enemy)
	{
		m_EnemiesPool.release(Enemy);
	}

	//thay cho destroy EnemyProjectTile
	public ProjecTileController spawnEnemyProjectTile(Vector3 position)
	{
		ProjecTileController obj = m_EnemyProjectTilePool.spawn(position, transform);
		obj.SetFromPlayer(false);
		return obj;
	}

	
	public void ReleaseEnemyProjectTile(ProjecTileController projectile)
	{
		m_EnemyProjectTilePool.release(projectile);
	}

	//thay cho destroy PowerUp
	public PowerUp spawnPowerUp(Vector3 position)
	{
		PowerUp obj = m_PowerUpPool.spawn(position, transform);
		return obj;
	}

	public void ReleasePowerUp(PowerUp powerUp)
	{
		m_PowerUpPool.release(powerUp);
	}

	//thay cho destroy PlayerProjectTile
	public ProjecTileController spawnPlayerProjectTile(Vector3 position)
	{
		ProjecTileController obj = m_PlayerProjectTilePool.spawn(position, transform);
		obj.SetFromPlayer(true);
		return obj;
	}

	public void ReleasePlayerProjectTile(ProjecTileController projectile)
	{
		m_EnemyProjectTilePool.release(projectile);
	}

	// spawn HitFX
	public ParticleFX spawnHitFX(Vector3 position)
	{
		ParticleFX fx = m_hitFxPool.spawn(position,transform);
		fx.Setpool(m_hitFxPool);
		return fx;
	}

	public void ReleaseHitFX(ParticleFX obj)
	{
		m_hitFxPool.release(obj);
	}

	// spawn ExplosionFX
	public ParticleFX spawnExplosionFX(Vector3 position)
	{
		ParticleFX fx = m_ExplosionFXsPool.spawn(position, transform);
		fx.Setpool(m_ExplosionFXsPool);
		return fx;
	}

	public bool isClear()
	{
		if (m_IsSpawningEnemies || m_EnemiesPool.activeobjs.Count > 0)
			return false;
		return true;
	}

	public void Clear()
	{
		m_EnemiesPool.Clear();
		m_EnemyProjectTilePool.Clear();
		m_PlayerProjectTilePool.Clear();
		m_PowerUpPool.Clear();
		m_hitFxPool.Clear();
		m_ExplosionFXsPool.Clear();
		m_ExplosionFXsPool.Clear();
		StopAllCoroutines();// dung cac Coroutines/IEnumerator
	}
}
