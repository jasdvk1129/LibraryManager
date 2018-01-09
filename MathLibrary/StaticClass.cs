namespace MathLibrary
{
    static class StaticClass
    {
        public static string Right(this string sSource, int iLength)
        {
            if (sSource.Trim().Length > 0)
                return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
            else
                return "";
        }
    }
}
