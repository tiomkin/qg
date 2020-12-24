using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //public Level[] allLevelData;
    public int timeLimitInSeconds;
    public int xpForCorrectAnswer;
    public int[] targetXPToGetLevel;
    public Question[] questions;
}
