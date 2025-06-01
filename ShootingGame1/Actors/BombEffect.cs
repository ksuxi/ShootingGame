class BombEffect : Actor
{
    private float mTimeCount; // 経過時間
    private const float DisplayTime = 1.0f; // 表示時間
    public BombEffect(Game game) : base(game)   
    {
        var sprite = new SpriteComponent(this);
        sprite.SetTexture(GetGame().LoadTexture(GetGame().GetAssetsPath() + "bomb.png"));
        SetScale(0.0f);
    }

    public override void UpdateActor(float deltaTime)
    {
        base.UpdateActor(deltaTime);

        float changeScale = mTimeCount / DisplayTime * 3.0f;

        if (changeScale > 1.0f)
        {
            changeScale = 1.0f;
        }
        SetScale(changeScale);

        mTimeCount += deltaTime;
        if (mTimeCount >= DisplayTime)
        {
            SetState(State.EDead);
        }
    }
}