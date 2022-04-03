namespace BowlingManager
{
    public interface IBowling
    {
        /// <summary>
        /// Entry point for bowling game.
        /// </summary>
        void RecordFrame();

        /// <summary>
        /// Get the each throw score for a frame.
        /// </summary>
        void GetThrowDetails();

        /// <summary>
        /// Calculate the scores for a frame. 
        /// </summary>
        void CalculateScoreForAFrame();

        /// <summary>
        /// 
        /// </summary>
        int Score { get; }
    }
}
