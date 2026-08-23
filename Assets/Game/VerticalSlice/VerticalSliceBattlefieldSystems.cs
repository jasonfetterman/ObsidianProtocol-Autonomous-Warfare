using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceBattlefieldCondition
    {
        Normal,
        Fire,
        Smoke,
        HeavySmoke,
        Rain,
        Storm,
        Damaged,
        Destroyed,
        Hazard
    }

    public sealed class VerticalSliceBattlefieldElement
    {
        public string ElementId { get; }

        public string ElementType { get; }

        public VerticalSliceBattlefieldCondition Condition
        {
            get;
            private set;
        }

        public bool Active { get; private set; }

        public VerticalSliceBattlefieldElement(
            string elementId,
            string elementType)
        {
            ElementId =
                elementId ?? string.Empty;

            ElementType =
                elementType ?? string.Empty;

            Condition =
                VerticalSliceBattlefieldCondition.Normal;

            Active = true;
        }

        public bool SetCondition(
            VerticalSliceBattlefieldCondition condition)
        {
            if (!Active)
            {
                return false;
            }

            Condition =
                condition;

            return true;
        }

        public bool SetActive(
            bool active)
        {
            Active =
                active;

            return true;
        }
    }

    public sealed class VerticalSliceBattlefieldSystems
    {
        private readonly Dictionary<
            string,
            VerticalSliceBattlefieldElement> elements =
            new Dictionary<
                string,
                VerticalSliceBattlefieldElement>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ElementCount =>
            elements.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            elements.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterElement(
            string elementId,
            string elementType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(elementId) ||
                string.IsNullOrWhiteSpace(elementType))
            {
                return false;
            }

            string id =
                elementId.Trim();

            if (elements.ContainsKey(id))
            {
                return false;
            }

            elements.Add(
                id,
                new VerticalSliceBattlefieldElement(
                    id,
                    elementType.Trim()));

            return true;
        }

        public bool SetCondition(
            string elementId,
            VerticalSliceBattlefieldCondition condition)
        {
            VerticalSliceBattlefieldElement element =
                GetElement(elementId);

            return element != null &&
                   element.SetCondition(condition);
        }

        public bool SetElementActive(
            string elementId,
            bool active)
        {
            VerticalSliceBattlefieldElement element =
                GetElement(elementId);

            return element != null &&
                   element.SetActive(active);
        }

        public VerticalSliceBattlefieldElement GetElement(
            string elementId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(elementId))
            {
                return null;
            }

            elements.TryGetValue(
                elementId.Trim(),
                out VerticalSliceBattlefieldElement element);

            return element;
        }

        public IReadOnlyCollection<
            VerticalSliceBattlefieldElement>
            GetElements()
        {
            return elements.Values;
        }

        public void Reset()
        {
            elements.Clear();

            Initialized = false;
        }
    }
}
