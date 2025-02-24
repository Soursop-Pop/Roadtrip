using UnityEngine;
using System.Collections;

[AddComponentMenu("SkythianCat/Hand Painted Nature Kit/WaveAnimation")]

public class WaveAnimation : MonoBehaviour {
	
	public float waveSpeed;
	public Vector2 waveDirection;

	void Update () {
		float dirX = Time.time * waveSpeed * waveDirection.x;
		float dirY = Time.time * waveSpeed * waveDirection.y;
		GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(dirX, dirY));
	}

}
