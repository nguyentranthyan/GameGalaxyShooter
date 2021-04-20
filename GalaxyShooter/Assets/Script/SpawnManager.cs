using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemiesPool
{
	public  EnemyController m_EnemyPrefabs;
	public List<EnemyController> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<EnemyController> activeobjs; //chua cac danh sach enemy dang su dung

	//spawn cac obj when using
	public EnemyController spawn(Vector3 position,Transform parent)
	{
		if (inActiveobjs.Count == 0)
		{
			EnemyController newObj = GameObject.Instantiate(m_EnemyPrefabs, parent);
			newObj.transform.position = position;
			activeobjs.Add(newObj);
			return newObj;
		}
		else
		{
			EnemyController oldObj = inActiveobjs[0];
			oldObj.gameObject.SetActive(true);
			oldObj.transform.SetParent(parent);
			oldObj.transform.position = position;
			activeobjs.Add(oldObj);
			inActiveobjs.RemoveAt(0);
			return oldObj;
		}
	}

	public void release(EnemyController obj)
	{
		if (activeobjs.Contains(obj))
		{
			activeobjs.Remove(obj);
			inActiveobjs.Add(obj);
			obj.gameObject.SetActive(false);
		}
	}

	//xoa cac ativeobj 
	public void Clear()
	{
		while (activeobjs.Count > 0)
		{
			EnemyController obj = activeobjs[0];
			obj.gameObject.SetActive(false);
			activeobjs.RemoveAt(0);
			inActiveobjs.Add(obj);
		}
	}
}
[System.Serializable]
public class ProjectTilePool
{
	public ProjecTileController m_projectTilePrefabs;
	public List<ProjecTileController> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<ProjecTileController> activeobjs; //chua cac danh sach enemy dang su dung

	//spawn cac obj when using
	public ProjecTileController spawn(Vector3 position, Transform parent)
	{
		if (inActiveobjs.Count == 0)
		{
			ProjecTileController newObj = GameObject.Instantiate(m_projectTilePrefabs, parent);
			newObj.transform.position = position;
			activeobjs.Add(newObj);
			return newObj;
		}
		else
		{
			ProjecTileController oldObj = inActiveobjs[0];
			oldObj.gameObject.SetActive(true);
			oldObj.transform.SetParent(parent);
			oldObj.transform.position = position;
			activeobjs.Add(oldObj);
			inActiveobjs.RemoveAt(0);
			return oldObj;
		}
	}

	public void release(ProjecTileController obj)
	{
		if (activeobjs.Contains(obj))
		{
			activeobjs.Remove(obj);
			inActiveobjs.Add(obj);
			obj.gameObject.SetActive(false);
		}
	}
	//xoa cac ativeobj 
	public void Clear()
	{
		while (activeobjs.Count > 0)
		{
			ProjecTileController obj = activeobjs[0];
			obj.gameObject.SetActive(false);
			activeobjs.RemoveAt(0);
			inActiveobjs.Add(obj);
		}
	}
}

[System.Serializable]
public class ParticleFXcontroller
{
	public ParticleFX m_ParticleFXPrefabs;
	public List<ParticleFX> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<ParticleFX> activeobjs; //chua cac danh sach enemy dang su dung

	//spawn cac obj when using
	public ParticleFX spawn(Vector3 position, Transform parent)
	{
		if (inActiveobjs.Count == 0)
		{
			ParticleFX newObj = GameObject.Instantiate(m_ParticleFXPrefabs, parent);
			newObj.transform.position = position;
			activeobjs.Add(newObj);
			return newObj;
		}
		else
		{
			ParticleFX oldObj = inActiveobjs[0];
			oldObj.gameObject.SetActive(true);
			oldObj.transform.SetParent(parent);
			oldObj.transform.position = position;
			activeobjs.Add(oldObj);
			inActiveobjs.RemoveAt(0);
			return oldObj;
		}
	}

	public void release(ParticleFX obj)
	{
		if (activeobjs.Contains(obj))
		{
			activeobjs.Remove(obj);
			inActiveobjs.Add(obj);
			obj.gameObject.SetActive(false);
		}
	}

	//xoa cac ativeobj 
	public void Clear()
	{
		while (activeobjs.Count > 0)
		{
			ParticleFX obj = activeobjs[0];
			obj.gameObject.SetActive(false);
			activeobjs.RemoveAt(0);
			inActiveobjs.Add(obj);
		}
	}
}

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private bool m_Active;

	//[SerializeField] private EnemyController m_EnemyPrefabs;
	[SerializeField] private PlayerController m_PlayerControllerPrefabs;
	[SerializeField] private EnemiesPool m_EnemiesPool;
	[SerializeField] private ProjectTilePool m_EnemyProjectTilePool;
	[SerializeField] private ProjectTilePool m_PlayerProjectTilePool;
	[SerializeField] private ParticleFXcontroller m_hitFxPool;
	[SerializeField] private ParticleFXcontroller m_shooterFxPool;
	[SerializeField] private ParticleFXcontroller m_ExplosionFXsPool;

	[SerializeField] private EnemyPath[] m_EnemyPath;
	[SerializeField] private int m_MinTotalEnemies;
	[SerializeField] private int m_MaxTotalEnemies;

	[SerializeField] private int m_tolalgroup;
	[SerializeField] private float m_EnemySpawnInterval;

	private PlayerController m_Player;
	public PlayerController Player => m_Player;//get m_player
	private bool m_IsSpawningEnemies;//kiem tra sao Enemy 
	private WaveData m_curWave;
	//using singleton
	private static SpawnManager m_Isntance;//global variable
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

	public void StartBatter(WaveData wave, bool resetPosition)
	{
		//luu du lieu voi wave data
		m_curWave = wave;
		m_MinTotalEnemies = m_curWave.minTotalEnemies;
		m_MaxTotalEnemies = m_curWave.maxTotalEnemies;
		m_tolalgroup = m_curWave.totalGroup;

		//check khoi tao player
		if (m_Player == null)
			m_Player = Instantiate(m_PlayerControllerPrefabs);//khoi tao player
		if(resetPosition)
			m_Player.transform.position = Vector3.zero;
		StartCoroutine(IESpawnEnemyGroup(m_tolalgroup));
	}

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

	private IEnumerator IESpawnEnemy(int m_TotalEnemies,EnemyPath path)
	{
		for (int i = 0; i < m_TotalEnemies; i++)
		{
			yield return new WaitUntil(()=>m_Active);
			yield return new WaitForSeconds(m_EnemySpawnInterval / m_curWave.speedMultiplier);

			//dung Instantiate lam ton dung luong memory
			//EnemyController enemy= Instantiate(m_EnemyPrefabs, null);
			EnemyController enemy = m_EnemiesPool.spawn(path.WayPoints[0].position, transform);// su dung pool
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

	public ParticleFX spawnshooterFX(Vector3 position)
	{
		ParticleFX fx = m_shooterFxPool.spawn(position, transform);
		fx.Setpool(m_shooterFxPool);
		return fx;
	}

	public void ReleaseshooterFX(ParticleFX obj)
	{
		m_shooterFxPool.release(obj);
	}

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
		m_shooterFxPool.Clear();
		m_hitFxPool.Clear();
		m_ExplosionFXsPool.Clear();
		StopAllCoroutines();// dung cac Coroutines/IEnumerator
	}
}
