using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TicTacToe : Form
    {
        private GameEngine engine = new GameEngine();
        public TicTacToe()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblPlayer1Name.Text = "?";
            lblPlayer2Name.Text = "?";
            SetComponentVisible(false);
        }

        private Panel GetPanelCellControlByCell(Cell cell)
        {
            if (cell == null || !cell.IsValidGameFieldCell()) return null;

            string panelCtrlName = "cell_" + cell.Row + "_" + cell.Column;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Name == panelCtrlName && ctrl is Panel)
                    return (Panel)ctrl;
            }
            return null;
        }
        private void ClearGameField()
        {
            engine.ClearGameField();
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Panel panelCell = GetPanelCellControlByCell(Cell.From(row, col));
                    if (panelCell != null)
                        panelCell.Controls.Clear();
                }
            }
            engine.SetPlayer1Turn();
            lblTurnPlayerName.Text = engine.GetWhooseTurnTitle();
        }
        private void SetComponentVisible(bool visible)
        {
            lblPlayer1Name.Visible = visible;
            lblPlayer1Score.Visible = visible;
            lblPlayer2Name.Visible = visible;
            lblPlayer2Score.Visible = visible;
            lblTurn.Visible = visible;
            lblTurnPlayerName.Visible = visible;

            lblReset.Visible = visible;
            lblStartNewGame.Visible = visible;

            lblNewGame.Visible = !visible;
            lblPvC.Visible = !visible;
            lblPvP.Visible = !visible;
        }
        private void ShowMainMenu(bool  show)
        {
            lblNewGame.Visible = show;
            lblPvC.Visible = show;
            lblPvP.Visible = show;
        }
        private void UpdateControls()
        {
            lblPlayer1Name.Text = engine.GetPlayer1Title();
            lblPlayer2Name.Text = engine.GetPlayer2Title();
            lblTurnPlayerName.Text = engine.GetWhooseTurnTitle();
            SetComponentVisible(true);
        }
        private void ButtonMouseEnter(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                Label lbl = sender as Label;
                lbl.BackColor = Color.FromArgb(64, 64, 64);
                Cursor = Cursors.Hand;
            }
        }
        private void ButtonMouseLeave(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                Label lbl = sender as Label;
                lbl.BackColor = System.Drawing.Color.Gray;
                Cursor = Cursors.Default;
            }
        }
        private void CellMouseEnter(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel pnl = sender as Panel;
                pnl.BackColor = System.Drawing.Color.LightGray;
                Cursor = Cursors.Hand;
            }
        }
        private void CellMouseLeave(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel pnl = sender as Panel;
                pnl.BackColor = System.Drawing.Color.White;
                Cursor = Cursors.Default;
            }
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void lblStartNewGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }
        private void StartNewGameInSelectedMode(GameEngine.GameMode selectedMode)
        {
            engine.StartGame(selectedMode);
            UpdateControls();
        }
        private void lblReset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }
        private void lblPvC_Click(object sender, EventArgs e)
        {
            StartNewGameInSelectedMode(GameEngine.GameMode.PvE);
        }
        private void lblPvP_Click(object sender, EventArgs e)
        {
            StartNewGameInSelectedMode(GameEngine.GameMode.PvP);
        }

        private void FillCell(object sender, EventArgs e)
        {
            if (!engine.IsGameStarted())
                return;
            if (!(sender is Panel))
                return;
            Panel pnl = sender as Panel;
            FillCell(pnl, GetRow(pnl), GetColumn(pnl));
        }
        private void FillCell(Panel panel, int row, int column)
        {
            Label markLabel = new Label();
            markLabel.Font = new Font(FontFamily.GenericMonospace, 72, FontStyle.Bold);
            markLabel.AutoSize = true;
            markLabel.Text = engine.GetMarkLabelText();
            markLabel.ForeColor = System.Drawing.Color.Gray;

            lblTurnPlayerName.Text = engine.GetWhooseNextTurnTitle();

            engine.MakeTurnAndFillGameFieldCell(row, column);

            panel.Controls.Add(markLabel);

            if (engine.IsWin())
            {
                MessageBox.Show("Victory!\nThe " + engine.GetWinner() + " won", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblPlayer1Score.Text = engine.GetPlayer1Score().ToString();
                lblPlayer2Score.Text = engine.GetPlayer2Score().ToString();
                ClearGameField();
            }
            else if (engine.IsDraw())
            {
                MessageBox.Show("Draw!", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearGameField();
            }
            else
            {
                if(engine.GetCurrentTurn() == GameEngine.WhooseTurn.Player2_Computer)
                {
                    Cell cellChosenByCoputer = engine.MakeComputerTurnAndGetCell();
                    if (!cellChosenByCoputer.IsErrorCell())
                    {
                        Panel panelCell = GetPanelCellControlByCell(cellChosenByCoputer);
                        if (panelCell != null)
                            FillCell(panelCell, cellChosenByCoputer.Row, cellChosenByCoputer.Column);
                        else
                            MessageBox.Show("Error!\nThe cell selected by the computer must not be null.", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Error\nFailed to select a cell for a move", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private int GetRow(Panel pnl)
        {
            var splitted = pnl.Name.Split('_');
            int.TryParse(splitted[1], out int row);
            return row;
        }
        private int GetColumn(Panel pnl)
        {
            var splitted = pnl.Name.Split('_');
            int.TryParse(splitted[2], out int col);
            return col;
        }
        private void ResetGame()
        {
            ClearGameField();
            engine.StartGame(engine.GetCurrentMode());
            lblPlayer1Score.Text = engine.GetPlayer1Score().ToString();
            lblPlayer2Score.Text = engine.GetPlayer2Score().ToString();
            UpdateControls();
        }
        private void StartNewGame()
        {
            ClearGameField();
            engine.PrepareForNewGame();

            lblPlayer1Score.Text = engine.GetPlayer1Score().ToString();
            lblPlayer2Score.Text = engine.GetPlayer2Score().ToString();

            ShowMainMenu(true);
            SetComponentVisible(false);
        }
    }
}
