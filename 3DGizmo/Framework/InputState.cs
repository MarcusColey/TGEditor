using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace TGEditor
{
    public class InputState
    {
        public KeyboardState lastKeys;
        public KeyboardState currentKeys;

        // The keys that were pressed in the current state
        Keys[] currentPressedKeys;

        public KeyboardState State { get { return currentKeys; } }
        public Keys[] PressedKeys { get { return currentPressedKeys; } }

        // Events for when a key is pressed, released and held.
        public event KeyboardEventHandler<Keys, KeyboardState> KeyPressed;
        public event KeyboardEventHandler<Keys, KeyboardState> KeyReleased;
        public event KeyboardEventHandler<Keys, KeyboardState> KeyHeld;

        public const int MaxInputs = 4;

        public readonly GamePadState[] CurrentGamePadStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

#if !XBOX
        // Nested class to easily ignore when building for Xbox.
        public MouseDevice Mouse;
#endif

        public InputState(GraphicsDevice graphics)
        {

            CurrentGamePadStates = new GamePadState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];

#if !XBOX
            Mouse = new MouseDevice(graphics);
#endif           
        }

        public void Update(GameTime gameTime)
        {
#if !XBOX
            Mouse.Update(gameTime);
#endif
            

            lastKeys = currentKeys;
            currentKeys = Keyboard.GetState();

            currentPressedKeys = currentKeys.GetPressedKeys();

            // For every key on the keyboard
            foreach (Keys key in GetEnumValues<Keys>())
            {
                // If it is a new key press
                if (WasKeyPressed(key))
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

                // If it was just released
                if (WasKeyReleased(key))
                    if (KeyReleased != null)
                        KeyReleased(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

                // If it has been held
                if (WasKeyHeld(key))
                    if (KeyHeld != null)
                        KeyReleased(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

            }

            for (int i = 0; i < MaxInputs; i++)
            {
                LastGamePadStates[i] = CurrentGamePadStates[i];
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }

        // Check is it's down
        public bool IsKeyDown(Keys Key)
        {
            return currentKeys.IsKeyDown(Key);
        }

        // Check if it's up
        public bool IsKeyUp(Keys Key)
        {
            return currentKeys.IsKeyUp(Key);
        }

        public bool WasKeyPressed(Keys Key)
        {
            if (lastKeys.IsKeyUp(Key) && currentKeys.IsKeyDown(Key))
                return true;

            return false;
        }

        public bool WasKeyReleased(Keys Key)
        {
            if (lastKeys.IsKeyDown(Key) && currentKeys.IsKeyUp(Key))
                return true;

            return false;
        }

        public bool WasKeyHeld(Keys Key)
        {
            if (lastKeys.IsKeyDown(Key) && currentKeys.IsKeyDown(Key))
                return true;

            return false;
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        public static List<T> GetEnumValues<T>()
        {
            Type currentEnum = typeof(T);
            List<T> resultSet = new List<T>();

            if (currentEnum.IsEnum)
            {
                FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                    resultSet.Add((T)field.GetValue(null));
            }
            else
                throw new ArgumentException("The argument must be of type Enum or of a type derived from Enum", "T");

            return resultSet;
        }
    }

    public class KeyboardEventArgs<O, S> : EventArgs
    {
        public O Object;

        public InputState Device;

        public KeyboardState State;

        public KeyboardEventArgs(O Object, InputState Device)
        {
            this.Object = Object;
            this.Device = Device;
            this.State = ((InputState)Device).currentKeys;
        }
    }

    public delegate void KeyboardEventHandler<O, S>(object sender, KeyboardEventArgs<O, S> e);
}
