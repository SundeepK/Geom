using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace geom
{
  public  class InputAction
    {
        private readonly Keys[] keys;   

        public InputAction(Keys[] keys, bool newPressOnly)
        {   
            this.keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];
        }

        public bool Evaluate(InputState state)
        {

            foreach (Keys key in keys)
            {
                if (state.IsNewKeyPress(key))
                    return true;
            }


            return false;
        }

    }
}
