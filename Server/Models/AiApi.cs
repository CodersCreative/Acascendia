using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace studbud.Server.Models
{
    public class AiApi
    {
        //private readonly HttpClient _httpClient;
        HttpClient client = new HttpClient();

        public AiApi(HttpClient httpClient)
        {
            //_httpClient = httpClient;
        }

        public async Task<List<Flashcard>> GenerateFlashcardsAsync(string prompt)
        {
            string prompt2 = prompt;
            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", "Vsf0g0De4A5r0H1sWqBhI6X9qjN0R4sO");

            var requestBody = new
            {
                model = "ministral-8b-2410",
                messages = new[]
                                {
                new { role = "user", content = $"Generate 5 flashcards from these notes:\n{prompt2}" }
            },
                max_tokens = 300
            };
            

            // 4. Serialize to JSON
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 5. Make POST request to Mistral chat API
            var response = await client.PostAsync("https://api.mistral.ai/v1/chat/completions", content);

            // 6. Read and print response
            var result = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(result);
            string flashcardsText = jsonDoc.RootElement
           .GetProperty("choices")[0]
           .GetProperty("message")
           .GetProperty("content")
           .GetString();

            var lines = flashcardsText.Split("\n");
            var cards = new List<Flashcard>();
            Flashcard currentCard = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("**Front:**") || line.Contains("Front:"))
                {
                    currentCard = new Flashcard();
                    currentCard.Front = line.Replace("**Front:**", "").Replace("Front:", "").Trim();
                }
                else if (line.StartsWith("**Back:**") || line.Contains("Back:"))
                {
                    currentCard.Back = line.Replace("**Back:**", "").Replace("Back:", "").Trim();
                    cards.Add(currentCard);
                }
            }

            return cards;
        }


    }

    public class Flashcard
    {
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
