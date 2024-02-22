using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DatosDeJugador
{
    public Transform position;
    public int vida;

    public DatosDeJugador(Transform pos)
    {
        position = pos;
        vida = 0;
    }
}

public class Struct : MonoBehaviour
{
    DatosDeJugador datosJugador1;
    DatosDeJugador datosJugador2;
    DatosDeJugador datosJugador3;

    List<DatosDeJugador> jugadores = new List<DatosDeJugador>();



    private void Update()
    {
        for (int i =0; i<10; i++)
        {
            DatosDeJugador jugadorNuevo = new DatosDeJugador(transform);
            jugadores.Add(jugadorNuevo);
        }

    }

}