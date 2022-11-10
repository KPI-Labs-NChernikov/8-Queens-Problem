namespace QueensProblem.Interfaces
{
    public interface IProblemSolver
    {
        IGameBoard Solve();

        int IterationsCount { get; }

        int StatesCount { get; }

        int MaxStatesInMemoryCount { get; }
    }
}
