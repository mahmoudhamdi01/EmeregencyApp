

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
		#region MyRegion
		//public async Task<string> TranslateVideoToTextAsync(string videoPath)
		//{

		//	if (string.IsNullOrEmpty(videoPath) || !File.Exists(videoPath))
		//		return string.Empty;
		//	var translatedText = await Task.Run(() =>
		//	{
		//		// Example: Return a fixed description for testing purposes
		//		return "The user is requesting an ambulance. Please send help immediately.";
		//	});

		//	return translatedText;
		//	// Save the video file temporarily
		//	#region Before
		//	//var tempFilePath = Path.Combine(Path.GetTempPath(), videoFile.FileName);
		//	//using (var stream = new FileStream(tempFilePath, FileMode.Create))
		//	//{
		//	//	await videoFile.CopyToAsync(stream);
		//	//}

		//	//// Process the video file using the AI model
		//	//var description = await ProcessVideoWithModelAsync(tempFilePath);

		//	//// Delete the temporary file after processing
		//	//if (File.Exists(tempFilePath))
		//	//	File.Delete(tempFilePath);

		//	//return description; 
		//	#endregion

		//} 
		#endregion

		//public async Task<string> TranslateVideoToTextAsync(string videoPath)
		//{
		//	if (string.IsNullOrEmpty(videoPath))
		//		return string.Empty;

		//	// Prepare the form data to send to Flask
		//	var formData = new MultipartFormDataContent();

		//	// Read the video file from the path and add it to the form data
		//	var fileStreamContent = new StreamContent(System.IO.File.OpenRead(videoPath));
		//	fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
		//	formData.Add(fileStreamContent, "video", Path.GetFileName(videoPath));

		//	try
		//	{
		//		// Send the video to Flask Server
		//		var response = await _httpClient.PostAsync($"{_flaskServerUrl}/predict", formData);
		//		response.EnsureSuccessStatusCode();

		//		// Read the response from Flask Server
		//		var responseBody = await response.Content.ReadAsStringAsync();
		//		var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

		//		// Extract the description from the response
		//		return result?.text ?? string.Empty;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"Error communicating with Flask Server: {ex.Message}");
		//		return string.Empty;
		//	}
		//	#region New
		//	//if (videoFile == null || videoFile.Length == 0)
		//	//	return string.Empty;

		//	//var formData = new MultipartFormDataContent();
		//	//var fileStreamContent = new StreamContent(videoFile.OpenReadStream());
		//	//fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
		//	//formData.Add(fileStreamContent, "video", videoFile.FileName);

		//	//try
		//	//{
		//	//	var response = await _httpClient.PostAsync($"{_flaskServerUrl}/predict", formData);
		//	//	response.EnsureSuccessStatusCode();

		//	//	var responseBody = await response.Content.ReadAsStringAsync();
		//	//	var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);

		//	//	return result?.text ?? string.Empty;
		//	//}
		//	//catch (HttpRequestException httpEx)
		//	//{
		//	//	Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
		//	//	return string.Empty;
		//	//}
		//	//catch (JsonException jsonEx)
		//	//{
		//	//	Console.WriteLine($"JSON Parsing Error: {jsonEx.Message}");
		//	//	return string.Empty;
		//	//}
		//	//catch (Exception ex)
		//	//{
		//	//	Console.WriteLine($"General Error: {ex.Message}");
		//	//	return string.Empty;
		//	//} 
		//	#endregion
		//}

		#region Before
		//public async Task<string> TranslateVideoToTextAsync(string videoPath)
		//{
		//	if (string.IsNullOrEmpty(videoPath))
		//		return string.Empty;

		//	Console.WriteLine($"[AIModelService] Sending video to: {_flaskServerUrl}/predict");

		//	var formData = new MultipartFormDataContent();
		//	var fileStreamContent = new StreamContent(File.OpenRead(videoPath));
		//	fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
		//	formData.Add(fileStreamContent, "video", Path.GetFileName(videoPath));

		//	try
		//	{
		//		var response = await _httpClient.PostAsync($"{_flaskServerUrl}/predict", formData);
		//		var responseBody = await response.Content.ReadAsStringAsync();
		//		Console.WriteLine($"[AIModelService] Flask Response: {responseBody}");

		//		if (!response.IsSuccessStatusCode)
		//		{
		//			Console.WriteLine($"[AIModelService] Status Code: {response.StatusCode}");
		//			return string.Empty;
		//		}

		//		var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
		//		return result?.text ?? string.Empty;
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"[AIModelService] Error communicating with Flask Server: {ex.Message}");
		//		return string.Empty;
		//	}
		//} 
		#endregion

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
