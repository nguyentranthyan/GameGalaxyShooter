using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanelController : MonoBehaviour
{
	//private GameManager m_gameManager;

	[SerializeField] private TextMeshProUGUI m_textResult;
	[SerializeField] private TextMeshProUGUI m_txtHighScore;

	// Start is called before the first frame update
	void Start()
    {
		//m_gameManager = FindObjectOfType<GameManager>();
	}

	public void BtnHome_pressed()
	{
		//m_gameManager.Home();
		GameManager.Instance.Home();
	}

	public void DisplayHighScore(int score)
	{
		m_txtHighScore.text = "HIGH SCORE: " + score;
	}

	public void DisplayResult(bool isWin)
	{
		if (isWin)
			m_textResult.text = "YOU WIN";
		else
			m_textResult.text = "YOU LOSE";
	}
}
