using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
     
    public Text nameText;
    public Text levelText;
    public Text questionText;
    public Text currentXp;
    public Text coinsText;
    public Text livesText;
    public Text hintOneText;
    public Text hintTwoText;
    public Text hintThreeText;
    public Text timerText;
    public GameObject questionPanel;
    public GameObject endRoundPanel;
    public GameObject rightAnswerPanel;
    public GameObject wrongAnswerPanel;
    public SimpleObjectPool answerButtonObjectPool;
    public RectTransform answerButtonParent;
    public ProgressBar radialProgressBar;
    public ProgressBar linearProgressBar;
    public int qtyNonRepeatingQuestions;
    private List<int> questionsAskedIndexes;
    
    private DataController dataController;
    private GameData currentGameData;
    private PlayerInfo playerInfo;
    private Question[] questionPool;
    private int questionsQty;
    private bool isLevelActive;
    private bool isShowedQuestion;
    private bool isAnswerClicked;
    private int timeToAnswer;
    private float timeRemaining;
    private int questionsAsked;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private string playerName;
    private int level;
    private int playerXp;
    private int targetXp;
    private int coins;
    private int lives;
    private int hintOneQty;
    private int hintTwoQty;
    private int hintThreeQty;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentGameData = dataController.GetGameData();
        playerInfo = dataController.GetPlayerInfo();
        questionPool = currentGameData.questions;
        questionsQty = questionPool.Length;
        timeToAnswer = currentGameData.timeLimitInSeconds;
        timeRemaining = currentGameData.timeLimitInSeconds;
        questionsAskedIndexes = new List<int>();
        //Debug.Log("questionsIndexes.Length at start: " + questionsAskedIndexes.Length.ToString());
        //string str = "";
        //for (int i = 0; i < questionsAskedIndexes.Length; i++)
        //{
        //    str += questionsAskedIndexes[i].ToString() + " | ";
        //}
        //Debug.Log("questionsIndexes at start: " + str);


        //questionIndex = 0;
        questionsAsked = 0;
        ShowPlayerInfo();
        UpdateRadialProgressBar();
        UpdateLinearProgressBar();
        ShowQuestion();
        isLevelActive = true;
        
        
    }

    private void UpdateRadialProgressBar()
    {
        radialProgressBar.current = playerXp;
        radialProgressBar.max = targetXp;
        radialProgressBar.UpdateProgressBar();
    }

    private void UpdateLinearProgressBar()
    {
        linearProgressBar.current = (int)Mathf.Round(timeRemaining);
        linearProgressBar.max = timeToAnswer;
        linearProgressBar.UpdateProgressBar();
    }

    private void ShowPlayerInfo()
    {
        if (playerInfo.name == "")
        {
            playerName = "Player" + Random.Range(10000, 99999).ToString();
        }
        else
        {
            playerName = playerInfo.name;
        }
        level = playerInfo.level;
        playerXp = playerInfo.playerXP;
        coins = playerInfo.coins;
        lives = playerInfo.lives;
        hintOneQty = playerInfo.hint1;
        hintTwoQty = playerInfo.hint2;
        hintThreeQty = playerInfo.hint3;

        targetXp = currentGameData.targetXPToGetLevel[level];
        

        nameText.text = playerName;
        levelText.text = level.ToString();
        currentXp.text = playerXp.ToString() + "/" + targetXp.ToString();
        coinsText.text = coins.ToString();
        livesText.text = lives.ToString();
        hintOneText.text = hintOneQty.ToString();
        hintTwoText.text = hintTwoQty.ToString();
        hintThreeText.text = hintThreeQty.ToString();
    }
    
    private void ShowQuestion()
    {
        if (questionPool.Length > questionsAsked)
        {
            questionsAsked ++;
            isAnswerClicked = false;
            ClearPopups();
            timeRemaining = currentGameData.timeLimitInSeconds;
            RemoveAnswerButtons();
            int questionIndex = GetRandomIndex(questionsQty);
            Question questionData = questionPool[questionIndex];
            questionText.text = questionData.questionText;
            if (questionsAsked <= qtyNonRepeatingQuestions)
            {
                questionsAskedIndexes.Add(questionIndex);
            }
            else
            {
                questionsAsked = 0;
                questionsAskedIndexes = new List<int>();
            }
            CreateAnswerButtons(questionData);
        }
        else
        {
            EndRound();
            
        }
        
    }

    private int GetRandomIndex(int questionsQty)
    {
        int questionIndex = Random.Range(0, questionsQty - 1);
        if (!IsAsked(questionIndex))
        {
            Debug.Log("isAsked in IF return");
            return questionIndex;
        }
        else
        {
            while (IsAsked(questionIndex))
            {
                Debug.Log("questionIndex before random: " + questionIndex);
                questionIndex = Random.Range(0, questionsQty - 1);
                Debug.Log("questionIndex after: " + questionIndex);
            }
        }
        //Debug.Log("isAsked after IF return");
        return questionIndex;
    }

    private bool IsAsked(int questionIndex)
    {
        if (questionsAskedIndexes.Count == 0)
        {
            Debug.Log("questionsIndexes.Count: " + questionsAskedIndexes.Count);
            return false;
        }
        else
        {
            for (int i = 0; i < questionsAskedIndexes.Count; i++)
            {
                if (questionsAskedIndexes[i]==questionIndex)
                {
                    Debug.Log($"IsAsked() FOR LOOP questionAsked Indexes = {questionsAskedIndexes[i]}; questionIndex={questionIndex} ");
                    return true;
                }
            }
        }
        return false;
    }

    private void CreateAnswerButtons(Question questionData)
    {
        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
    }
    public void AnswerButtonClicked(bool isCorrect)
    {
        isAnswerClicked = true;
        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
            playerXp += currentGameData.xpForCorrectAnswer;
            if (playerXp >= targetXp)
            {
                level += 1;
                playerXp = 0;
                targetXp = currentGameData.targetXPToGetLevel[level];
                levelText.text = level.ToString();
            }
            
            currentXp.text = playerXp.ToString() + "/" + targetXp.ToString();
            
        }
        else
        {
            ShowWrongAnswerPopup();
        }

        Invoke("ShowQuestion", 2);

    }

    private void ShowCorrectAnswerPopup()
    {
        rightAnswerPanel.SetActive(true);
    }

    private void ShowWrongAnswerPopup()
    {
        wrongAnswerPanel.SetActive(true);
    }

    private void ClearPopups()
    {
        rightAnswerPanel.SetActive(false);
        wrongAnswerPanel.SetActive(false);
    }

    public void EndRound()
    {
        isLevelActive = false;
        SubmitPlayerInfo();
        questionPanel.SetActive(false);
        endRoundPanel.SetActive(true);
        string indexes = "";
        for (int i = 0; i < questionsAskedIndexes.Count; i++)
        {
            indexes += $"{questionsAskedIndexes[i].ToString()} |";
        }
        Debug.Log(indexes);
    }

    private void SubmitPlayerInfo()
    {
        playerInfo.name = playerName;
        playerInfo.level = level;
        playerInfo.playerXP = playerXp;
        playerInfo.coins = coins;
        playerInfo.lives = lives;
        playerInfo.hint1 = hintOneQty;
        playerInfo.hint2 = hintTwoQty;
        playerInfo.hint3 = hintThreeQty;
        dataController.SubmitPlayerInfo(playerInfo);
    }

    public void ReturnToMenu()
    {
        //dataController.SubmitPlayerInfo(playerXp);
        SceneManager.LoadScene("Main");
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.Round(timeRemaining).ToString();
    }    
    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelActive & !isAnswerClicked)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
            UpdateLinearProgressBar();
            if (timeRemaining <= 0f)
            {
                EndRound();
            }
        }
        UpdateRadialProgressBar();
        
    }
}
