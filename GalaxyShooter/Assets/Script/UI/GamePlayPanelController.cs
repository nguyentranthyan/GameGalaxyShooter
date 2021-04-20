using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GamePlayPanelController : MonoBehaviour
{
	//private GameManager m_gameManager;
	//private SpawnManager m_SpawnManager;
	[SerializeField] private TextMeshProUGUI m_txtScore;
	[SerializeField] private Image m_ImgHPBar;

	// Start is called before the first frame update
	void Start()
    {
		//m_gameManager = FindObjectOfType<GameManager>();
		//m_SpawnManager = FindObjectOfType<SpawnManager>();
	}

	private void OnEnable()
	{
		GameManager.Instance.onScoreChanged += onScoreChanged;
		SpawnManager.Instance.Player.onHPChange += onHPChange;
	}

	private void onHPChange(int curHp, int maxHp)
	{
		m_ImgHPBar.fillAmount = curHp * 1f / maxHp;
	}

	private void OnDisable()
	{
		GameManager.Instance.onScoreChanged += onScoreChanged;
		SpawnManager.Instance.Player.onHPChange += onHPChange;	
	}

	//update score when change
	private void onScoreChanged(int score)
	{
		m_txtScore.text = "SCORE: " + score;
	}

	//public void displayScore(int score)
	//{
	//	m_txtScore.text = "SCORE: " + score;
	//}

	public void BtnPause_pressed()
	{
		//m_gameManager.Pause();
		GameManager.Instance.Pause();
	}

	

}
