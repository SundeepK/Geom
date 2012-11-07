using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace geom
{
    public class InputState
    {
        public const int MaxInputs = 4;


        public  KeyboardState CurrentKeyboardStates;

        public  KeyboardState LastKeyboardStates;

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
        CurrentKeyboardStates = new KeyboardState();
        
        LastKeyboardStates = new KeyboardState();
        
        }

        /// <summary>
        /// Reads the latest state user input.
        /// </summary>
        public void Update()
        {
            LastKeyboardStates = CurrentKeyboardStates;
            CurrentKeyboardStates = Keyboard.GetState();

        }

        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardStates.IsKeyDown(key) && LastKeyboardStates.IsKeyUp(key));
        }
    }
}
