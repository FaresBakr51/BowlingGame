using UnityEngine;

public abstract class AiStates : MonoBehaviour
{
    public abstract void ProcessState(AiController _controller);
}
