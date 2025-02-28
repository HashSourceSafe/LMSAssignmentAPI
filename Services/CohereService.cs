using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LMSAssignmentAPI.Models;
using Newtonsoft.Json;

namespace LMSAssignmentAPI.Services
{
    public class CohereService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "KHgq4T6JgiDQst0fbv7sJYjszXrsblwimGCGCwuy";
        private const string ApiUrl = "https://api.cohere.ai/generate";

        public CohereService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateQuestionsJsonAsync(string module)
        {
            string message = "";
            message = $"Given the following syllabus text:\n\n" +
                  $"{module}\n\n" +
                  $"Generate 25 medium-level multiple choice questions in JSON format based on the syllabus. " +
                  $"Each question should include:\n" +
                  $"- Four answer choices.\n" +
                  $"- One correct answer.\n" +
                  $"Return them in JSON format as follows:\n\n" +
                  $"{{ \"questions\": [ " +
                  $"{{ \"question\": \"<question text>\", " +
                  $"\"choices\": [\"<choice1>\", \"<choice2>\", \"<choice3>\", \"<choice4>\"], " +
                  $"\"correctAnswer\": \"<correct choice>\" }} ] }}";

            var maxTokens = Math.Max(512, 4096 - (message.Length / 4) - 100); // Subtract an additional buffer
            var requestBody = new
            {
                model = "command-r-plus-08-2024",
                message = message,
                max_tokens = maxTokens
            };

            var content = new StringContent(
               System.Text.Json.JsonSerializer.Serialize(requestBody),
               Encoding.UTF8,
               "application/json"
           );

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

            var response = await _httpClient.PostAsync("https://api.cohere.ai/v1/chat", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Cohere API error: {response.StatusCode}, Details: {errorContent}");
            }

            
            return await response.Content.ReadAsStringAsync();
        }


        //private List<Question> ParseResponse(string response)
        //{
        //    var outerData = JsonConvert.DeserializeObject<CohereOuterResponse>(response);
        //    var questionList = new List<Question>();

        //    if (outerData != null && !string.IsNullOrEmpty(outerData.text))
        //    {
        //        var parsedData = JsonConvert.DeserializeObject<CohereResponse>(outerData.text);

        //        if (parsedData != null && parsedData.questions != null)
        //        {
        //            foreach (var item in parsedData.questions)
        //            {
                        
        //                var labels = new[] { "A", "B", "C", "D" }; // Define choice labels

        //                questionList.Add(new Question
        //                {
        //                    QuestionText = item.question,
        //                    Module = "Sample Module",
        //                    Marks = 1,
        //                    AnswerKey = item.correctAnswer,
        //                    Choices = item.choices.Select((choice, index) => new QuestionPart
        //                    {
        //                        QnPartOrder = index + 1, // Assign order: 1, 2, 3, 4
        //                        Label = labels.Length > index ? labels[index] : "E", // Prevents index out of range
        //                        QuestionText = choice // Store choice text
        //                    }).ToList()
        //                });

        //            }
        //        }
        //    }  
        //    return questionList;
        //}


      
        public class CohereOuterResponse
        {
            public string text { get; set; }  // The inner JSON is stored as a string here
        }

       
        public class CohereResponse
        {
            public List<CohereQuestion> questions { get; set; }
        }

        public class CohereQuestion
        {
            public string question { get; set; }
            public List<string> choices { get; set; }
            public string correctAnswer { get; set; }
            public string CO { get; set; }
        }
    }
}
