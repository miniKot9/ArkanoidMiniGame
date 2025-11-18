using UnityEngine;
using System.Collections;

public class MainBoard : MonoBehaviour
{

	[Header("Scripts")]
	[SerializeField] private WallSetup _wallSetup;
	[SerializeField] private GamePlayer _gamePlayer;

	[Header("Bricks")]
	[SerializeField] private GameObject _brickObject;

	[SerializeField] private float _verticalOffset = 1f;
	[SerializeField] private float _leftRightOffset = 0.2f;

	[SerializeField] private int _brickObjectCountWidth = 8;
	[SerializeField] private int _brickObjectCountHeigth = 5;


	private void Awake()
	{
		Camera mainCamera = Camera.main;
		float screenHeight = 2f * mainCamera.orthographicSize;
		float screenWidth = screenHeight * mainCamera.aspect;

		_wallSetup.SetupWalls(screenHeight, screenWidth);

		GenerateRandomLevel(screenHeight, screenWidth);

		_gamePlayer.Initialize(screenHeight, screenWidth);
	}

	private void Start()
	{
		StartCoroutine(StartGame());
	}

	public void GenerateRandomLevel (float screenHeight, float screenWidth)
	{

		float availableWidth = screenWidth - _leftRightOffset;
		float brickWidth = (availableWidth ) / _brickObjectCountWidth;
		float brickHeight = brickWidth * 0.5f; 

		float startX = -screenWidth / 2 + brickWidth / 2 + _leftRightOffset / 2;
		float startY = screenHeight / 2 - brickHeight / 2 - _verticalOffset;

		int unbreakebleBlocks = 0;

		for (int x = 0; x < _brickObjectCountWidth; x++)
		{
			for (int y = 0; y < _brickObjectCountHeigth; y++)
			{
				bool isUnbreakeble = false;
				if (unbreakebleBlocks < StaticActions.CurrentLevelSettings.MaxBricksUnbreakeble && 
					(x+y* _brickObjectCountWidth) % (_brickObjectCountWidth) == unbreakebleBlocks % (_brickObjectCountWidth) && y > 2)
				{
					unbreakebleBlocks++;
					isUnbreakeble = true;
				}

				float posX = startX + x * (brickWidth);
				float posY = startY - y * (brickWidth);

				Vector3 spawnPosition = new Vector3(posX, posY, 0);

				GameObject brick = Instantiate(_brickObject, spawnPosition, Quaternion.identity,transform);
				brick.transform.localScale = new Vector3(brickWidth, brickWidth, 1f);

				float BrickCount = x + y * _brickObjectCountHeigth;
				float BrickTimeFade = 1.5f / (_brickObjectCountWidth * _brickObjectCountHeigth);
				brick.GetComponent<Brick>().OnStart(Random.RandomRange(1, StaticActions.CurrentLevelSettings.MaxHealthBricks+1), isUnbreakeble, BrickCount* BrickTimeFade);
			}
		}

		_gamePlayer.BricksOnStart = _brickObjectCountWidth* _brickObjectCountHeigth- unbreakebleBlocks;
	}

	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(3);

		_gamePlayer.InitBall(); 
	}
}
