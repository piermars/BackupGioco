using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class Giocatore : MonoBehaviour {
    // Rendere moveSpeed pubblico può causare malfunzionamento con altre classi che utilizzano la stessa variabile, motivo per la quale è più sicuro e comodo renderla private e inserirci SerializeField
    [SerializeField] private float moveSpeed = 6.5f; // Può essere comodo renderla pubblica all'inizio giusto per far comparire il valore nell'editor e trovare più comodamente la velocità adatta
    private void Update() { // Update is called once per frame
        Vector2 inputVettore = new Vector2(0, 0); // Creiamo prima Vector2 per raccogliere gli input da tastiera orizzontale e verticale (WASD)
        if (Input.GetKey(KeyCode.W)) {
            inputVettore.y = +1; 
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVettore.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVettore.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVettore.x = +1;
        }
        inputVettore = inputVettore.normalized; // Se non utilizziamo normalized, il personaggio diagonalmente va più veloce

        Vector3 DirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Creiamo questo vettore poiché la posizione del giocatore va tra x,y,z e non solo x,y
        transform.position += DirGiocatore * moveSpeed * Time.deltaTime; // Con l'ultimo costrutto, evitiamo che la velocità di un giocatore vada in base ai suoi fps
    }
}
