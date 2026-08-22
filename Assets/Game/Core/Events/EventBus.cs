using System;

namespace ObsidianProtocol.Game.Core
{
    public static class EventBus
    {
        public static void Subscribe<T>(Action<T> handler)
        {
            EventChannel<T>.Subscribe(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            EventChannel<T>.Unsubscribe(handler);
        }

        public static void Publish<T>(T eventData)
        {
            EventChannel<T>.Publish(eventData);
        }

        private static class EventChannel<T>
        {
            private static event Action<T> OnEvent;

            public static void Subscribe(Action<T> handler)
            {
                OnEvent += handler;
            }

            public static void Unsubscribe(Action<T> handler)
            {
                OnEvent -= handler;
            }

            public static void Publish(T eventData)
            {
                OnEvent?.Invoke(eventData);
            }
        }
    }
}
