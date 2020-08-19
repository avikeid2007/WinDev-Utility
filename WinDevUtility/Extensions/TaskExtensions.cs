using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WinDevUtility.Extensions
{
    public static class TaskExtensions
    {
        public async static void AwaitAsync(this Task task, Action CompleteCallback, Action<Exception> ErrorCallback)
        {
            try
            {
                await task;
                CompleteCallback?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorCallback?.Invoke(ex);
            }

        }
    }
}
