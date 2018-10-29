using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model
{
    internal class Schedule
    {
        private DateTime sTime;
        private DateTime eTime;

        public DateTime StartTime
        {
            get => sTime;
            set => sTime = value;
        }

        public DateTime EndTime
        {
            get => eTime;
            set => eTime = value;
        }

        public Schedule(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
