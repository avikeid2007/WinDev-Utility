using System;
using System.Threading.Tasks;

namespace WinDevUtilityUno.Extensions
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
