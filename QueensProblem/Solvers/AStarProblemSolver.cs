using QueensProblem.Interfaces;

namespace QueensProblem.Solvers
{
    public class AStarProblemSolver : BaseProblemSolver
    {
        private readonly Func<IGameBoard, int> _heuristicFunction;

        private PriorityQueue<IGameBoard, int> _queue = new();

        private HashSet<IGameBoard> _visited = new();

        public AStarProblemSolver(IGameBoard gameBoard, Func<IGameBoard, int> heuristicFunction) : base(gameBoard)
        {
            _heuristicFunction = heuristicFunction ?? throw new ArgumentNullException(nameof(heuristicFunction));
        }

        public override IGameBoard Solve()
        {
            ReinitializeFields();
            _queue.Enqueue(GameBoard, _heuristicFunction.Invoke(GameBoard));
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

        protected override void ReinitializeFields()
        {
            _queue = new();
            _visited = new();
            base.ReinitializeFields();
        }

        private void UpdateMaxStatesInMemoryCount()
        {
            var statesInMemory = _queue.Count + _visited.Count;
            if (statesInMemory > MaxStatesInMemoryCount)
                MaxStatesInMemoryCount = statesInMemory;
        }

        protected override void CheckAndAddState(IGameBoard state)
        {
            if (!_visited.Contains(state) && !_queue.UnorderedItems.Any(i => state.Equals(i.Element)))
                _queue.Enqueue(state, _heuristicFunction.Invoke(state));
        }
    }
}
