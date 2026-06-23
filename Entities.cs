using SFML.Graphics;
using SFML.System;
using SFML.Window;
using RayLib;
using SFML.Audio;

public class Player : Actor
{
    public float Angle { get; private set; } = 0f;
    public float Speed = 1f;
    public float RotationSpeed = 2.5f;
    public float FOV = 0.66f;

    public float Health = 50f;

    public Weapon weapon = new();

    private Font font = new Font("arial.ttf");
    private Text hpText;


    public Player(float x, float y)
    {
        Position = new Vector2f(x, y);
        hpText = new Text(font, "", 24);
        hpText.FillColor = Color.White;

    }

    public int Update(float dt, MapHandler map, RenderWindow window)
    {
        weapon.Update(dt);

        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            if (weapon.TryShoot()) Shoot(map);
        }

        HandleMouse(window);
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            Move(MathF.Cos(Angle) * Speed * dt, MathF.Sin(Angle) * Speed * dt, map);

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            Move(-MathF.Cos(Angle) * Speed * dt, -MathF.Sin(Angle) * Speed * dt, map);

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            Move(-MathF.Sin(Angle) * Speed * dt, MathF.Cos(Angle) * Speed * dt, map);

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            Move(MathF.Sin(Angle) * Speed * dt, -MathF.Cos(Angle) * Speed * dt, map);
        
        int tx = (int)Position.X;
        int ty = (int)Position.Y;

        if (map.Map[ty, tx] == 3 && map.ExitUnlocked)
        {
            return 1;
        }
        if(Health<=0) return 2;
        return 0;
    }

    public void HandleMouse(RenderWindow window)
    {
        var center = new Vector2i((int)window.Size.X / 2, (int)window.Size.Y / 2);
        var mouse = Mouse.GetPosition(window);
        int deltaX = mouse.X - center.X;
        Angle += deltaX * 0.0025f;
        Mouse.SetPosition(center, window);
    }

    private void Shoot(MapHandler map)
    {
        var hit = Raycasting.castRay(Position.X, Position.Y, Angle, map.Map);

        foreach (var enemy in map.Enemies)
        {
            if (!enemy.Alive) continue;

            Vector2f dir = enemy.Position - Position;
            float dist = MathF.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            float angleToEnemy = MathF.Atan2(dir.Y, dir.X);
            float diff = MathF.Abs(Normalize(angleToEnemy - Angle));
            if (diff < 0.1f && dist < hit.Distance)
            {
                enemy.TakeHit();
                return;
            }
        }
    }

    private float Normalize(float a)
    {
        while (a < -MathF.PI) a += 2 * MathF.PI;
        while (a > MathF.PI) a -= 2 * MathF.PI;
        return a;
    }



    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

    public void Draw(RenderWindow window)
    {
        weapon.Draw(window);
        hpText.DisplayedString = $"HP: {Health}";
        hpText.Position = new Vector2f(20, window.Size.Y - 40);
        window.Draw(hpText);
    }
}

// ======================================================
//  ENEMY — AI: idle, widzenie, pogoń, atak
// ======================================================

public class Enemy : Actor
{
    public bool Alive = true;

    public int HP = 3; // 3 strzały do zabicia
    public float Speed = 2f; // szybszy niż gracz
    public float AttackRange = 1.0f;
    private static SoundBuffer deathBuffer = new SoundBuffer("death.ogg");
    private static Sound deathSound = new Sound(deathBuffer);

    public float Damage = 10f;
    public float AttackCooldown = 1.0f;

    private float attackTimer = 0f;

    MapHandler map;

    // STUN
    private float stunTimer = 0f;
    private const float StunDuration = 0.4f;

    // Idle movement
    private Vector2f idleDirection;
    private float idleTimer = 0f;

    // Grafika
    private RectangleShape sprite;

    public Enemy(float x, float y, MapHandler map)
    {
        this.map = map;
        Position = new Vector2f(x, y);
        PickRandomIdleDirection();

        sprite = new RectangleShape(new Vector2f(0.6f, 0.6f));
        sprite.Origin = new Vector2f(0.3f, 0.3f);
    }

