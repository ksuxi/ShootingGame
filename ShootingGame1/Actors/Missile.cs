using System.Numerics;

class Missile : Actor
{
    private float mMissileSpeed = 1000.0f;
    private ColliderComponent mCollider;
    public Missile(Game game) : base(game)
    {
        var sprite = new SpriteComponent(this, 90);
        sprite.SetTexture(GetGame().LoadTexture(GetGame().GetAssetsPath() + "missile.png"));

        mCollider = new ColliderComponent(this);
        mCollider.SetRadius(50.0f * GetScale());
    }

    public override void UpdateActor(float deltaTime)
    {
        base.UpdateActor(deltaTime);

        Vector2 pos = GetPosition();
        pos.Y -= mMissileSpeed * deltaTime;
        if (pos.Y < 50.0f)
        {
            SetState(State.EDead);
        }
        SetPosition(pos);

        foreach (var enemy in GetGame().GetEnemies())
        {
            if (ColliderComponent.Intersect(mCollider, enemy.GetCollider()))
            {
                SetState(State.EDead);
                enemy.SetState(State.EDead);

                var bomb = new BombEffect(GetGame());
                bomb.SetPosition(enemy.GetPosition());
                break;
            }
        }
    }
}