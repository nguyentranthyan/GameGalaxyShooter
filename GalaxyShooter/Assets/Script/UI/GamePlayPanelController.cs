using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GamePlayPanelController : MonoBehaviour
{
	
	[SerializeField] private TextMeshProUGUI m_txtScore;
	[SerializeField] private Image m_ImgHPBar;
	[SerializeField] private Text m_TextHPBar;

	private void OnEnable()
	{
		GameManager.Instance.onScoreChanged += onScoreChanged;
		SpawnManager.Instance.m_Player.onHPChange += onHPChange;
	}

	private void onHPChange(int curHp, int maxHp)
	{
		m_ImgHPBar.fillAmount = curHp * 1f / maxHp;
		m_TextHPBar.text = curHp.ToString()+"/100";
	}

	private void OnDisable()
	{
		GameManager.Instance.onScoreChanged -= onScoreChanged;
		SpawnManager.Instance.m_Player.onHPChange -= onHPChange;	
	}

	//update score when change
	private void onScoreChanged(int score)
	{
		m_txtScore.text = "SCORE: " + score;
	}

	public void BtnPause_pressed()
	{
		GameManager.Instance.Pause();
	}
}
