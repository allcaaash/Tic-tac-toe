using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TicTacToe.GameEngine;

namespace TicTacToe
{
    internal class GameEngine
    {
        internal enum GameMode
        {
            None,
            PvP,
            PvE
        }
        internal enum WhooseTurn
        {
            Player1,
            Player2,
            Player2_Computer
        }
        private GameMode Mode { get; set; } = GameMode.None;
        private WhooseTurn Turn { get; set; } = WhooseTurn.Player1;
        private string Winner { get; set; } = "";
        private int player1Score = 0;
        private int player2Score = 0;
        private int numOfDraws = 0;
        public int GetPlayer1Score() { return player1Score; }
        public int GetPlayer2Score() { return player2Score; }
        public GameMode GetCurrentMode() { return Mode; }
        public bool IsGameStarted() { return Mode != GameMode.None; }
        public WhooseTurn GetCurrentTurn() { return Turn; }
        public string GetWinner() { return Winner; }
        public bool IsPlayer1Turn() { return Turn == WhooseTurn.Player1; }
        public void SetPlayer1Turn() { Turn = WhooseTurn.Player1; }

        const char EMPTY_CELL = '-';
        const char X_MARK = 'X';
        const char O_MARK = 'O';

        public const string PLAYER_HUMAN_TITLE = "Player";
        public const string PLAYER_COMPUTER_TITLE = "Computer";

        private char[][] gameField = new char[][]
        {
            new char[] {EMPTY_CELL, EMPTY_CELL, EMPTY_CELL},
            new char[] {EMPTY_CELL, EMPTY_CELL, EMPTY_CELL},
            new char[] {EMPTY_CELL, EMPTY_CELL, EMPTY_CELL}
        };
        private Cell ComputerTryAttackHorizontalCell() { return GetHorizontalCellForAttackOrDefence(O_MARK); }
        private Cell ComputerTryAttackVerticalCell() { return GetVerticalCellForAttackOrDefence(O_MARK); }
        private Cell ComputerTryAttackDiagonalCell() { return GetDiagonalCellForAttackOrDefence(O_MARK); }
        private Cell ComputerTryDefendHorizontalCell() { return GetHorizontalCellForAttackOrDefence(X_MARK); }
        private Cell ComputerTryDefendVerticalCell() { return GetVerticalCellForAttackOrDefence(X_MARK); }
        private Cell ComputerTryDefendDiagonalCell() { return GetDiagonalCellForAttackOrDefence(X_MARK); }

