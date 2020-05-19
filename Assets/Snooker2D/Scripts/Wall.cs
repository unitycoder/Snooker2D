using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	float hitVelocity = 0;
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag("Ball"))
		{
			hitVelocity = other.relativeVelocity.sqrMagnitude;
			
			//Debug.Log("hitvel:"+hitVelocity);
			if (hitVelocity>1)
			{
				//audio.volume = Mathf.Clamp01(100/Mathf.Clamp(hitVelocity,0,100));
				//Debug.Log ("volume:"+audio.volume);
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
