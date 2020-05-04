namespace MathLibrary
{
    /// <summary>
    /// 設備轉換函式
    /// </summary>
    public class DeviceMathClass
    {
        public double WattMagnification_MWH10(int _no)
        {
            double ans = 1;
            switch (_no)
            {
                case 0: ans = 0.0001; break;
                case 1: ans = 0.001; break;
                case 2: ans = 0.01; break;
                case 3: ans = 0.1; break;
                case 4: ans = 1; break;
                case 5: ans = 10; break;
                case 6: ans = 100; break;
                case 7: ans = 1000; break;
            }
            return ans;
        }

        public double WattHourMagnification_MWH10(int _no)
        {
            double ans = 1;
            switch (_no)
            {
                case 0: ans = 0.0001; break;
                case 1: ans = 0.001; break;
                case 2: ans = 0.01; break;
                case 3: ans = 0.1; break;
                case 4: ans = 1; break;
                case 5: ans = 10; break;
                case 6: ans = 100; break;
                case 7: ans = 1000; break;
            }
            return ans;
        }

        public double ReginLuxTransfor_1K(int SourcesValue, double mEditValue)
        {
            //最大值為64345
            double ans = 0;
            int mValue = SourcesValue - 32760;
            ans = mValue / 31.585 + mEditValue;
            ans = (ans >= 1000 ? 1000 : ans);
            ans = (ans <= 0 ? 0 : ans);
            return ans;
        }
        public double ReginLuxTransfor_10K(int SourcesValue, double mEditValue)
        {
            //最大值為64345
            double ans = 0;
            int mValue = SourcesValue - 32760;
            ans = mValue / 3.1585 + mEditValue;
            ans = (ans >= 10000 ? 10000 : ans);
            ans = (ans <= 0 ? 0 : ans);
            return ans;
        }

        public double CurrentMagnification_CPM10(int _no)
        {
            double ans = 1;
            switch (_no)
            {
                case 0: ans = 0.001; break;
                case 1: ans = 0.01; break;
                case 2: ans = 0.1; break;
            }
            return ans;
        }

        public double ValtageMagnification_CPM10(int _no)
        {
            double ans = 1;
            switch (_no)
            {
                case 0: ans = 0.1; break;
                case 1: ans = 1; break;
                case 2: ans = 10; break;
                case 3: ans = 100; break;
            }
            return ans;
        }

        public double WattMagnification_CPM10(int _no)
        {
            double ans = 1;
            switch (_no)
            {
                case 0: ans = 0.0001; break;
                case 1: ans = 0.001; break;
                case 2: ans = 0.01; break;
                case 3: ans = 0.1; break;
                case 4: ans = 1; break;
                case 5: ans = 10; break;
                case 6: ans = 100; break;
                case 7: ans = 1000; break;
            }
            return ans;
        }

        public double TOYO_PM650_V(int _val, int _pt)
        {
            int ans = 0;
            ans = _val * _pt / 10;
            return ans;
        }

        public double TOYO_PM650_A(int _val, int _ct)
        {
            int ans = 0;
            ans = _val * _ct / 1000;
            return ans;
        }

        public double TOYO_PM650_W(int _val, int _pt, int _ct)
        {
            int ans = 0;
            ans = _val * _pt * _ct;
            return ans;
        }
    }
}
