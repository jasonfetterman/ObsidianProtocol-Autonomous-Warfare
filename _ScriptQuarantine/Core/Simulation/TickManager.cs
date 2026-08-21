using System;
using System.Collections.Generic;

public class TickManager
{
    private readonly List<Action> tickActions = new();

    public void Register(Action action)
    {
        if (action == null)
            return;

        if (!tickActions.Contains(action))
            tickActions.Add(action);
    }

    public void Unregister(Action action)
    {
        if (action == null)
            return;

        if (tickActions.Contains(action))
            tickActions.Remove(action);
    }

    // Called every frame by TickDriver
    public void Tick()
    {
        for (int i = 0; i < tickActions.Count; i++)
        {
            tickActions[i]?.Invoke();
        }
    }
}

