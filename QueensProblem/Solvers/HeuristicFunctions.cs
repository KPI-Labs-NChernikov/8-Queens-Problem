using QueensProblem.Interfaces;

namespace QueensProblem.Solvers
{
    public static class HeuristicFunctions
    {
        public static int GetAttackingPairsWithVisibilityCount(IGameBoard gameBoard)
        {
            if (gameBoard is null)
                throw new ArgumentNullException(nameof(gameBoard));
            return GetAttackingPairsWithVisibilityInRowsCount(gameBoard) 
                + GetAttackingPairsWithVisibilityInDiagonalsCount(gameBoard);
        }

        private static int GetAttackingPairsWithVisibilityInRowsCount(IGameBoard gameBoard)
        {
            var resultCount = 0;
            for (var i = 0; i < gameBoard.Size; i++)
            {
                var firstQueenFound = false;
                for (var j = 0; j < gameBoard.Size; j++)
                {
                    if (gameBoard[i, j] == ChessBoardCell.Queen)
                    {
                        if (!firstQueenFound)
                            firstQueenFound = true;
                        else
                            resultCount++;
                    }
                }
            }
            return resultCount;
        }

        private static int GetAttackingPairsWithVisibilityInDiagonalsCount(IGameBoard gameBoard)
        {
            var resultCount = 0;
            var size = gameBoard.Size;
            var kIncreaseUpTo = size * 2;

            for (var k = 0; k < kIncreaseUpTo; k++)
            {
                var firstQueenFound = false;
                var startI = k < size ? k : size - 1;
                var startJ = k < size ? 0 : k - size + 1;
                var endI = k < size ? 0 : k - size + 1;
                var endJ = k < size ? k : size - 1;
                for (int i = startI, j = startJ; i >= endI && j <= endJ; i--, j++)
                {
                    if (gameBoard[i, j] == ChessBoardCell.Queen)
                    {
                        if (!firstQueenFound)
                            firstQueenFound = true;
                        else
                            resultCount++;
                    }
                }
            }

            for (var k = 0; k < kIncreaseUpTo; k++)
            {
                var firstQueenFound = false;
                var startI = k < size ? 0 : k - size + 1;
                var startJ = k < size ? size - k - 1 : 0;
                var endI = k < size ? k : size - 1;
                var endJ = k < size ? size - 1 : k - size + 1;
                for (int i = startI, j = startJ; i <= endI && j <= endJ; i++, j++)
                {
                    if (gameBoard[i, j] == ChessBoardCell.Queen)
                    {
                        if (!firstQueenFound)
                            firstQueenFound = true;
                        else
                            resultCount++;
                    }
                }
            }
            return resultCount;
        }
    }
}
