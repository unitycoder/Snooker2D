using UnityEngine;
using System.Collections;

public class Pocket : MonoBehaviour {


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Ball"))
		{
			Destroy(other.gameObject);
		}
	}

}
