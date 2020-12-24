using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Data;

public class DataController : MonoBehaviour
{
    private Level[] allLevelData;
    private GameData allGameData;
    private PlayerInfo playerInfo;
    private string gameDataFileName = "gameData.json";
    private string playerInfoFileName = "playerInfo.json";
    private Question question;
    private List<Question> questionsList;
    private Question[] questions;
    private Answer answer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        LoadPlayerInfo();
        SceneManager.LoadScene("Main");
    }

    //public Level GetCurrentLevelData() 
    //{
    //    return allLevelData[0];
    //}

    public GameData GetGameData()
    {
        return allGameData;
    }

    public PlayerInfo GetPlayerInfo()
    {
        return playerInfo;
    }

    public void SubmitPlayerInfo(PlayerInfo info)
    {
        playerInfo.name = info.name;
        playerInfo.level = info.level;
        playerInfo.playerXP = info.playerXP;
        playerInfo.coins = info.coins;
        playerInfo.lives = info.lives;
        playerInfo.hint1 = info.hint1;
        playerInfo.hint2 = info.hint2;
        playerInfo.hint3 = info.hint3;
        SavePlayerInfo();
    }

    private void LoadPlayerInfo()
    {
        playerInfo = new PlayerInfo();
        string filePath = Path.Combine(Application.streamingAssetsPath, playerInfoFileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            PlayerInfo loadedPlayerInfo = JsonUtility.FromJson<PlayerInfo>(dataAsJson);
            playerInfo.name = loadedPlayerInfo.name;
            playerInfo.level = loadedPlayerInfo.level;
            playerInfo.playerXP = loadedPlayerInfo.playerXP;
            playerInfo.coins = loadedPlayerInfo.coins;
            playerInfo.lives = loadedPlayerInfo.lives;
            playerInfo.hint1 = loadedPlayerInfo.hint1;
            playerInfo.hint2 = loadedPlayerInfo.hint2;
            playerInfo.hint3 = loadedPlayerInfo.hint3;

        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    public int GetPlayerXP()
    {
        return playerInfo.playerXP;
    }

    private void SavePlayerInfo()
    {
        string dataAsJson = JsonUtility.ToJson(playerInfo);
        string filePath = Path.Combine(Application.streamingAssetsPath, playerInfoFileName);
        File.WriteAllText(filePath, dataAsJson); ;
    }

    private void LoadGameData()
    {
        allGameData = new GameData();
        string gameDataFilePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        if (File.Exists(gameDataFilePath))
        {
            string dataAsJson = File.ReadAllText(gameDataFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allGameData.timeLimitInSeconds = loadedData.timeLimitInSeconds;
            allGameData.xpForCorrectAnswer = loadedData.xpForCorrectAnswer;
            allGameData.targetXPToGetLevel = loadedData.targetXPToGetLevel;
            allGameData.questions = loadedData.questions;
        }
        else
        {
            Debug.LogError("Cannot load gamedata!");
        }
        //LoadRandomQuestions();


    }

    //private void LoadGameSettings()
    //{
    //    string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
    //    if (File.Exists(filePath))
    //    {
    //        string dataAsJson = File.ReadAllText(filePath);
    //        GameSettings loadedData = JsonUtility.FromJson<GameSettings>(dataAsJson);
    //        allGameData.gameSettings = loadedData;

    //    }
    //}
    //private void LoadRandomQuestions()
    //{
    //    questionsList = new List<Question>();
    //    int[] questionIDs = GetRandomNumbers(3);
    //    string questionTableName = "Question";
    //    //string categoryTableName = "Category";
    //    string sqlQuery = $"SELECT * FROM {questionTableName} ORDER BY id DESC;";
    //    string expression = "id IN (";
    //    DataTable questionsTable = DB.GetTable(sqlQuery);
    //    DataRow[] dataRows = questionsTable.Select(expression);
    //    DataRowCollection questionRows = questionsTable.Rows;

    //    foreach (DataRow row in questionRows)
    //    {
    //        Answer[] allAnswers = new Answer[3];
    //        int correctAnswerNum = Convert.ToInt32(row["correctAnswer"]);
    //        question = new Question();
    //        question.questionText = row["questionText"].ToString();
    //        //question.correctAnswer = correctAnswerNum;
    //        question.questionCategory = row["category"].ToString();
            
    //        for (int i = 0; i <= 2; i++)
    //        {
    //            answer = new Answer();
    //            answer.answerText = row[$"answer{i + 1}"].ToString();
    //            if (correctAnswerNum == (i+1))
    //            {
    //                answer.isCorrect = true;
    //            } else
    //            {
    //                answer.isCorrect = false;
    //            }
    //            allAnswers[i] = answer;

    //        }
    //        question.answers = allAnswers;
    //        questionsList.Add(question);
    //    }
    //    questions = questionsList.ToArray();
    //    allGameData.questions = questions;
    //}

    //private int[] GetRandomNumbers(int max)
    //{
    //    int[] numbers = new int[max-1];
    //    for (int i = 0; i<= max; i++)
    //    {
    //        numbers[i] = UnityEngine.Random.Range(1, max);
    //    }
    //    return numbers;
    //}

    //private string GenerateIdRangeString(int[] ids)
    //{
    //    string idRangeString = "";
    //    for (int i = 0; i < ids.Length; i++)
    //    {
    //        if (i != ids.Length - 1)
    //        {
    //            idRangeString += ids[i].ToString() + ",";
    //        }
    //        else
    //        {
    //            idRangeString += ids[i].ToString();
    //        }
    //    }
    //    //Debug.Log("Final string of Ids: "+ idRangeString);
    //    return idRangeString;
    //}
}
