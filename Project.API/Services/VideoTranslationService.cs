
namespace Project.API.Services
{
	public class VideoTranslationService : IVideoTranslationService
	{
		private readonly IAIModelServices _aIModelServices;

		public VideoTranslationService(IAIModelServices aIModelServices)
		{
			_aIModelServices = aIModelServices;
		}
		public async Task<string> TranslateVideoContentAsync(string videoFile)
		{
			return await _aIModelServices.TranslateVideoToTextAsync(videoFile);
		}
	}
}
