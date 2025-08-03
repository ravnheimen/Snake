using System.Numerics;
using System.Reflection;
using Raylib_cs;

namespace Snake;

class Util
{

    public static Direction GetDirection(Direction direction)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Up) && direction != Direction.DOWN)
            direction = Direction.UP;
        else if (Raylib.IsKeyPressed(KeyboardKey.Down) && direction != Direction.UP)
            direction = Direction.DOWN;
        else if (Raylib.IsKeyPressed(KeyboardKey.Left) && direction != Direction.RIGHT)
            direction = Direction.LEFT;
        else if (Raylib.IsKeyPressed(KeyboardKey.Right) && direction != Direction.LEFT)
            direction = Direction.RIGHT;

        return direction;

    }

    public static bool CheckPause(bool pause)
    {
        if (pause)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                pause = false;
        }
        else if (!pause)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                pause = true;
        }
        return pause;
    }

    public static int SpeedChange(int score, int speed)
    {
        if (speed < 20)
        {
            if (speed <= 3)
                speed++;
            else if (speed <= 10 && (score % 5 == 0))
                speed++;
            else if (speed > 10 && (score % 10 == 0))
                speed++;

        }


        return speed;
    }

    public static bool CheckBorderCollision(bool gameOver, int screenX, int screenY, Rectangle bounds)
    {
        Vector2 gameAreaSize = new Vector2(screenX-50, screenY-50);
        Rectangle gameAreaBounds = new Rectangle(50, 50, gameAreaSize);
        bounds.X += 25;
        bounds.Y += 25;
        if (!Raylib.CheckCollisionRecs(bounds, gameAreaBounds))
            gameOver = true;

        return gameOver;
    }

    public static bool CheckSelfCollision(List<Rectangle> body, bool selfCollision)
    {
        for (int i = 10; i < body.Count; i++)
        {
            if (Raylib.CheckCollisionRecs(body[0], body[i]))
                selfCollision = true;
            else
                selfCollision = false;
        }

        return selfCollision;
    }

    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        int r = Convert.ToByte(hex.Substring(0, 2), 16);
        int g = Convert.ToByte(hex.Substring(2, 2), 16);
        int b = Convert.ToByte(hex.Substring(4, 2), 16);

        Color color = new Color(r, g, b, 255);

        return color;
    }

    public static int ReadHigScore()
    {
        string filePath = "HighScore.txt";
        int highScore = 0;

        if (File.Exists(filePath))
        {
            string textScore = File.ReadAllText(filePath);
            bool valid = int.TryParse(textScore, out highScore);

            if (!valid)
                highScore = 0;

            return highScore;
        }
        else
        {
            File.Create(filePath);
            highScore = 0;
            return highScore;
        }
    }

    public static void WriteHighScore(int newScore)
    {
        string filePath = "HighScore.txt";
        int highScore = 0;

        if (File.Exists(filePath))
        {
            string textScore = File.ReadAllText(filePath);
            bool valid = int.TryParse(textScore, out int oldScore);

            if (oldScore <= newScore)
                highScore = newScore;
            else if (oldScore > newScore)
                highScore = oldScore;

        }
        else
        {
            File.Create(filePath);
            highScore = newScore;
        }

        string newHighScore = $"{highScore}";

        File.WriteAllText(filePath, newHighScore);

    }
    
}