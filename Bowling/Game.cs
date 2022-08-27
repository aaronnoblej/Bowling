using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Game
    {
        // PROPERTIES
        public LinkedList<Frame> Frames { get; set; }
        public int Total { get; set; }

        // CONSTRUCTORS
        public Game()
        {
            this.Frames = new LinkedList<Frame>();
            this.Total = 0;
        }

        // METHODS
        /// <summary>
        /// Loops through each frame to calculate the total score of the game of bowling.
        /// </summary>
        public void CalculateScore()
        {
            foreach(Frame frame in Frames)
            {
                frame.Score = GetFrameScore(frame);
                Total += frame.Score;
            }
        }

        /// <summary>
        /// Finds the score for a single frame.
        /// </summary>
        /// <param name="frame">The frame to find the score for.</param>
        /// <returns>The score a given frame.</returns>
        private int GetFrameScore(Frame frame)
        {
            var nextFrame = GetNextFrame(frame);
            // Strike
            if (frame.Type == Frame.FrameType.Strike)
            {
                return 10 + GetShot(nextFrame, 0) + GetShot(nextFrame, 1);
            }
            // Spare
            else if (frame.Type == Frame.FrameType.Spare)
            {
                return 10 + GetShot(nextFrame, 0);
            }
            // Open or Tenth Frame
            else
            {
                return frame.KnockedPins();
            }
        }

        /// <summary>
        /// Gets a throw for a given frame at a given index.
        /// If the index is larger than the amount of throws for that frame, looks at the following frame.
        /// </summary>
        /// <param name="frame">The frame to get a shot from.</param>
        /// <param name="index">The index of the throw within the frame.</param>
        /// <returns>The value of the shot at the given index for the given frame.</returns>
        private int GetShot(Frame frame, int index)
        {
            // if there is not a shot at this index, go to the next frame
            while(index >= frame.Shots.Length)
            {
                index = frame.Shots.Length - index;
                frame = GetNextFrame(frame);
                if(frame == null)
                {
                    return 0;
                }
            }
            int nextShot = frame.Shots[index];

            return nextShot;
        }

        /// <summary>
        /// Finds the proceeding frame of the given frame.
        /// </summary>
        /// <param name="current">The current frame.</param>
        /// <returns>The next frame in the sequence of the game.</returns>
        private Frame GetNextFrame(Frame current)
        {
            var next = Frames.Find(current).Next;
            return next?.Value;
        }

    }
}
