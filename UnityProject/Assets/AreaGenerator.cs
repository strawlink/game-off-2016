using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGenerator : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab = null;

	private void Awake()
	{
		
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1))
		{
			StartCoroutine(StartGeneration());
		}

		if(Input.GetKeyDown(KeyCode.F2))
		{
			foreach (var item in _placedTile)
			{
				Destroy(item.Value);
			}
		}
	}


	// TODO: Update playerPosition from the actual data
	private Vector2 _playerPosition = new Vector2(15, 15);



	private float _radiusBeforeSpawning = 10f;


	private int _width = 30;
	private int _height = 30;

	private Dictionary<Vector2, GameObject> _placedTile = new Dictionary<Vector2, GameObject>();

	private const int TILE_SPAWN_HEIGHT = 30;
	private const float TILE_GROUND_HEIGHT = 0f;

	private IEnumerator StartGeneration()
	{
		yield return null;


		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				// TODO: Object pooling
				Vector2 position = new Vector2(x, y);


				GameObject go = GameObject.Instantiate(_tilePrefab, new Vector3(x, TILE_SPAWN_HEIGHT, y), Quaternion.identity) as GameObject;


				bool alreadyExisted = _placedTile.ContainsKey(position);
				if (alreadyExisted)
				{
					_placedTile[position] = go;
				}
				else
				{
					_placedTile.Add(position, go);
				}

				float distance = Vector2.Distance(position, _playerPosition);
				TileSpawnBehaviour tile = go.GetComponent<TileSpawnBehaviour>();
				tile.StartAnimation(false, position, TILE_SPAWN_HEIGHT, TILE_GROUND_HEIGHT, (_radiusBeforeSpawning - distance) / _radiusBeforeSpawning);

			}
		}



		





	}
}