using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour {

	public float pixelsToUnits = 100;

	void Update () {

		GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnits / 2;
	}
}
