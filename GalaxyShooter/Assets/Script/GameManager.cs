using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tao enum quan ly state machine cua game
public enum GameState
{
	Home,
	GamePlay,
	Pause,
	GameOver
}

public class GameManager : MonoBehaviour
{
	//using singleton
	private static GameManager m_Isntance;//global variable
	public static GameManager Instance
	{
		get
		{
			if (m_Isntance == null)
				m_Isntance = FindObjectOfType<GameManager>();
			return m_Isntance;
		}
	}
	//private SpawnManager m_SpawnManager;
	private AudioManager m_AudioManager;
	private GameState m_gameState;

	//lien ket den cac Script Controller UI
	[SerializeField] private HomePanelController m_Home;
	[SerializeField] private GamePlayPanelController m_GamePlay;
	[SerializeField] private PausePanelController m_Pause;
	[SerializeField] private GameOverPanelController m_GameOver;
	[SerializeField] private WaveData[] m_Waves;

	private bool m_win;//check win
	private int m_score; //score final when you play
	private int m_curWaveIndex;

	private void Awake()
	{
		if (m_Isntance == null)
			m_Isntance = this;
		else if (m_Isntance != this)
			Destroy(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {
		m_AudioManager = FindObjectOfType<AudioManager>();
		//m_SpawnManager = FindObjectOfType<SpawnManager>();
		m_Home.gameObject.SetActive(false);
		m_GamePlay.gameObject.SetActive(false);
		m_Pause.gameObject.SetActive(false);
		m_GameOver.gameObject.SetActive(false);
		SetState(GameState.Home);
	}

	//setState game show 
	private void SetState(GameState state)
	{
		m_gameState = state;
		m_Home.gameObject.SetActive(m_gameState == GameState.Home);
		m_GamePlay.gameObject.SetActive(m_gameState == GameState.GamePlay);
		m_Pause.gameObject.SetActive(m_gameState == GameState.Pause);
		m_GameOver.gameObject.SetActive(m_gameState == GameState.GameOver);

		if (m_gameState == GameState.Pause)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

		if (m_gameState == GameState.Home)
			m_AudioManager.PlayHomeMusic();
		else
			m_AudioManager.PlaybattleMusic();
	}

	//using Observers giup thong bao score thay doi
	public Action<int> onScoreChanged;

	public void AddScore(int value)
	{
		m_score += value;
		if (onScoreChanged != null)
			onScoreChanged(m_score);
		//m_GamePlay.displayScore(m_score);
		if (SpawnManager.Instance.isClear())
		{
			m_curWaveIndex++;
			if(m_curWaveIndex >= m_Waves.Length)
				Gameover(true);//You win
			else
			{
				WaveData wave = m_Waves[m_curWaveIndex];
				SpawnManager.Instance.StartBatter(wave,false);
			}
		}
	}
	
	//kiem tra trang thai Active khi player di chuyen
	public bool isActive()
	{
		return m_gameState == GameState.GamePlay;
	}

	public void Play()
	{
		m_curWaveIndex = 0;
		WaveData wave = m_Waves[m_curWaveIndex];
		SpawnManager.Instance.StartBatter(wave,true);
		SetState(GameState.GamePlay);
		m_score = 0;
		if (onScoreChanged != null)
			onScoreChanged(m_score);
		//m_GamePlay.displayScore(m_score);
		
	}
	public void Pause()
	{
		SetState(GameState.Pause);
	}
	public void Home()
	{
		SetState(GameState.Home);
		SpawnManager.Instance.Clear();
	}
	public void Resume()
	{
		SetState(GameState.GamePlay);
	}

	public void Gameover(bool win)
	{
		int curHighScore = PlayerPrefs.GetInt("HighScore");
		if (curHighScore < m_score)
		{
			PlayerPrefs.SetInt("HighScore", m_score);
			curHighScore = m_score;
		}
		m_win = win;
		SetState(GameState.GameOver);
		m_GameOver.DisplayResult(m_win);
		m_GameOver.DisplayHighScore(curHighScore);
	}
}
 