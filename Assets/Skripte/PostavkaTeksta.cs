using UnityEngine;
using UnityEngine.UI;

public class PostavkaTeksta : MonoBehaviour
{
    void Start() => GetComponent<Text>().text = PlayerPrefs.GetString("pobednik");
}