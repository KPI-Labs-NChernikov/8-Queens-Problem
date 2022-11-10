namespace QueensProblem.Interfaces
{
    public interface IGameBoard : ICloneable
    {
        int Size { get; }

        ChessBoardCell this[int i, int j] { get; }

        void ReplaceQueenInColumn(int oldI, int newI, int j);

        int FindQueenRowIndex(int j);

        bool IsSafe { get; }

        string State { get; }
    }
}
