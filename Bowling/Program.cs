using System;

namespace Bowling
{
    class Program
    {
        // TO DO:
        // Remove any zeroes that proceed a strike
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("-----------BOWLING SCORE CALCULATOR-----------");
            Console.WriteLine("----------------------------------------------\n");

            Console.WriteLine("Enter the number of pins knocked down for each frame, each throw separated by a space (ex: 7 3)");
            Console.WriteLine("- Each frame should only have 2 throws max (with the exception of the tenth frame)");
            Console.WriteLine("- Strikes can optionally be input as \"X\" or \"x\"");
            Console.WriteLine("- The final throw for spares can be optionally input as \"/\"\n");

            var game = new Game();
            var frameCount = 10;
            for (int i = 1; i <= frameCount; i++)
            {
                while (true) {
                    Console.Write($"Frame {i}: ");
                    var input = Console.ReadLine();
                    var valid = ParseInput(input, out var shots);
                    if (!valid)
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        continue;
                    }
                    else
                    {
                        // Create a frame
                        var frame = new Frame(shots);
                        var tenth = i == frameCount;
                        if (!frame.ValidateFrame(tenth))
                        {
                            Console.WriteLine("The numbers for this frame are incorrect (there may be too many or too little pins counted).");
                            continue;
                        }

                        game.Frames.AddLast(frame);
                        break;
                    }
                }
            }

            game.CalculateScore();
            Console.WriteLine($"\nTotal score: {game.Total}");
            foreach (Frame f in game.Frames) { Console.WriteLine(f.Score); }

            Console.ReadLine();
        }

        /// <summary>
        /// Transforms the user input into an array of numbers representing the number of pins knocked down for each shot.
        /// If any errors occur, the parsed input and set to null and false is returned.
        /// </summary>
        /// <param name="input">The user's input for a frame.</param>
        /// <param name="parsedInput">The output variable where the successfully parsed input is stored.</param>
        /// <returns>True if parsing was successful, false if an error occurred.</returns>
        private static bool ParseInput(string input, out int[] parsedInput)
        {
            var strings = input.Split(" ");
            parsedInput = new int[strings.Length];
            try
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    var s = strings[i];
                    if (s.ToLower().Equals("x"))
                    {
                        parsedInput[i] = 10;
                    }
                    else if (s.Equals("/"))
                    {
                        parsedInput[i] = 10 - parsedInput[i - 1];
                    }
                    else
                    {
                        parsedInput[i] = int.Parse(s);
                    }
                }
            }
            catch(Exception)
            {
                parsedInput = null;
                return false;
            }
            return true;
        }
    }
}
