using dywebsdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Common
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>转换后的日期时间</returns>
        public static DateTime StampToDateTime(long timeStamp)
        {
            //create a new DateTime value based on the Unix Epoch
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            //add the timestamp to the value
            DateTime newDateTime = converted.AddSeconds(timeStamp);

            //return the value in string format
            return newDateTime.ToLocalTime();
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>Unix时间戳格式</returns>
        public static long DateTimeToStamp(DateTime dateTime)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (long)span.TotalSeconds;
        }
    }
}
