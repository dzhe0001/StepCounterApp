using System;
using System.Threading.Tasks;
using CoreMotion;
using Foundation;

namespace StepCounterApp;

public class StepCounterService : IStepCounterService
{
    private readonly CMPedometer _pedometer = new CMPedometer();
    private NSDate? _startDate;

    public event EventHandler<int>? StepUpdated;
    public event EventHandler<string>? StatusChanged;

    public async Task<bool> StartAsync()
    {
        try
        {
            if (!CMPedometer.IsStepCountingAvailable)
            {
                StatusChanged?.Invoke(this, "Step counting not available");
                return false;
            }

            _startDate = NSDate.Now;
            StatusChanged?.Invoke(this, "running");

            _pedometer.StartPedometerUpdates(_startDate!, (data, error) =>
            {
                if (error != null)
                {
                    StatusChanged?.Invoke(this, $"error: {error.LocalizedDescription}");
                    return;
                }

                if (data?.NumberOfSteps != null)
                {
                    var steps = data.NumberOfSteps.Int32Value;
                    StepUpdated?.Invoke(this, steps);
                }
            });

            await Task.Delay(200);
            return true;
        }
        catch (Exception ex)
        {
            StatusChanged?.Invoke(this, $"error: {ex.Message}");
            return false;
        }
    }

    public void Stop()
    {
        _pedometer.StopPedometerUpdates();
        StatusChanged?.Invoke(this, "stopped");
    }
}
