using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinningSunshineLibrary
{
    public class SolarAngleClass
    {
        /// <summary>
        /// 計算太陽方位角
        /// </summary>
        /// <param name="declination">太陽赤緯</param>
        /// <param name="hourAngle">時角</param>
        /// <param name="zenithAngle">太陽天頂角</param>
        /// <returns></returns>
        public static double CalculateAzimuthAngleOfSun(double declination, double hourAngle, double zenithAngle)
        {
            double sinAzimuthAngleOfSun = Math.Cos(declination * Math.PI / 180d) * Math.Sin(hourAngle * Math.PI / 180d) / Math.Sin(zenithAngle * Math.PI / 180d);
            double radiansAzimuthAngleOfSun = Math.Asin(sinAzimuthAngleOfSun);
            double azimuthAngleOfSun = radiansAzimuthAngleOfSun * 180d / Math.PI;
            return azimuthAngleOfSun;
        }
        /// <summary>
        /// 計算太陽高度角(仰角)
        /// </summary>
        /// <param name="zenithAngle">太陽天頂角</param>
        /// <returns></returns>
        public static double CalculateElevationAngleOfSun(double zenithAngle)
        {
            return 90 - zenithAngle > 0 ? 90 - zenithAngle : 0;
        }

        /// <summary>
        /// 計算太陽天頂角
        /// </summary>
        /// <param name="latitude">緯度</param>
        /// <param name="declination">太陽赤緯</param>
        /// <param name="hourAngle">時角</param>
        /// <returns></returns>
        public static double CalculateZenithAngleOfSun(double latitude, double declination, double hourAngle)
        {
            // 計算天頂角
            double cos = Math.Cos(latitude * Math.PI / 180d) * Math.Cos(declination * Math.PI / 180d) * Math.Cos(hourAngle * Math.PI / 180d);
            double sin = Math.Sin(latitude * Math.PI / 180d) * Math.Sin(declination * Math.PI / 180d);
            double cosZenithAngleOfSun = cos + sin;
            double radiansZenithAngleOfSun = Math.Acos(cosZenithAngleOfSun);
            // 弧度轉換角度
            double zenithAngleOfSun = radiansZenithAngleOfSun * 180d / Math.PI;

            return zenithAngleOfSun;
        }

        /// <summary>
        /// 計算時角
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns></returns>
        public static double CalculateHourAngle(DateTime time)
        {
            // 取得起始時間
            DateTime beginTime = DateTime.Parse($"{time:yyyy/MM/dd} 00:00:00");
            // 計算小時數
            double hours = time.Subtract(beginTime).TotalHours;
            // ω時角計算
            double hourAngle = (hours - 12d) * 360d / 24d;

            return hourAngle;
        }
        /// <summary>
        /// 計算太陽赤緯
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static double CalculateDeclination(DateTime date)
        {
            // 取得天數
            int dateNumber = date.DayOfYear;
            // δ日赤緯計算
            double Sindeclination = -Math.Sin(23.45d * Math.PI / 180d) * Math.Cos(360d * Math.PI / 180d * (dateNumber + 10) / 365.25d);
            double radiansDeclination = Math.Asin(Sindeclination);
            // 弧度轉換角度
            double declination = radiansDeclination * 180d / Math.PI;
            return declination;
        }
    }
}
