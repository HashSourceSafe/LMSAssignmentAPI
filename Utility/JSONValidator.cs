using Newtonsoft.Json.Linq;

namespace LMSAssignmentAPI.Utility
{
    public class JSONValidator
    {

        public string ValidateAndFixJson(string jsonData)
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
}
