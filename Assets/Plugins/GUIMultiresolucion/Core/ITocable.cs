using UnityEngine;
using System.Collections;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Interfaz con metodos para objetos que se quieran tocar
/// </summary>
public interface ITocable{
	bool seHaTocado(Vector2 posicionToque);
	void inicioDelToque();
	void finDelToque();
	void moviendoToque();
}


