class Ship : Actor
{
	public Ship(Game game) : base(game)
    {
        var sprite = new SpriteComponent(this);
        sprite.SetTexture(GetGame().LoadTexture(GetGame().GetAssetsPath() + "ship.png"));
        var mCollider = new ColliderComponent(this);
        mCollider.SetRadius(70.0f * GetScale());
    }
}