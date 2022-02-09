using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	[SerializeField]
	Text label;


	float count;

	IEnumerator Start()
	{
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				count = (1 / Time.deltaTime);
				label.text = "FPS :" + (Mathf.Round(count));
			}
			else
			{
				label.text = "Pause";
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	
}
