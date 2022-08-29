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
        public Frame(List<int> shots, bool isTenthFrame)
        {
            this.Shots = shots;
            SetType(isTenthFrame);
        }

        /// <summary>
        /// Finds and sets the type of frame (strike, spare, open, or tenth frame) based on the shots.
        /// </summary>
        private void SetType(bool tenth = false)
        {
            if (tenth)
            {
                this.Type = FrameType.Tenth;
            }
            else if (IsStrike(Shots[0]))
            {
                this.Type = FrameType.Strike;
            }
            else if (IsSpare(Shots[0], Shots[1]))
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
        public int PinsKnockedDown()
        {
            return Shots.Sum();
        }

        /// <summary>
        /// Indicates if the specified single roll is classified as a strike.
        /// </summary>
        /// <param name="roll">The roll of a frame.</param>
        /// <returns>True if the roll is a strike.</returns>
        public static bool IsStrike(int roll)
        {
            return roll == 10;
        }

        /// <summary>
        /// Indicates if the specified rolls are classified as a spare.
        /// </summary>
        /// <param name="roll1">The first roll of the play.</param>
        /// <param name="roll2">The second roll of the play.</param>
        /// <returns>True if the two rolls are a spare.</returns>
        public static bool IsSpare(int roll1, int roll2)
        {
            return roll1 + roll2 == 10 && roll2 > 0;
        }
    }
}
