using System;

namespace PredefineConstant.Enum.Common
{
    public enum StatisticsTimeSpan
    {
        FifteenMinutes,
        ThirtyMinutes,
        OneHour,
        OneDay
    }

    public static class StatisticsTimeSpanEx
    {
        public static int ToMinute(this StatisticsTimeSpan sts)
        {
            switch (sts)
            {
                case StatisticsTimeSpan.FifteenMinutes:
                    return 15;
                case StatisticsTimeSpan.ThirtyMinutes:
                    return 30;
                case StatisticsTimeSpan.OneHour:
                    return 60;
                case StatisticsTimeSpan.OneDay:
                    return 60 * 24;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}