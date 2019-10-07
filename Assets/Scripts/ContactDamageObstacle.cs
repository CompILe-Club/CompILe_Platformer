using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageObstacle : MonoBehaviour
{

	public int damage;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerMovement>().DealDamage(damage);
		}
	}
}
