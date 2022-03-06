using System;
using System.Collections.Generic;

public class TimerManager
{
    private class TimerInstance
    {
        public float TotalTime;
        public float TimeLeft;
        public float Pct
        {
            get
            {
                return 1f - (TimeLeft / TotalTime);
            }
        }

        public Action<object> CompleteCallback;
        public Action<float, float, object> UpdateCallback;
        public object Cookie;
    }

    private List<TimerInstance> timers;
    private Stack<TimerInstance> timersToRemove;

    public TimerManager()
    {
        timers = new List<TimerInstance>();
        timersToRemove = new Stack<TimerInstance>();
        Service.UpdateManager.AddObserver(Update, true);
    }

    public void CreateTimer(
        float duration, 
        Action<object> completeCallback, 
        Action<float, float, object> updateCallback, 
        object cookie)
    {
        TimerInstance timer = new TimerInstance 
        {
            TimeLeft = duration,
            CompleteCallback = completeCallback,
            UpdateCallback = updateCallback,
            Cookie = cookie 
        };

        timers.Add(timer);
    }

    public void Update(float dt)
    {
        for (int i = 0, count = timers.Count; i < count; ++i)
        {
            timers[i].TimeLeft -= dt;
            if (timers[i].TimeLeft <= 0)
            {
                timers[i].CompleteCallback.Invoke(timers[i].Cookie);
                timersToRemove.Push(timers[i]);
            }
            else if(timers[i].UpdateCallback != null)
            {
                timers[i].UpdateCallback(dt, timers[i].Pct, timers[i].Cookie);
            }
        }

        while(timersToRemove.Count > 0)
        {
            timers.Remove(timersToRemove.Pop());
        }
    }
}
