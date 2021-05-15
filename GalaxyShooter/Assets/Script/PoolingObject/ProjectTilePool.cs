using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
