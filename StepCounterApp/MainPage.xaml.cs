using System;
using Microsoft.Maui.Controls;

namespace StepCounterApp;

public partial class MainPage : ContentPage
{
    private readonly IStepCounterService _steps;
    private int _lastDisplayedSteps = 0;

    public MainPage(IStepCounterService steps)
    {
        InitializeComponent();
        _steps = steps ?? throw new ArgumentNullException(nameof(steps));

        _steps.StepUpdated += OnStepUpdated;
        _steps.StatusChanged += OnStatusChanged;
    }

    private void OnStatusChanged(object? sender, string status)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StatusLabel.Text = $"Status: {status}";
        });
    }

    private void OnStepUpdated(object? sender, int stepCount)
    {
        _lastDisplayedSteps = stepCount;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StepCountLabel.Text = $"Steps: {stepCount}";
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StatusLabel.Text = "Status: idle";
        StepCountLabel.Text = "Steps: 0";
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        StartButton.IsEnabled = false;
        StopButton.IsEnabled = true;
        StatusLabel.Text = "Status: starting…";
        var ok = await _steps.StartAsync();
        if (!ok)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            StatusLabel.Text = "Status: permission denied or not available";
        }
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        _steps.Stop();
        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
        StatusLabel.Text = "Status: stopped";
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _steps.Stop();
        _steps.StepUpdated -= OnStepUpdated;
        _steps.StatusChanged -= OnStatusChanged;
    }
}