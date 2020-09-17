using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{
    public class SolarAngleClass
    {
        /// <summary>
        /// 地點緯度
        /// </summary>
        private double Phi { get; set; }
        /// <summary>
        /// 地點經度
        /// </summary>
        private double Lambda { get; set; }
        /// <summary>
        /// 計算指定地點的太陽相關角度
        /// </summary>
        /// <param name="phi">地點緯度</param>
        /// <param name="lambda">地點經度</param>
        public SolarAngleClass(double phi, double lambda)
        {
            Phi = phi;
            Lambda = lambda;
        }

        /// <summary>
        /// 取得當前時間地點的太陽高度角
        /// </summary>
        /// <returns></returns>
        public double GetSolarAngle()
        {
            int dateNumber = DateTime.Now.DayOfYear - 1;            //取得一年中日期的天數(取0開始)
            DateTime todayStartTime = Convert.ToDateTime($"{DateTime.Now:yyyy/MM/dd} 00:00:00");            //定義時間起始點
            double delta = IncidentAngle(dateNumber);           //計算太陽傾角
            double hf = DateTime.Now.Subtract(todayStartTime).TotalHours;
            double ho = SolarZenithAngle(hf, delta);
            return ho;
        }

        /// <summary>
        /// 取得指定時間地點的太陽高度角
        /// </summary>
        /// <returns></returns>
        public double GetSolarAngle(DateTime dateTime)
        {
            int dateNumber = dateTime.DayOfYear - 1;            //取得一年中日期的天數(取0開始)
            DateTime todayStartTime = Convert.ToDateTime($"{dateTime:yyyy/MM/dd} 00:00:00");            //定義時間起始點
            double delta = IncidentAngle(dateNumber);           //計算太陽傾角
            double hf = dateTime.Subtract(todayStartTime).TotalHours;
            double ho = SolarZenithAngle(hf, delta);
            return ho;
        }

        /// <summary>
        /// 計算太陽高度角
        /// </summary>
        /// <param name="HourFloat">時間(小時)</param>
        /// <param name="Delta">太陽傾角</param>
        /// <returns></returns>
        public double SolarZenithAngle(double HourFloat, double Delta)
        {
            //公式：ho=arcsin[sinφsinδ+cosφcosδcos(15t+λ-300)]
            double a = 15 * HourFloat + Lambda - 300;//时角
            double b = Math.Cos(Phi * Math.PI / 180d) * Math.Cos(Delta * Math.PI / 180d) * Math.Cos(a * Math.PI / 180d);
            double c = Math.Sin(Phi * Math.PI / 180) * Math.Sin(Delta * Math.PI / 180);
            double d = c + b;
            double ho = Math.Asin(d) / (Math.PI / 180d);
            return ho;
        }

        /// <summary>
        /// 計算太陽傾角δ
        /// </summary>
        /// <param name="DateNumber">一年中日期天數(0,1,2......364)</param>
        /// <returns>返回太陽傾角(單位：度)</returns>
        public double IncidentAngle(int DateNumber)
        {
            //公式：δ=[0.006918-0.399912cosQo+0.0702578sinQo-0.006758cosQo+0.000907sin2Qo-0.002697cos3Qo+0.001480sin3Qo]*180/π
            //Qo=360dn/365，度
            double Qo = Convert.ToDouble((Convert.ToDouble(360 * DateNumber) / Convert.ToDouble(365)) * (Math.PI / 180d));
            double a = 0.399912 * Math.Cos(Qo);
            double b = 0.0702578 * Math.Sin(Qo);
            double c = 0.006758 * Math.Cos(Qo);
            double d = 0.000907 * Math.Sin(2 * Qo);
            double e = 0.002697 * Math.Cos(3 * Qo);
            double f = 0.001480 * Math.Sin(3 * Qo);
            double g = 180 / Math.PI;
            double h = 0.006918 - a + b - c + d - e + f;
            double Delta = Math.Round(h * g, 1);
            return Delta;
        }
    }
}
