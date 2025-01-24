using System;
using System.Collections;

public class GSC_CommandHandler
{
    private Func<GSC_ContainerUnit, IEnumerator> Coroutine;
    private Action<GSC_ContainerUnit> OnTerminate;
    public GSC_ContainerUnit Unit { get; private set; }
    
    public GSC_CommandHandler(Func<GSC_ContainerUnit, IEnumerator> coroutine,
        Action<GSC_ContainerUnit> onTerminate)
    {
        Coroutine = coroutine;
        OnTerminate = onTerminate;
    }

    public string HandlerName => Unit.Calling;

    public IEnumerator Run()
    {
        yield return Coroutine(Unit);
    }

    public void OnEnd() => OnTerminate?.Invoke(Unit);
    public void SetUnit(GSC_ContainerUnit unit) => Unit = unit;
    public bool IsCoroutine() => Coroutine != null;
}

