using UnityEngine;
using UnityEngine.UI;

public class Animiranje : MonoBehaviour
{
    [HideInInspector] public static int igraceviPoeni = 0;
    [HideInInspector] public static int protivnikoviPoeni = 0;

    void DodajPoenIgracu()
    {
        
    }

    void DodajPoenProtivniku()
    {
        
    }

    void Unisti() => Destroy(this.gameObject);
}