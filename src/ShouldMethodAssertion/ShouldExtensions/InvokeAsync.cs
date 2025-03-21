using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.ShouldExtensions;

public struct InvokeAsync
{
    public static InvokeAsync That(Func<Task> asyncAction) => new InvokeAsync(asyncAction);
    public static InvokeAsync That<T1>(Func<T1, Task> asyncAction, T1 arg1) => new InvokeAsync(async () => await asyncAction(arg1).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2>(Func<T1, T2, Task> asyncAction, T1 arg1, T2 arg2) => new InvokeAsync(async () => await asyncAction(arg1, arg2).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3>(Func<T1, T2, T3, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ConfigureAwait(false));
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ConfigureAwait(false));


    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That(Func<ValueTask> asyncAction) => new InvokeAsync(async () => await asyncAction().ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1>(Func<T1, ValueTask> asyncAction, T1 arg1) => new InvokeAsync(async () => await asyncAction(arg1).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2>(Func<T1, T2, ValueTask> asyncAction, T1 arg1, T2 arg2) => new InvokeAsync(async () => await asyncAction(arg1, arg2).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3>(Func<T1, T2, T3, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4>(Func<T1, T2, T3, T4, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ValueTask> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => new InvokeAsync(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ConfigureAwait(false));


    public static InvokeAsync<TResult> That<TResult>(Func<Task<TResult>> asyncFunc) => new InvokeAsync<TResult>(asyncFunc);
    public static InvokeAsync<TResult> That<T1, TResult>(Func<T1, Task<TResult>> asyncAction, T1 arg1) => new InvokeAsync<TResult>(async () => await asyncAction(arg1).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, TResult>(Func<T1, T2, Task<TResult>> asyncAction, T1 arg1, T2 arg2) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ConfigureAwait(false));
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ConfigureAwait(false));


    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<TResult>(Func<ValueTask<TResult>> asyncFunc) => new InvokeAsync<TResult>(async () => await asyncFunc().ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, TResult>(Func<T1, ValueTask<TResult>> asyncAction, T1 arg1) => new InvokeAsync<TResult>(async () => await asyncAction(arg1).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, TResult>(Func<T1, T2, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, TResult>(Func<T1, T2, T3, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ConfigureAwait(false));
    [OverloadResolutionPriority(-1)]
    public static InvokeAsync<TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ValueTask<TResult>> asyncAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => new InvokeAsync<TResult>(async () => await asyncAction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ConfigureAwait(false));


    internal Func<Task> AsyncAction { get; }

    private InvokeAsync(Func<Task> asyncAction)
    {
        AsyncAction = asyncAction;
    }
}

public struct InvokeAsync<TResult>
{
    internal Func<Task<TResult>> AsyncFunc { get; }

    internal InvokeAsync(Func<Task<TResult>> asyncFunc)
    {
        AsyncFunc = asyncFunc;
    }
}