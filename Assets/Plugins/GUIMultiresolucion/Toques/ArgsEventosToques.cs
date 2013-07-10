using System;
using System.Collections.Generic;

namespace GUIMultiresolucion.Toques{
	/// <summary>
	/// Argumentos del cambio de estado de un toque
	/// </summary>
	public class ArgsEventosToques: EventArgs{
		/// <summary>
        /// Lista de toques que participan en el evento
        /// </summary>
        public List<Toque> toques;

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="ArgsEventosToques"/>.
        /// </summary>
        /// <param name="_toques">Lis de los toques</param>
        public ArgsEventosToques(List<Toque> _toques)
        {
            toques = _toques;
        }
	}
}	

