using System.Collections;
using UnityEngine;

public class TileSpawnBehaviour : MonoBehaviour
{
	private void Awake()
	{
		
	}

	private float _speed;

	public void StartAnimation(bool instant, Vector2 position, float startHeight, float endHeight, float distanceFromPlayer)
	{
		startHeight += Random.Range(2f, 4f);
		_speed = Random.Range(0.65f, 1f) + distanceFromPlayer;	

		Vector3 newPos = new Vector3(position.x, endHeight, position.y);

		if(instant)
		{
			transform.position = newPos;
		}
		else
		{
			StartCoroutine(Animate(endHeight, newPos));
		}
	}

	private IEnumerator Animate(float endHeight, Vector3 newPos)
	{
		while (transform.position.y > endHeight + 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * _speed);
			_speed += Random.Range(0.6f, 1.0f) * Time.deltaTime;
			//_speed += Random.Range(1f, 10f) * Time.deltaTime;
			yield return null;
		}

		transform.position = newPos;
		StartCoroutine(AnimateScale());

	}

	[SerializeField] private AnimationCurve _scaleCurve;
	private const float SCALE_SPEED = 3f;

	private IEnumerator AnimateScale()
	{
		float scale = 0;
		while (scale < 1f)
		{
			scale += Time.deltaTime * SCALE_SPEED;
			float eval = _scaleCurve.Evaluate(scale);
			transform.localScale = new Vector3(	eval,
												transform.localScale.y,
												eval);
			yield return null;
		}
	}
}