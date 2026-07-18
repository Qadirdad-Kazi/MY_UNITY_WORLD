using UnityEngine;
using UnityEngine.InputSystem;

namespace MyWorld.Core
{
    /// <summary>
    /// Input helpers for projects using the new Input System package only.
    /// </summary>
    public static class GameInput
    {
        public static bool KeyDown(Key key)
        {
            var kb = Keyboard.current;
            return kb != null && kb[key].wasPressedThisFrame;
        }

        public static bool KeyHeld(Key key)
        {
            var kb = Keyboard.current;
            return kb != null && kb[key].isPressed;
        }

        public static float Horizontal
        {
            get
            {
                float v = 0f;
                var kb = Keyboard.current;
                if (kb != null)
                {
                    if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) v -= 1f;
                    if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) v += 1f;
                }
                var gp = Gamepad.current;
                if (gp != null) v += gp.leftStick.ReadValue().x;
                return Mathf.Clamp(v, -1f, 1f);
            }
        }

        public static float Vertical
        {
            get
            {
                float v = 0f;
                var kb = Keyboard.current;
                if (kb != null)
                {
                    if (kb.sKey.isPressed || kb.downArrowKey.isPressed) v -= 1f;
                    if (kb.wKey.isPressed || kb.upArrowKey.isPressed) v += 1f;
                }
                var gp = Gamepad.current;
                if (gp != null) v += gp.leftStick.ReadValue().y;
                return Mathf.Clamp(v, -1f, 1f);
            }
        }

        public static Vector2 MouseDelta
        {
            get
            {
                var mouse = Mouse.current;
                return mouse != null ? mouse.delta.ReadValue() * 0.05f : Vector2.zero;
            }
        }

        public static bool SprintHeld =>
            KeyHeld(Key.LeftShift) || KeyHeld(Key.RightShift);

        public static bool JumpPressed =>
            KeyDown(Key.Space);
    }
}
