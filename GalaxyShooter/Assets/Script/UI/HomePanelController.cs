	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomePanelController : MonoBehaviour
{
	//private GameManager m_gameManager;
	[SerializeField] private TextMeshProUGUI m_txtHighScore;
	private void OnEnable()
	{
		m_txtHighScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");
	}
	// Start is called before the first frame update
	private void Start()
	{
		//m_gameManager = FindObjectOfType<GameManager>();
	}

	public void BtnPlay_Pressed()
	{
		//m_gameManager.Play();
		GameManager.Instance.Play();
	}
}
