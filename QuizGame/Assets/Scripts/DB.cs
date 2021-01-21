﻿using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;

static class DB
{
    private const string fileName = "database.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static DB()
    {
        DBPath = GetDBPath();
    }

    /// <summary> Возвращает путь к БД. Если её нет в нужной папке на Андроиде,
    /// то копирует её с исходного apk файла. </summary>
    private static string GetDBPath()
    {
    //#if UNITY_EDITOR
    //        return Path.Combine(Application.streamingAssetsPath, fileName);
    //#if UNITY_STANDALONE
    //    string filePath = Path.Combine(Application.dataPath, fileName);
    //    if(!File.Exists(filePath)) UnpackDatabase(filePath);
    //    return filePath;
    //#elif UNITY_ANDROID
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(filePath)) UnpackDatabase(filePath);
            return filePath;
    //#endif
    }

    /// <summary> Распаковывает базу данных в указанный путь. </summary>
    /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        UnityWebRequest reader = UnityWebRequest.Get(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.downloadHandler.data);
    }

    /// <summary> Этот метод открывает подключение к БД. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> Этот метод закрывает подключение к БД. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> Этот метод выполняет запрос query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }

    /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
    /// <param name="query"> Собственно запрос. </param>
    /// <returns> Возвращает значение 1 строки 1 столбца, если оно имеется. </returns>
    public static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }
}


//#endif