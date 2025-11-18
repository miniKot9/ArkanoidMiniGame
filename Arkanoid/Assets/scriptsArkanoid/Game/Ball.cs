using UnityEngine;

public class Ball : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float _speed = 5f;
	[SerializeField] private float _maxBounceAngle = 10f;
	[SerializeField] private float _maxBounceAngleRocket = 30f;
	[SerializeField] private ParticleSystem _particlesOnHit;
	[SerializeField] private AudioSource _audio;

	private Rigidbody2D rb;
	private bool isLaunched = false;
	private Vector3 startPosition;


	private int _hitCounterToRandomRotate = 0;

	private Vector2 _directionSaved;

	void Update()
	{
		if (isLaunched && rb.linearVelocity.magnitude != _speed)
		{
			rb.linearVelocity = rb.linearVelocity.normalized * _speed;
		}
	}

	public void LaunchBall()
	{
		rb = GetComponent<Rigidbody2D>();
		startPosition = transform.position;

		isLaunched = true;

		Vector2 direction = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
		rb.linearVelocity = direction * _speed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 newVelocity = AddBounceVariation(rb.linearVelocity.normalized, Random.Range(-_maxBounceAngle, _maxBounceAngle));
		_particlesOnHit.Play();
		_hitCounterToRandomRotate++;

		_audio.pitch = Random.RandomRange(0.9f,1.1f);
		_audio.Play();

		if (collision.gameObject.tag == "Brick")
		{
			collision.gameObject.GetComponent<Brick>().OnHit();
		}

		if (collision.gameObject.tag == "BottomWall")
		{
			StaticActions.BallLeave();
			Destroy(gameObject);
		}

		if (collision.gameObject.tag == "Rocket")
		{
			CalculateReflectionDirection(rb.linearVelocity.normalized, collision);
			_hitCounterToRandomRotate = 0;
			StaticActions.BallReflectedByRacket();
		}

		if (_hitCounterToRandomRotate >= 3)
		{
			newVelocity = AddBounceVariation(rb.linearVelocity.normalized, 45);
			_hitCounterToRandomRotate = 0;
		}

		if (isLaunched)
		{
			rb.linearVelocity = newVelocity * _speed;
		}

		Vector2 AddBounceVariation(Vector2 originalDirection, float BounceAngle)
		{
			float randomAngle = BounceAngle * Mathf.Deg2Rad;

			float cos = Mathf.Cos(randomAngle);
			float sin = Mathf.Sin(randomAngle);

			Vector2 variedDirection = new Vector2(
				originalDirection.x * cos - originalDirection.y * sin,
				originalDirection.x * sin + originalDirection.y * cos
			);

			return variedDirection.normalized;
		}

		Vector2 CalculateReflectionDirection(Vector2 originalDirection, Collision2D collision)
		{
			Vector2 ballCenter = (Vector2)transform.position;

			Vector2 colliderCenter = collision.collider.bounds.center;

			Vector2 toCollider = colliderCenter - ballCenter;

			Vector2 toColliderNormalized = toCollider.normalized;

			float angle = Vector2.SignedAngle(originalDirection, toColliderNormalized);

			float maxDeviationAngle = 30f;
			angle = Mathf.Clamp(angle, -maxDeviationAngle, maxDeviationAngle);

			float angleRad = angle * Mathf.Deg2Rad;
			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);

			Vector2 newDirection = new Vector2(
				originalDirection.x * cos - originalDirection.y * sin,
				originalDirection.x * sin + originalDirection.y * cos
			);

			return newDirection.normalized;
		}
	}


	private void StopBall ()
	{
		_directionSaved = rb.linearVelocity;
		rb.linearVelocity = Vector2.zero;
		isLaunched = false;
	}

	private void ResumeBall ()
	{
		rb.linearVelocity = _directionSaved;
		isLaunched = true;
	}

	private void OnEnable()
	{
		StaticActions.ResumeBalls += ResumeBall;
		StaticActions.StopBalls += StopBall;
	}
	private void OnDisable()
	{
		StaticActions.ResumeBalls -= ResumeBall;
		StaticActions.StopBalls -= StopBall;
	}
}
