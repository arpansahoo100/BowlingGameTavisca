namespace BowlingGame
{
    using BowlingManager;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            IBowling bowling = new Bowling();

            bowling.RecordFrame();
            string finalScore = "\nThe FINAL score is : " + bowling.Score;

            Console.WriteLine(finalScore);
        }
    }
}
