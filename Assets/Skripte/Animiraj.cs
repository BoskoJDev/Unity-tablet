using UnityEngine;

public class Animiraj : Mirror.NetworkBehaviour
{
    [SerializeField] MenadzerIgraca igrac;
    private Animator animator;
    private static float red = 1.0f;

    void Start() => this.animator = GetComponent<Animator>();

    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > red)
        {
            this.animator.enabled = false;
            red++;
        }
    }

    void Poeni() => this.igrac.poeni += this.animator.GetInteger("poeni");
}