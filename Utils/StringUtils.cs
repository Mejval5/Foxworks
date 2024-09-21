using UnityEngine;

namespace Fox.Utils
{
    public static class StringUtils
    {
        public static string GetRandomDocumentID()
        {
            const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int charAmount = 20;
            string myString = "";
            for (int i = 0; i < charAmount; i++)
            {
                myString += glyphs[Random.Range(0, glyphs.Length)];
            }
            return myString;
        }
    }
}