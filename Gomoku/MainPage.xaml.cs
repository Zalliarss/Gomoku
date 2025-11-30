using Microsoft.Maui.Controls;
using System;

namespace Gomoku
{
    public partial class MainPage : TabbedPage, IGameView
    {
        static void MPageText(string a)
        {
            Console.WriteLine(a);
        }
        private readonly Presenter presenter;
        private int gamesPlayed = 0;
        private int gamesWon = 0;

        public MainPage()
        {
            try
            {
                InitializeComponent();

                MPageText("MainPage constructor started");

                var model = new Logic();
                presenter = new Presenter(model, this);
                BoardDrawable.Instance.AttachModel(model);

                
                this.BarBackgroundColor = Colors.Transparent;
                this.BarTextColor = Colors.Transparent;

                MPageText("MainPage constructor completed");
            }
            catch (Exception ex)
            {
                MPageText($"Критическая ошибка инициализации MainPage: {ex.Message}");
                DisplayAlert("Ошибка запуска", "Не удалось инициализировать приложение", "OK");
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                MPageText("MainPage appearing");
                ShowMainMenu();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnAppearing: {ex.Message}");
            }
        }

      

        private void ShowMainMenu()
        {
            try
            {
                MainMenuView.IsVisible = true;
                GameView.IsVisible = false;
                RulesView.IsVisible = false;
                AboutView.IsVisible = false;
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в ShowMainMenu: {ex.Message}");
            }
        }

        private void ShowGameScreen()
        {
            try
            {
                MainMenuView.IsVisible = false;
                GameView.IsVisible = true;
                RulesView.IsVisible = false;
                AboutView.IsVisible = false;
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в ShowGameScreen: {ex.Message}");
            }
        }

        private void ShowRulesScreen()
        {
            try
            {
                MainMenuView.IsVisible = false;
                GameView.IsVisible = false;
                RulesView.IsVisible = true;
                AboutView.IsVisible = false;
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в ShowRulesScreen: {ex.Message}");
            }
        }

        private void ShowAboutScreen()
        {
            try
            {
                MainMenuView.IsVisible = false;
                GameView.IsVisible = false;
                RulesView.IsVisible = false;
                AboutView.IsVisible = true;
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в ShowAboutScreen: {ex.Message}");
            }
        }

       

        private void OnTwoPlayersFromMenu(object sender, System.EventArgs e)
        {
            try
            {
                presenter.SetGameMode(false);
                ShowGameScreen();
                presenter.NewGame();
                
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnTwoPlayersFromMenu: {ex.Message}");
                DisplayAlert("Ошибка", "Не удалось начать игру", "OK");
            }
        }

        private void OnVsAIFromMenu(object sender, System.EventArgs e)
        {
            try
            {
                presenter.SetGameMode(true, 1);
                ShowGameScreen();
                presenter.NewGame();
                
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnVsAIFromMenu: {ex.Message}");
                DisplayAlert("Ошибка", "Не удалось начать игру с ИИ", "OK");
            }
        }

        private void OnRulesFromMenu(object sender, System.EventArgs e)
        {
            try
            {
                ShowRulesScreen();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnRulesFromMenu: {ex.Message}");
            }
        }

        private void OnAboutFromMenu(object sender, System.EventArgs e)
        {
            try
            {
                ShowAboutScreen();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnAboutFromMenu: {ex.Message}");
            }
        }

        private async void OnExitClicked(object sender, System.EventArgs e)
        {
            try
            {
                bool result = await DisplayAlert("Выход",
                    "Вы уверены, что хотите выйти из игры?",
                    "Да", "Нет");

                if (result)
                {
                    
                    if (Application.Current != null)
                    {
                        Application.Current.Quit();
                    }
                }
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnExitClicked: {ex.Message}");
            }
        }

        private void OnBackToMenuClicked(object sender, System.EventArgs e)
        {
            try
            {
                ShowMainMenu();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnBackToMenuClicked: {ex.Message}");
            }
        }

        

        private void UpdateStats()
        {
            try
            {
                StatsLabel.Text = $"Игр сыграно: {gamesPlayed} | Побед: {gamesWon}";
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в UpdateStats: {ex.Message}");
            }
        }

     

        private void BoardView_StartInteraction(object sender, TouchEventArgs e)
        {
            try
            {
                if (e.Touches == null || e.Touches.Length == 0)
                    return;

                var p = e.Touches[0];

                double viewWidth = BoardView.Width;
                double viewHeight = BoardView.Height;

                if (viewWidth <= 0 || viewHeight <= 0)
                {
                    viewWidth = 400;
                    viewHeight = 400;
                }

                float cellSize = (float)(Math.Min(viewWidth, viewHeight) / Logic.Size);

                float offsetX = (float)((viewWidth - (cellSize * Logic.Size)) / 2);
                float offsetY = (float)((viewHeight - (cellSize * Logic.Size)) / 2);

                int x = (int)((p.X - offsetX) / cellSize);
                int y = (int)((p.Y - offsetY) / cellSize);

                if (x < 0 || y < 0 || x >= Logic.Size || y >= Logic.Size) return;

                MPageText($"Clicked: {x}, {y}");
                presenter.OnCellClicked(x, y);
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в BoardView_StartInteraction: {ex.Message}");
                DisplayAlert("Ошибка", "Не удалось обработать касание", "OK");
            }
        }

        private void OnNewGameClicked(object sender, System.EventArgs e)
        {
            try
            {
                presenter.NewGame();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в OnNewGameClicked: {ex.Message}");
                DisplayAlert("Ошибка", "Не удалось начать новую игру", "OK");
            }
        }

    

        public void Refresh()
        {
            try
            {
                BoardView.Invalidate();
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в Refresh: {ex.Message}");
            }
        }

        public void ShowWinner(string message)
        {
            try
            {
                gamesPlayed++;

                
                if (message.Contains("Вы победили") || message.Contains("Победил игрок"))
                {
                    gamesWon++;
                }

                UpdateStats();

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await DisplayAlert("Игра окончена!", message, "OK");
                        presenter.NewGame();
                    }
                    catch (Exception ex)
                    {
                        MPageText($"Ошибка в ShowWinner (внутренняя): {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в ShowWinner: {ex.Message}");
            }
        }

        public void UpdateStatus(string status)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        StatusLabel.Text = status;
                    }
                    catch (Exception ex)
                    {
                        MPageText($"Ошибка при обновлении статуса: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                MPageText($"Ошибка в UpdateStatus: {ex.Message}");
            }
        }


    }
}