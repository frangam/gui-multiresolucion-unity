using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Toques;

public class EventosMenu: MonoBehaviour {

	#region Unity
	void Start(){
		GetComponent<EventosToquesGUIComponente>().estadoCambiado += HandleestadoCambiado;
	}
	#endregion
	
	#region gestion eventos de toques
	void HandleestadoCambiado (object sender, ArgsEventosToquesGUIComponentes e){
		//segun el estado del evento
		switch(e.estado){
			case EstadosToque.INICIADO:
				Debug.Log("tocando boton menu");
			break;
		}
	}
	#endregion
}
