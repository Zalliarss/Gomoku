namespace Gomoku
{
    public class Presenter
    {
        static void PresenterText(string a)
        {
            Console.WriteLine(a);
        }
        public Logic Model { get; }
        private readonly IGameView view;
        private bool isProcessingMove = false;

        public Presenter(Logic model, IGameView view)
        {
            try
            {
                Model = model;
                this.view = view;
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в конструкторе Presenter: {ex.Message}");
            }
        }

        public async void OnCellClicked(int x, int y)
        {
            try
            {
                if (isProcessingMove || Model.IsAIMakingMove) return;
                if (Model.VsComputer && Model.CurrentPlayer != Model.HumanPlayer) return;

                isProcessingMove = true;

                if (Model.MakeMove(x, y))
                {
                    view.Refresh();

                   
                    int winner = Model.CheckGameState();
                    if (winner != 0)
                    {
                        HandleGameEnd(winner);
                        isProcessingMove = false;
                        return;
                    }

                    Model.SwitchPlayer();
                    view.Refresh();
                    UpdateStatusText();

                    if (Model.VsComputer && Model.CurrentPlayer != Model.HumanPlayer)
                    {
                        await Task.Delay(100);
                        await MakeAIMove();
                    }
                }

                isProcessingMove = false;
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в OnCellClicked: {ex.Message}");
                isProcessingMove = false;
                view.UpdateStatus("Ошибка при обработке хода");
            }
        }

        private async Task MakeAIMove()
        {
            try
            {
                await Model.MakeAIMoveAsync();
                view.Refresh();

                int winner = Model.CheckGameState();
                if (winner != 0)
                {
                    HandleGameEnd(winner);
                    return;
                }

                Model.SwitchPlayer();
                view.Refresh();
                UpdateStatusText();
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в MakeAIMove: {ex.Message}");
                view.UpdateStatus("Ошибка хода ИИ");

                if (Model.CurrentPlayer != Model.HumanPlayer)
                {
                    Model.SwitchPlayer();
                }
            }
        }

        private void HandleGameEnd(int winner)
        {
            try
            {
                if (winner == -1)
                {
                    view.ShowWinner("Ничья! Доска полностью заполнена.");
                }
                else if (winner == Model.HumanPlayer)
                {
                    view.ShowWinner("Вы победили!");
                }
                else if (Model.VsComputer)
                {
                    view.ShowWinner("Компьютер победил!");
                }
                else
                {
                    
                    string playerColor = GetPlayerColor(winner);
                    view.ShowWinner($"Победил игрок {winner} ({playerColor})!");
                }
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в HandleGameEnd: {ex.Message}");
                view.ShowWinner("Игра завершена");
            }
        }

        public void NewGame()
        {
            try
            {
                Model.Reset();
                view.Refresh();
                UpdateStatusText();
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в NewGame: {ex.Message}");
                view.UpdateStatus("Ошибка при запуске новой игры");
            }
        }

        public void SetGameMode(bool vsComputer, int humanPlaysAs = 1)
        {
            try
            {
                Model.SetGameMode(vsComputer, humanPlaysAs);
                UpdateStatusText();
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в SetGameMode: {ex.Message}");
                view.UpdateStatus("Ошибка при смене режима");
            }
        }

        private void UpdateStatusText()
        {
            try
            {
                if (Model.VsComputer)
                {
                    if (Model.CurrentPlayer == Model.HumanPlayer)
                        view.UpdateStatus("Ваш ход (Чёрные)");
                    else if (Model.IsAIMakingMove)
                        view.UpdateStatus("Компьютер думает...");
                    else
                        view.UpdateStatus("Ход компьютера (Белые)");
                }
                else
                {
                    string playerColor = Model.CurrentPlayer == 1 ? "Чёрные" : "Белые";
                    view.UpdateStatus($"Ход игрока {Model.CurrentPlayer} ({playerColor})");
                }
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в UpdateStatusText: {ex.Message}");
                view.UpdateStatus("Ошибка обновления статуса");
            }
        }

        private string GetPlayerColor(int player)
        {
            try
            {
                return player == 1 ? "Чёрные" : "Белые";
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в GetPlayerColor: {ex.Message}");
                return "Игрок";
            }
        }

        public void SwitchPlayer()
        {
            try
            {
                Model.SwitchPlayer();
            }
            catch (Exception ex)
            {
                PresenterText($"Ошибка в SwitchPlayer: {ex.Message}");
            }
        }
    }
}