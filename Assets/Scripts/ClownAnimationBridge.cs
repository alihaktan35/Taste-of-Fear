using UnityEngine;

public class ClownAnimationBridge : MonoBehaviour
{
    // Animation Event'ler bu fonksiyonları çağıracak
    public void OnClownArrived()
    {
        GameManagerOrder.Instance.OnClownArrived();
    }
    
    public void OnClownExit()
    {
        GameManagerOrder.Instance.OnClownExit();
    }
}