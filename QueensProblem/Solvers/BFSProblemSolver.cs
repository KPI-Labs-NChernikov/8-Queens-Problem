using QueensProblem.Interfaces;

namespace QueensProblem.Solvers
{
    public class BFSProblemSolver : BaseProblemSolver
    {
        private Queue<IGameBoard> _queue = new();

        private HashSet<IGameBoard> _visited = new();

        public BFSProblemSolver(IGameBoard gameBoard) : base(gameBoard)
        {   }

        public override IGameBoard Solve()
        {
            ReinitializeFields();
            _queue.Enqueue(GameBoard);
            IGameBoard current;
            while (_queue.Count != 0)
            {
                current = _queue.Dequeue();
                _visited.Add(current);
                UpdateMaxStatesInMemoryCount();
                if (current.IsSafe)
                    return current;
                IterationsCount++;
                GenerateNextState(current);
            }
            UpdateMaxStatesInMemoryCount();
            return null!;
        }

        private void UpdateMaxStatesInMemoryCount()
        {
            var statesInMemory = _queue.Count + _visited.Count;
            if (statesInMemory > MaxStatesInMemoryCount)
                MaxStatesInMemoryCount = statesInMemory;
        }

        protected override void ReinitializeFields()
        {
            _queue = new();
            _visited = new();
            base.ReinitializeFields();
        }

        protected override void CheckAndAddState(IGameBoard state)
        {
            if (!_visited.Contains(state) && !_queue.Contains(state))
                _queue.Enqueue(state);
        }
    }
}
