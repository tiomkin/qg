using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    //public string name;
    public int timeLimitInSeconds;
    public int xpForCorrectAnswer;
    public Question[] questions;
}
