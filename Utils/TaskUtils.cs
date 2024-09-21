using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Foxworks.Utils
{
	public static class TaskUtils
	{
		private const int MinimumTaskDurationToIntroduceDelay = 100; // Introduce delay if the task was longer than a couple of frames (at 60fps) to avoid flicker

		/// <summary>
		/// Logs the resulting exception from a Task in case of faulty execution.
		/// </summary>
		/// <param name="task">The task to log the exception from</param>
		public static void LogUncaughtException(this Task task)
		{
			task.ContinueWith(t => { UnityEngine.Debug.LogException(t.Exception); }, TaskContinuationOptions.OnlyOnFaulted);
		}

		/// <summary>
		/// Runs the given <paramref name="task"/> and runs an additional delay if the task didn't take more than the given <paramref name="delay"/>
		/// </summary>
		/// <param name="task">Task to run</param>
		/// <param name="delay">Minimum time this task will take to run, in milliseconds</param>
		/// <param name="forceDelay">Will ignore the MinimumTaskDurationToIntroduceDelay parameter if true</param>
		public static async Task WithMinimumDelay(this Task task, int delay, bool forceDelay = false)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();

			await task;

			if ((int) stopwatch.ElapsedMilliseconds < delay && ((int) stopwatch.ElapsedMilliseconds > MinimumTaskDurationToIntroduceDelay || forceDelay))
			{
				await Task.Delay(delay - (int) stopwatch.ElapsedMilliseconds);
			}
		}

		/// <summary>
		/// Waits until the given predicate becomes <c>true</c> with a limit time of timeout.
		/// </summary>
		/// <param name="predicate">A predicate which returns true when ready.</param>
		/// <param name="timeout">Timeout duration in seconds.</param>
		/// <returns>The Task representing the asynchronous wait.</returns>
		/// <exception cref="TimeoutException">In case of timeout.</exception>
		public static async Task WaitUntilWithTimeout(Func<bool> predicate, double timeout)
		{
			DateTime startTime = DateTime.UtcNow;
			while (!predicate() && (DateTime.UtcNow - startTime).TotalSeconds <= timeout)
			{
				await Task.Delay(17);
			}

			// Check timeout
			bool hasTimeout = (DateTime.UtcNow - startTime).TotalSeconds > timeout;
			if (hasTimeout)
			{
				throw new TimeoutException("Async operation timed out.");
			}
		}

		/// <summary>
		/// Waits for the specified amount of frames.
		/// </summary>
		/// <param name="frames"></param>
		public static async Task WaitXFrames(int frames)
		{
			int targetFrame = Time.frameCount + frames;
			while (Time.frameCount < targetFrame)
			{
				await Task.Yield();
			}
		}

		/// <summary>
		/// Waits until all tasks have been executed one after another.
		/// Any task failure will stop the sequence of executions.
		/// </summary>
		/// <param name="taskWrappers"></param>
		/// <returns>The task that represents the completion of all of the supplied tasks.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static async Task WhenAllSequentialAsync(this IEnumerable<Func<Task>> taskWrappers)
		{
			if (taskWrappers is null)
			{
				throw new ArgumentNullException(nameof(taskWrappers));
			}

			foreach (Func<Task> taskWrapper in taskWrappers)
			{
				await taskWrapper.Invoke();
			}
		}

		/// <summary>
		/// Waits until all tasks have been executed one after another.
		/// Any task failure will stop the sequence of executions.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="taskWrappers"></param>
		/// <returns>The list of the tasks results.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static async Task<List<T>> WhenAllSequentialAsync<T>(this IEnumerable<Func<Task<T>>> taskWrappers)
		{
			if (taskWrappers is null)
			{
				throw new ArgumentNullException(nameof(taskWrappers));
			}

			List<T> results = new();
			foreach (Func<Task<T>> taskWrapper in taskWrappers)
			{
				results.Add(await taskWrapper.Invoke());
			}

			return results;
		}

		/// <summary>
		/// Catches any thrown exceptions from the provided task.
		/// Invokes the optional errorHandler callback with the caught exception.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="errorHandler"></param>
		/// <returns></returns>
		public static async Task TryAsync(this Task task, Action<Exception> errorHandler = null)
		{
			try
			{
				await task;
			}
			catch (Exception exception)
			{
				errorHandler?.Invoke(exception);
			}
		}

		/// <summary>
		/// Catches any thrown exceptions from the provided task.
		/// Invokes the optional errorHandler callback with the caught exception.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="errorHandler"></param>
		/// <returns></returns>
		[return: MaybeNull]
		public static async Task<T> TryAsync<T>(this Task<T> task, Action<Exception> errorHandler = null) where T : class
		{
			try
			{
				return await task;
			}
			catch (Exception exception)
			{
				errorHandler?.Invoke(exception);
				return null;
			}
		}

		/// <summary>
		/// Throws a TimeoutException is the TimeSpan timeout has expired during the execution of the task.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="timeout"></param>
		/// <param name="throwTimeoutException"></param>
		/// <returns></returns>
		/// <exception cref="TimeoutException"></exception>
		public static async Task TimeoutAfter(this Task task, TimeSpan timeout, bool throwTimeoutException = true)
		{
			using CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource();

			Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
			if (completedTask == task)
			{
				timeoutCancellationTokenSource.Cancel();
				await task; // Very important in order to propagate exceptions
				return;
			}

			if (throwTimeoutException)
			{
				throw new TimeoutException("The operation has timed out.");
			}
		}

		/// <summary>
		/// Throws a TimeoutException is the TimeSpan timeout has expired during the execution of the task.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="timeout"></param>
		/// <param name="throwTimeoutException"></param>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		/// <exception cref="TimeoutException"></exception>
		public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, bool throwTimeoutException = true)
		{
			using CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource();

			Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
			if (completedTask == task)
			{
				timeoutCancellationTokenSource.Cancel();
				return await task; // Very important in order to propagate exceptions
			}

			if (throwTimeoutException)
			{
				throw new TimeoutException("The operation has timed out.");
			}

			return default;
		}

		/// <summary>
		/// Throws an OperationCanceledException if the cancellation token is canceled during the execution of the task.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="token"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static async Task<T> ThrowIfCancelled<T>(this Task<T> task, CancellationToken token)
		{
			if (token != default)
			{
				token.ThrowIfCancellationRequested();
				await Task.WhenAny(task, token.WhenCanceled());
				token.ThrowIfCancellationRequested();
			}

			return await task;
		}

		/// <summary>
		/// Returns a task that completes when the cancellationToken is canceled.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private static Task WhenCanceled(this CancellationToken cancellationToken)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			cancellationToken.Register(s => ((TaskCompletionSource<bool>) s).SetResult(true), taskCompletionSource);
			return taskCompletionSource.Task;
		}
	}
}