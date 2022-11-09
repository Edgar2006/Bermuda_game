using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        GameManager.instance.LoadLevel(1);
    }
}