using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Declaring Fields 
    public Text nameText;
    public Text levelText;
    public Text questionText;
    public Text currentXp;
    public Text coinsText;
    public Text livesText;
    public Text hint1TextActive;
    public Text hint2TextActive;
    public Text hint3TextActive;
    public Text hint1TextInActive;
    public Text hint2TextInActive;
    public Text hint3TextInActive;
    public Text timerText;
    public GameObject questionPanel;
    public GameObject endRoundPanel;
    //public GameObject answersPanel;
    public GameObject rightAnswerPanel;
    public GameObject wrongAnswerPanel;
    public GameObject chestsPanel;
    public RectTransform chestsParent;
    public GameObject[] activePanels;
    public GameObject chest;
    public HintPlayersHelp hbPlayersHelp;
    public HintDeleteWrong hbDeleteWrong;
    public HintAddTime hbAddTime;
    public SimpleObjectPool answerButtonObjectPool;
    public SimpleObjectPool chestObjectPool;
    public RectTransform answerButtonParent;
    public ProgressBar radialProgressBar;
    public ProgressBar linearProgressBar;
    public int qtyNonRepeatingQuestions;
    public int qtyAnswersToShowChests;
    public int qtyOfShowingChests;
    private List<int> questionsAskedIndexes;

    private DataController dc;
    private GameData currentGameData;
    private PlayerInfo playerInfo;
    private Question[] questionPool;
    private Question currentQuestion;
    public List<AnswerButton> answerButtonsList;
    private int questionsQty;
    private bool isLevelActive;
    private bool isShowedQuestion;
    private bool isAnswerClicked;
    private int timeToAnswer;
    private float timeRemaining;
    private int questionsAsked;
    private int correctAnswersCounter;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private List<GameObject> chestGameObjects = new List<GameObject>();
    private List<Chest> chestsList = new List<Chest>();
    private string playerName;
    private int level;
    private int playerXp;
    private int targetXp;
    private int coins;
    private int lives;

    enum Prize { None, Gold, Hint, Live};
   
    //private HintButton hintButton1, hintButton2, hintButton3;
    private List<HintButton> hintButtons;
    #endregion
    private void Awake()
    {
        dc = FindObjectOfType<DataController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameController script starts");
        isLevelActive = true;
        if (Time.timeScale != 1) Time.timeScale = 1;
        //isAnswerClicked = true;
        currentGameData = dc.GetGameData();
        playerInfo = dc.GetPlayerInfo();
        questionPool = currentGameData.questions;
        questionsQty = questionPool.Length;
        timeToAnswer = currentGameData.timeLimitInSeconds;
        timeRemaining = currentGameData.timeLimitInSeconds;

        Debug.Log("Start() timeRemaining: " + timeRemaining.ToString());
        questionsAskedIndexes = new List<int>();
        answerButtonsList = new List<AnswerButton>();
        //variable to count questions to perform row of non-repeating questions; 
        questionsAsked = 0;
        //variable to count correct answers for showing chests to win a prize;
        correctAnswersCounter = 0;

        ShowPlayerInfo();
        HideHintsPrice();
        UpdateRadialProgressBar();
        UpdateLinearProgressBar();
        ShowQuestion();




    }

    private void HideHintsPrice()
    {
        hbPlayersHelp.HideHintPrice();
        hbDeleteWrong.HideHintPrice();
        hbAddTime.HideHintPrice();
    }

    #region Updating Progress Bars and Timer
    private void UpdateRadialProgressBar()
    {
        radialProgressBar.current = playerXp;
        radialProgressBar.max = targetXp;
        radialProgressBar.UpdateProgressBar();
    }

    private void UpdateLinearProgressBar()
    {
        //Debug.Log("UpdateLinearProgressBar() timeRemaining: " + timeRemaining.ToString());
        linearProgressBar.current = (int)Mathf.Round(timeRemaining);

        //Debug.Log("UpdateLinearProgressBar() current: " + linearProgressBar.current.ToString());
        linearProgressBar.max = timeToAnswer;
        //Debug.Log("UpdateLinearProgressBar() max: " + linearProgressBar.max.ToString());
        linearProgressBar.UpdateProgressBar();
        //Debug.Log("UpdateLinearProgressBar finished");
    }

    IEnumerator linearProgressBarRoutine()
    {
        while (timeRemaining > 0 && !isAnswerClicked)
        {
            //Debug.Log("*********************OnCoroutine first updating *********************** ");
            UpdateTimerText();
            UpdateLinearProgressBar();
            //Debug.Log("OnCoroutine timeRemaining before wait: " + timeRemaining);
            yield return new WaitForSeconds(1.0f);
            if (!isAnswerClicked)
            {
                timeRemaining -= 1.0f;
                UpdateTimerText();
                UpdateLinearProgressBar();
                //Debug.Log("OnCoroutine timeRemaining: " + timeRemaining);
            }
        }
        if (timeRemaining <= 0.0f) EndRound();
    }

    private void UpdateTimerText()
    {

        //timerText.text = Mathf.Round(timeRemaining).ToString();
        timerText.text = timeRemaining.ToString();
    }

    #endregion

    #region PlayerInfo and GameData
    private void ShowPlayerInfo()
    {
        UpdateHintsQtyUI();
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
        hbPlayersHelp.HintQty = playerInfo.hintPlayersHelp;
        hbDeleteWrong.HintQty = playerInfo.hintDeleteWrong;
        hbAddTime.HintQty = playerInfo.hintAddTime;
        //Debug.Log("Hint class name: " + hintButton1.name);

        targetXp = currentGameData.targetXPToGetLevel[level];
        

        nameText.text = playerName;
        levelText.text = level.ToString();
        currentXp.text = playerXp.ToString() + "/" + targetXp.ToString();
        coinsText.text = coins.ToString();
        livesText.text = lives.ToString();
        hint1TextActive.text = hbPlayersHelp.HintQty.ToString();
        hint2TextActive.text = hbDeleteWrong.HintQty.ToString();
        hint3TextActive.text = hbAddTime.HintQty.ToString();
    }

    private void SubmitPlayerInfo()
    {
        playerInfo.name = playerName;
        playerInfo.level = level;
        playerInfo.playerXP = playerXp;
        playerInfo.coins = coins;
        playerInfo.lives = lives;
        playerInfo.hintPlayersHelp = hbPlayersHelp.HintQty;
        playerInfo.hintDeleteWrong = hbDeleteWrong.HintQty;
        playerInfo.hintAddTime = hbAddTime.HintQty;
        dc.SubmitPlayerInfo(playerInfo);
    }

    private void SubmitGameData()
    {
        dc.SubmitGameData(questionPool);
    }

    #endregion

    #region Question Methods
    private void ShowQuestion()
    {
        if (questionPool.Length > questionsAsked)
        {
            questionsAsked ++;
            isAnswerClicked = false;
            ClearPopups();
            timeRemaining = currentGameData.timeLimitInSeconds;
            HintPlayersHelpResetUI();
            HintAddTimeReset();
            RemoveAnswerButtons();
            int questionIndex = GetRandomIndex(questionsQty);
            Question questionData = questionPool[questionIndex];
            currentQuestion = questionData;
            questionText.text = questionData.questionText;
            //questionPool[questionIndex].timesAsked += 1;
            if (questionsAsked <= qtyNonRepeatingQuestions)
            {
                questionsAskedIndexes.Add(questionIndex);
            }
            else
            {
                questionsAskedIndexes.RemoveAt(0);
                questionsAskedIndexes.Add(questionIndex);
                questionsAsked--;
            }
            CreateAnswerButtons(questionData);
            StartCoroutine(linearProgressBarRoutine());
            //while (timeRemaining > 0)
            //{
            //    StartCoroutine(linearProgressBarRoutine());
            //}
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
            return questionIndex;
        }
        else
        {
            while (IsAsked(questionIndex))
            {
                questionIndex = Random.Range(0, questionsQty - 1);
            }
        }
        return questionIndex;
    }

    private bool IsAsked(int questionIndex)
    {
        if (questionsAskedIndexes.Count == 0)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < questionsAskedIndexes.Count; i++)
            {
                if (questionsAskedIndexes[i]==questionIndex)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    #region Answer Buttons
    private void CreateAnswerButtons(Question questionData)
    {
        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
            answerButtonsList.Add(answerButton);
        }
    }

    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
            
        }
        while (answerButtonsList.Count > 0)
        { 
            answerButtonsList.RemoveAt(0);
        }
    }
    public void AnswerButtonClicked(AnswerButton btn, Answer answer, bool isCorrect)
    {
        Debug.Log("AnswerButtonClicked");
        isAnswerClicked = true;
        //answer.timesAnswered += 1;
        StopCoroutine(linearProgressBarRoutine());
        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
            playerXp += currentGameData.xpForCorrectAnswer;
            coins += currentGameData.coinsForCorrectAnswer;
            if (playerXp >= targetXp)
            {
                level += 1;
                playerXp = 0;
                targetXp = currentGameData.targetXPToGetLevel[level];
                levelText.text = level.ToString();
            }
            UpdateRadialProgressBar();
            currentXp.text = playerXp.ToString() + "/" + targetXp.ToString();
            coinsText.text = coins.ToString();
            correctAnswersCounter++;
            if (correctAnswersCounter == qtyAnswersToShowChests)
            {
                ShowChests();
            }

        }
        else
        {
            ShowWrongAnswerPopup();
            MinusLive();
        }

        Invoke("ShowQuestion", 2);

    }

    private void MinusLive()
    {
        if (lives > 0) lives -= 1;
        livesText.text = lives.ToString();
    }
    #endregion

    #region Hints
    public void HintButtonClicked(HintButton hb)
    {
        Debug.Log("HintButtonClicked object name: " + hb.name);
        hb.UseHint();
    }

    public void UpdateHintsQtyUI()
    {
        hint1TextActive.text = hbPlayersHelp.HintQty.ToString();
        hint2TextActive.text = hbDeleteWrong.HintQty.ToString();
        hint3TextActive.text = hbAddTime.HintQty.ToString();
    }

    public void HintPlayersHelp()
    {
        hbPlayersHelp.GetComponent<Button>().interactable = false;
        foreach (AnswerButton answer in answerButtonsList)
        {

            float procent = ((float)answer.GetAnswerData().timesAnswered / (float)currentQuestion.timesAsked) * 100;
            Debug.Log("Answer answere times: " + answer.GetAnswerData().timesAnswered);
            Debug.Log("Question asked: " + currentQuestion.timesAsked);
            answer.hintImage.SetActive(true);
            answer.hintText.text = Mathf.Round(procent).ToString() + "%";
        }
    }

    public void HintDeleteWrong()
    {
        int i = 0;
        while (i < answerButtonsList.Count)
        {
            if (!answerButtonsList[i].GetAnswerData().isCorrect)
            {
                answerButtonsList[i].hintImage.SetActive(false);
                answerButtonsList[i].hintText.text = "";
                answerButtonObjectPool.ReturnObject(answerButtonGameObjects[i]);
                answerButtonGameObjects.RemoveAt(i);
                answerButtonsList.RemoveAt(i);
                break;

            } else
            {
                i++;
            }
        }
    }

    public void HintAddTime()
    {
        timeRemaining += currentGameData.timeLimitInSeconds + 1;
        timeToAnswer = (int)timeRemaining + 1;
        //linearProgressBar.UpdateProgressBar();
    }

    public void HintPlayersHelpResetUI()
    {
        hbPlayersHelp.GetComponent<Button>().interactable = true;
        foreach (AnswerButton answer in answerButtonsList)
        {
            answer.hintImage.SetActive(false);
            answer.hintText.text = "";
        }
    }

    public void HintAddTimeReset()
    {
        timeToAnswer = currentGameData.timeLimitInSeconds;
    }

    #endregion

    #region Chests
    public void ChestClicked(Chest chest)
    {
        chest.OpenChest(Random.Range(0,3));
        
        foreach (Chest ch in chestsList)
        { 
            if (ch.chestClosed)
            {
                Destroy(ch);
            }
        }
    }

    private void ShowChests()
    {
        Debug.Log("Show chests ()");
        //Debug.Log("Show chests () is chestsPanek active: ");
        PauseGame();
        for (int i = 0; i < activePanels.Length; i++)
        {
            activePanels[i].SetActive(false);
        }
        //for (int i = 1; i <= qtyOfShowingChests; i++)
        //{
        //    chestsList.Add(Instantiate(chest, chestsParent.transform).GetComponent<Chest>());
        //}
        //CreateChests();
        chestsPanel.SetActive(true);
        CreateChests();
        correctAnswersCounter = 0;
    }

    private void CreateChests()
    {
        Debug.Log("Create chests ()");
        for (int i = 0; i < qtyOfShowingChests; i++)
        {
            Debug.Log("Create chests () qtyOfShowingChests " + qtyOfShowingChests);
            GameObject chestGameObject = chestObjectPool.GetObject();
            chestGameObjects.Add(chestGameObject);
            chestGameObject.transform.SetParent(chestsParent);

            Chest chest = chestGameObject.GetComponent<Chest>();
            //chestsList.Add(chest);
        }
    }

    private void RemoveChests()
    {
        while (chestGameObjects.Count > 0)
        {
            chestObjectPool.ReturnObject(chestGameObjects[0]);
            chestGameObjects.RemoveAt(0);

        }
        while (chestsList.Count > 0)
        {
            chestsList.RemoveAt(0);
        }
    }

    private void RandomPrize()
    {
        //prize = (Prize)Random.Range(0, 3);
    }
    #endregion

    #region Popups
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
    #endregion

    public void ReturnToMenu()
    {
        //dataController.SubmitPlayerInfo(playerXp);
        //StopCoroutine(linearProgressBarRoutine());
        SceneManager.LoadScene("Main");
    }

    #region Pause Resume End Game
    public void ResumeGame()
    {
        isLevelActive = true;
        questionPanel.SetActive(true);
        answerButtonParent.gameObject.SetActive(true);
        endRoundPanel.SetActive(false);
        timeRemaining = timeToAnswer;

        Time.timeScale = 1;
        HideHintsPrice();
        UpdateHintsQtyUI();
        UpdateTimerText();
        UpdateLinearProgressBar();
        ShowQuestion();
    }

    public void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
        }
    }

    public void EndRound()
    {
        isLevelActive = false;
        StopCoroutine(linearProgressBarRoutine());
        PauseGame();
        //SubmitGameData();
        questionPanel.SetActive(false);
        answerButtonParent.gameObject.SetActive(false);
        endRoundPanel.SetActive(true);
        MinusLive();
        SubmitPlayerInfo();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (isLevelActive & !isAnswerClicked)
        //{
            //timeRemaining -= Time.deltaTime;
            //UpdateTimerText();
            //UpdateLinearProgressBar();
            //if (timeRemaining <= 0f)
            //{
            //    EndRound();
            //}
        //}
        //UpdateRadialProgressBar();
        
    }
}
