using Keras.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Project.API.Services
{
	public class AIModelServices : IAIModelServices
	{
		private readonly HttpClient _httpClient;
		private readonly string _flaskServerUrl;

		public AIModelServices(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_flaskServerUrl = configuration["FlaskServerUrl"]; // احصل على عنوان Flask من appsettings.json
		}

		public async Task<string> TranslateVideoToTextAsync(string videoPath)
		{
			if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
			{
				Console.WriteLine("[AIModelService] Invalid or missing video path.");
				return string.Empty;
			}

			Console.WriteLine($"[AIModelService] Sending video to: {_flaskServerUrl}/predict");

			var formData = new MultipartFormDataContent();

			await using var stream = File.OpenRead(videoPath);
			var fileStreamContent = new StreamContent(stream);
			fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			formData.Add(fileStreamContent, "video", Path.GetFileName(videoPath));

			try
			{
				var response = await _httpClient.PostAsync($"{_flaskServerUrl}/predict", formData);
				var responseBody = await response.Content.ReadAsStringAsync();

				Console.WriteLine($"[AIModelService] Flask Response: {responseBody}");

				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"[AIModelService] Status Code: {response.StatusCode}");
					return string.Empty;
				}

				var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

				// تأكد أن المفتاح موجود قبل استخدامه
				if (result?.text != null)
					return result.text.ToString();

				Console.WriteLine("[AIModelService] 'text' field not found in response.");
				return string.Empty;
			}
			catch (HttpRequestException httpEx)
			{
				Console.WriteLine($"HTTP Error: {httpEx.Message}");
				return $"HTTP Error: {httpEx.Message}";
			}
			catch (JsonException jsonEx)
			{
				Console.WriteLine($"JSON Error: {jsonEx.Message}");
				return $"JSON Error: {jsonEx.Message}";
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[AIModelService] General Error: {ex.Message}");
			}

			return string.Empty;
		}


		// Private method to process the video with the AI model
		private async Task<string> ProcessVideoWithModelAsync(string videoPath)
		{
			// Simulate translation for testing purposes
			// Replace this with actual AI model processing logic
			return await Task.FromResult("The user is requesting an ambulance. Please send help immediately.");
		}
	}
}
