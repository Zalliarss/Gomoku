using Microsoft.Maui.Graphics;

namespace Gomoku
{
    public class BoardDrawable : IDrawable
    {
        public static BoardDrawable Instance { get; } = new BoardDrawable();
        private Logic _model = new Logic();

        public void AttachModel(Logic model)
        {
            _model = model;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_model == null) return;

            float size = Math.Min(dirtyRect.Width, dirtyRect.Height);
            float cell = size / (Logic.Size - 1); 
            float offsetX = (dirtyRect.Width - size) / 2f;
            float offsetY = (dirtyRect.Height - size) / 2f;

            canvas.SaveState();
            canvas.Translate(offsetX, offsetY);

          
            canvas.FillColor = Color.FromArgb("#DEB887");
            canvas.FillRectangle(0, 0, size, size);

          
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 1.5f;

            for (int i = 0; i < Logic.Size; i++)
            {
                float pos = i * cell;
               
                canvas.DrawLine(0, pos, size, pos);
            
                canvas.DrawLine(pos, 0, pos, size);
            }

            
            DrawStarPoints(canvas, size, cell);

            
            for (int y = 0; y < Logic.Size; y++)
            {
                for (int x = 0; x < Logic.Size; x++)
                {
                    var v = _model.Board[x, y];
                    if (v == 0) continue;

                  
                    float cx = x * cell;
                    float cy = y * cell;
                    float r = cell / 3f; 

                    if (v == 1)
                    {
                       
                        canvas.FillColor = Colors.Black;
                        canvas.FillCircle(cx, cy, r);
                    }
                    else if (v == 2)
                    {
                        
                        canvas.FillColor = Colors.White;
                        canvas.FillCircle(cx, cy, r);
                        canvas.StrokeColor = Colors.Black;
                        canvas.StrokeSize = 1;
                        canvas.DrawCircle(cx, cy, r);
                    }
                }
            }

            canvas.RestoreState();
        }

        private void DrawStarPoints(ICanvas canvas, float size, float cell)
        {
            canvas.FillColor = Colors.Black;

            int[] starPoints = { 3, 7, 11 };

            foreach (int x in starPoints)
            {
                foreach (int y in starPoints)
                {
                    
                    if ((x == 7 && y == 7) ||
                        (x == 3 && y == 3) || (x == 3 && y == 11) ||
                        (x == 11 && y == 3) || (x == 11 && y == 11))
                    {
                        float px = x * cell;
                        float py = y * cell;
                        canvas.FillCircle(px, py, 4);
                    }
                }
            }
        }
    }
}