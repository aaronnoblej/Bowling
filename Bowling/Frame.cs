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
        public int[] Shots { get; set; }
        public FrameType Type { get; private set; }
        public int Score { get; set; } = 0;

        // CONSTRUCTORS
        public Frame(params int[] shots)
        {
            this.Shots = shots;
        }

        // METHODS
        /// <summary>
        /// Validates the shots for the frame according to the following:
        /// All shots should be between 0 and 10 pins.
        /// All normal frames should have two shots unless a strike (in which case the only shot should be 10 pins).
        /// The tenth frame should only have a third shot if a spare or strike takes place before it.
        /// </summary>
        /// <returns>True if all above conditions are met, otherwise false.</returns>
        public bool ValidateFrame(bool tenthFrame = false)
        {
            if(Shots.Any(n => n < 0))
            {
                return false;
            }

            var valid = false;
            // Normal frame validation
            if(!tenthFrame)
            {
                if(Shots.Length == 1)
                {
                    valid = KnockedPins() == 10;
                }
                else if(Shots.Length > 0 && Shots.Length <= 2)
                {
                    valid = KnockedPins() <= 10;
                }
            }
            // Tenth frame validation
            else
            {
                if (Shots.Length == 3)
                {
                    // There can only be three shots in the last frame if there is a strike or spare before the final shot
                    valid = Shots[0] + Shots[1] >= 10 && Shots[2] <= 10;
                }
                else if(Shots.Length == 2)
                {
                    valid = KnockedPins() < 10;
                }
            }

            // Once validated, the type can be set
            if(valid)
            {
                SetType();
            }
            return valid;
        }

        /// <summary>
        /// Finds and sets the type of frame (strike, spare, open, or tenth frame) based on the shots.
        /// </summary>
        private void SetType()
        {
            if (Shots.Length == 3)
            {
                this.Type = FrameType.Tenth;
            }
            else if (Shots[0] == 10 && Shots.Length == 1)
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
