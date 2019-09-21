using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] float maxSpeed;
	[SerializeField] float jumpHeight;
	[SerializeField] float friction;
	[SerializeField] float raycastHeight;
	Vector3 pos;
	Vector2 velocity;
	public float gravity;
	RaycastHit2D findFloor;
	bool grounded;


    // Start is called before the first frame update
    void Start()
    {
		pos = transform.position;
		velocity = new Vector2(0,0);
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
		pos = transform.position;
		float direction = Input.GetAxis("Horizontal") * speed;

		if (velocity.x < 0.005 && velocity.x > -0.005) {
			velocity.x = 0;
		}

		//Apply direction to the velocity
		velocity.x += direction * Time.deltaTime;
		velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

		//Slows down and then stops the player.
		velocity.x *= friction;

		//Flip the player according to the last direction pressed.
		//Fix this to not change when no movement is done.
		GetComponent<SpriteRenderer>().flipX = (direction > 0);

		//Find if grounded
		findFloor = Physics2D.Raycast(transform.position, -Vector2.up, raycastHeight);
		grounded = (findFloor.collider != null);

		velocity.y = (grounded) ? 0 : velocity.y - gravity * Time.deltaTime;

		//Jumping
		if (Input.GetButtonDown("Jump") && grounded) {
			velocity.y = jumpHeight + Mathf.Abs(velocity.x);
		}

		//Apply velocity changes to position.
		pos.x += velocity.x;
		pos.y += velocity.y;
		transform.position = pos;

    }
		
}
