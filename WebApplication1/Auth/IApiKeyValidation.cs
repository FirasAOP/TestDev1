namespace WebApplication1.Auth
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string userApiKey);
    }
}
