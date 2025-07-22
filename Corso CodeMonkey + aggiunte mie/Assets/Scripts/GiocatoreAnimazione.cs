using UnityEngine;

public class GiocatoreAnimazione : MonoBehaviour
{

    private const string STA_CAMMINANDO = "StaCamminando"; // Così facendo siamo più sicuri quando scriviamo la componente all'ultima riga perché, se sbagliamo a scrivere, ce lo sottolinea di rosso


    private Animator animator; // Leggere la documentazione
    private void Awake() {
        animator = GetComponent<Animator>(); // Abbiamo un riferimento al nostro componente Animator (parametro?)
        animator.SetBool(STA_CAMMINANDO, // continuare da 1:38:11 circa
    }
}
