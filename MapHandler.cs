using SFML.Graphics;
using SFML.System;
using RayLib;

public class MapHandler
{
    public int[,] Map { get; }
    public Player Player { get; }

    private const int screenWidth = 1280;
    private const int screenHeight = 720;

    private const int rayWidth = 320;   // liczba promieni
    private const float scaleX = (float)screenWidth / rayWidth;

    public MapHandler()
    {
        Map = new int[,] //1 - walls 2 - playerstartingpoint 0 - empty space
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,0,1,1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        Player = SpawnPlayer();
    }

    private Player SpawnPlayer()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        for (int x = 0; x < Map.GetLength(1); x++)
            if (Map[y, x] == 2)
                {
                    Map[y,x] = 0; //clear map from starting point to empty space so player can move 
                    return new Player(x + 0.5f, y + 0.5f);
                }

        throw new Exception("Starting point of player not found");
    }

    public void Update(float dt, RenderWindow window)
    {
        Player.Update(dt, this, window);
    }

    public void Draw(RenderWindow window)
    {
        //floor and "sky"
        for (int y = 0; y < screenHeight / 2; y++)
        {
            float t = (float)y / (screenHeight / 2);
            byte shade = (byte)(100 + 80 * t);

            RectangleShape row = new RectangleShape(new Vector2f(screenWidth, 1));
            row.Position = new Vector2f(0, y);
            row.FillColor = new Color(shade, shade, shade);

            window.Draw(row);
        }

        for (int y = screenHeight / 2; y < screenHeight; y++)
        {
            float t = (float)(y - screenHeight / 2) / (screenHeight / 2);
            byte shade = (byte)(50 + 100 * t);

            RectangleShape row = new RectangleShape(new Vector2f(screenWidth, 1));
            row.Position = new Vector2f(0, y);
            row.FillColor = new Color(shade, shade, shade);

            window.Draw(row);
        }

        // walls
        for (int x = 0; x < rayWidth; x++)
        {
            float cameraX = (2f * x / rayWidth) - 1f;
            float rayAngle = Player.Angle + cameraX * Player.FOV;

            var hit = Raycasting.castRay(
                Player.Position.X,
                Player.Position.Y,
                rayAngle,
                Map
            );

            float wallHeight = screenHeight / hit.Distance;
            int screenX = (int)(x * scaleX);

            RectangleShape column = new RectangleShape(
                new Vector2f(scaleX, wallHeight)
            );

            column.Position = new Vector2f(
                screenX,
                (screenHeight - wallHeight) / 2f
            );

            column.FillColor = new Color(hit.Shade, hit.Shade, hit.Shade);

            window.Draw(column);
        }
    }
}
