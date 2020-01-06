using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLibrary
{
    public class PsychrometricProperty
    {
        private const float Rda = 287.055f;
        private const float Rw = 461.520f;

        private float altitude = float.NaN;

        /// <summary>
        /// 海拔高度(altitude, m)
        /// </summary>
        public float Altitude
        {
            private get
            {
                return (this.altitude >= -5000 && this.altitude <= 11000) ? this.altitude : float.NaN;
            }
            set
            {
                this.altitude = value;
            }
        }

        /// <summary>
        /// 大氣壓壓力(barometric pressure, kPa)
        /// </summary>
        private float BarometricPressure
        {
            get
            {
                if (Altitude != float.NaN)
                {
                    // Equation (3)
                    return (float)(101.325 * Math.Pow((1 - (2.25577 * 1e-5 * Altitude)), 5.2559));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        private float dryBulbTemperature = float.NaN;

        /// <summary>
        /// 乾球溫度(dry-bulb temperature, ℃)
        /// </summary>
        public float DryBulbTemperature
        {
            private get
            {
                return (this.dryBulbTemperature >= -60 && this.dryBulbTemperature <= 160) ? this.dryBulbTemperature : float.NaN;
            }
            set
            {
                this.dryBulbTemperature = value;
            }
        }

        /// <summary>
        /// 飽和壓力(saturated Pressure, kPa)
        /// </summary>
        public float SaturatedPressure
        {
            get
            {
                if (DryBulbTemperature != float.NaN)
                {
                    float saturatedPressure = float.NaN;
                    float absoluteTemperature = this.DryBulbTemperature + 273.15f;

                    if (this.DryBulbTemperature >= -100 && this.DryBulbTemperature < 0)
                    {
                        const double C1 = -5.6745359 * 1e3;
                        const double C2 = 6.3925247 * 1e00;
                        const double C3 = -9.6778430 * 1e-03;
                        const double C4 = 6.2215701 * 1e-07;
                        const double C5 = 2.0747825 * 1e-09;
                        const double C6 = -9.4840240 * 1e-13;
                        const double C7 = 4.1635019 * 1e00;

                        // Equation (5)
                        saturatedPressure = (float)((C1 / absoluteTemperature) + C2 + (C3 * absoluteTemperature) + (C4 * Math.Pow(absoluteTemperature, 2)) + (C5 * Math.Pow(absoluteTemperature, 3)) + (C6 * Math.Pow(absoluteTemperature, 4)) + (C7 * Math.Log(absoluteTemperature)));
                    }
                    else if (this.DryBulbTemperature >= 0 && this.DryBulbTemperature <= 200)
                    {
                        const double C8 = -5.8002206 * 1e03;
                        const double C9 = 1.3914993 * 1e00;
                        const double C10 = -4.8640239 * 1e-2;
                        const double C11 = 4.1764768 * 1e-5;
                        const double C12 = -1.4452093 * 1e-8;
                        const double C13 = 6.545967 * 1e00;

                        // Equation (6)
                        saturatedPressure = (float)((C8 / absoluteTemperature) + (C9) + (C10 * absoluteTemperature) + (C11 * Math.Pow(absoluteTemperature, 2)) + (C12 * Math.Pow(absoluteTemperature, 3)) + (C13 * Math.Log(absoluteTemperature)));
                    }

                    return (float)(Math.Exp(saturatedPressure) / 1e03); // Pa -> kPa
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        private float relativeHumidity = float.NaN;

        /// <summary>
        /// 相對溼度(relative humidity, %)
        /// </summary>
        public float RelativeHumidity
        {
            private get
            {
                return (this.relativeHumidity > 0 && this.relativeHumidity <= 100) ? this.relativeHumidity : float.NaN;
            }
            set
            {
                this.relativeHumidity = value;
            }
        }

        /// <summary>
        /// 蒸氣壓(vapor Pressure, kPa)
        /// </summary>
        public float VaporPressure
        {
            get
            {
                if (dryBulbTemperature != float.NaN && RelativeHumidity != float.NaN)
                {
                    // Equation (24) ??
                    return (RelativeHumidity / 100) * SaturatedPressure;
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// 濕度比(humidity ratio, kg/kg da)
        /// </summary>
        public float HumidityRatio
        {
            get
            {
                if (BarometricPressure != float.NaN && VaporPressure != float.NaN)
                {
                    return (float)((0.62198 * VaporPressure) / (BarometricPressure - VaporPressure));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// 飽和比(Saturation humidity ratio)
        /// </summary>
        public float SaturationHumidityRatio
        {
            get
            {
                if (BarometricPressure != float.NaN && SaturatedPressure != float.NaN)
                {
                    return (float)((0.62198 * SaturatedPressure) / (BarometricPressure - SaturatedPressure));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// 比容(specific volume of dry air, m3/kg da)
        /// </summary>
        public float SpecificVolume
        {
            get
            {
                if (BarometricPressure != float.NaN && DryBulbTemperature != float.NaN)
                {
                    // Equation (28)
                    return (float)(((Rda / 1e3) * (DryBulbTemperature + 273.15) * (1 + (1.6078 * HumidityRatio))) / BarometricPressure);
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// 比焓(specfic enthalpy of dry air, kJ/kg da)
        /// </summary>
        public float SpecificEnthalpy
        {
            get
            {
                if (DryBulbTemperature != float.NaN && HumidityRatio != float.NaN)
                {
                    // Equation (28)
                    return (float)((1.006 * DryBulbTemperature) + (HumidityRatio * (2501 + (1.805 * DryBulbTemperature))));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// 露點溫度(dew-point temperature, ℃)
        /// </summary>
        public float DewPointTemperature
        {
            get
            {
                if (VaporPressure != float.NaN)
                {
                    if (RelativeHumidity == 100)
                    {
                        return DryBulbTemperature;
                    }
                    else
                    {
                        //希臘字 α
                        double alpha = Math.Log(VaporPressure);

                        const double C14 = 6.54f;
                        const double C15 = 14.526f;
                        const double C16 = 0.7389f;
                        const double C17 = 0.09486f;
                        const double C18 = 0.4569f;

                        float dewPointTemperature = (float)(C14 + (C15 * alpha) + (C16 * Math.Pow(alpha, 2)) + (C17 * Math.Pow(alpha, 3)) + (C18 * Math.Pow(VaporPressure, 0.1984)));

                        if (dewPointTemperature >= 0 && dewPointTemperature <= 93)
                        {
                            return dewPointTemperature;
                        }
                        else if (dewPointTemperature < 0)
                        {
                            return (float)(6.09f + (12.608f * alpha) + (0.4959f * Math.Pow(alpha, 2)));
                        }
                    }
                }
                else
                {
                    return float.NaN;
                }

                return float.NaN;
            }
        }

        /*----------------------------- 求濕球溫度所需參數　-----------------------------*/
        private float IterationeWetBulbTemperature { get; set; } = float.NaN;

        private float IterationeP
        {
            get
            {
                if (IterationeWetBulbTemperature != float.NaN)
                {
                    float pws = float.NaN;
                    float absoluteTemperature = IterationeWetBulbTemperature + 273.15f;

                    if (IterationeWetBulbTemperature >= -100 && IterationeWetBulbTemperature < 0)
                    {
                        const double C1 = -5.6745359 * 1e3;
                        const double C2 = 6.3925247 * 1e00;
                        const double C3 = -9.6778430 * 1e-03;
                        const double C4 = 6.2215701 * 1e-07;
                        const double C5 = 2.0747825 * 1e-09;
                        const double C6 = -9.4840240 * 1e-13;
                        const double C7 = 4.1635019 * 1e00;

                        // Equation (5)
                        pws = (float)((C1 / absoluteTemperature) + C2 + (C3 * absoluteTemperature) + (C4 * Math.Pow(absoluteTemperature, 2)) + (C5 * Math.Pow(absoluteTemperature, 3)) + (C6 * Math.Pow(absoluteTemperature, 4)) + (C7 * Math.Log(absoluteTemperature)));
                    }
                    else if (IterationeWetBulbTemperature >= 0 && IterationeWetBulbTemperature <= 200)
                    {
                        const double C8 = -5.8002206 * 1e03;
                        const double C9 = 1.3914993 * 1e00;
                        const double C10 = -4.8640239 * 1e-2;
                        const double C11 = 4.1764768 * 1e-5;
                        const double C12 = -1.4452093 * 1e-8;
                        const double C13 = 6.545967 * 1e00;

                        // Equation (6)
                        pws = (float)((C8 / absoluteTemperature) + (C9) + (C10 * absoluteTemperature) + (C11 * Math.Pow(absoluteTemperature, 2)) + (C12 * Math.Pow(absoluteTemperature, 3)) + (C13 * Math.Log(absoluteTemperature)));
                    }

                    return (float)(Math.Exp(pws) / 1e03); // Pa -> kPa
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        private float IterationeSaturationHumidityRatio
        {
            get
            {
                if (Altitude != float.NaN)
                {
                    return (float)((0.62198 * IterationeP) / (BarometricPressure - IterationeP));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        private float IterationeHumidityRatio
        {
            get
            {
                if (IterationeWetBulbTemperature != float.NaN)
                {
                    return (float)((((2501 - (2.381 * IterationeWetBulbTemperature)) * IterationeSaturationHumidityRatio) - (1.006 * (DryBulbTemperature - IterationeWetBulbTemperature))) / (2501 + (1.85 * DryBulbTemperature) - (4.186 * IterationeWetBulbTemperature)));
                }
                else
                {
                    return float.NaN;
                }
            }
        }

        /*----------------------------- 求濕球溫度所需參數　-----------------------------*/

        /// <summary>
        /// 濕球溫度(wet-blub temperature, ℃)
        /// </summary>
        public float WetBlubTemperature
        {
            get
            {
                if (DryBulbTemperature != float.NaN && DewPointTemperature != float.NaN && DryBulbTemperature >= DewPointTemperature)
                {
                    if (RelativeHumidity == 100)
                    {
                        return DryBulbTemperature;
                    }
                    else
                    {
                        for (IterationeWetBulbTemperature = DewPointTemperature; IterationeWetBulbTemperature <= DryBulbTemperature; IterationeWetBulbTemperature += 0.001f)
                        {
                            if (Math.Abs((HumidityRatio - IterationeHumidityRatio) / HumidityRatio) < 0.01)
                            {
                                return IterationeWetBulbTemperature;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    return float.NaN;
                }

                return float.NaN;
            }
        }
    }
}