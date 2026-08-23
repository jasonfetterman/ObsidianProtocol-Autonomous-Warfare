using System;

namespace ObsidianProtocol.Game.VR
{
    public enum VRInputAction
    {
        None,
        Select,
        Confirm,
        Cancel,
        PrimaryAction,
        SecondaryAction,
        Grab,
        Release,
        Move,
        Rotate,
        Activate,
        Menu,
        OperatorExit
    }

    public enum VRHand
    {
        Left,
        Right
    }

    public sealed class VRInputState
    {
        public VRHand Hand { get; }

        public VRInputAction Action { get; }

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        public bool Pressed { get; }

        public VRInputState(
            VRHand hand,
            VRInputAction action,
            float x,
            float y,
            float z,
            bool pressed)
        {
            Hand = hand;
            Action = action;
            X = x;
            Y = y;
            Z = z;
            Pressed = pressed;
        }
    }

    public sealed class VRInputFramework
    {
        private VRInputState leftState;
        private VRInputState rightState;

        public bool Initialized { get; private set; }

        public bool InputEnabled { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            leftState = null;
            rightState = null;

            InputEnabled = true;
            Initialized = true;

            return true;
        }

        public bool SetInputEnabled(
            bool enabled)
        {
            if (!Initialized)
            {
                return false;
            }

            InputEnabled = enabled;

            if (!enabled)
            {
                ClearInput();
            }

            return true;
        }

        public bool SubmitInput(
            VRInputState input)
        {
            if (!Initialized ||
                !InputEnabled ||
                input == null ||
                input.Action == VRInputAction.None)
            {
                return false;
            }

            if (input.Hand == VRHand.Left)
            {
                leftState = input;
            }
            else
            {
                rightState = input;
            }

            return true;
        }

        public VRInputState GetInput(
            VRHand hand)
        {
            return hand == VRHand.Left
                ? leftState
                : rightState;
        }

        public bool IsPressed(
            VRHand hand,
            VRInputAction action)
        {
            VRInputState input =
                GetInput(hand);

            return input != null &&
                   input.Action == action &&
                   input.Pressed;
        }

        public void ClearInput()
        {
            leftState = null;
            rightState = null;
        }

        public void Reset()
        {
            leftState = null;
            rightState = null;

            InputEnabled = false;
            Initialized = false;
        }
    }
}
