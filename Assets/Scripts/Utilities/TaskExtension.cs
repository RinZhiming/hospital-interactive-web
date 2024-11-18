using System;
using System.Threading;
using System.Threading.Tasks;

public static class TaskExtension
{
    public static Task ContinueWithMainThread(this Task task, Action<Task> continueAction)
    {
        return task.ContinueWith(
            continueAction, 
            CancellationToken.None, 
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    public static Task<T> ContinueWithMainThread<T>(this Task<T> task, Action<Task<T>> continueAction)
    {
        return (Task<T>)task.ContinueWith(
            continueAction,
            CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
    }
}