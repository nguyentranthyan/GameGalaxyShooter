using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
	[SerializeField] private Transform[] m_WayPoints;
	public Transform[] WayPoints=> m_WayPoints;
	[SerializeField] private Color m_color;
	//ham dung de vẽ path chay tren unity 
	private void OnDrawGizmos()
	{
		if(m_WayPoints!=null && m_WayPoints.Length > 1)
		{
			Gizmos.color = m_color;//to mau duong di

			for(int i = 0; i < m_WayPoints.Length - 1; i++)
			{
				Gizmos.DrawLine(m_WayPoints[i].position, m_WayPoints[i + 1].position);
			}
			Gizmos.DrawLine(m_WayPoints[0].position, m_WayPoints[m_WayPoints.Length - 1].position);
		}
	}
}
