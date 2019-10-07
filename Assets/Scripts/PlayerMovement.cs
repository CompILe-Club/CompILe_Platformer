using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Sprite standingSprite;
	public Sprite crouchingSprite;
	public int health;
	public float invincibilityTime;
	private float currentInvincibilityTime;
	[SerializeField] float speed;
	[SerializeField] float maxSpeed;
	[SerializeField] float jumpHeight;
	[SerializeField] float wallJumpPower;
	[SerializeField] float friction;
	[SerializeField] float raycastHeight;
	[SerializeField] float horizontalRaycastLength;
	Vector3 pos;
	Vector2 velocity;
	public float gravity;
	RaycastHit2D findFloor;
	bool grounded;
	bool touchingWall;
	bool crouching;
	bool invincible;


    // Start is called before the first frame update
    void Start()
    {
		pos = transform.position;
		velocity = new Vector2(0,0);
		health = 3;
		invincible = false;	
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

		InvincibilityFrames();
		RenderSprite();

		//Find if grounded
		findFloor = Physics2D.Raycast(transform.position, -Vector2.up, raycastHeight);
		grounded = (findFloor.collider != null);

		velocity.y = (grounded) ? 0 : velocity.y - gravity * Time.deltaTime;

		FindWall();

		GetPlayerInput();

		//Apply velocity changes to position.
		//It is best to make sure that these are always the last lines in the object,
		// as all values must be modified before they are applied to the objects position.
		pos.x += velocity.x;
		pos.y += velocity.y;
		transform.position = pos;

    }

	void GetPlayerInput() {
		//Jumping
		if (Input.GetButtonDown("Jump") && grounded) {
			velocity.y = jumpHeight + Mathf.Abs(velocity.x);
		}else if (Input.GetButtonDown("Jump") && touchingWall) {
			velocity.y = jumpHeight + Mathf.Abs(velocity.x);
			velocity.x *= -wallJumpPower;
		}

		//crouching
		//Most of these numbers are guesses based on how much shorter I think the crouching sprite is
		//It seems to work ok
		if (Input.GetButtonDown("Crouch") && grounded && !crouching) {
			GetComponent<SpriteRenderer>().sprite = crouchingSprite;
			GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y - 2f);
			maxSpeed /= 3;
			velocity.x /= 3;
			raycastHeight -= 1.5f;
			crouching = true;
		}

		if (Input.GetButtonUp("Crouch") && crouching) {
			GetComponent<SpriteRenderer>().sprite = standingSprite;
			GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y + 2f);
			maxSpeed *= 3;
			raycastHeight += 1.5f;
			crouching = false;
		}
	}

	void FindWall() {
		RaycastHit2D findWall;

		int direction = (GetComponent<SpriteRenderer>().flipX) ? 1 : -1;
		//Check if touching a wall
		findWall = Physics2D.Raycast(transform.position, Vector2.right * direction, horizontalRaycastLength);
		touchingWall = (findWall.collider != null);
	}

	void InvincibilityFrames() {
		if (invincible) {
			currentInvincibilityTime += Time.deltaTime;
			if (currentInvincibilityTime > invincibilityTime) {
				invincible = false;
				currentInvincibilityTime = 0f;
			}
		}
	}

	void RenderSprite() {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		//Flip the player according to the last direction pressed.
		//Fix this to not change when no movement is done.
		sr.flipX = (velocity.x > 0);
		sr.color = (invincible) ? new Color(255, 50, 100) : Color.white;
	}

	public void DealDamage(int damage) {
		if (!invincible) {
			health -= damage;
			velocity.x = 0f;
			invincible = true;
		}
	}
		
}