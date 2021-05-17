using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelController : MonoBehaviour
{
	public void BtnQuit_pressed()
	{
		GameManager.Instance.Home();
	}
	public void BtnResume_pressed()
	{	
		GameManager.Instance.Resume();
	}
}
