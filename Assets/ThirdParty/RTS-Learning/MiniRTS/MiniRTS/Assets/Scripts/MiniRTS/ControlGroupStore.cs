using System;
using System.Collections.Generic;

namespace MiniRTS
{
    /// <summary>
    /// Pure, one-based control-group storage. Assignments are copied so later
    /// selection changes cannot mutate a saved group.
    /// </summary>
    public sealed class ControlGroupStore<T>
    {
        private readonly List<T>[] groups;

        public int GroupCount => groups.Length;

        public ControlGroupStore(int groupCount)
        {
            if (groupCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(groupCount));
            }

            groups = new List<T>[groupCount];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new List<T>();
            }
        }

        public void Assign(int groupNumber, IReadOnlyList<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<T> group = GetGroup(groupNumber);
            group.Clear();
            for (int i = 0; i < values.Count; i++)
            {
                group.Add(values[i]);
            }
        }

        public void Clear(int groupNumber)
        {
            GetGroup(groupNumber).Clear();
        }

        public IReadOnlyList<T> Recall(int groupNumber)
        {
            return new List<T>(GetGroup(groupNumber));
        }

        private List<T> GetGroup(int groupNumber)
        {
            if (groupNumber < 1 || groupNumber > groups.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(groupNumber));
            }

            return groups[groupNumber - 1];
        }
    }
}
