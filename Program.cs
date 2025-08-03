using System.Numerics;
using System.Runtime;
using Raylib_cs;

namespace Snake;

enum State { TITLE, GAMEPLAY, ENDING }
public enum Direction { UP, DOWN, LEFT, RIGHT }



class Program
{
    static void Main()
    {
        const int screenX = 1280;
        const int screenY = 720;

        int highScore = 0;
        int frameCounter = 0;
        bool gameOver = false;
        bool pause = false;
        bool selfCollision = false;

        int speed = 3;
        int score = 0;

        #region Color's


        Color darkBlue = Util.HexToColor("#384B70");
        Color lightBlue = Util.HexToColor("#507687");
        Color lightColor = Util.HexToColor("#FCFAEE");
        Color redColor = Util.HexToColor("#B8001F");

        Color snakeColor = lightColor;
        Color foodColor = redColor;
        Color backgroundColor = darkBlue;
        Color titleColor = lightColor;
        Color subtleTextColor = lightBlue;

        #endregion

        #region Initialization
        Raylib.InitWindow(screenX, screenY, "SNAKE");
        State state = State.TITLE;
        Raylib.SetTargetFPS(60);

        // Player Init
        Vector2 size = new Vector2(25, 25);
        Vector2 position = new Vector2(screenX / 2 - 25, screenY / 2 - 25);
        List<Rectangle> body = new List<Rectangle>();
        Player snake = new Player(position, size, body);
        Direction direction = Direction.RIGHT;

        // Food Init
        Vector2 foodPosition = new Vector2(700, 300);
        float foodSize = 10;
        Food food = new Food(foodPosition, foodSize);
        #endregion

        while (!Raylib.WindowShouldClose())
        {
            // ------------------Logic----------------------------------
            //----------------------------------------------------------
            switch (state)
            {
                case State.TITLE: //---------Title Screen---------------
                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        frameCounter = 0;
                        state = State.GAMEPLAY;
                    }

                    if (frameCounter < 2)
                    {
                        highScore = Util.ReadHigScore();
                        frameCounter++;
                    }


                    break;

                case State.GAMEPLAY://-------Gameplay-------------------
                    if (!gameOver && !pause)
                    {
                        direction = Util.GetDirection(direction);
                        snake.SnakeUpdate(direction, speed);

                        if (Raylib.CheckCollisionCircleRec(food.Position, food.Size, snake.Bounds))
                        {
                            food.Respawn(screenX, screenY);
                            snake.Grow();
                            score++;
                            speed = Util.SpeedChange(score, speed);
                        }

                        
                        
                            
                    }
                    else if (gameOver)
                    {
                        Thread.Sleep(1500);
                        state = State.ENDING;
                    }


                    pause = Util.CheckPause(pause);
                    gameOver = Util.CheckBorderCollision(gameOver, screenX, screenY, snake.Bounds);
                    if (Util.CheckSelfCollision(snake.Body, selfCollision))
                            gameOver = true;
                    break;
                case State.ENDING://----------Ending Screen-------------

                    

                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        Util.WriteHighScore(score);
                        score = 0;
                        gameOver = false;
                        snake.Position = new Vector2(screenX / 2 - 25, screenY / 2 - 25);
                        direction = Direction.RIGHT;
                        speed = 3;
                        snake.Body = new List<Rectangle>();
                        snake = new Player(snake.Position, snake.Size, snake.Body);
                        state = State.TITLE;
                    }

                    break;
            }

            //---------------------Graphics------------------------------
            //-----------------------------------------------------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(backgroundColor);

            switch (state)
            {
                case State.TITLE://---------Title Screen-----------------
                    Raylib.DrawText("SNAKE", 500, 100, 100, titleColor);

                    Raylib.DrawText($"High Score:{highScore} ", screenX / 2 - 300, screenY / 2 - 100, 100, titleColor);

                    Raylib.DrawText("Press [ENTER] To START",
                        Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("Press [ENTER] To START", 20) / 2,
                        Raylib.GetScreenHeight() / 2 + 60, 20, subtleTextColor);
                    break;
               
                case State.GAMEPLAY://-----------Gameplay-------------------

                    snake.Draw(snakeColor);

                    Raylib.DrawCircleV(food.Position, food.Size, foodColor);


                    Raylib.DrawText($"Score: {score}", 30, 30, 30, Color.Black);

                    if (pause)
                        Raylib.DrawText("Paused", screenX / 2 - 200, screenY / 2 - 50, 50, Color.White);

                    break;

                case State.ENDING://-----------Ending Screen-------------

                    Raylib.DrawText($"Your Score: {score}", screenX / 2 - 175, screenY / 2 - 50, 50, titleColor);
                    Raylib.DrawText("Press [ENTER] To Return To Menu",
                        Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("Press [ENTER] To Return To Menu", 20) / 2,
                        Raylib.GetScreenHeight() / 2 + 60, 20, subtleTextColor);
                    break;
            }
            Raylib.EndDrawing();

        }
        Raylib.CloseWindow();
    }
}
