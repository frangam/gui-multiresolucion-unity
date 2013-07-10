using System;
using System.Collections.Generic;
using UnityEngine;

namespace GUIMultiresolucion.Toques{
	/// <summary>
	/// Argumentos de los eventos de los toques sobre los GUIComponentes
	/// </summary>
	[AddComponentMenu("GUIMultiresolucion/Eventos en componentes")]
	public class ArgsEventosToquesGUIComponentes: EventArgs{
		
		/// <summary>
        /// Estado previo del evento en el componente gui
        /// </summary>
        public EstadosToque estadoPrevio;
        /// <summary>
        /// Estado del toque
        /// </summary>
        public EstadosToque estado;

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="ArgsEventosToquesGUIComponentes"/>.
        /// </summary>
        /// <param name="_estado">Estado actual del toque.</param>
        /// <param name="_estadoPrevio">Estado previo del toque</param>
        public ArgsEventosToquesGUIComponentes(EstadosToque _estado, EstadosToque _estadoPrevio)
        {
            estado = _estado;
            estadoPrevio = _estadoPrevio;
        }
	}
}	

