namespace ShouldMethodAssertion.ShouldExtensions;

public static class Invoke
{
    public static Action That(Action action) => action;
    public static Action<T1> That<T1>(Action<T1> action) => action;
    public static Action<T1, T2> That<T1, T2>(Action<T1, T2> action) => action;
    public static Action<T1, T2, T3> That<T1, T2, T3>(Action<T1, T2, T3> action) => action;
    public static Action<T1, T2, T3, T4> That<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => action;
    public static Action<T1, T2, T3, T4, T5> That<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6> That<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7> That<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8> That<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> That<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) => action;
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) => action;

    public static Func<TResult> That<TResult>(Func<TResult> func) => func;
    public static Func<T1, TResult> That<T1, TResult>(Func<T1, TResult> func) => func;
    public static Func<T1, T2, TResult> That<T1, T2, TResult>(Func<T1, T2, TResult> func) => func;
    public static Func<T1, T2, T3, TResult> That<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) => func;
    public static Func<T1, T2, T3, T4, TResult> That<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, TResult> That<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, TResult> That<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> That<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func) => func;
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> That<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func) => func;
}
