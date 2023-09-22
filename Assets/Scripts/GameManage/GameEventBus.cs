using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameEventType
{
    waiting,leaderboard,menuLeader,arcademode,steamBuild,IGTbuild,PolyCadebuild,XboxBuild,ArcadeBuild,AndroidBuild
}
public class GameEventBus : MonoBehaviour
{
    private static readonly IDictionary<GameEventType,UnityEvent> Events = new Dictionary<GameEventType, UnityEvent>();

    public static void Subscribe(GameEventType eventtype , UnityAction listner)
    {
        UnityEvent thisEvent;
        if (Events.TryGetValue(eventtype, out thisEvent))
        {
            thisEvent.AddListener(listner);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listner);
            Events.Add(eventtype, thisEvent);
        }
    }
    public static void UnSubscribe(GameEventType eventtype, UnityAction listner)
    {
        UnityEvent thisEvent;
        if(Events.TryGetValue(eventtype,out thisEvent))
        {
            thisEvent.RemoveListener(listner);
        }
    }

    public static void Publish(GameEventType type)
    {
        UnityEvent thisEvent;
        if(Events.TryGetValue(type , out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
