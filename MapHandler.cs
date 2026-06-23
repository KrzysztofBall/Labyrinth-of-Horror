using SFML.Graphics;
using SFML.System;
using RayLib;

public class MapHandler
{
    public int[,] Map { get; }
    public Player Player { get; }

    public List<Enemy> Enemies { get; } = new List<Enemy>();

    private const int screenWidth = 1280;
    private const int screenHeight = 720;

    private const int rayWidth = 320;   // liczba promieni
    private int val = 0;
    private const float scaleX = (float)screenWidth / rayWidth;
    public bool ExitUnlocked => Enemies.All(e => !e.Alive);


    public MapHandler()
    {
        Map = new int[,] //1 - walls 2 - playerstartingpoint 0 - empty space 3 - exit
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 2, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 3, 0, 1 },
            { 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1 },
            { 1, 4, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 0, 0, 1, 0, 0, 4, 0, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 4, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        Player = SpawnPlayer();
        SpawnEnemies();
    }

    private Player SpawnPlayer()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        for (int x = 0; x < Map.GetLength(1); x++)
            if (Map[y, x] == 2)
                {
                    Map[y,x] = 0;
                    return new Player(x + 0.5f, y + 0.5f);
                }

        throw new Exception("Starting point of player not found");
    }

    private void SpawnEnemies()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        for (int x = 0; x < Map.GetLength(1); x++)
        {
            if (Map[y, x] == 4)
            {
                Enemies.Add(new Enemy(x + 0.5f, y + 0.5f,this));
                Map[y, x] = 0; 
            }
        }
    }


    public int Update(float dt, RenderWindow window)
    {
        val = Player.Update(dt,this,window);
        if(val == 1) return 1;
        if(val == 2) return 2;

        foreach (var enemy in Enemies) enemy.Update(dt, this, window);
        return 0;
    }


    public void Draw(RenderWindow window)
    {
        //ceiling
        for (int y = 0; y < screenHeight / 2; y++)
        {
            float t = (float)y / (screenHeight / 2);
            byte shade = (byte)(100 + 80 * t);

            RectangleShape row = new RectangleShape(new Vector2f(screenWidth, 1));
            row.Position = new Vector2f(0, y);
            row.FillColor = new Color(shade, shade, shade);

            window.Draw(row);
        }

        //floor
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

            // Domyślny kolor ściany
            Color wallColor = new Color(hit.Shade, hit.Shade, hit.Shade);

            // Jeśli trafiliśmy w wyjście (tile == 3)
            if (hit.Tile == 3)
            {
                if (!ExitUnlocked) wallColor = new Color(hit.Shade, 0, 0);      // czerwony odcień
                else wallColor = new Color(0, hit.Shade, 0);      // zielony odcień
            }

        column.FillColor = wallColor;


            window.Draw(column);
        }

    }

    private const int MiniTile = 8; // rozmiar jednego kafla na minimapie
    private const int MiniPadding = 10; // odstęp od krawędzi ekranu


    public void DrawMinimap(RenderWindow window)
{
    int rows = Map.GetLength(0);
    int cols = Map.GetLength(1);

    // tło minimapy
    RectangleShape bg = new RectangleShape(new Vector2f(cols * MiniTile + 4, rows * MiniTile + 4));
    bg.FillColor = new Color(0, 0, 0, 150);
    bg.Position = new Vector2f(MiniPadding - 2, MiniPadding - 2);
    window.Draw(bg);

    // rysowanie mapy
    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            RectangleShape tile = new RectangleShape(new Vector2f(MiniTile, MiniTile));
            tile.Position = new Vector2f(MiniPadding + x * MiniTile, MiniPadding + y * MiniTile);

            if (Map[y, x] == 1)
                tile.FillColor = new Color(80, 80, 80); // ściana
            else if (Map[y,x] == 3)
                {
                    if (!ExitUnlocked) tile.FillColor = Color.Red;
                    else tile.FillColor = Color.Green;
                }
            else
                tile.FillColor = new Color(30, 30, 30); // pusta przestrzeń

            window.Draw(tile);
        }
    }

    // rysowanie przeciwników
    foreach (var enemy in Enemies)
    {
        if (!enemy.Alive) continue;

        CircleShape e = new CircleShape(MiniTile * 0.4f);
        e.FillColor = Color.Red;
        e.Origin = new Vector2f(e.Radius, e.Radius);
        e.Position = new Vector2f(
            MiniPadding + enemy.Position.X * MiniTile,
            MiniPadding + enemy.Position.Y * MiniTile
        );

        window.Draw(e);
    }

    // rysowanie gracza
    CircleShape p = new CircleShape(MiniTile * 0.5f);
    p.FillColor = Color.Cyan;
    p.Origin = new Vector2f(p.Radius, p.Radius);
    p.Position = new Vector2f(
        MiniPadding + Player.Position.X * MiniTile,
        MiniPadding + Player.Position.Y * MiniTile
    );
    window.Draw(p);

    // kierunek patrzenia gracza
    Vertex[] dir = new Vertex[2];
    dir[0] = new Vertex(p.Position, Color.Cyan);
    dir[1] = new Vertex(
        p.Position + new Vector2f(MathF.Cos(Player.Angle), MathF.Sin(Player.Angle)) * 20f,
        Color.Cyan
    );

    window.Draw(dir, PrimitiveType.Lines);
}

}
