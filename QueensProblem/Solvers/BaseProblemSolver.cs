using QueensProblem.Interfaces;

namespace QueensProblem.Solvers
{
    public abstract class BaseProblemSolver : IProblemSolver
    {
        protected IGameBoard GameBoard { get; }

        public BaseProblemSolver(IGameBoard gameBoard)
        {
            GameBoard = gameBoard ?? throw new ArgumentNullException(nameof(gameBoard));
        }

        public int IterationsCount { get; protected set; }
        public int StatesCount { get; protected set; }
        public int MaxStatesInMemoryCount { get; protected set; }

        public abstract IGameBoard Solve();

        protected virtual void ReinitializeFields()
        {
            IterationsCount = 0;
            StatesCount = 0;
            MaxStatesInMemoryCount = 0;
        }

        protected virtual void GenerateNextState(IGameBoard currentBoard)
        {
            for (int j = 0; j < currentBoard.Size; j++)
            {
                var oldQueenI = currentBoard.FindQueenRowIndex(j);
                for (int k = 0; k < currentBoard.Size; k++)
                {
                    if (k != oldQueenI)
                    {
                        StatesCount++;
                        var state = (IGameBoard)currentBoard.Clone();
                        state.ReplaceQueenInColumn(oldQueenI, k, j);
                        CheckAndAddState(state);
                    }
                }
            }
        }

        protected abstract void CheckAndAddState(IGameBoard state);
    }
}
