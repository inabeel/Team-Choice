namespace TeamChoice.WebApis.Providers.Security
{
    public interface IApiEncryptor
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
