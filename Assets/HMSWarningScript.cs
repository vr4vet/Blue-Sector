using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMSWarningScript : MonoBehaviour
{
	public GameObject hmsText;
	public GameObject hmsCanvas;
	public GameObject tpPoint1;
	public GameObject tpPoint2;
	public GameObject tpPoint3;

	// Use this for initialization
	void Start()
	{
		tpPoint1.SetActive(false);
		tpPoint2.SetActive(false);
		tpPoint3.SetActive(false);

	}

	public void deleteHMSWarning()
	{


		tpPoint1.SetActive(true);
		tpPoint2.SetActive(true);
		tpPoint3.SetActive(true);
		Destroy(hmsCanvas);
		Destroy(hmsText);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
