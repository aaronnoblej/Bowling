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
            while(index >= frame.Shots.Count)
            {
                index = frame.Shots.Count - index;
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

        public void PrintScoreSheet()
        {
            Console.WriteLine("____________________________________________________________________________________");
            foreach (Frame f in Frames)
            {
                if(f.Type == Frame.FrameType.Open)
                {
                    Console.Write($" {f.Shots[0]} | {f.Shots[1]} |");
                }
                else if (f.Type == Frame.FrameType.Strike)
                {
                    Console.Write($"   | X |");
                }
                else if (f.Type == Frame.FrameType.Spare)
                {
                    Console.Write($" {f.Shots[0]} | / |");
                }
                else
                {
                    string[] tenthBoxes = { "", "", "" };
                    for (int i = 0; i < f.Shots.Count; i++)
                    {
                        if (f.Shots[i] == 10)
                        {
                            if(i == 0 || (i > 0 && f.Shots[i-1] == 10))
                            {
                                tenthBoxes[i] = "X";
                            }
                            else
                            {
                                tenthBoxes[i] = "/";
                            }
                        }
                        else if((i > 0 && f.Shots[i] + f.Shots[i - 1] == 10) && f.Shots[i-1] != 10)
                        {
                            tenthBoxes[i] = "/";
                        }
                        else
                        {
                            tenthBoxes[i] = f.Shots[i].ToString();
                        }
                    }
                    Console.Write($" {tenthBoxes[0]} | {tenthBoxes[1]} | {(f.Shots.Count == 3 ? tenthBoxes[2] : " ")} |");
                }
            }
            Console.WriteLine();
            Console.WriteLine("   |___|   |___|   |___|   |___|   |___|   |___|   |___|   |___|   |___|   |___|___|");

            var cumulative = 0;
            foreach(Frame f in Frames)
            {
                var prev = Frames.Find(f).Previous;
                cumulative = prev == null ? f.Score : f.Score + cumulative;

                int boxLength = f.Type == Frame.FrameType.Tenth ? 12 : 8;
                Console.Write(FormatCumulativeScore(cumulative, boxLength));
            }
            Console.WriteLine();
            Console.WriteLine("_______|_______|_______|_______|_______|_______|_______|_______|_______|___________|");
        }

        /// <summary>
        /// Used for formatting text in the command prompt.
        /// </summary>
        /// <param name="score">The score to be printed.</param>
        /// <param name="boxLength">The length of the ASCII art box.</param>
        /// <returns></returns>
        private static string FormatCumulativeScore(int score, int boxLength)
        {
            string result = $"  {score}";
            while(result.Length < boxLength-1)
            {
                result += " ";
            }
            return result + "|";
        }

    }
}
