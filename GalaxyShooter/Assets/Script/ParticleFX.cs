using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
	[SerializeField] private float m_lifeTime;
	private float m_currentLifeTime;
	private ParticleFXcontroller m_pool;

	private void OnEnable()
	{
		m_currentLifeTime = m_lifeTime;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (m_currentLifeTime <= 0)
		{
			m_pool.release(this);
		}
		m_currentLifeTime -= Time.deltaTime;
    }

	public void Setpool(ParticleFXcontroller pool)
	{
		m_pool = pool;
	}
}
