using SDL2;

class ScrollSpriteComponent : SpriteComponent
{
    private float mScrollSpeedY;
    private float mOffsetY;
    public ScrollSpriteComponent(Actor actor, int drawOrder = 100) : base(actor, drawOrder)
    {
        mScrollSpeedY = 0.0f;
        mOffsetY = 0.0f;
    }

    public override void Update(float deltaTime)
    {
        mOffsetY += mScrollSpeedY * deltaTime;
        if (mOffsetY > Game.ScreenHeight)
        {
            mOffsetY -= Game.ScreenHeight;
        }
    }

    public override void Draw(nint renderer)
    {
        SDL.SDL_Rect r_bottom;
        r_bottom.w = (int)(mTexWidth * mActor.GetScale());
        r_bottom.h = (int)(mTexHeight * mActor.GetScale());
        r_bottom.x = (int)(mActor.GetPosition().X - r_bottom.w / 2);
        r_bottom.y = (int)(mActor.GetPosition().Y - r_bottom.h / 2 + mOffsetY);
        SDL.SDL_RenderCopy(renderer,
                                mTexture,
                                0,
                                ref r_bottom);

        SDL.SDL_Rect r_top;
        r_top.w = (int)(mTexWidth * mActor.GetScale());
        r_top.h = (int)(mTexHeight * mActor.GetScale());
        r_top.x = (int)(mActor.GetPosition().X - r_top.w / 2);
        r_top.y = (int)(mActor.GetPosition().Y - r_top.h / 2 - Game.ScreenHeight + mOffsetY);
        SDL.SDL_RenderCopy(renderer,
                                mTexture,
                                0,
                                ref r_top);
    }

    public void SetScrollSpeedY(float speed) { mScrollSpeedY = speed; }
}