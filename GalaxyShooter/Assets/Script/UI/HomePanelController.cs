	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomePanelController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_txtHighScore;
	private void OnEnable()
	{
		m_txtHighScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");
	}
	
	public void BtnPlay_Pressed()
	{
		GameManager.Instance.Play();
	}
}
