using UnityEngine;
using UnityEngine.UI;

public class Treperenje : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = "Pobednik je " + PlayerPrefs.GetString("pobednik");
    }
}