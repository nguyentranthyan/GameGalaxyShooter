using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
	[SerializeField] private Material m_bigstart;
	[SerializeField] private Material m_medstart;
	[SerializeField] private Material m_nebular;
	[SerializeField] private float m_bigstartScollspeed;
	[SerializeField] private float m_medstartScollspeed;
	[SerializeField] private float m_nebulaScollspeed;

	private int m_main;

    // Start is called before the first frame update
    void Start()
    {
		m_main = Shader.PropertyToID("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 offset = m_bigstart.GetTextureOffset(m_main);
		offset += new Vector2(0, m_bigstartScollspeed * Time.deltaTime);
		m_bigstart.SetTextureOffset(m_main, offset);

		offset = m_medstart.GetTextureOffset(m_main);
		offset += new Vector2(0, m_medstartScollspeed * Time.deltaTime);
		m_medstart.SetTextureOffset(m_main, offset);

		offset = m_nebular.GetTextureOffset(m_main);
		offset += new Vector2(0, m_nebulaScollspeed * Time.deltaTime);
		m_nebular.SetTextureOffset(m_main, offset);
	}
}
