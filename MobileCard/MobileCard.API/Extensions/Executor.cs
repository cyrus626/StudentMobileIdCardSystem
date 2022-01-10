using System;
using System.Threading.Tasks;

namespace MobileCard.API.Extensions
{
    public static class Executor
    {
        #region Properties

        //private static ILogger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        public static void CautiouslyExecute(Action operation, string errorMessage, TimeSpan retryDelay,
            int maxAttempts = 5, Action<int, Exception> errorCallback = null)
        {
            var attempts = 0;

            while (true)
                try
                {
                    attempts++;
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == maxAttempts) throw;

                    errorCallback?.Invoke(attempts, ex);

                    var message = errorMessage +
                                  $"\nAttemts: ({attempts} of {maxAttempts}).\nRetrying after " +
                                  $"{retryDelay.TotalSeconds} seconds\n";

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        //Console.WriteLine(ex);
                        Console.WriteLine(errorMessage);

                    Task.Delay(retryDelay).Wait();
                }
        }

        public static TResult CautiouslyExecute<TResult>(Func<TResult> operation, string errorMessage,
            TimeSpan retryDelay, int maxAttempts = 5, Action<int, Exception> errorCallback = null)
        {
            var attempts = 0;

            while (true)
                try
                {
                    attempts++;
                    var res = operation();

                    return res;
                }
                catch (Exception ex)
                {
                    if (attempts == maxAttempts) throw;

                    errorCallback?.Invoke(attempts, ex);

                    var message = errorMessage +
                                  $"\nAttemts: ({attempts} of {maxAttempts}).\nRetrying after " +
                                  $"{retryDelay.TotalSeconds} seconds\n";

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        Console.WriteLine(errorMessage);

                    Task.Delay(retryDelay).Wait();
                }
        }


        public static async Task CautiouslyExecuteAsync(Func<Task> operation, string errorMessage, TimeSpan retryDelay,
            int maxAttempts = 5, Action<int, Exception> errorCallback = null)
        {
            var attempts = 0;

            while (true)
                try
                {
                    attempts++;
                    await operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == maxAttempts) throw;

                    errorCallback?.Invoke(attempts, ex);

                    var message = errorMessage +
                                  $"\n Attemts: ({attempts} of {maxAttempts}).\nRetrying after " +
                                  $"{retryDelay.TotalSeconds} seconds";

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        Console.WriteLine(errorMessage);

                    Task.Delay(retryDelay).Wait();
                }
        }

        public static async Task<T> CautiouslyExecuteAsync<T>(Func<Task<T>> operation, string errorMessage,
            TimeSpan retryDelay, int maxAttempts = 5, Action<int, Exception> errorCallback = null)
        {
            var attempts = 0;

            while (true)
                try
                {
                    attempts++;
                    return await operation();
                }
                catch (Exception ex)
                {
                    if (attempts == maxAttempts) throw;

                    errorCallback?.Invoke(attempts, ex);

                    var message = errorMessage +
                                  $"\n Attemts: ({attempts} of {maxAttempts}).\nRetrying after " +
                                  $"{retryDelay.TotalSeconds} seconds";

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        Console.WriteLine(errorMessage);

                    Task.Delay(retryDelay).Wait();
                }
        }

        #endregion
    }
}