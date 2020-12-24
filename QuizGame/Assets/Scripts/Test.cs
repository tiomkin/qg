using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;

public class Test : MonoBehaviour
{
    //Question question;
    private Question question;
    private Question[] questions;
    private List<Question> questionsList;
    private Answer answer;
    // Start is called before the first frame update
    void Start()
    {
        questionsList = new List<Question>();
        string questionTableName = "Question";
        //string categoryTableName = "Category";
        string sqlQuery = $"SELECT * FROM {questionTableName} ORDER BY id DESC;";
        DataTable questionsTable = DB.GetTable(sqlQuery);
        DataRowCollection questionRows = questionsTable.Rows;
        int[] ids = GetRandomNumbers(5,questionsTable.Rows.Count); 
        string idRangeString = GenerateIdRangeString(ids);
        //Debug.Log("IdRangeString: " + idRangeString);

        //foreach (DataRow row in questionRows)
        //{
        //    Answer[] allAnswers = new Answer[3];
        //    int correctAnswerNum = Convert.ToInt32(row["correctAnswer"]);
        //    //int correctAnswerNum = 1;
        //    question = new Question();
        //    question.questionText = row["questionText"].ToString();
        //    question.correctAnswer = correctAnswerNum;
        //    //question.questionCategory = Convert.ToInt32(row["category"]);
        //    question.questionCategory = row["category"].ToString();
        //    //Debug.Log("Type: " + row["correctAnswer"].GetType().Name);
        //    for (int i = 0; i <= 2; i++)
        //    {
        //        answer = new Answer();
        //        answer.answerText = row[$"answer{i + 1}"].ToString();
        //        if (correctAnswerNum == (i + 1))
        //        {
        //            answer.isCorrect = true;
        //        }
        //        else
        //        {
        //            answer.isCorrect = false;
        //        }
        //        allAnswers[i] = answer;

        //    }
        //    question.answers = allAnswers;
        //    questionsList.Add(question);
        //}
        //questions = questionsList.ToArray();
        //allGameData.questions = questionsList;
        //foreach (Question question in questionsList)
        //{
        //    Debug.Log(question.questionText);
        //}            
        //string questionText = DB.ExecuteQueryWithAnswer($"SELECT questionText FROM {questionTableName} WHERE id = 1;");
        //Debug.Log("Questionf from DB: " + questionText);
        
    }

    private int[] GetRandomNumbers(int max, int countOfNumbers)
    {
        int[] numbers = new int[countOfNumbers];
        for (int i = 0; i < countOfNumbers; i++)
        {
            numbers[i] = UnityEngine.Random.Range(1, max);
        }
        //Debug.Log("Max: " + max);
        //Debug.Log("countOfNumbers "+countOfNumbers);
        for (int i = 0; i<numbers.Length; i++)
        {
            Debug.Log("Array: " + numbers[i]);
        }
        return numbers;
    }
    private string GenerateIdRangeString(int[] ids)
    {
        string idRangeString = "";
        for (int i = 0; i < ids.Length; i++)
        {
            if (i != ids.Length - 1)
            {
                idRangeString += ids[i].ToString() + ",";
            }
            else
            {
                idRangeString += ids[i].ToString();
            }
        }
        //Debug.Log("Final string of Ids: "+ idRangeString);
        return idRangeString;
    }

}
