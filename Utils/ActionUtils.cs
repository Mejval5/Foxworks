using System;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Foxworks.Utils
{
    public static class ActionUtils
    {
	    /// <summary>
	    ///     This method invokes the given action with the given parameter.
	    ///     No exception is thrown if subscriber throws an exception.
	    ///     The exceptions of the subscribers are logged.
	    /// </summary>
	    /// <param name="action">The action</param>
	    public static void InvokeAndLogExceptions(Action action)
        {
            InvokeAndLogExceptionsPrivate(action);
        }

	    /// <summary>
	    ///     This method invokes the given action with the given parameter.
	    ///     No exception is thrown if subscriber throws an exception.
	    ///     The exceptions of the subscribers are logged.
	    /// </summary>
	    /// <typeparam name="T">Type of param</typeparam>
	    /// <param name="action">The action</param>
	    /// <param name="param">The parameter</param>
	    public static void InvokeAndLogExceptions<T>(Action<T> action, T param)
        {
            InvokeAndLogExceptionsPrivate(action, param);
        }

	    /// <summary>
	    ///     This method invokes the given action with the given parameter.
	    ///     No exception is thrown if subscriber throws an exception.
	    ///     The exceptions of the subscribers are logged.
	    /// </summary>
	    /// <typeparam name="T1">Type of param 1</typeparam>
	    /// <typeparam name="T2">Type of param 2</typeparam>
	    /// <param name="action">The action</param>
	    /// <param name="param1">The first parameter</param>
	    /// <param name="param2">The second parameter</param>
	    public static void InvokeAndLogExceptions<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2)
        {
            InvokeAndLogExceptionsPrivate(action, param1, param2);
        }

	    /// <summary>
	    ///     This method invokes the given action with the given parameter.
	    ///     No exception is thrown if subscriber throws an exception.
	    ///     The exceptions of the subscribers are logged.
	    /// </summary>
	    /// <typeparam name="T1">Type of param 1</typeparam>
	    /// <typeparam name="T2">Type of param 2</typeparam>
	    /// <typeparam name="T3">Type of param 3</typeparam>
	    /// <param name="action">The action</param>
	    /// <param name="param1">The first parameter</param>
	    /// <param name="param2">The second parameter</param>
	    /// <param name="param3">The third parameter</param>
	    public static void InvokeAndLogExceptions<T1, T2, T3>(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
        {
            InvokeAndLogExceptionsPrivate(action, param1, param2, param3);
        }

        private static void InvokeAndLogExceptionsPrivate(MulticastDelegate action, params object[] parameters)
        {
            if (action == null)
            {
                Debug.LogError("Error trying to dispatch a null action.");
                return;
            }

            foreach (Delegate localDelegate in action.GetInvocationList())
            {
                try
                {
                    localDelegate.Method.Invoke(localDelegate.Target, parameters);
                }
                catch (TargetInvocationException targetInvocationException)
                {
                    // The inner exception has the proper call stack
                    Exception exception = targetInvocationException.InnerException ?? targetInvocationException;

                    // Find the exception throw location
                    string throwLocationInfo = FindExceptionThrowLocationInfo(exception);
                    if (string.IsNullOrEmpty(throwLocationInfo))
                    {
                        throwLocationInfo = "[Unknown location]";
                    }

                    // Find the event raise location
                    string raiseLocationInfo = GetCurrentLocationInfo(3);
                    if (string.IsNullOrEmpty(raiseLocationInfo))
                    {
                        raiseLocationInfo = "[Unknown location]";
                    }

                    Debug.LogError($"An exception occured in {throwLocationInfo} while dispatching event raised in {raiseLocationInfo}. Exception: {exception}");
                }
                catch (Exception exception)
                {
                    // This is highly unlikely to occur
                    Debug.LogError($"An exception occured while dispatching an event. Exception: {exception}");
                }
            }
        }

        private static string GetCurrentLocationInfo(int stackFramesToSkip = 0)
        {
            // Get the current StackTrace
            StackTrace stackTrace = new(true);

            StackFrame stackFrame = stackTrace.GetFrame(stackFramesToSkip);
            if (stackFrame == null)
            {
                return string.Empty;
            }

            MethodBase stackFrameMethod = stackFrame.GetMethod();
            if (stackFrameMethod == null)
            {
                return string.Empty;
            }

            Type stackFrameType = stackFrameMethod.DeclaringType;
            string className = stackFrameType != null ? stackFrameType.FullName : "N/A";

            return className + ":" + stackFrameMethod.Name + ":" + stackFrame.GetFileLineNumber();
        }

        private static string FindExceptionThrowLocationInfo(Exception exception, int stackFramesToSkip = 0)
        {
            // Get the current StackTrace
            StackTrace stackTrace = new(exception, true);

            StackFrame stackFrame = stackTrace.GetFrame(stackFramesToSkip);
            if (stackFrame == null)
            {
                return string.Empty;
            }

            MethodBase stackFrameMethod = stackFrame.GetMethod();
            if (stackFrameMethod == null)
            {
                return string.Empty;
            }

            Type stackFrameType = stackFrameMethod.DeclaringType;
            string className = stackFrameType != null ? stackFrameType.FullName : "N/A";

            return className + ":" + stackFrameMethod.Name + ":" + stackFrame.GetFileLineNumber();
        }
    }
}