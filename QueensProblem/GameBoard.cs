using QueensProblem.Interfaces;
using System.Text;

namespace QueensProblem
{
    public class GameBoard : IGameBoard
    {
        const int _size = 8;

        public int Size => _size;

        private readonly ChessBoardCell[,] _matrix = new ChessBoardCell[_size, _size];

        public GameBoard(string initialState)
        {
            CheckStateString(initialState);
            InitializeBoardFromStateWithCharactersCheck(initialState);
        }

        private GameBoard(ChessBoardCell[,] matrix)
        {
            _matrix = matrix;
        }

        private static void CheckStateString(string state)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (state.Length != _size)
                throw new ArgumentOutOfRangeException(nameof(state),
                    $"The state should be a string with the length of {_size} characters");
        }

        private void InitializeBoardFromStateWithCharactersCheck(string state)
        {
            var sizeAsCharacter = char.Parse(_size.ToString());
            for (var j = 0; j < state.Length; j++)
            {
                var character = state[j];
                if (character < '1' || character > sizeAsCharacter)
                    throw new FormatException($"The initial state must contain only numbers from 1 to {sizeAsCharacter}");
                var rowNumber = int.Parse(character.ToString()) - 1;
                var colNumber = j;
                _matrix[rowNumber, colNumber] = ChessBoardCell.Queen;
            }
        }

        public ChessBoardCell this[int i, int j]
        {
            get
            {
                CheckRowIndex(i);
                CheckColumnIndex(j);
                return _matrix[i, j];
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var character = _matrix[i, j] == ChessBoardCell.Queen ? 'Q' : '-';
                    builder.Append(character);
                    if (j != _size - 1)
                        builder.Append(' ');
                }
                if (i != _size - 1)
                    builder.AppendLine();
            }
            return builder.ToString();
        }

        public object Clone()
        {
            var matrixCopy = new ChessBoardCell[_size, _size];
            Array.Copy(_matrix, matrixCopy, _matrix.Length);
            return new GameBoard(matrixCopy);
        }

        public int FindQueenRowIndex(int j)
        {
            CheckColumnIndex(j);
            for (var i = 0; i < _size; i++)
                if (_matrix[i, j] == ChessBoardCell.Queen)
                    return i;
            throw new InvalidOperationException("No queens in this column. " +
                "Please, report this bug if you hadn't changed the internal matrix manually");
        }

        public void ReplaceQueenInColumn(int oldI, int newI, int j)
        {
            CheckRowIndex(newI, nameof(newI));
            CheckRowIndex(oldI, nameof(oldI));
            CheckColumnIndex(j);
            CheckPositionForQueen(oldI, j);
            _matrix[oldI, j] = ChessBoardCell.Empty;
            _matrix[newI, j] = ChessBoardCell.Queen;
        }

        private static void CheckRowIndex(int i, string customIName = "i")
        {
            if (i < 0 || i >= _size)
                throw new ArgumentOutOfRangeException(customIName, $"The row number must be in range [0;{_size - 1}]");
        }

        private static void CheckColumnIndex(int j)
        {
            if (j < 0 || j >= _size)
                throw new ArgumentOutOfRangeException(nameof(j), $"The column number must be in range [0;{_size - 1}]");
        }

        public bool IsSafe => AreRowsSafe && AreDiagonalsSafe;

        private bool AreRowsSafe
        {
            get
            {
                for (var i = 0; i < _size; i++)
                {
                    var sum = 0;
                    for (var j = 0; j < Size; j++)
                    {
                        sum += (int)_matrix[i, j];
                    }
                    if (sum > (int)ChessBoardCell.Queen)
                        return false;
                }
                return true;
            }
        }

        private bool AreDiagonalsSafe
        {
            get
            {
                const int kIncreaseUpTo = _size * 2;    

                for (var k = 0; k < kIncreaseUpTo; k++)
                {
                    var startI = k < _size ? k : _size - 1;
                    var startJ = k < _size ? 0 : k - _size + 1;
                    var endI = k < _size ? 0 : k - _size + 1;
                    var endJ = k < _size ? k : _size - 1;

                    var sum = 0;
                    for (int i = startI, j = startJ; i >= endI && j <= endJ ; i--, j++)
                    {
                        sum += (int)_matrix[i, j];
                    }
                    if (sum > (int)ChessBoardCell.Queen)
                        return false;
                }

                for (var k = 0; k < kIncreaseUpTo; k++)
                {
                    var startI = k < _size ? 0 : k - _size + 1;
                    var startJ = k < _size ? _size - k - 1 : 0;
                    var endI = k < _size ? k : _size - 1;
                    var endJ = k < _size ? _size - 1 : k - _size + 1;

                    var sum = 0;
                    for (int i = startI, j = startJ; i <= endI && j <= endJ; i++, j++)
                    {
                        sum += (int)_matrix[i, j];
                    }
                    if (sum > (int)ChessBoardCell.Queen)
                        return false;
                }

                return true;
            }
        }

        public string State
        {
            get
            {
                var builder = new StringBuilder();
                for (var j = 0; j < _size; j++)
                {
                    for (var i = 0; i < _size; i++)
                    {
                        if (_matrix[i, j] == ChessBoardCell.Queen)
                        {
                            builder.Append(i + 1);
                            break;
                        }
                    }
                }
                return builder.ToString();
            }
        }

        private void CheckPositionForQueen(int i, int j)
        {
            if (_matrix[i, j] != ChessBoardCell.Queen)
                throw new InvalidOperationException($"No queen at {i};{j}");
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not GameBoard other)
                return false;
            for (var i = 0; i < _size; i++)
                for (var j = 0; j < _size; j++)
                    if (_matrix[i, j] != other._matrix[i, j])
                        return false;
            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            for (var i = 0; i < _size; i++)
                for (var j = 0; j < _size; j++)
                    hashCode.Add(_matrix[i, j]);
            return hashCode.ToHashCode();
        }
    }
}
