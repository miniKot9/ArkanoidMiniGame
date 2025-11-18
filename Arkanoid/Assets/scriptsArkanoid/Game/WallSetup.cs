using UnityEngine;

public class WallSetup : MonoBehaviour
{
	[Header("Walls")]
	[SerializeField] private GameObject _leftWall;
	[SerializeField] private GameObject _rightWall;
	[SerializeField] private GameObject _topWall;
	[SerializeField] private GameObject _buttomWall;

	[Header("Params")]
	[SerializeField] private float _borderScale = 1;
	[SerializeField] private float _upperBorderScale = 1;

	public void SetupWalls(float screenHeight, float screenWidth)
	{
		_leftWall.transform.position = new Vector3(-screenWidth / 2, 0, 0);
		_leftWall.transform.localScale = new Vector3(_borderScale, screenHeight, _borderScale);

		_rightWall.transform.position = new Vector3(screenWidth / 2, 0, 0);
		_rightWall.transform.localScale = new Vector3(_borderScale, screenHeight, _borderScale);

		_topWall.transform.position = new Vector3(0, screenHeight / 2, 0);
		_topWall.transform.localScale = new Vector3(screenWidth, _upperBorderScale, _upperBorderScale);

		_buttomWall.transform.position = new Vector3(0, -screenHeight / 2, 0);
		_buttomWall.transform.localScale = new Vector3(screenWidth, _upperBorderScale, _upperBorderScale);
	}

}
