using UnityEngine;

public class ChangeQuality : MonoBehaviour {

	void Start () {
		SetQuality ("HD");
	}

	public static void SetQuality(string suffix)
	{		
		var renderers = FindObjectsOfType<SpriteRenderer>();

		foreach (var renderer in renderers)
		{
			// sprite name without suffix
			var name = renderer.sprite.name.Split('@')[0];
			var sprite = Resources.Load<Sprite>(name + "@" + suffix);

			if (sprite)
				renderer.sprite = sprite;
		}

		Resources.UnloadUnusedAssets();
	}
}
