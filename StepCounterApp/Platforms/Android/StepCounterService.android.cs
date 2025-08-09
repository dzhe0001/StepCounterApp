using System;
using System.Threading.Tasks;

namespace StepCounterApp;

public class StepCounterService : IStepCounterService
{
    public event EventHandler<int>? StepUpdated;
    public event EventHandler<string>? StatusChanged;

    public Task<bool> StartAsync()
    {
        StatusChanged?.Invoke(this, "not implemented on Android in this sample");
        return Task.FromResult(false);
    }

    public void Stop()
    {
        StatusChanged?.Invoke(this, "stopped");
    }
}
