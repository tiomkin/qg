using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Text answerText;
    public Text hintText;
    public GameObject hintImage;
    private Answer _answerData;
    private GameController _gc;

    //private void Awake()
    //{
    //    gameController = FindObjectOfType<GameController>();
    //}

    // Start is called before the first frame update
    void Start()
    {
        _gc = FindObjectOfType<GameController>();
    }

    public Answer GetAnswerData()
    {
        return _answerData;
    }
    public void Setup(Answer data)
    {
        _answerData = data;
        answerText.text = _answerData.answerText;

    }

    public void HandleClick()
    {
        Debug.Log("HandleClick");
        _gc.AnswerButtonClicked(this, _answerData, _answerData.isCorrect);
    }

}