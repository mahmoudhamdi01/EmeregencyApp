namespace Project.API.Services
{
    public interface IAIModelServices
    {
		//Task<string> TranslateVideoToTextAsync(IFormFile videoFile);
		Task<string> TranslateVideoToTextAsync(string videoPath);

	}
}
