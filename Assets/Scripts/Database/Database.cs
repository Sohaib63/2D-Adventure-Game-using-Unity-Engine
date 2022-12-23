using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class Database : MonoBehaviour
{
    string dbfile="URI=file:SqliteTest.db";
    string query;
    // Start is called before the first frame update
    void Start()
    {
        CreateScoreTable();
    }

    private void CreateScoreTable(){
        using (var connection = new SqliteConnection(dbfile))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "create table if not exists PlayerScore(" + "id INTEGER PRIMARY KEY AUTOINCREMENT ," + "score varchar(30))";
                command.ExecuteNonQuery();
                Debug.Log("New table created");
            }
            connection.Close();
        }
    }

    public void InsertScore(int score){
        using (var connection = new SqliteConnection(dbfile))
        {
            connection.Open();
            query="Insert into PlayerScore(score) values(@score)";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@score",score);
                command.ExecuteReader();
                Debug.Log("Data inserted");
            }
        }
    }

    public void GetData(){
        using (var connection = new SqliteConnection(dbfile))
        {
            connection.Open();
            query="Select * from PlayerScore";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqliteDataReader reader=command.ExecuteReader();
                while (reader.Read())
                {
                    Debug.Log(reader["score"]);
                }
                
            }
        }
    }


    public void UpdateScore(int id, int score){
        using (var connection = new SqliteConnection(dbfile))
        {
            connection.Open();
            query="Update PlayerScore set score=@score where id=@id";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@score",score);
                command.Parameters.AddWithValue("@id",id);
                command.ExecuteReader();
                Debug.Log("Data updated");
            }
        }
    }


    public void DeleteScore(int id){
        using (var connection = new SqliteConnection(dbfile))
        {
            connection.Open();
            query="Delete from PlayerScore where id=@id";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@id",id);
                command.ExecuteReader();
                Debug.Log("Data deleted");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
