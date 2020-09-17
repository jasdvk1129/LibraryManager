using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{
    public class WetBulbTempClass
    {
        /// <summary>
        /// 濕求溫度計算
        /// </summary>
        /// <param name="Tw">室外溫度、溼度計算濕焓值</param>
        /// <returns></returns>
        public double CalHZ_S(double Tw)
        {
            double mm0, mm1 = 0, mm2;
            mm0 = Tw / 234.5;
            mm1 = Tw * (18.678 - mm0) / (Tw + 257.14);
            mm2 = Math.Exp(mm1);
            double hw = 1.01 * Tw + 0.6219 * (2500 + 1.84 * Tw) * 611.2 * mm2 / (101326 - 611.2 * mm2);
            return hw;
        }

        /// <summary>
        /// 濕求溫度計算(簡化算法)
        /// </summary>
        /// <param name="Td">室外乾球溫度</param>
        /// <param name="Hr">室外濕度</param>
        /// <returns></returns>
        public double CalHZ(double Td, double Hr)
        {
            double t1, Pv, d, h;
            t1 = (18.678 - Td / 234.5) * Td / (Td + 257.14);
            Pv = 611.2 * Math.Exp(t1);      //水蒸氣飽和壓力
            d = 0.6219 * (0.01 * Hr * Pv / (101326 - 0.01 * Hr * Pv));          //KG÷KG
            h = 1.01 * Td + (2500 + 1.84 * Td) * d;
            return h;
        }

        /// <summary>
        /// 濕求溫度計算(精確算法)
        /// </summary>
        /// <param name="Td">室外乾球溫度</param>
        /// <param name="Hr">室外濕度</param>
        /// <returns></returns>
        public double CalTw(double Td, double Hr)
        {
            int kk = 0;         //迭代次數
            double val = 0;     //迭代的失球溫度初值
            double e = 0;
            double e_1 = 0, e_2 = 0;
            double ee_1 = 0, ee_2 = 0;
            double II = 0;

            if (Td < 60 && Td >= 0)
            {
                e = CalHZ_S(val) - CalHZ(Td, Hr);
                e = Math.Abs(e);
                while (e > 0.02)
                {
                    e = CalHZ_S(val) - CalHZ(Td, Hr);           //比例
                    e_2 = e_1;
                    e_1 = e;

                    II += e;

                    ee_2 = ee_1;
                    ee_1 = e_1 - e_2;           //微分


                    if (e > 0)          //大於目標值
                    {
                        val -= 0.1 * Math.Abs(e) + 0.001 * II + 0.01 * ee_1;        //修正輸入值
                    }
                    else
                    {
                        //小於目標值
                        val += 0.1 * Math.Abs(e);
                    }

                    if (e_1 - e_2 < 0)
                    {
                        val += 0.005;
                    }
                    else
                    {
                        val -= 0.005;
                    }

                    e = CalHZ_S(val) - CalHZ(Td, Hr);
                    e = Math.Abs(e);
                    kk++;

                    if (kk > 2000)        //迭代超過1000跳出
                    {
                        if (e > 3)
                        {
                            e = CalHZ_S(val) - CalHZ(Td, Hr);
                            if (e < 0)          //繼續+
                            {
                                val += -0.1 * e;
                                if (val > Td)
                                {
                                    val = Td * 0.99;
                                }
                            }
                        }
                        return val;
                    }
                }
            }
            return val;
        }
    }
}
