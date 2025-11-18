using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GamePlayer : MonoBehaviour
{

	[SerializeField] private GameObject _ballObject;

	[SerializeField] private GameObject _rocketObject;
	[SerializeField] private float _rocketPositionY = 1;


	[Header("UI")]
	[SerializeField] private TextMeshProUGUI _textCombo;
	[SerializeField] private TextMeshProUGUI _textStart;

	[SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private TextMeshProUGUI _scoreTextWinMenu;
	[SerializeField] private TextMeshProUGUI _blicksCountText;

	[SerializeField] private GameObject _menu;
	[SerializeField] private GameObject _menuLose;
	[SerializeField] private GameObject _menuWin;

	[Header("Audio")]
	[SerializeField] private AudioSource _comboSound;
	[SerializeField] private AudioSource _blockSound;
	[SerializeField] private AudioSource _owersSound;
	[Header("AudioClips")]
	[SerializeField] private AudioClip _winSound;
	[SerializeField] private AudioClip _loseSound;
	[SerializeField] private AudioClip _startSound;
	[SerializeField] private AudioClip _startNumsSound;


	private bool _gameIsStopped = false;
	private bool _gameIsDone = false;

	private float _screenHeight;
	private float _screenWidth;


	private int _bricksBroken = 0;
	public int BricksOnStart = 0;

	private int _combo = 0;
	private int _score = 0;

	private int _ballsOnBoard = 1;


	public void Initialize(float screenHeight, float screenWidth)
	{
		_screenHeight = screenHeight;
		_screenWidth = screenWidth;
		_rocketObject.transform.position = new Vector2(0, -_screenHeight / 2 + _rocketPositionY);
		_blicksCountText.text = "Bricks" + _bricksBroken + " / " + BricksOnStart;

		StartCoroutine(StartAnimateNums());
	}

	public void MoveRocket(float pos)
	{
		_rocketObject.transform.position = new Vector2(_screenWidth * pos - (_screenWidth / 2), -_screenHeight / 2 + _rocketPositionY);
	}

	public void InitBall()
	{
		GameObject go = Instantiate(_ballObject, _rocketObject.transform.position + Vector3.up, Quaternion.identity);
		go.GetComponent<Ball>().LaunchBall();
	}


	private void OnEnable()
	{
		StaticActions.BallReflectedByRacket = BallReflectedByRacket;
		StaticActions.BrickBroken = BrickBroken;
		StaticActions.BallLeave = BallLeave;
	}

	private void OnDisable()
	{
		StaticActions.BallReflectedByRacket = null;
		StaticActions.BrickBroken = null;
		StaticActions.BallLeave = null;
	}

	public void BallReflectedByRacket()
	{
		_combo = 0;
	}

	private void BallLeave()
	{
		_ballsOnBoard--;

		if (_ballsOnBoard <= 0)
		{
			_gameIsDone = true;
			_gameIsStopped = true;
			_menuLose.SetActive(true);

			_owersSound.clip = _loseSound;
			_owersSound.Play();
		}
	}

	public void BrickBroken()
	{
		_blockSound.Play();

		_bricksBroken++;
		_combo++;
		ComboAnimate();

		_comboSound.pitch = 0.9f + _combo * 0.1f;
		_comboSound.Play();

		_score += (int)((80 + _combo * 20 ) * StaticActions.CurrentLevelSettings.RewardMod);

		_scoreText.text = "Points " + _score;
		_blicksCountText.text = "Bricks " + _bricksBroken + " / " + BricksOnStart;

		if (_bricksBroken >= BricksOnStart)
		{
			_gameIsDone = true;
			_gameIsStopped = true;
			_menuWin.SetActive(true);

			_scoreTextWinMenu.text = _scoreText.text;

			_owersSound.clip = _winSound;
			_owersSound.Play();
			StaticActions.StopBalls();
		}
	}

	private void ComboAnimate()
	{
		_textCombo.text = "X" + _combo;
		_textCombo.GetComponent<Animator>().SetTrigger("Combo");
	}


	IEnumerator StartAnimateNums()
	{
		for (int i = 3; i > 0; i--)
		{
			_owersSound.clip = _startNumsSound;
			_owersSound.Play();
			_textStart.text = "" + i;
			_textStart.GetComponent<Animator>().SetTrigger("Count");
			yield return new WaitForSeconds(0.75f);
		}
		_owersSound.clip = _startSound;
		_owersSound.Play();
		_textStart.text = "Start!";
		_textStart.GetComponent<Animator>().SetTrigger("Start");
	}

	public void PauseGame()
	{
		if (_gameIsStopped || _gameIsDone) return;

		StaticActions.StopBalls();
		_gameIsStopped = true;
		_menu.SetActive(true);
	}

	public void ResumeGame()
	{
		if (!_gameIsStopped || _gameIsDone) return;

		StaticActions.ResumeBalls();
		_gameIsStopped = false;
		_menu.SetActive(false);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(2);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene(1);
	}
}
