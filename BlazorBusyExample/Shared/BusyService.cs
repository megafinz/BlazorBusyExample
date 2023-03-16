namespace BlazorBusyExample.Shared;

using System.Collections.Concurrent;

internal sealed class BusyState
{
    private class ActivatedBusyState : IDisposable
    {
        private readonly BusyState _busyState;

        public ActivatedBusyState(BusyState busyState)
        {
            _busyState = busyState;
            _busyState.IncreaseBusiness();
        }

        public void Dispose()
        {
            _busyState.DecreaseBusiness();
        }
    }

    private readonly object _lock = new();
    private int _busyCount;
    
    public bool IsBusy => _busyCount > 0;

    private void IncreaseBusiness() => HandleBusinessChange(() => _busyCount++);

    private void DecreaseBusiness() => HandleBusinessChange(() => _busyCount--);

    private void HandleBusinessChange(Action action)
    {
        lock (_lock)
        {
            var oldValue = IsBusy;
            
            action();

            _busyCount = Math.Max(0, _busyCount);
            
            var newValue = IsBusy;
            
            if (oldValue != newValue)
            {
                Changed?.Invoke();
            }
        }
    }

    public event Action? Changed;
    
    public IDisposable Activate() => new ActivatedBusyState(this);
}

internal sealed class BusyService
{
    private readonly ConcurrentDictionary<string, BusyState> _busyStates = new();

    public BusyState GetBusyState(string key)
    {
        return _busyStates.GetOrAdd(key, _ => new BusyState());
    }

    public IDisposable UseBusyState(string key)
    {
        return GetBusyState(key).Activate();
    }
}
