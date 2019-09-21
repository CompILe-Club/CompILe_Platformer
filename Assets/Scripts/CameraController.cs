using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

{
	public GameObject player;
	public float camSpeedX;
	public float camSpeedY;

	Vector2 velocity;


	void FixedUpdate() {

		float camPosX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, camSpeedX);
		float camPosY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + 10, ref velocity.y, camSpeedY);

		transform.position = new Vector3(camPosX, camPosY, -1);
	}

}
