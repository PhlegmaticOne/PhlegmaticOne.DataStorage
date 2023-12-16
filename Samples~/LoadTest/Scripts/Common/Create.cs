using UnityEngine;

namespace LoadTest.Common
{
    public static class Create
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char RandomChar() => Chars[Random.Range(0, Chars.Length)];

        public static string RandomString(int minLength, int maxLength)
        {
            var length = Random.Range(minLength, maxLength);
            var stringChars = new char[length];

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = Chars[Random.Range(0, Chars.Length)];

            return new string(stringChars);
        }
    }
}