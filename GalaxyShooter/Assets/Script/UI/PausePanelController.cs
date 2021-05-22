using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelController : MonoBehaviour
{
	public void BtnQuit_pressed()
	{
		Application.Quit();
	}
	public void BtnResume_pressed()
	{	
		GameManager.Instance.Resume();
	}
}
