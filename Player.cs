using System.Numerics;
using Raylib_cs;

namespace Snake;

public class Player
{
    public Vector2 Position;
    public Vector2 Size { get; set; }
    public Rectangle Bounds { get; set; }
    public List<Rectangle> Body;

    private bool shouldGrow = false;

    public Player(Vector2 position, Vector2 size, List<Rectangle> body)
    {
        this.Position = position;
        this.Size = size;
        this.Body = body;

        this.Bounds = new Rectangle(this.Position, this.Size);

        this.Body.Add(Bounds);
    }

    public void SnakeUpdate(Direction direction, int speed)
    {
        Vector2 newHeadPos = Position;

        if (direction == Direction.UP)
            newHeadPos.Y -= speed;
        else if (direction == Direction.DOWN)
            newHeadPos.Y += speed;
        else if (direction == Direction.LEFT)
            newHeadPos.X -= speed;
        else if (direction == Direction.RIGHT)
            newHeadPos.X += speed;

        this.Position = newHeadPos;

        Rectangle newHead = new Rectangle(newHeadPos, Size);
        Body.Insert(0, newHead);
        if (!shouldGrow)
            Body.RemoveAt(Body.Count - 1);
        else
            shouldGrow = false;
        Bounds = newHead;
    }


    public void Grow()
    {
        shouldGrow = true;
    }

    public void Draw(Color snakeColor)
    {
        foreach (var segment in Body)
        {
            Raylib.DrawRectangleRec(segment, snakeColor);

        }
    }

}