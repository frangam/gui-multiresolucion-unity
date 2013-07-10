namespace GUIMultiresolucion.Toques{
	/// <summary>
    /// Los posibles estados de un toque
    /// </summary>
    public enum EstadosToque{
		/// <summary>
        /// El toque es posible
        /// </summary>
		POSIBLE,
        /// <summary>
        /// Acaba de comenzar el toque
        /// </summary>
        INICIADO,
        /// <summary>
        /// Un toque que ha empezado ya, esta cambiando
        /// </summary>
        CAMBIADO,
        /// <summary>
        /// Ha finalizado el toque
        /// </summary>
        FINALIZADO,
        /// <summary>
        /// Toque cancelado
        /// </summary>
        CANCELADO,
    }
}

