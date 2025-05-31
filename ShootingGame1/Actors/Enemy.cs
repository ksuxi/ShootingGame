using System.Numerics;

class Enemy : Actor
{
    private MoveType mEnemyMoveType;
    private float mEnemySpeed;
    private float mEnemyShakeWidth;
    private Vector2 mInitPosition;
    private float mTimeCount;
    private float mWaitTime;
    private ColliderComponent mCollider;  // ÉRÉâÉCÉ_

    public enum MoveType
        {
            STRAIGHT, // íºê¸à⁄ìÆ
            SHAKE,    // óhÇÍÇ»Ç™ÇÁà⁄ìÆ
        };

    public Enemy(Game game) : base(game)
    {
        var sprite = new SpriteComponent(this);
        sprite.SetTexture(GetGame().LoadTexture(GetGame().GetAssetsPath() + "enemy.png"));
        mCollider = new ColliderComponent(this);
        mCollider.SetRadius(50.0f * GetScale());
        game.AddEnemy(this);
    }

    public override void UpdateActor(float deltaTime)
    {
        base.UpdateActor(deltaTime);

        mTimeCount++;
        if (mTimeCount < mWaitTime)
        {
            return;
        }

        Vector2 pos = GetPosition();
        switch (mEnemyMoveType)
        {
            case MoveType.STRAIGHT:
                pos.Y += mEnemySpeed * deltaTime;
                break;
            case MoveType.SHAKE:
                pos.X = mInitPosition.X + (MathF.Sin(mTimeCount / 10.0f) * mEnemyShakeWidth);
                pos.Y += mEnemySpeed * deltaTime;
                break;
            default:
                break;
        }

        if (pos.Y > Game.ScreenHeight)
        {
            SetState(State.EDead);
            GetGame().SetNextScene(new EndScene(GetGame()));
        }
        SetPosition(pos);
    }

    public override void SetPosition(Vector2 pos)
    {
        base.SetPosition(pos);
        mInitPosition = pos;
    }

    public void SetEnemyMoveType(MoveType moveType) { mEnemyMoveType = moveType; }
    public void SetEnemySpeed(float speed) { mEnemySpeed = speed; }
    public void SetEnemyShakeWidth(float width) { mEnemyShakeWidth = width; }
    public void SetWaitTime(float time) { mWaitTime = time; }
}