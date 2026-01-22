using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TeamChoice.WebApis.Providers.Security
{
    public class EncryptDecryptService : IApiEncryptor
    {
        private const string KeyString = "ea!GC02rh48bspmf"; // 16 bytes for AES-128
        private readonly byte[] _keyBytes;

        public EncryptDecryptService()
        {
            _keyBytes = Encoding.UTF8.GetBytes(KeyString);
        }

        public string Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data)) return data;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _keyBytes;
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7; // PKCS7 is compatible with PKCS5 for AES

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(data);
                        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while encrypting: ", ex);
            }
        }

        public string Decrypt(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData)) return encryptedData;

            // Log.Info($"Decrypting data: {encryptedData}"); // Logger placeholder
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _keyBytes;
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        byte[] inputBytes = Convert.FromBase64String(encryptedData);
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                        string result = Encoding.UTF8.GetString(decryptedBytes);

                        // Log.Info($"Decrypted data: {result}");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while decrypting: ", ex);
            }
        }

        public Dictionary<string, object> DecryptFieldsRecursively(object input)
        {
            // Convert input to JToken to traverse generically like ObjectMapper
            var json = JsonConvert.SerializeObject(input);
            var token = JToken.Parse(json);

            var result = ProcessToken(token);

            // Ensure the root result is a dictionary as per Java return type
            return result as Dictionary<string, object> ?? new Dictionary<string, object>();
        }

        private object ProcessToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var child in token.Children<JProperty>())
                    {
                        dict[child.Name] = ProcessToken(child.Value);
                    }
                    return dict;

                case JTokenType.Array:
                    var list = new List<object>();
                    foreach (var child in token)
                    {
                        list.Add(ProcessToken(child));
                    }
                    return list;

                case JTokenType.String:
                    return TryDecrypt(token.Value<string>());

                default:
                    // Return primitives (Integer, Boolean, etc.) as is
                    return token.ToObject<object>();
            }
        }

        private object TryDecrypt(string value)
        {
            try
            {
                // Attempt to decrypt; if it's not valid base64 or not encrypted properly, logic might differ.
                // The Java code catches Exception and returns original value.
                return Decrypt(value);
            }
            catch
            {
                return value; // Return the original value if decryption fails
            }
        }
    }
}