        public void ResetScore()
        {
            player1Score = 0;
            player2Score = 0;
            numOfDraws = 0;
        }
        public void PrepareForNewGame()
        {
            Mode = GameMode.None;
            ResetScore();
        }
        public void StartGame(GameMode mode)
        {
            if (mode == GameMode.None)
                return;
            ResetScore();
            Mode = mode;
            Turn = WhooseTurn.Player1;
        }
        public string GetPlayer1Title()
        {
            switch (Mode)
            {
                case GameMode.PvE:
                    return PLAYER_HUMAN_TITLE;
                case GameMode.PvP:
                    return PLAYER_HUMAN_TITLE + " 1";
                default:
                    return "";
            }
        }
        public string GetPlayer2Title()
        {
            switch (Mode)
            {
                case GameMode.PvE:
                    return PLAYER_COMPUTER_TITLE;
                case GameMode.PvP:
                    return PLAYER_HUMAN_TITLE + " 2";
                default:
                    return "";
            }
        }
        public string GetMarkLabelText()
        {
            switch (Turn)
            {
                case WhooseTurn.Player1:
                    return X_MARK.ToString();
                case WhooseTurn.Player2:
                case WhooseTurn.Player2_Computer:
                    return O_MARK.ToString();
                default:
                    return "";
            }
        }
        /// <summary>
        /// Возвращает строку с именем игрока, чей ход в данный момент
        /// </summary>
        /// <returns>Строка с именем игрока</returns>
        public string GetWhooseTurnTitle()
        {
            switch (Mode)
            {
                case GameMode.PvE:
                    return Turn == WhooseTurn.Player1 ? PLAYER_HUMAN_TITLE : PLAYER_COMPUTER_TITLE;
                case GameMode.PvP:
                    return Turn == WhooseTurn.Player1 ? PLAYER_HUMAN_TITLE + " 1" : PLAYER_HUMAN_TITLE + " 2";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Возвращает строку с именем игрока, для которого будет следующий ход
        /// </summary>
        /// <returns>Строка с именем игрока</returns>
        public string GetWhooseNextTurnTitle()
        {
            switch (Mode)
            {
                case GameMode.PvE:
                    return Turn == WhooseTurn.Player1 ? PLAYER_COMPUTER_TITLE : PLAYER_HUMAN_TITLE;
                case GameMode.PvP:
                    return Turn == WhooseTurn.Player1 ? PLAYER_HUMAN_TITLE + " 2" : PLAYER_HUMAN_TITLE + " 1";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Очищает игровое поле
        /// </summary>
        public void ClearGameField()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                    gameField[row][col] = EMPTY_CELL;
            }
        }
        public void MakeTurnAndFillGameFieldCell(int row, int column)
        {
            if (IsPlayer1Turn())
            {
                gameField[row][column] = X_MARK;
                if (Mode == GameMode.PvE)
                    Turn = WhooseTurn.Player2_Computer;
                else if (Mode == GameMode.PvP)
                    Turn = WhooseTurn.Player2;
            }
            else
            {
                gameField[row][column] = O_MARK;
                Turn = WhooseTurn.Player1;
            }
        }
        private Cell GetHorizontalCellForAttackOrDefence(char checkMark)
        {
            for (int row = 0; row < 3; row++)
            {
                int currentSumHorizontal = 0;
                int freeCol = -1;
                for (int col = 0; col < 3; col++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                        freeCol = col;
                    currentSumHorizontal += gameField[row][col] == checkMark ? 1 : 0;
                }

                if (currentSumHorizontal == 2 && freeCol >= 0)
                    return Cell.From(row, freeCol);
            }
            return Cell.ErrorCell();
        }
        private Cell GetVerticalCellForAttackOrDefence(char checkMark)
        {
            for (int col = 0; col < 3; col++)
            {
                int currentSumVert = 0;
                int freeRow = -1;
                for (int row = 0; row < 3; row++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                        freeRow = row;
                    currentSumVert += gameField[row][col] == checkMark ? 1 : 0;
                }

                if (currentSumVert == 2 && freeRow >= 0)
                    return Cell.From(freeRow, col);
            }
            return Cell.ErrorCell();
        }
        private Cell GetDiagonalCellForAttackOrDefence(char checkMark)
        {
            int diagonal1Sum = 0;
            int diagonal2Sum = 0;
            int freeCol1 = -1, freeRow1 = -1;
            int freeCol2 = -1, freeRow2 = -1;
            for (int row = 0; row < 3; row++)
            {
                diagonal1Sum += gameField[row][row] == checkMark ? 1 : 0;
                diagonal2Sum += gameField[row][2 - row] == checkMark ? 1 : 0;

                if (gameField[row][row] == EMPTY_CELL)
                {
                    freeCol1 = row;
                    freeRow1 = row;
                }
                if (gameField[row][2 - row] == EMPTY_CELL)
                {
                    freeCol2 = 2 - row;
                    freeRow2 = row;
                }

                if (diagonal1Sum == 2 && freeRow1 >= 0 && freeCol1 >= 0)
                    return Cell.From(freeRow1, freeCol1);
                else if (diagonal2Sum == 2 && freeRow2 >= 0 && freeCol2 >= 0)
                    return Cell.From(freeRow2, freeCol2);
            }
            return Cell.ErrorCell();
        }
        private Cell ComputerTryAttackCell()
        {
            // Пытаемся атаковать по горизонтальным клеткам
            Cell attackedHorizontalCell = ComputerTryAttackHorizontalCell();
            if (!attackedHorizontalCell.IsErrorCell())
                return attackedHorizontalCell;

            // Пытаемся атаковать по вертикальным клеткам
            Cell attackedVerticalCell = ComputerTryAttackVerticalCell();
            if (!attackedVerticalCell.IsErrorCell())
                return attackedVerticalCell;

            // Пытаемся атаковать по диагональным клеткам
            Cell attackedDiagonalCell = ComputerTryAttackDiagonalCell();
            if (!attackedDiagonalCell.IsErrorCell())
                return attackedDiagonalCell;

            // Нет приемлемых клеток для атаки - возвращаем спецклетку с признаком ошибки
            return Cell.ErrorCell();
        }
        private Cell ComputerTryDefendCell()
        {
            // Пытаемся защищаться по горизонтальным клеткам
            Cell defendedHorizontalCell = ComputerTryDefendHorizontalCell();
            if (!defendedHorizontalCell.IsErrorCell())
                return defendedHorizontalCell;

            // Пытаемся защищаться по вертикальным клеткам
            Cell defendedVerticalCell = ComputerTryDefendVerticalCell();
            if (!defendedVerticalCell.IsErrorCell())
                return defendedVerticalCell;

            // Пытаемся защищаться по диагональным клеткам
            Cell defendedDiagonalCell = ComputerTryDefendDiagonalCell();
            if (!defendedDiagonalCell.IsErrorCell())
                return defendedDiagonalCell;

            // Нет приемлемых клеток для обороны - возвращаем спецклетку с признаком ошибки
            return Cell.ErrorCell();
        }
        private Cell ComputerTrySelectRandomFreeCell()
        {
            Random random = new Random();
            int randomRow, randomCol;
            const int max_attempts = 10;
            int current_attempt = 0;
            do
            {
                randomRow = random.Next(3);
                randomCol = random.Next(3);
                current_attempt++;
            } while (gameField[randomRow][randomCol] != EMPTY_CELL && current_attempt <= max_attempts);

            if (current_attempt > max_attempts)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameField[row][col] == EMPTY_CELL)
                            return Cell.From(row, col);
                    }
                }
            }

