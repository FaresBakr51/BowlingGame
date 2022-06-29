using UnityEngine;

public class ControllButtons : MonoBehaviour
{
   [SerializeField]  private MainMenuAndNetworkManager _mainManager;
     
    public void StartAnimate()
    {
        _mainManager.PlayNextMainButtAnimation();
    }
}
