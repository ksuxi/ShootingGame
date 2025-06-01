using System.Numerics;

class GameScene : Scene
{
    public GameScene(Game game) : base(game)
    {

    }

    public override void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var enemy = new Enemy(mGame);
            enemy.SetPosition(new Vector2(MyMath.GetRand(100.0f, Game.ScreenWidth - 100.0f), -100.0f));
            enemy.SetEnemySpeed(MyMath.GetRand(300.0f, 550.0f));
            enemy.SetScale(MyMath.GetRand(0.5f, 1.5f));

            if (i % 2 == 0)
            {
                enemy.SetEnemyMoveType(Enemy.MoveType.SHAKE);
                enemy.SetEnemyShakeWidth(MyMath.GetRand(5.0f, 15.0f));
            }

            enemy.SetWaitTime(i / 3 * MyMath.GetRand(80.0f, 100.0f));
        }
    }

    public override void Update(float deltaTime)
    {
        if (mGame.GetEnemies() .Count == 0)
        {
            mGame.SetGameClear(true);
            mGame.SetNextScene(new EndScene(mGame));
        }
    }

    public override void ProcessInput(nint state)
    {
        mGame.GetShip().ProcessKeyboard(state);
    }

    public override string GetSceneName()
    {
        return "GAME";
    }
}