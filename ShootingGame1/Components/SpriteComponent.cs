using SDL2;

class SpriteComponent : Component
{
    protected IntPtr mTexture;
    protected int mDrawOrder; 
    protected int mTexWidth;  
    protected int mTexHeight; 
    public SpriteComponent(Actor actor, int drawOrder = 100) : base(actor)
    {
        mTexture = IntPtr.Zero;
        mDrawOrder = drawOrder;
        mTexWidth = 0;
        mTexHeight = 0;
        mActor.GetGame().AddSprite(this);
    }

    public override void Dispose()
    {
        base.Dispose();
        mActor.GetGame().RemoveSprite(this);
    }

    public virtual void Draw(IntPtr renderer)
    {
        if (mTexture != IntPtr.Zero)
        {
            SDL.SDL_Rect r;
            r.w = (int)(mTexWidth * mActor.GetScale());
            r.h = (int)(mTexHeight * mActor.GetScale());
            r.x = (int)(mActor.GetPosition().X - r.w / 2);
            r.y = (int)(mActor.GetPosition().Y - r.h / 2);

            double degrees = mActor.GetRotation() * (180.0 / Math.PI);

            SDL.SDL_RenderCopyEx(renderer,
                                    mTexture,
                                    0,
                                    ref r,
                                    degrees,
                                    0,
                                    SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }

    public void SetTexture(IntPtr texture)
    {
        mTexture = texture;
        SDL.SDL_QueryTexture(texture, out _, out _, out mTexWidth, out mTexHeight);
    }

    public int GetDrawOrder() { return mDrawOrder; }
}