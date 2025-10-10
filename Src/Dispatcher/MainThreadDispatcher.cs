using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ArchipelagoMod.Src.Dispatcher
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();
        private static int _mainThreadId;

        void Awake()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Enqueue an action to be executed on the main Unity thread.
        /// </summary>
        public static void Enqueue(Action action)
        {
            if (Thread.CurrentThread.ManagedThreadId == _mainThreadId)
            {
                // If we’re already on the main thread, execute immediately
                action();
            }
            else
            {
                lock (_executionQueue)
                {
                    _executionQueue.Enqueue(action);
                }
            }
        }

        void Update()
        {
            // Execute queued actions on main thread
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    var action = _executionQueue.Dequeue();
                    action?.Invoke();
                }
            }
        }
    }
}

