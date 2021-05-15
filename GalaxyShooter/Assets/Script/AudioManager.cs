using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource m_music;
	[SerializeField] private AudioSource m_SFX;

	[SerializeField] private AudioClip m_HomeMusicClip;
	[SerializeField] private AudioClip m_BattleMusicClip;
	[SerializeField] private AudioClip m_LazerSFXClip; //PlayerProjectTile
	[SerializeField] private AudioClip m_PlasmaSFXClip; //EnemyProjectTile
	[SerializeField] private AudioClip m_HitSFXClip;
	[SerializeField] private AudioClip m_ExplosionSFXClip;//playerAndEnemyDestroy


	#region singleton
	[Header("Golbal variables")]
	private static AudioManager m_Isntance;
	public static AudioManager Instance
	{
		get
		{
			if (m_Isntance == null)
				m_Isntance = FindObjectOfType<AudioManager>();
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
	public void PlayHomeMusic()
	{
		if (m_music.clip == m_HomeMusicClip)
			return;
		m_music.loop = true;
		m_music.clip = m_HomeMusicClip;
		m_music.Play();
	}
	public void PlaybattleMusic()
	{
		if (m_music == m_BattleMusicClip)
			return;
		m_music.loop = true;
		m_music.clip = m_BattleMusicClip;
		m_music.Play();
	}
	public void PlayLazerSFXClip()
	{
		m_SFX.PlayOneShot(m_LazerSFXClip);
	}
	public void PlayPlasmaSFXClip()
	{
		m_SFX.PlayOneShot(m_PlasmaSFXClip);
	}
	public void PlayHitSFXClip()
	{
		m_SFX.PlayOneShot(m_HitSFXClip);
	}
	public void PlayExplosionSFXClip()
	{
		m_SFX.PlayOneShot(m_ExplosionSFXClip);
	}
}
