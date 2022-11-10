using Demo;
using Demo.HelperClasses;
using QueensProblem;
using QueensProblem.Interfaces;
using QueensProblem.Solvers;
using System.Diagnostics;

Console.WriteLine("Welcome to the 8 queens problem solver");
Console.WriteLine("Please enter the state of the problem: ");
IGameBoard board = GetGameBoardFromInput();
Console.WriteLine($"{Environment.NewLine}Your board:");
Console.WriteLine(board.ToString());
Console.WriteLine();
Algorithm chosenAlgorithm = 0;
var algorithmsMenu = new LiteMenu
{
    IsQuitable = true,
    Name = "algorithm",
    Items = new MenuItem[]
    {
        new MenuItem
        {
            Text = "BFS",
            Action = () => {chosenAlgorithm = Algorithm.BFS; }
        },
        new MenuItem
        {
            Text = "A-star with F1 (quantity of pairs of queens under attack considering visibility)",
            Action = () => {chosenAlgorithm = Algorithm.AStar; }
        },
        new MenuItem
        {
            Text = "both",
            Action = () => {chosenAlgorithm = Algorithm.BFS | Algorithm.AStar; }
        }
    }
};
algorithmsMenu.Print();
Console.WriteLine();
if (chosenAlgorithm.HasFlag(Algorithm.BFS))
{
    Console.WriteLine("BFS:");
    SolveAndPrintReport(new BFSProblemSolver(board));
}
if (chosenAlgorithm.HasFlag(Algorithm.AStar))
{
    Console.WriteLine("A* with F1 (quantity of pairs of queens under attack considering visibility):");
    SolveAndPrintReport(new AStarProblemSolver(board, HeuristicFunctions.GetAttackingPairsWithVisibilityCount));
}

static IGameBoard GetGameBoardFromInput()
{
    IGameBoard board = null!;
    bool restart;
    do
    {
        restart = true;
        try
        {
            var entered = Console.ReadLine()?.Trim();
            board = new GameBoard(entered!);
            restart = false;
        }
        catch (ArgumentException exc)
        {
            HandleExceptionOnStateStringInput(exc);
        }
        catch (FormatException exc)
        {
            HandleExceptionOnStateStringInput(exc);
        }
    }
    while (restart);
    return board;
}

static void HandleExceptionOnStateStringInput(Exception exc)
{
    var initialColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(exc.Message);
    Console.WriteLine("Please, enter the state of the problem once more: ");
    Console.ForegroundColor = initialColor;
}

static void SolveAndPrintReport(IProblemSolver solver)
{
    GC.Collect();
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    var result = solver.Solve();
    stopWatch.Stop();
    Console.WriteLine("Result:");
    Console.WriteLine(result);
    Console.WriteLine($"Result state: {result.State}");
    Console.WriteLine($"Time: {stopWatch.Elapsed.TotalSeconds} seconds");
    Console.WriteLine($"Iterations: {solver.IterationsCount}");
    Console.WriteLine($"Generated states: {solver.StatesCount}");
    Console.WriteLine($"Maximum states stored in memory simultaneously: {solver.MaxStatesInMemoryCount}");
    Console.WriteLine();
}
