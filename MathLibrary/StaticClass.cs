namespace MathLibrary
{
    static class StaticClass
    {
        /// <summary>
        /// 自右邊擷取字串
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="iLength">擷取長度</param>
        /// <returns></returns>
        public static string Right(this string sSource, int iLength)
        {
            if (sSource.Trim().Length > 0)
                return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
            else
                return "";
        }
    }
}
