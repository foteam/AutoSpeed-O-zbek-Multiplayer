using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Clusters;

public class MongoDBManager : MonoBehaviour
{
    private static MongoClient client;
    private IMongoDatabase database;
    private static IMongoCollection<BsonDocument> userCollection;
    
    static string connectionString = "mongodb+srv://sarvar:Sarik1212@ascluster.9o4atl2.mongodb.net/?retryWrites=true&w=majority"; // Замените на свой адрес и порт MongoDB
    static string databaseName = "aspeedbase";
    static string users = "users";// Замените на имя вашей базы данных
    
    private void Start()
    {
        ConnectToDatabase();
    }

    private void ConnectToDatabase()
    {
        try
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            userCollection = database.GetCollection<BsonDocument>(users);
            Debug.Log("Connected to MongoDB");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to MongoDB: {ex.Message}");
            TryReconnect();
        }
    }
    private void TryReconnect()
    {
        // Опционально: здесь можно добавить логику переподключения
        Debug.Log("Attempting to reconnect to MongoDB...");

        // Пример: пауза перед новой попыткой подключения
        StartCoroutine(ReconnectRoutine());
    }
    private IEnumerator ReconnectRoutine()
    {
        yield return new WaitForSeconds(5f); // Интервал между попытками переподключения

        // Повторная попытка подключения после паузы
        ConnectToDatabase();
    }

    private bool IsMongoDBConnected()
    {
        // Проверяем состояние соединения
        return client?.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;
    }

    private void OnDestroy()
    {
        // Освобождаем ресурсы при уничтожении объекта
        client?.Cluster.Dispose();
    }
    
    public static bool CheckConnectionStatus()
    {
        if (client.Cluster.Description.State == ClusterState.Connected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Добавление документа в коллекцию
    public static async Task UserInsertDocument(BsonDocument document)
    {
        userCollection.InsertOne(document);
    }
    
    // Поиск документа по критериям
    public static async Task<BsonDocument> UserFindDocument(FilterDefinition<BsonDocument> filter)
    {
        return await userCollection.Find(filter).FirstOrDefaultAsync();
    }

    // Обновление документа
    public static async Task UserUpdateDocument(FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
    {
        userCollection.UpdateOne(filter, update);
    }

    // Удаление документа
    public static async Task UserDeleteDocument(FilterDefinition<BsonDocument> filter)
    {
        userCollection.DeleteOne(filter);
    }
}