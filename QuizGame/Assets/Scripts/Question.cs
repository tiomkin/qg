using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public int id;
    public string questionText;
    public Answer[] answers;
    public string questionCategory;
    public int difficulty;
    public int timesAsked;
}
