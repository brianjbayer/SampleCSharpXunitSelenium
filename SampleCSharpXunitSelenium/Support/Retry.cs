using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCSharpXunitSelenium.Support
{
    public static class Retry
    {
        public static int MaxRetry = 7;
        public static int SleepDuration = 2000;

        public static void WithRetry(Action retryAction, int numberOfRetries = 0, int sleepMillisecondsOnError = 0)
        {
            numberOfRetries = numberOfRetries < 1 ? Retry.MaxRetry : numberOfRetries;
            sleepMillisecondsOnError = sleepMillisecondsOnError < 1 ? Retry.SleepDuration : sleepMillisecondsOnError;
            int tries = 0;
            do
            {
                try
                {
                    retryAction();
                    return;
                }
                catch
                {
                    tries++;
                    if (tries >= numberOfRetries)
                    {
                        throw;
                    }

                    //wait for retry
                    Thread.Sleep(sleepMillisecondsOnError * tries);

                }
            } while (true);
        }

        public static async Task WithRetry(Func<Task> retryAction, int numberOfRetries = 0, int sleepMillisecondsOnError = 0)
        {
            numberOfRetries = numberOfRetries < 1 ? Retry.MaxRetry : numberOfRetries;
            sleepMillisecondsOnError = sleepMillisecondsOnError < 1 ? Retry.SleepDuration : sleepMillisecondsOnError;
            int tries = 0;
            do
            {
                try
                {
                    await retryAction();
                    return;
                }
                catch
                {
                    tries++;
                    if (tries >= numberOfRetries)
                    {
                        throw;
                    }

                    //wait for retry
                    Thread.Sleep(sleepMillisecondsOnError * tries);

                }
            } while (true);
        }
    }
}
