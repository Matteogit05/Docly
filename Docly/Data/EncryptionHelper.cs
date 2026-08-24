using System.Security.Cryptography;
using System.Text;

namespace Docly.Helpers
{
    public static class EncryptionHelper
    {
        private static byte[] _key = Array.Empty<byte>();
        private static byte[] _iv = Array.Empty<byte>();
        public static void Initialize(string keyString, string ivString)
        {
            if (string.IsNullOrWhiteSpace(keyString) || keyString.Length != 32)
                throw new ArgumentException("La chiave di crittografia deve essere di 32 caratteri.");
            
            if (string.IsNullOrWhiteSpace(ivString) || ivString.Length != 16)
                throw new ArgumentException("Il vettore di inizializzazione (IV) deve essere di 16 caratteri.");

            _key = Encoding.UTF8.GetBytes(keyString);
            _iv = Encoding.UTF8.GetBytes(ivString);
        }

        private static void CheckInitialization()
        {
            if (_key.Length == 0 || _iv.Length == 0)
                throw new InvalidOperationException("EncryptionHelper non è stato inizializzato con le chiavi.");
        }

        // 1. Crittografia per i Testi (Database)
        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            CheckInitialization(); // <-- Controllo sicurezza
            
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            CheckInitialization(); // <-- Controllo sicurezza
            
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch 
            { 
                return cipherText; 
            }
        }

        // 2. Crittografia per gli Allegati (File System)
        public static byte[] EncryptBytes(byte[] plainBytes)
        {
            CheckInitialization(); // <-- Controllo sicurezza
            
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
            }
            return ms.ToArray();
        }

        public static byte[] DecryptBytes(byte[] cipherBytes)
        {
            CheckInitialization(); // <-- Controllo sicurezza
            
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var msOut = new MemoryStream();
            cs.CopyTo(msOut);
            return msOut.ToArray();
        }
    }
}