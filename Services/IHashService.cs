namespace failure_api.Services
{
    public interface IHashService
    {
        string HashGoogleId(string id);
        bool VerifyGoogleId(string id, string hash);
    }
}