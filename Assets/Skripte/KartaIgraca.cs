using UnityEngine;
using UnityEngine.UI;

public class KartaIgraca : Mirror.NetworkBehaviour
{
    public Karta karta;
    [SerializeField] Sprite okrenuto;

    public bool jeNaStolu = false;

    void Start()
    {
        Button b = GetComponentInChildren<Button>();
        b.interactable = hasAuthority;
        b.image.sprite = hasAuthority ? this.karta.Sprajt : this.okrenuto;
        b.transform.parent.localScale = new Vector3(0.87f, 0.87f);
    }
}