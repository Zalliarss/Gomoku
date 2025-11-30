namespace Gomoku
{
    public class Logic
    {
        static void LogicText(string a)
        {
            Console.WriteLine(a);
        }
        public const int Size = 15;

        private int[,] board = new int[Size, Size];
        private int currentPlayer = 1;
        private bool vsComputer = false;
        private int humanPlayer = 1;
        private bool isAIMakingMove = false;

        public int[,] Board => board;
        public int CurrentPlayer => currentPlayer;
        public bool VsComputer => vsComputer;
        public int HumanPlayer => humanPlayer;
        public bool IsAIMakingMove => isAIMakingMove;

        public void SetGameMode(bool againstComputer, int humanPlaysAs = 1)
        {
            try
            {
                vsComputer = againstComputer;
                humanPlayer = humanPlaysAs;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в SetGameMode: {ex.Message}");
            }
        }

        public bool MakeMove(int x, int y)
        {
            try
            {
                if (x < 0 || x >= Size || y < 0 || y >= Size) return false;
                if (board[x, y] != 0 || isAIMakingMove) return false;

                board[x, y] = currentPlayer;
                return true;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в MakeMove: {ex.Message}");
                return false;
            }
        }

      

       
        public int CheckWin(int x, int y, int player)
        {
            try
            {
                if (player == 0) return 0;

                int[][] dirs =
                {
                    new[]{1, 0}, 
                    new[]{0, 1},    
                    new[]{1, 1},   
                    new[]{1, -1},  
                };

                foreach (var d in dirs)
                {
                    int count = 1;

                   
                    for (int i = 1; i < 5; i++)
                    {
                        int nx = x + d[0] * i;
                        int ny = y + d[1] * i;
                        if (nx < 0 || ny < 0 || nx >= Size || ny >= Size) break;
                        if (board[nx, ny] != player) break;
                        count++;
                    }

                  
                    for (int i = 1; i < 5; i++)
                    {
                        int nx = x - d[0] * i;
                        int ny = y - d[1] * i;
                        if (nx < 0 || ny < 0 || nx >= Size || ny >= Size) break;
                        if (board[nx, ny] != player) break;
                        count++;
                    }

                    if (count >= 5) return player;
                }

                return 0;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в CheckWin: {ex.Message}");
                return 0;
            }
        }

       
        public int CheckGameState()
        {
            try
            {
                
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] != 0)
                        {
                            int result = CheckWin(x, y, board[x, y]);
                            if (result != 0) return result;
                        }
                    }
                }

                
                if (IsBoardFull()) return -1;

                return 0;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в CheckGameState: {ex.Message}");
                return 0;
            }
        }

        private bool IsBoardFull()
        {
            try
            {
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] == 0) return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в IsBoardFull: {ex.Message}");
                return true;
            }
        }

        public async Task MakeAIMoveAsync()
        {
            try
            {
                if (!vsComputer || currentPlayer != humanPlayer % 2 + 1) return;

                isAIMakingMove = true;
                await Task.Delay(300);

                var (x, y) = FindBestMove();
                if (x >= 0 && y >= 0)
                {
                    board[x, y] = currentPlayer;
                }

                isAIMakingMove = false;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в MakeAIMoveAsync: {ex.Message}");
                isAIMakingMove = false;
            }
        }

        public void SwitchPlayer()
        {
            try
            {
                currentPlayer = (currentPlayer == 1) ? 2 : 1;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в SwitchPlayer: {ex.Message}");
            }
        }

        private (int x, int y) FindBestMove()
        {
            try
            {
               
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] == 0)
                        {
                           
                            if (WouldWin(x, y, currentPlayer))
                            {
                                return (x, y);
                            }
                        }
                    }
                }

              
                int opponent = humanPlayer;
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] == 0)
                        {
                            
                            if (WouldWin(x, y, opponent))
                            {
                                return (x, y);
                            }
                        }
                    }
                }

               
                return FindStrategicPosition();
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в FindBestMove: {ex.Message}");
                return FindRandomMove();
            }
        }

      
        private bool WouldWin(int x, int y, int player)
        {
            try
            {
                board[x, y] = player;
                bool wouldWin = CheckWin(x, y, player) == player;
                board[x, y] = 0;
                return wouldWin;
            }
            catch
            {
                return false;
            }
        }

        private (int x, int y) FindStrategicPosition()
        {
            try
            {
                List<(int x, int y, int score)> moves = new List<(int, int, int)>();
                Random rand = new Random();

                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] == 0)
                        {
                            int score = EvaluatePosition(x, y);
                            moves.Add((x, y, score));
                        }
                    }
                }

                if (moves.Count > 0)
                {
                    moves.Sort((a, b) => b.score.CompareTo(a.score));
                    int topMoves = Math.Min(3, moves.Count);
                    var bestMove = moves[rand.Next(topMoves)];
                    return (bestMove.x, bestMove.y);
                }

                return FindRandomMove();
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в FindStrategicPosition: {ex.Message}");
                return FindRandomMove();
            }
        }

        private int EvaluatePosition(int x, int y)
        {
            try
            {
                int score = 0;
                int aiPlayer = currentPlayer;

               
                int center = Size / 2;
                score += 10 - (Math.Abs(x - center) + Math.Abs(y - center));

               
                score += EvaluateLine(x, y, aiPlayer, 5) * 10;

                int opponent = (aiPlayer == 1) ? 2 : 1;
                score += EvaluateLine(x, y, opponent, 4) * 8;

                return score;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в EvaluatePosition: {ex.Message}");
                return 0;
            }
        }

        private int EvaluateLine(int x, int y, int player, int targetLength)
        {
            int score = 0;
            int[][] dirs = { new[] { 1, 0 }, new[] { 0, 1 }, new[] { 1, 1 }, new[] { 1, -1 } };

            foreach (var d in dirs)
            {
                int count = 1;
                bool blocked1 = false, blocked2 = false;

               
                for (int i = 1; i < targetLength; i++)
                {
                    int nx = x + d[0] * i, ny = y + d[1] * i;
                    if (nx < 0 || ny < 0 || nx >= Size || ny >= Size)
                    {
                        blocked1 = true;
                        break;
                    }
                    if (board[nx, ny] == player) count++;
                    else if (board[nx, ny] != 0) { blocked1 = true; break; }
                }

              
                for (int i = 1; i < targetLength; i++)
                {
                    int nx = x - d[0] * i, ny = y - d[1] * i;
                    if (nx < 0 || ny < 0 || nx >= Size || ny >= Size)
                    {
                        blocked2 = true;
                        break;
                    }
                    if (board[nx, ny] == player) count++;
                    else if (board[nx, ny] != 0) { blocked2 = true; break; }
                }

                if (count >= targetLength)
                {
                    if (!blocked1 && !blocked2) score += 5;
                    else if (!blocked1 || !blocked2) score += 3;
                }
            }

            return score;
        }

        private (int x, int y) FindRandomMove()
        {
            try
            {
                Random rand = new Random();
                List<(int x, int y)> emptyCells = new List<(int, int)>();

                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if (board[x, y] == 0)
                            emptyCells.Add((x, y));
                    }
                }

                return emptyCells.Count > 0 ? emptyCells[rand.Next(emptyCells.Count)] : (-1, -1);
            }
            catch (Exception ex)
            {
               LogicText($"Ошибка в FindRandomMove: {ex.Message}");
                return (7, 7);
            }
        }

        public void Reset()
        {
            try
            {
                board = new int[Size, Size];
                currentPlayer = 1;
                isAIMakingMove = false;
            }
            catch (Exception ex)
            {
                LogicText($"Ошибка в Reset: {ex.Message}");
            }
        }
    }
}