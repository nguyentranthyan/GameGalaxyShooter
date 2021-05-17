using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerPool
{
	public PowerUp[] PowerUpPrefabs;
	public List<PowerUp> inActiveobjs; //chua cac danh sach enemy da chet hoac chua su dung
	public List<PowerUp> activeobjs; //chua cac danh sach enemy dang su dung

	//spawn cac obj when using
	public PowerUp spawn(Vector3 position, Transform parent)
	{
		if (inActiveobjs.Count == 0)
		{
			int randomPowerUp = Random.Range(0, 3);
			PowerUp newObj = GameObject.Instantiate(PowerUpPrefabs[randomPowerUp], parent);
			newObj.transform.position = position;
			activeobjs.Add(newObj);
			return newObj;
		}
		else
		{
			PowerUp oldObj = inActiveobjs[0];
			oldObj.gameObject.SetActive(true);
			oldObj.transform.SetParent(parent);
			oldObj.transform.position = position;
			activeobjs.Add(oldObj);
			inActiveobjs.RemoveAt(0);
			return oldObj;
		}
	}

	public void release(PowerUp obj)
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
			PowerUp obj = activeobjs[0];
			obj.gameObject.SetActive(false);
			activeobjs.RemoveAt(0);
			inActiveobjs.Add(obj);
		}
	}
}
