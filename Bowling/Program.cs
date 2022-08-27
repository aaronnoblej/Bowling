using System;
using System.Collections.Generic;

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

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Enter the number of pins knocked down for each throw.");
            Console.WriteLine("- Strikes can optionally be input as \"X\" or \"x\"");
            Console.WriteLine("- The final throw for spares can be optionally input as \"/\"");
            Console.WriteLine("----------------------------------------------------------");

            var game = new Game();
            var frameCount = 10;
            for (int i = 1; i <= frameCount; i++)
            {
                var rollNum = 1;
                var tenth = i == frameCount;
                var currentRolls = new List<int>();
                var rollAgain = true;
                var previousRoll = 0;

                while (rollAgain) {
                    Console.Write($"Frame {i} (Roll {rollNum}): ");
                    var input = Console.ReadLine();
                    var valid = ParseAndValidateInput(input, tenth, rollNum, out var roll, out rollAgain, previousRoll);

                    if (valid)
                    {
                        currentRolls.Add(roll);
                        previousRoll = roll;
                        rollNum++;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }
                }
                Frame frame = new Frame(currentRolls, tenth);
                game.Frames.AddLast(frame);
                Console.WriteLine("------------------------");
            }

            game.CalculateScore();
            Console.WriteLine($"Total score: {game.Total}");
            game.PrintScoreSheet();

            Console.ReadLine();
        }

        /// <summary>
        /// First parses the user input and transforms it into a number (necessary for X and /). Once parsing completes, validates that the input is correct.
        /// If any errors occur, the parsed input and set to null and false is returned.
        /// </summary>
        /// <param name="input">The user's input for a frame.</param>
        /// <param name="tenth">Indicates whether or not to parse for the tenth frame.</param>
        /// <param name="rollNumber">The roll number in the frame.</param>
        /// <param name="parsedInput">The output variable where the successfully parsed input is stored.</param>
        /// <param name="rollAgain">Indicates if another roll should take place in this frame.</param>
        /// <param name="previousRoll">The previous roll of the frame. Default is zero.</param>
        /// <returns>True if parsing and validation was successful, false if an error occurred.</returns>
        private static bool ParseAndValidateInput(string input, bool tenth, int rollNumber, out int parsedInput, out bool rollAgain, int previousRoll = 0)
        {
            // Parse input (into number)
            rollAgain = true;
            try
            {
                if (input.ToLower().Equals("x"))
                {
                    parsedInput = 10;
                }
                else if (input.Equals("/") && rollNumber > 1)
                {
                    parsedInput = 10 - previousRoll;
                }
                else
                {
                    parsedInput = int.Parse(input);
                }
            }
            catch(Exception)
            {
                parsedInput = 0;
                rollAgain = true;
                return false;
            }

            // Validate input
            var valid = false;
            // Normal frame
            if(!tenth)
            {
                if (parsedInput >= 0 && parsedInput <= 10 - previousRoll)
                {
                    if (parsedInput == 10 || rollNumber == 2)
                    {
                        rollAgain = false;
                    }
                    valid = true;
                }
            }
            // Tenth frame
            else
            {
                if(parsedInput >= 0 && parsedInput <= 10)
                {
                    if ((rollNumber == 2 && previousRoll + parsedInput < 10) || rollNumber == 3)
                    {
                        rollAgain = false;
                    }
                    valid = true;
                }
            }
            
            return valid;
        }
    }
}
