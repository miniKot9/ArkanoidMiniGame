using System;
using UnityEngine;

public static class StaticActions
{
	public static Action BrickBroken;
	public static Action BallReflectedByRacket;
	public static Action BallLeave;

	public static Action PauseGame;
	public static Action ResumeGame;

	public static Action StopBalls;
	public static Action ResumeBalls;

	public static LevelsSettings CurrentLevelSettings = new LevelsSettings
	{
		MaxBricksUnbreakeble = 0,
		MaxHealthBricks = 1,
		RewardMod = 1
	};

}
