using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemiesPool
{
	public  EnemyController m_EnemyPrefabs;
	public List<EnemyController> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<EnemyController> activeobjs; //chua cac danh sach enemy dang su dung

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
}
[System.Serializable]
public class ProjectTilePool
{
	public ProjecTileController m_projectTilePrefabs;
	public List<ProjecTileController> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<ProjecTileController> activeobjs; //chua cac danh sach enemy dang su dung

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
}

[System.Serializable]
public class ParticleFXcontroller
{
	public ParticleFX m_ParticleFXPrefabs;
	public List<ParticleFX> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<ParticleFX> activeobjs; //chua cac danh sach enemy dang su dung

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
}

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private bool m_Active;

	//[SerializeField] private EnemyController m_EnemyPrefabs;
	[SerializeField] private EnemiesPool m_EnemiesPool;
	[SerializeField] private ProjectTilePool m_EnemyProjectTilePool;
	[SerializeField] private ProjectTilePool m_PlayerProjectTilePool;
	[SerializeField] private ParticleFXcontroller m_hitFxPool;
	[SerializeField] private ParticleFXcontroller m_shooterFxPool;

	[SerializeField] private EnemyPath[] m_EnemyPath;

	[SerializeField] private int m_MinTotalEnemies;
	[SerializeField] private int m_MaxTotalEnemies;
	[SerializeField] private int m_tolalgroup;

	[SerializeField] private float m_EnemySpawnInterval;
	

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(IESpawnEnemyGroup(m_tolalgroup));
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	private IEnumerator IESpawnEnemyGroup(int pgroup)
	{
		for (int i = 0; i < pgroup; i++){
			int totalEnemies = Random.Range(m_MinTotalEnemies, m_MaxTotalEnemies);
			EnemyPath path = m_EnemyPath[Random.Range(0, m_EnemyPath.Length)];
			yield return StartCoroutine(IESpawnEnemy(totalEnemies,path));
			yield return new WaitForSeconds(3f);
		}
	}

	private IEnumerator IESpawnEnemy(int m_TotalEnemies,EnemyPath path)
	{
		for (int i = 0; i < m_TotalEnemies; i++)
		{
			yield return new WaitUntil(()=>m_Active);
			yield return new WaitForSeconds(m_EnemySpawnInterval);

			//dung Instantiate lam ton dung luong memory
			//EnemyController enemy= Instantiate(m_EnemyPrefabs, null);
			EnemyController enemy = m_EnemiesPool.spawn(path.WayPoints[0].position, transform);// su dung pool
			enemy.Init(path.WayPoints);
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
}
