using System.Numerics;

namespace Snake;

class Food
{
    public Vector2 Position;
    public float Size;

    public Food(Vector2 Position, float size)
    {
        this.Position = Position;
        Size = size;

    }

    public void Respawn(int screenX, int screenY)
    {
        Random random = new Random();

        int x = random.Next(20, screenX - 20);
        int y = random.Next(20, screenY - 20);

        Position = new Vector2(x, y);

    }


}