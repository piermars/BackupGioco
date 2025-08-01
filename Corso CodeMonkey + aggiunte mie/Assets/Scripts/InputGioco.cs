using System;
using UnityEngine;
using UnityEngine.UI;

public class InputGioco : MonoBehaviour
{

    public event EventHandler SuInterazioneAzione; // da approfondire
    private InputAzioniGiocatore inputAzioniGiocatore; // è lo script dell'Input Actions, è come riferirsi all'Input Actions stesso

    private void Awake() { // Metodo invocato solo per gli input del giocatore
        inputAzioniGiocatore = new InputAzioniGiocatore(); // Sto creando una nuova istanza (oggetto) della classe, è come dire "Ok, crea un nuovo oggetto che rappresenta il mio schema di input (quello creato graficamente in Unity), così posso usarlo nello script per leggere cosa fa il giocatore"
        inputAzioniGiocatore.Giocatore.Enable(); // Giocatore si riferisce al parametro nell'Action Maps, non alla classe

        inputAzioniGiocatore.Giocatore.Interazione.performed += Interazione_performed;
    }

    private void Interazione_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // anziché mettere una if condition in l'interazione avviene se non è null, mettiamo il ? per rendere il codice più compatto
        SuInterazioneAzione?.Invoke(this, EventArgs.Empty); // ? è il null conditional operator, il codice si esegue solo se il valore non è nullo, altrimenti si ferma a prima delle parentesi
    }


    public Vector2 OttieniVettoreMovimentoNormalizzato () {
        Vector2 inputVettore = inputAzioniGiocatore.Giocatore.Movimento.ReadValue<Vector2>(); // Creiamo prima Vector2 per raccogliere gli input da tastiera orizzontale e verticale (WASD)

        inputVettore = inputVettore.normalized; // Se non utilizziamo normalized, il personaggio diagonalmente va più veloce
        
        return inputVettore;
    }
}
//if (Input.GetKey(KeyCode.W)) {
//    inputVettore.y = +1;
//}
//if (Input.GetKey(KeyCode.S)) {
//    inputVettore.y = -1;
//}
//if (Input.GetKey(KeyCode.A)) {
//inputVettore.x = -1;
//}
//if (Input.GetKey(KeyCode.D)) {
//inputVettore.x = +1;
//}

