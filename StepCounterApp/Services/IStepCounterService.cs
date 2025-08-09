using System;
using System.Threading.Tasks;

namespace StepCounterApp;

public interface IStepCounterService
{
    event EventHandler<int>? StepUpdated;
    event EventHandler<string>? StatusChanged;

    Task<bool> StartAsync();

    void Stop();
}
