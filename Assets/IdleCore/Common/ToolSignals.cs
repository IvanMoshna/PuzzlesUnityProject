using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ToolSignals
    {
        public readonly Dictionary<int, List<IRecieve>> signals = new Dictionary<int, List<IRecieve>>(new FastDict());

        public void Add(IRecieve recieve, Type type)
        {
            List<IRecieve> cachedSignals;
            if (signals.TryGetValue(type.GetHashCode(), out cachedSignals))
            {
                cachedSignals.Add(recieve);
                return;
            }

            signals.Add(type.GetHashCode(), new List<IRecieve> {recieve});
        }

        public void Remove(IRecieve recieve, Type type)
        {
            List<IRecieve> cachedSignals;
            if (signals.TryGetValue(type.GetHashCode(), out cachedSignals))
            {
                cachedSignals.Remove(recieve);
            }
        }


        public void Add(object obj)
        {
            var all = obj.GetType().GetInterfaces();
            var reciever = obj as IRecieve;
            foreach (var intType in all)
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
                {
                    Add(reciever, intType.GetGenericArguments()[0]);
                }
            }
        }

        public void Remove(object obj)
        {
            var all = obj.GetType().GetInterfaces();
            var reciever = obj as IRecieve;
            foreach (var intType in all)
            {
                if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IReceive<>))
                {
                    Remove(reciever, intType.GetGenericArguments()[0]);
                }
            }
        }

        public void Send<T>(T val = default(T)) where T : new()
        {
            List<IRecieve> cachedSignals;

            if (!signals.TryGetValue(typeof(T).GetHashCode(), out cachedSignals)) return;
            var len = cachedSignals.Count;
            for (var i = 0; i < len; i++)
            {
                (cachedSignals[i] as IReceive<T>).HandleSignal(val);
            }
        }

        public void PrintReceivers<T>()
        {
            List<IRecieve> cachedSignals;

            if (!signals.TryGetValue(typeof(T).GetHashCode(), out cachedSignals)) return;
            var len = cachedSignals.Count;
            for (var i = 0; i < len; i++)
            {
                Debug.Log(((cachedSignals[i] as IReceive<T>) as MonoBehaviour).gameObject);
            }
        }

        public void Dispose()
        {
            signals.Clear();
        }

        public class FastDict : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }
    }

    public interface IReceive<T> : IRecieve
    {
        void HandleSignal(T signal);
    }

    public interface IRecieve
    {
    }
}