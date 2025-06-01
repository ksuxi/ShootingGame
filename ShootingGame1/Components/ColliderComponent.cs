using System.Numerics;

class ColliderComponent : Component
{
    private float mRadius;
    public ColliderComponent(Actor actor) :base(actor)
    {

    }

    public Vector2 GetCenter()
    {
        return mActor.GetPosition();
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

    public static bool Intersect(ColliderComponent a, ColliderComponent b)
    {
        Vector2 diff = a.GetCenter() - b.GetCenter();
        float distSq = diff.LengthSquared();

        float radDiff = a.GetRadius() + b.GetRadius();
        return distSq <= radDiff * radDiff;
    }
}