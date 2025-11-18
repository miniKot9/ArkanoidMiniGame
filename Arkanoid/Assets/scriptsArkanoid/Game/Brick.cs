using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{

	[Header("Animations")]
	[SerializeField] private float _standartSize = 0.8f;
	[SerializeField] private float _animationSpeed = 2f;
	[SerializeField] private AnimationCurve _animationScaleHit;
	[SerializeField] private AnimationCurve _animationScaleDestroy;
	[SerializeField] private AnimationCurve _animationSpawn;
	[SerializeField] private GameObject _spriteAnimate;

	[Header("Sprites")]
	[SerializeField] private Sprite[] _healthStatusSprites;
	[SerializeField] private Sprite _unbreakebleSprite;


	private int _health = 1;
	private bool _isUnbreakeble = false;

	public void OnStart(int Health, bool IsUnbreakeble, float TimeToSpawn)
	{
		_health = Health;
		_isUnbreakeble = IsUnbreakeble;
		StartCoroutine(SpawnAnimation(TimeToSpawn));

		if (_isUnbreakeble)
			_spriteAnimate.GetComponent<SpriteRenderer>().sprite = _unbreakebleSprite;
		else
			SetTexture();

	}

	private void SetTexture()
	{
		if (_health > _healthStatusSprites.Length)
			_spriteAnimate.GetComponent<SpriteRenderer>().sprite = _healthStatusSprites[_healthStatusSprites.Length];
		else
			_spriteAnimate.GetComponent<SpriteRenderer>().sprite = _healthStatusSprites[_health - 1];
	}

	public void OnHit()
	{
		if (_isUnbreakeble) return;

		_health--;
		StopAllCoroutines();
		_spriteAnimate.transform.localScale = Vector3.one * _standartSize;

		if (_health <= 0)
		{
			StartCoroutine(DestroyBrick());
			
		} else
		{
			StartCoroutine(Animate(_animationScaleHit,_animationSpeed*2));
			SetTexture();
		}
	}

	IEnumerator DestroyBrick()
	{
		GetComponent<Collider2D>().enabled = false;

		StaticActions.BrickBroken();

		yield return StartCoroutine(Animate(_animationScaleDestroy, _animationSpeed));

		gameObject.SetActive(false);
	}

	IEnumerator Animate(AnimationCurve curveScale, float AnimationSpeed)
	{
		for (float i = 0; i < 1; i += Time.deltaTime * AnimationSpeed)
		{
			_spriteAnimate.transform.localScale = Vector3.one * _standartSize * curveScale.Evaluate(i);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator SpawnAnimation(float TimeToSpawn)
	{
		_spriteAnimate.transform.localScale = Vector3.zero;

		yield return new WaitForSeconds(TimeToSpawn);

		GetComponent<Collider2D>().enabled = true;
		StartCoroutine(Animate(_animationSpawn, _animationSpeed));
	}

}