    private void PickRandomIdleDirection()
    {
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);
        idleDirection = new Vector2f(MathF.Cos(angle), MathF.Sin(angle));
        idleTimer = 1.0f + (float)Random.Shared.NextDouble() * 2f;
    }

    public void Update(float dt, MapHandler map, RenderWindow window)
    {
        if (!Alive) return;

        // --- BLOKADA WCHODZENIA W GRACZA ---
float minDist = map.Player.Radius + Radius + 0.5f; // 0.25 + 0.25 = 0.5

Vector2f toPlayer = map.Player.Position - Position;
float d = MathF.Sqrt(toPlayer.X * toPlayer.X + toPlayer.Y * toPlayer.Y);

if (d < minDist)
{
    // odsuń przeciwnika od gracza, ale tylko minimalnie
    Vector2f push = toPlayer / d;
    Position -= push * (minDist - d);

            // UWAGA: NIE return tutaj!
            // Bo przeciwnik nadal powinien móc zaatakować
        }


        attackTimer -= dt;

        // STUN
        if (stunTimer > 0f)
        {
            stunTimer -= dt;
            return; // przeciwnik stoi
        }

        if (CanSeePlayer(map))
            ChasePlayer(dt, map);
        else
            IdleMove(dt, map);
    }

    private bool CanSeePlayer(MapHandler map)
    {
        Vector2f dir = map.Player.Position - Position;
        float angle = MathF.Atan2(dir.Y, dir.X);

        var hit = Raycasting.castRay(Position.X, Position.Y, angle, map.Map);

        float distToPlayer = MathF.Sqrt(dir.X * dir.X + dir.Y * dir.Y);

        return hit.Distance >= distToPlayer - 0.1f;
    }

    private void ChasePlayer(float dt, MapHandler map)
    {
        Vector2f dir = map.Player.Position - Position;
        float len = MathF.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
        dir /= len;

        Move(dir.X * Speed * dt, dir.Y * Speed * dt, map);

        if (len < AttackRange)
            TryAttackPlayer(map);
    }

    private void TryAttackPlayer(MapHandler map)
    {
        if (attackTimer <= 0f)
        {
            map.Player.TakeDamage(Damage);
            attackTimer = AttackCooldown;
        }
    }

    private void IdleMove(float dt, MapHandler map)
    {
        if(CanSeePlayer(map)) return;
        idleTimer -= dt;

        Move(idleDirection.X * Speed * 0.3f * dt,
             idleDirection.Y * Speed * 0.3f * dt,
             map);

        if (idleTimer <= 0f)
            PickRandomIdleDirection();
    }

    public void TakeHit()
    {
        HP--;

        if (HP <= 0)
        {
            Alive = false;
            deathSound.Play();
            return;
        }

        stunTimer = StunDuration;
    }

    static private float Normalize(float a)
    {
        while (a < -MathF.PI) a += 2 * MathF.PI;
        while (a > MathF.PI) a -= 2 * MathF.PI;
        return a;
    }

    static private float Clamp(float value, float min, float max)
{
    if (value < min) return min;
    if (value > max) return max;
    return value;
}


    public void Draw(RenderWindow window, MapHandler map)
{
    if (!Alive) return;

    // wektor do gracza
    float dx = Position.X - map.Player.Position.X;
    float dy = Position.Y - map.Player.Position.Y;
    float dist = MathF.Sqrt(dx * dx + dy * dy);

    // kąt do przeciwnika
    float angleToEnemy = MathF.Atan2(dy, dx);
    float diff = Normalize(angleToEnemy - map.Player.Angle);

    // poza FOV
    if (MathF.Abs(diff) > map.Player.FOV)
        return;

    // wysokość i szerokość sprite’a
    float spriteHeight = 600f / dist;
    //spriteHeight = Clamp(spriteHeight, 20f, 300f);

    float spriteWidth = (Radius * 2f) * (600f / dist);
    //spriteWidth = Clamp(spriteWidth, 5f, 200f);

    // pozycja środka sprite’a na ekranie
    float centerX = (diff / map.Player.FOV + 1f) * 0.5f * window.Size.X;

    // rysujemy sprite jako pionowe paski
    int half = (int)(spriteWidth / 2f);

    for (int i = -half; i <= half; i++)
    {
        float columnX = centerX + i;

        if (columnX < 0 || columnX >= window.Size.X)
            continue;

        // kąt promienia dla tej kolumny
        float cameraX = (columnX / window.Size.X) * 2f - 1f;
        float rayAngle = map.Player.Angle + cameraX * map.Player.FOV;

        // raycast
        var hit = Raycasting.castRay(
            map.Player.Position.X,
            map.Player.Position.Y,
            rayAngle,
            map.Map
        );

        // jeśli ściana bliżej → nie rysujemy tej kolumny
        if (hit.Distance < dist - Radius)
            continue;

        // rysujemy pionowy pasek przeciwnika
        RectangleShape col = new RectangleShape(new Vector2f(1, spriteHeight));
        col.Position = new Vector2f(columnX, (window.Size.Y - spriteHeight) / 2f);

        if (HP == 3) col.FillColor = new Color(0, 150, 0);
        else if (HP == 2) col.FillColor = new Color(180, 180, 0);
        else col.FillColor = new Color(150, 0, 0);

        window.Draw(col);
    }
}



}

