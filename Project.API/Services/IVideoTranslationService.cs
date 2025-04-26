namespace Project.API.Services
{
	public interface IVideoTranslationService
	{
		Task<string> TranslateVideoContentAsync(string videoFile);
	}
}
