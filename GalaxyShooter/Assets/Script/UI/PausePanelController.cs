using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelController : MonoBehaviour
{
	//private GameManager m_gameManager;
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
	public void BtnResume_pressed()
	{
		//m_gameManager.Resume();
		GameManager.Instance.Resume();
	}
}
