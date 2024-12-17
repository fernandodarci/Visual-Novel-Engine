using System;
using System.Collections;

public abstract class GSC_CommandHandlerBase 
{
    public abstract bool IsCoroutine();
    public abstract bool MoveNext();
    public abstract object GetCurrent();
    public abstract void TerminateHandler();
}

public class GSC_CommandHandler : GSC_CommandHandlerBase
{
    public GSC_CommandHandler(Func<GSC_ContainerUnit, IEnumerator> coroutine,
        Action<GSC_ContainerUnit> onTerminate)
    {
        Coroutine = coroutine;
        OnTerminate = onTerminate;
        Unit = null;
    }

    public Func<GSC_ContainerUnit, IEnumerator> Coroutine { get; private set; }
    private Action<GSC_ContainerUnit> OnTerminate { get; set; }
    private GSC_ContainerUnit Unit { get; set; }

    public override bool IsCoroutine() => Coroutine != null;
    public override bool MoveNext() => Coroutine(Unit).MoveNext();
    public override object GetCurrent() => Coroutine(Unit).Current;
    public override void TerminateHandler() => OnTerminate?.Invoke(Unit);
    public void AttachUnit(GSC_ContainerUnit unit) => Unit = unit;
    
}

public class GSC_CommandHandler<TParam> : GSC_CommandHandlerBase
{
    public GSC_CommandHandler(Func<GSC_ContainerUnit<TParam>, IEnumerator> coroutine,
        Action<GSC_ContainerUnit<TParam>> onTerminate)
    {
        Coroutine = coroutine;
        OnTerminate = onTerminate;
    }

    public Func<GSC_ContainerUnit<TParam>, IEnumerator> Coroutine { get; private set; }
    private Action<GSC_ContainerUnit<TParam>> OnTerminate { get; set; }
    private GSC_ContainerUnit<TParam> Unit { get; set; }

    public override bool IsCoroutine() => Coroutine != null;
    public override bool MoveNext() => Coroutine(Unit).MoveNext();
    public override object GetCurrent() => Coroutine(Unit).Current;
    public override void TerminateHandler() => OnTerminate?.Invoke(Unit);

    public void AttachUnit(GSC_ContainerUnit<TParam> unit) => Unit = unit;
    
}
