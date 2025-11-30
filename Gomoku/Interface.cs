namespace Gomoku
{
    public interface IGameView
    {
        void Refresh();
        void ShowWinner(string message);
        void UpdateStatus(string status);
    }
}