using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	float speed = 0.25f;
	float ratio = 0;
	
	public void StartFade()
	{
		ratio=0;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Renderer>().material.color = Color.white;
		
		StartCoroutine("FadeOut");
		
	}
	
	IEnumerator FadeOut()
	{
		
		while(ratio < 1)
		{
			ratio += Time.deltaTime * speed;
			GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.clear,ratio);
			yield return null;
		}
		GetComponent<Renderer>().enabled = false;
	}
}
