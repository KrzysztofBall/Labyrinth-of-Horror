using SFML.Graphics;

public class MapHandler
{
    public int[,] Map {get;}
    public Player Player {get;}
    public MapHandler()
    {
        Map = new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,0,1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,1,0,1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,1,0,1,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };
        Player = SpawnPlayer();
    }
    
    private Player SpawnPlayer()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        for (int x = 0; x < Map.GetLength(1); x++)
            if (Map[y, x] == 2)
                return new Player(x + 0.5f, y + 0.5f);

        throw new Exception("Starting point of player not found");
    }

    public void Update(float dt,RenderWindow window)
    {
        //todo
        Player.Update(dt,this,window);
    }

    public void Draw(RenderWindow window)
    {
        //raycasting
    }

    public bool IsWall(int x, int y)
    {
        return Map[y, x] != 0;
    }

}