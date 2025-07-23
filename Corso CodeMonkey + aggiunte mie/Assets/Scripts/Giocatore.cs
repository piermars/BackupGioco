using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class Giocatore : MonoBehaviour {
    // Rendere moveSpeed pubblico può causare malfunzionamento con altre classi che utilizzano la stessa variabile, motivo per la quale è più sicuro e comodo renderla private e inserirci SerializeField
    [SerializeField] private float moveSpeed = 6.5f; // Può essere comodo renderla pubblica all'inizio giusto per far comparire il valore nell'editor e trovare più comodamente la velocità adatta
    
    private bool staCamminando; // Variabile che servirà sotto per l'animazione di camminata
    
    // Movimento del personaggio
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

        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Creiamo questo vettore poiché la posizione del giocatore va tra x,y,z e non solo x,y
        transform.position += dirGiocatore * moveSpeed * Time.deltaTime; // Con l'ultimo costrutto, evitiamo che la velocità di un giocatore vada in base ai suoi fps

        staCamminando = dirGiocatore != Vector3.zero; // il giocatore si muove se la direzione del giocatore è diversa dal vettore zero

        // Rotazione giocatore
        float rotateSpeed = 10f; // Velocità di rotazione
        transform.forward = Vector3.Slerp(transform.forward, dirGiocatore, Time.deltaTime * rotateSpeed);
    }
    public bool StaCamminando() {
        return staCamminando;
    }
}
