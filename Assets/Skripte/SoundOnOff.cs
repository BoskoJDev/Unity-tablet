using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour
{
    [SerializeField] Button dugme;
    [SerializeField] Sprite cujno;
    [SerializeField] Sprite necujno;
    private AudioSource audio;
    public static bool zvukSePusta = true;

    void Start() => this.audio = GetComponent<AudioSource>();

    public void OnOff() => zvukSePusta = !zvukSePusta;

    void Update()
    {
        if (zvukSePusta)
        {
            this.audio.volume = PlayerPrefs.GetFloat("zvuk");
            this.dugme.image.sprite = this.cujno;
        }
        else
        {
            this.audio.volume = 0.0f;
            this.dugme.image.sprite = this.necujno;
        }
    }
}