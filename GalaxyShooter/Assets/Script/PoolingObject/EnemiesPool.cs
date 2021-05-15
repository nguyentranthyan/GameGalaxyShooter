using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemiesPool
{
	public EnemyController m_EnemyPrefabs;
	public List<EnemyController> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<EnemyController> activeobjs; //chua cac danh sach enemy dang su dung

	//spawn cac obj when using
	public EnemyController spawn(Vector3 position, Transform parent)
	{
		//neu cac danh sach enemy da chet hoac chua su dung = 0=> tao moi
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
	//xoa thay Destroy
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