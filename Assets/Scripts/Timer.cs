using UnityEngine;

public class Timer
{
    private float targetTime;     // The time after which the timer completes
    private float currentTime;    // The elapsed time
    private bool isRunning;       // Whether the timer is running

    public Timer(float targetTime)
    {
        this.targetTime = targetTime;
        Reset();
    }

    // Update the timer and check if it has finished
    public bool UpdateAndCheck(float deltaTime)
    {
        if (!isRunning) return false; // Timer is paused/stopped

        currentTime += deltaTime;
        if (currentTime >= targetTime)
        {
            Reset(); // Reset after completion
            return true; // Timer has completed
        }
        return false; // Timer is still running
    }

    // Reset the timer to start again
    public void Reset()
    {
        currentTime = 0f;
        isRunning = true;
    }

    // Stop the timer
    public void Stop()
    {
        isRunning = false;
    }

    // Start or restart the timer
    public void Start()
    {
        isRunning = true;
    }

    // Check if the timer is currently running
    public bool IsRunning()
    {
        return isRunning;
    }

    // Set a new target time for the timer
    public void SetTargetTime(float newTargetTime)
    {
        targetTime = newTargetTime;
    }
}