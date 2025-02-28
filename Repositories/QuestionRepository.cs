using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
public class QuestionRepository
{
    private readonly string _connectionString;

    public QuestionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> SaveQuestionsWithChoices(string jsonData)
    {

        string validJson = ValidateAndFixJson(jsonData);
        if (string.IsNullOrEmpty(validJson))
        {
            Console.WriteLine("Error: Invalid JSON format");
            return false;
        }

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@JsonData", validJson, DbType.String);

        try
        {
            await connection.ExecuteAsync("Proc_SaveQuestions", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }


    }
    private string ValidateAndFixJson(string jsonData)
    {
        try
        {
            // Try to parse the JSON
            JObject jsonObj = JObject.Parse(jsonData);

            // Check if "text" contains a nested JSON string
            if (jsonObj["text"] != null && jsonObj["text"].Type == JTokenType.String)
            {
                string innerJson = jsonObj["text"].ToString();

                // Check if inner JSON is valid
                if (IsValidJson(innerJson))
                {
                    return innerJson; // Use the corrected JSON
                }
                else
                {
                    Console.WriteLine("Error: Extracted inner JSON is invalid.");
                    return null;
                }
            }

            return jsonData; // JSON is already valid, return as is
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON Validation Error: {ex.Message}");
            return null; // JSON is invalid
        }
    }

    private bool IsValidJson(string jsonString)
    {
        try
        {
            JToken.Parse(jsonString);
            return true; // JSON is valid
        }
        catch
        {
            return false; // JSON is invalid
        }
    }
}

