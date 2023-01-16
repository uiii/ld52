using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvesterController : MonoBehaviour
{
	public GameController game;
	public Stats stats;

	[Header("Harvester settings")]
	public float driftFactor = 0.9f;
	public float accelerationFactor = 10.0f;
	public float breakFactor = 5.0f;
	public float turnFactor = 30f;
	public float maxSpeed = 2f;

	public Tilemap field;

	public float accelerationInput = 0;
	public float breakInput = 0;
	public float steeringInput = 0;

	public float rotationAngle = 0;

	public float velocityVsUp = 0;

	Rigidbody2D rigidBody;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	void Update()
	{
		accelerationInput = game.controls.Harvester.Accelerate.ReadValue<float>();
		breakInput = game.controls.Harvester.Break.ReadValue<float>();
		//steeringInput = game.controls.Harvester.Steer.ReadValue<Vector2>().x;
		steeringInput = game.controls.Harvester.SteerRight.ReadValue<float>() - game.controls.Harvester.SteerLeft.ReadValue<float>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		ApplyEngineForce();

		KillOrthogonalVelocity();

		ApplySteering();
	}

	void ApplyEngineForce()
	{
		velocityVsUp = Vector2.Dot(transform.up, rigidBody.velocity);

		if (velocityVsUp > maxSpeed && accelerationInput > 0) {
			return;
		}

		if (velocityVsUp < -maxSpeed && breakInput > 0) {
			return;
		}

		/*if (rigidBody.velocity.sqrMagnitude > maxSpeed * maxSpeed && (accelerationInput > 0)) {
			return;
		}*/

		if (accelerationInput == 0 && breakInput == 0) {
			rigidBody.drag = Mathf.Lerp(rigidBody.drag, 3.0f, Time.fixedDeltaTime * 3);
		} else {
			rigidBody.drag = 0;
		}

		Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
		Vector2 reverseVector = -transform.up * breakInput * breakFactor;

		rigidBody.AddForce(engineForceVector, ForceMode2D.Force);
		rigidBody.AddForce(reverseVector, ForceMode2D.Force);
	}

	void ApplySteering()
	{
		float minSpeedBeforeAllowTurningFactor = (rigidBody.velocity.magnitude / 8);
		minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

		rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

		rigidBody.MoveRotation(rotationAngle);
	}

	void KillOrthogonalVelocity()
	{
		Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidBody.velocity, transform.up);
		Vector2 rightVelocity = transform.right * Vector2.Dot(rigidBody.velocity, transform.right);

		rigidBody.velocity = forwardVelocity + rightVelocity * driftFactor;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var crop = other.GetComponentInParent<CropController>();

		if (crop != null && velocityVsUp > 0) {
			if (crop.isBurning) {
				stats.DecreaseHealth(2);
			}

			crop.Crop();
		}
	}
}
