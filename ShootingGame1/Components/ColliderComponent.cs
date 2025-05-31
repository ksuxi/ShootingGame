class ColliderComponent : Component
{
    private float mRadius;
    public ColliderComponent(Actor actor) :base(actor)
    {

    }

    public void SetRadius(float radius)
    {
        mRadius = radius;
    }

    public float GetRadius()
    {
        return mActor.GetScale() * mRadius;
    }

    public float GetScale()
    {
        return mActor.GetScale() * mRadius;
    }
}