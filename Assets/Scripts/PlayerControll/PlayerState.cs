public interface PlayerState
{

    void Handle(PlayerController _playercontroller);
}
public interface PinNotify{
    void Moved();
}
public struct GameModes
{

    public static bool _rankedMode;
    public static bool _battleRoyale;
}
public interface GameModeState
{

    void GameMode(PlayerController controll);
}