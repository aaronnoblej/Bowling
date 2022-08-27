using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Frame
    {
        public enum FrameType
        {
            Strike,
            Spare,
            Open,
            Tenth
        }

        // PROPERTIES
        public List<int> Shots { get; set; }
        public FrameType Type { get; private set; }
        public int Score { get; set; } = 0;

        // CONSTRUCTORS
        public Frame(List<int> shots, bool tenth)
        {
            this.Shots = shots;
            SetType(tenth);
        }

        /// <summary>
        /// Finds and sets the type of frame (strike, spare, open, or tenth frame) based on the shots.
        /// </summary>
        private void SetType(bool tenth = false)
        {
            if (Shots.Count == 3 || tenth)
            {
                this.Type = FrameType.Tenth;
            }
            else if (Shots[0] == 10 && Shots.Count == 1)
            {
                this.Type = FrameType.Strike;
            }
            else if (Shots[0] + Shots[1] == 10)
            {
                this.Type = FrameType.Spare;
            }
            else
            {
                this.Type = FrameType.Open;
            }
        }

        /// <summary>
        /// Returns the sum of pins knocked down.
        /// </summary>
        /// <returns>Sum of pins knocked down.</returns>
        public int KnockedPins()
        {
            return Shots.Sum();
        }
    }
}
