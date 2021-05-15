using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
	[Header("Enemy's Path variables")]
	[SerializeField] private Transform[] m_WayPoints;
	[SerializeField] private Color m_color;
	public Transform[] WayPoints=> m_WayPoints;
	private bool m_show;
	

	//Ham dung de vẽ path chay tren unity 
	private void OnDrawGizmos()
	{
		if (!m_show) return;
		if(m_WayPoints!=null && m_WayPoints.Length > 1)
		{
			//to mau duong di
			Gizmos.color = m_color;

			for(int i = 0; i < m_WayPoints.Length - 1; i++)
			{
				Transform from = m_WayPoints[i];
				Transform to = m_WayPoints[i + 1];
				Gizmos.DrawLine(from.position, to.position);
			}
			Gizmos.DrawLine(m_WayPoints[0].position, m_WayPoints[m_WayPoints.Length - 1].position);
		}
	}
}
