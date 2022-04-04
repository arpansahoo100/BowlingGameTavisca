namespace BowlingManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Bowling : IBowling
    {
        #region Member Variables
        /// <summary>
        /// The current frame.
        /// </summary>
        private int frame = 1;

        /// <summary>
        /// First throw in a frame.
        /// </summary>
        private int throw1;

        /// <summary>
        /// Second throw in a frame.
        /// </summary>
        private int throw2;

        /// <summary>
        /// Third throw in the 10th frame.
        /// </summary>
        private int throw3;

        /// <summary>
        /// Keeps track of the frame for which need to compute the score.
        /// </summary>
        private int frameForWhichScoreNeedsToBeCalculated = 0;

        /// <summary>
        /// The total of first two throws in a normal frame.
        /// </summary>
        private int frameScore;

        /// <summary>
        /// Keeps the count of Game score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Keeps track of all frames individual scores.
        /// </summary>
        public List<int> tenFrameScores = new List<int>();

        /// <summary>
        /// A JaggedArray (to allow for three numbers in the 10th frame)
        /// </summary>
        public int[][] tenFrames = new int[10][];
        #endregion


        #region Member Functions
        /// <summary>
        /// Entry point for bowling game.
        /// </summary>
        public void RecordFrame()
        {
            GetThrowDetails();
            CalculateScoreForAFrame();

            frame++;
            while (frame <= 10)
            {
                RecordFrame();
            }
        }

        /// <summary>
        /// Get the each throw score for a frame.
        /// </summary>
        public void GetThrowDetails()
        {
            Console.WriteLine("Throws for Frame {0}:", frame);
            int throwOne = 0;
            int throwTwo = 0;
            int throwThree = 0;
            switch (frame)
            {
                case 10:
                    Console.Write("Bowl {0}:", 1);
                    throwOne = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Bowl {0}:", 2);
                    throwTwo = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Bowl {0}:", 3);
                    throwThree = Convert.ToInt32(Console.ReadLine());
                    tenFrames[frame - 1] = new[] { throwOne, throwTwo, throwThree };
                    break;

                default:
                    Console.Write("Bowl {0}:", 1);
                    throwOne = Convert.ToInt32(Console.ReadLine());
                    if (throwOne != 10)
                    {
                        Console.Write("Bowl {0}:", 2);
                        throwTwo = Convert.ToInt32(Console.ReadLine());
                    }
                    tenFrames[frame - 1] = new[] { throwOne, throwTwo };
                    break;
            }

            tenFrameScores.Add(throwOne + throwTwo);
        }

        /// <summary>
        /// Get the each throw score for a frame.
        /// </summary>
        public void CalculateScoreForAFrame()
        {
            int computingFrame = frameForWhichScoreNeedsToBeCalculated;
            throw1 = tenFrames[computingFrame][0];
            throw2 = tenFrames[computingFrame][1];
            frameScore = tenFrameScores[computingFrame];

            if (frameScore == 10 && computingFrame < 9)
            {
                // We need to record the next frame throws for counting bonus for a Spare or Strike.
                if (tenFrames.Count(s => s != null) - 1 == computingFrame)
                    return;

                int nextFrameScore = tenFrameScores[computingFrame + 1];

                if (throw1 == 10)   // it's a strike
                {
                    // Check to see if the next throw was also a strike
                    if (tenFrames[computingFrame + 1][0] == 10)   // it's a strike
                    {
                        // bonus = 10 + the first throw of the next frame
                        if (computingFrame < 8)
                        {
                            frame++;
                            GetThrowDetails();
                            int strikeBonus = 10 + tenFrames[computingFrame + 2][0];
                            ColorMessage(ConsoleColor.Green, "StrikeBonus: " + strikeBonus);
                            tenFrameScores[computingFrame] += strikeBonus;
                            Score += tenFrameScores[computingFrame];
                            frameForWhichScoreNeedsToBeCalculated++;
                            CalculateScoreForAFrame();
                            return;
                        }
                        else  // frame 9, look at just the first two throws of frame 10
                        {
                            int strikeBonus = tenFrames[computingFrame + 1][0] + tenFrames[computingFrame + 1][1];
                            ColorMessage(ConsoleColor.Green, "StrikeBonus: " + strikeBonus);
                            tenFrameScores[computingFrame] += strikeBonus;
                        }
                    }
                    else
                    {
                        // bonus = the total of the two throws in the next frame (nextFrameScore)
                        ColorMessage(ConsoleColor.Green, "StrikeBonus: " + nextFrameScore);
                        tenFrameScores[computingFrame] += nextFrameScore;
                    }
                }
                else // spare
                {
                    ColorMessage(ConsoleColor.Cyan, "SpareBonus: " + tenFrames[computingFrame + 1][0]);
                    tenFrameScores[computingFrame] += tenFrames[computingFrame + 1][0];
                }
                frameForWhichScoreNeedsToBeCalculated++;
            }
            else
            {
                throw3 = tenFrames[computingFrame].Length == 3 ? tenFrames[computingFrame][2] : 0;
                if (throw1 == 10) // strike in the tenth frame
                {
                    // bonus = throw2 + throw3 (throw2 is already included in tenFrameScores[i])
                    ColorMessage(ConsoleColor.Green, "StrikeBonus: " + (throw2 + throw3));
                    tenFrameScores[computingFrame] += throw3;
                }
                else if (frameScore == 10) // spare in the first two throws of the 10th frame
                {
                    // bonus = last throw of the 10th frame
                    ColorMessage(ConsoleColor.Cyan, "SpareBonus: " + throw3);
                    tenFrameScores[computingFrame] += throw3;
                }
                frameForWhichScoreNeedsToBeCalculated++;
            }

            Score += tenFrameScores[computingFrame];
            if (tenFrames.Count(s => s != null) > frameForWhichScoreNeedsToBeCalculated)
            {
                CalculateScoreForAFrame();
            }
            Console.WriteLine("Score Till Now: " + Score);;
        }

        private void ColorMessage(ConsoleColor color, string message)
        {
            // Set text color
            Console.ForegroundColor = color;

            // Write message
            Console.WriteLine(message);

            // Reset text color
            Console.ResetColor();
        }

        #endregion
    }
}
