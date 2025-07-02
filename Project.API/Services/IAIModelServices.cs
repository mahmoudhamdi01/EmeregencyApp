namespace Project.API.Services
{
    public interface IAIModelServices
    {
		Task<string> TranslateVideoToTextAsync(string videoPath);
	}
}
