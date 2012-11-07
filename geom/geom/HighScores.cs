using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace geom
{
    class HighScores
    {

        public int highScore { get; private set; }
   

        public HighScores()
        {

            highScore = 0;
 
        }

        public void SetHighScore(int score)
        {
            highScore = score;
        }

    }
}
