using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Phone.Scheduler;

namespace GermanWords.Helper
{
    public class BackgroundAgentHelper
    {
        public static string periodicTaskName = "PeriodicTask";

        public static string RegisterBackgroundAgent()
        {
            PeriodicTask periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
            if (periodicTask != null)
            {
                RemoveBackgroundAgent();
            }

            periodicTask = new PeriodicTask(periodicTaskName);
            periodicTask.Description = "用于更新动态磁贴";
            try
            {
                ScheduledActionService.Add(periodicTask);
            }
            catch (InvalidOperationException exception) 
            {
                return exception.Message;
            }
            catch (SchedulerServiceException) 
            {
                return "FAIL";
            }
            return "SUCCESS";
        }

        public static void RemoveBackgroundAgent()
        {
            try
            {
                ScheduledActionService.Remove(periodicTaskName);
            }
            catch (Exception) { }
        }
    }
}