            return Cell.From(randomRow, randomCol);
        }
        /// <summary>
        /// Возвращает true, если есть хотя бы одна незанятая клетка на игровом поле и false в противном случае
        /// </summary>
        /// <returns>true при наличии хотя бы одной свободной клетки на поле, иначе false</returns>
        public bool IsAnyFreeCell()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (gameField[row][col] == EMPTY_CELL)
                        return true;
                }
            }
            return false;
        }

        public Cell MakeComputerTurnAndGetCell()
        {
            // Стратегия 1 - атака, если до подебы 1 ход
            Cell attackedCell = ComputerTryAttackCell();
            if (!attackedCell.IsErrorCell())
                return attackedCell;

            // Стратегия 2 - защита, чтобы не дать выиграть человеку
            Cell defendedCell = ComputerTryDefendCell();
            if (!defendedCell.IsErrorCell())
                return defendedCell;

            // Стратегия 3 - случайно выбранная клетка
            if (IsAnyFreeCell())
            {
                Cell randomFreeCell = ComputerTrySelectRandomFreeCell();
                return randomFreeCell;
            }

            return Cell.ErrorCell();
        }
        /// <summary>
        /// Возвращает true и увеличивает счётчик ничьих, если произошла очередная ничья.        
        /// </summary>
        /// <returns>true, если произошла ничья, в противном случае false</returns>
        public bool IsDraw()
        {
            bool isNoFreeCellsLeft = !IsAnyFreeCell();
            if (isNoFreeCellsLeft)
                numOfDraws++;
            return isNoFreeCellsLeft;
        }
        /// <summary>
        /// Проверяет наличие победы какого-либо из игроков по горизонтальным клеткам игрового поля
        /// </summary>
        /// <returns></returns>
        private bool CheckWinOnHorizontalCellsAndUpdateWinner()
        {
            for (int row = 0; row < 3; row++)
            {
                int sumX = 0; int sumO = 0;
                for (int col = 0; col < 3; col++)
                {
                    sumX += gameField[row][col] == X_MARK ? 1 : 0;
                    sumO += gameField[row][col] == O_MARK ? 1 : 0;
                }
                if (sumX == 3)
                {
                    // X победили
                    Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 1" : PLAYER_HUMAN_TITLE;
                    player1Score++;
                    return true;
                }
                else if (sumO == 3)
                {
                    // O победили
                    Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 2" : PLAYER_COMPUTER_TITLE;
                    player2Score++;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет наличие победы какого-либо из игроков по вертикальным клеткам игрового поля
        /// </summary>
        /// <returns></returns>
        private bool CheckWinOnVerticalCellsAndUpdateWinner()
        {
            for (int col = 0; col < 3; col++)
            {
                int sumX = 0; int sumO = 0;
                for (int row = 0; row < 3; row++)
                {
                    sumX += gameField[row][col] == X_MARK ? 1 : 0;
                    sumO += gameField[row][col] == O_MARK ? 1 : 0;
                }

                if (sumX == 3)
                {
                    // X победили
                    Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 1" : PLAYER_HUMAN_TITLE;
                    player1Score++;
                    return true;
                }
                else if (sumO == 3)
                {
                    // O победили
                    Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 2" : PLAYER_COMPUTER_TITLE;
                    player2Score++;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет наличие победы какого-либо из игроков по диагональным клеткам игрового поля
        /// </summary>
        /// <returns></returns>
        private bool CheckWinOnDiagonalCellsAndUpdateWinner()
        {
            int diag1sumX = 0, diag2sumX = 0;
            int diag1sumO = 0, diag2sumO = 0;
            for (int row = 0; row < 3; row++)
            {
                if (gameField[row][row] == O_MARK)
                    diag1sumO++;
                if (gameField[row][row] == X_MARK)
                    diag1sumX++;
                if (gameField[row][2 - row] == O_MARK)
                    diag2sumO++;
                if (gameField[row][2 - row] == X_MARK)
                    diag2sumX++;
            }

            if (diag1sumX == 3 || diag2sumX == 3)
            {
                Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 1" : PLAYER_HUMAN_TITLE;
                player1Score++;
                return true;
            }
            else if (diag1sumO == 3 || diag2sumO == 3)
            {
                Winner = Mode == GameMode.PvP ? PLAYER_HUMAN_TITLE + " 2" : PLAYER_COMPUTER_TITLE;
                player2Score++;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Возвращает true, если кто-то из игроков выиграл
        /// </summary>
        /// <returns>true, если какой-то из игроков выиграл, иначе false</returns>
        public bool IsWin()
        {
            if (CheckWinOnHorizontalCellsAndUpdateWinner())
                return true;

            if (CheckWinOnVerticalCellsAndUpdateWinner())
                return true;

            if (CheckWinOnDiagonalCellsAndUpdateWinner())
                return true;

            return false;
        }
    }
}
