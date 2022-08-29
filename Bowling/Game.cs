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
        public LinkedList<Frame> Frames { get; set; } // linked list in order to easily get next/prev frames
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
        public int GetFrameScore(Frame frame)
        {
            var nextFrame = GetNextFrame(frame);
            int score;
            // Strike
            if (frame.Type == Frame.FrameType.Strike)
            {
                score = 10 + GetShot(nextFrame, 0) + GetShot(nextFrame, 1);
            }
            // Spare
            else if (frame.Type == Frame.FrameType.Spare)
            {
                score = 10 + GetShot(nextFrame, 0);
            }
            // Open or Tenth Frame
            else
            {
                score = frame.PinsKnockedDown();
            }
            return score;
        }

        /// <summary>
        /// Gets a throw for a given frame at a given index.
        /// If the index is larger than the amount of throws for that frame, looks at the following frame.
        /// </summary>
        /// <param name="frame">The frame to get a shot from.</param>
        /// <param name="index">The index of the throw within the frame.</param>
        /// <returns>The value of the shot at the given index for the given frame.</returns>
        public int GetShot(Frame frame, int index)
        {
            // if there is not a shot at this index for this frame, go to the next frame and adjust the index
            while(index >= frame.Shots.Count)
            {
                index = frame.Shots.Count - index;
                frame = GetNextFrame(frame);
                if(frame == null) // if we reach the end of the final frame, return zero
                {
                    return 0;
                }
            }
            int nextShot = frame.Shots[index];

            return nextShot;
        }

        /// <summary>
        /// Finds the frame that follows the given frame.
        /// </summary>
        /// <param name="current">The current frame.</param>
        /// <returns>The next frame in the sequence of the game. If no frame is found, returns null.</returns>
        public Frame GetNextFrame(Frame current)
        {
            var next = Frames.Find(current).Next;
            return next?.Value;
        }

        /// <summary>
        /// Prints an ASCII text art score sheet onto the console.
        /// </summary>
        public void PrintScoreSheet()
        {
            Console.WriteLine("____________________________________________________________________________________");

            // Print each shot in each box
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
                    string[] tenthBoxes = new string[3];
                    for (int i = 0; i < f.Shots.Count; i++)
                    {
                        // Player knocks down 10 pins on one roll
                        if (f.Shots[i] == 10)
                        {
                            // Strike if 10 pins on first shot or 10 pins on second/third shot and previous shot is end of a strike/spare
                            if(i == 0 || (i > 0 && f.Shots[i - 1] == 10))
                            {
                                tenthBoxes[i] = "X";
                            }
                            // Spare if 10 pins second/third shot but previous shot is not a strike or spare
                            else
                            {
                                tenthBoxes[i] = "/";
                            }
                        }
                        // Spare if second/third roll, this roll and previous rolls add up to ten pins, AND the previous roll was not the end of a strike/spare
                        else if(i > 0 && f.Shots[i] + f.Shots[i - 1] == 10 && f.Shots[i - 1] != 10)
                        {
                            tenthBoxes[i] = "/";
                        }
                        // Open
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

            // Print the score in each box, cumulative
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
        /// Lines up the cumulative score text in the ASCII art.
        /// </summary>
        /// <param name="score">The score to be printed.</param>
        /// <param name="boxLength">The length of the ASCII text art box.</param>
        /// <returns>The formatted string containing the cumulative score.</returns>
        private static string FormatCumulativeScore(int score, int boxLength)
        {
            string result = $"  {score}";
            while(result.Length < boxLength-1)
            {
                result += " ";
            }
            result += "|";

            return result;
        }

    }
}
