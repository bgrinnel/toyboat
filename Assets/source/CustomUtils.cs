using System;
using UnityEngine;

public static class CustomUtils 
{

    public class TimeoutCondition{
        private float Timeout;
        private float ElapsedTime;
        private Func<bool> Condition;
        private Action OnTimeout;
        public TimeoutCondition(Func<bool> condition, float timeout, System.Action onTimeout = null) {
            Timeout = timeout;
            Condition = condition;
            OnTimeout = onTimeout;
            ElapsedTime = 0f;
        }

        public bool WaitingFor() {
            ElapsedTime += Time.deltaTime;
            bool not_condition = !Condition.Invoke();
            if (ElapsedTime >= Timeout && not_condition)
            {
                OnTimeout?.Invoke();
            }
            return not_condition;
        }
    }
    public static System.Collections.IEnumerator Timer(float duration, System.Action onCompletion = null) 
    {
        yield return new WaitForSeconds(duration);

        onCompletion?.Invoke();
    }

    public static System.Collections.IEnumerator WaitFor(Func<bool> condition, float timeout, System.Action onSuccess, System.Action onTimeout) 
    {
        float elapsedTime = 0f;

        while (!condition())
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeout)
            {
                onTimeout?.Invoke();
                yield break;  // Exit the coroutine
            }
            yield return null;
        }

        // If the condition was met within the timeout, call the success callback
        onSuccess?.Invoke();
    }

    public static Vector2 To2D(Vector3 vec3)
    {
        return new Vector2(vec3.z, vec3.x);
    }

    public static Vector3 To3D(Vector2 vec2)
    {
        return new Vector3(vec2.y, 0f, vec2.x);
    }
}