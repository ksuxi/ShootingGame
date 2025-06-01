using System;

class MyMath
{
    public static float GetRand(float min, float max)
    {
        Random random = new Random();
        return (float)random.NextDouble() * (max - min) + min;
    }
}