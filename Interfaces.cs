using SFML.Graphics;
using SFML.System;

public interface IEntity
{
    Vector2f Position { get; }
    Vector2f Hitbox { get; }
}

public abstract class Actor : IEntity
{
    public Vector2f Position { get; set; }
    public float Radius { get; protected set; } = 0.25f;

    public Vector2f Hitbox => new Vector2f(Radius * 2, Radius * 2);

    bool IsBlocked(int tx, int ty,MapHandler map) //can I move there?
    {
        int tile = map.Map[ty, tx];

        if (tile == 1) return true; // wall

        if (tile == 3 && !map.ExitUnlocked)
            return true; // closed exit

        return false; // empty space/open exit
    }


    protected void Move(float dx, float dy, MapHandler map)
    {
        float newX = Position.X + dx;
        float newY = Position.Y + dy;

        // x axis
        float checkX = newX + MathF.Sign(dx) * Radius;
        int tileX = (int)checkX;
        int tileY = (int)Position.Y;

        bool blockX = tileX < 0 || tileY < 0 ||
              tileX >= map.Map.GetLength(1) ||
              tileY >= map.Map.GetLength(0) ||
              IsBlocked(tileX, tileY,map);


        if (!blockX)
            Position = new Vector2f(newX, Position.Y);

        // y axis
        float checkY = newY + MathF.Sign(dy) * Radius;
        tileX = (int)Position.X;
        tileY = (int)checkY;

        bool blockY = tileX < 0 || tileY < 0 ||
                      tileX >= map.Map.GetLength(1) ||
                      tileY >= map.Map.GetLength(0) ||
                      IsBlocked(tileX,tileY,map);

        if (!blockY)
            Position = new Vector2f(Position.X, newY);
    }

}
public interface IState
{
    void Update(float dt, RenderWindow window);
    void Draw(RenderWindow window);
}