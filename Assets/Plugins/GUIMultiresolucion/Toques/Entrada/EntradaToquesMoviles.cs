using UnityEngine;
using System.Collections.Generic;

namespace GUIMultiresolucion.Toques.Entrada{

    /// <summary>
    /// Entrada de toques para dispositivos moviles
    /// </summary>
    [AddComponentMenu("GUIMultiresolucion/Entrada de toques/Moviles")]
    public class EntradaToquesMoviles : EntradaToques
    {
        #region atributos privados

        private Dictionary<int, EstadoToque> estadosToques = new Dictionary<int, EstadoToque>();
        private HashSet<int> idsToques = new HashSet<int>();

        #endregion

        #region Unity

        /// <inheritdoc />
        protected override void Update()
        {
            base.Update();

            for (var i = 0; i < Input.touchCount; ++i)
            {
                var t = Input.GetTouch(i);

                switch (t.phase)
                {
                    case TouchPhase.Began:
                        if (idsToques.Contains(t.fingerId))
                        {
                            // finalizar el toque previo (quizas se perdio el frame)
                            finalizarToque(t.fingerId);
                            int id = iniciarToque(t.position);
                            estadosToques[t.fingerId] = new EstadoToque(id, t.phase, t.position);
                        } else
                        {
                            idsToques.Add(t.fingerId);
                            int id = iniciarToque(t.position);
                            estadosToques.Add(t.fingerId, new EstadoToque(id, t.phase, t.position));
                        }
                        break;
                    case TouchPhase.Moved:
                        if (idsToques.Contains(t.fingerId))
                        {
                            var ts = estadosToques[t.fingerId];
                            estadosToques[t.fingerId] = new EstadoToque(ts.id, t.phase, t.position);
                            moverToque(ts.id, t.position);
                        } else
                        {
                            //quizas se perdio la fase Begin
                            idsToques.Add(t.fingerId);
                            int id = iniciarToque(t.position);
                            estadosToques.Add(t.fingerId, new EstadoToque(id, t.phase, t.position));
                        }
                        break;
                    case TouchPhase.Ended:
                        if (idsToques.Contains(t.fingerId))
                        {
                            var ts = estadosToques[t.fingerId];
                            idsToques.Remove(t.fingerId);
                            estadosToques.Remove(t.fingerId);
                            finalizarToque(ts.id);
                        } else
                        {
                            //quizas perdimos por completo una transicion de un dedo de begin a ended
                            int id = iniciarToque(t.position);
                            finalizarToque(id);
                        }
                        break;
                    case TouchPhase.Canceled:
                        if (idsToques.Contains(t.fingerId))
                        {
                            var ts = estadosToques[t.fingerId];
                            idsToques.Remove(t.fingerId);
                            estadosToques.Remove(t.fingerId);
                            finalizarToque(ts.id);
                        } else
                        {
                            //quizas perdimos por completo una transicion de un dedo de begin a ended
                            int id = iniciarToque(t.position);
                            cancelarToque(id);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (idsToques.Contains(t.fingerId))
                        {
                            //no hacer nada
                        } else
                        {
                            idsToques.Add(t.fingerId);
                            int id = iniciarToque(t.position);
                            estadosToques.Add(t.fingerId, new EstadoToque(id, t.phase, t.position));
                        }
                        break;
                }
            }
        }

        #endregion
    }

	internal struct EstadoToque
    {
        public int id;
        public TouchPhase fase;
        public Vector2 posicion;

        public EstadoToque(int _id, TouchPhase _fase, Vector2 _posicion)
        {
            id = _id;
            fase = _fase;
            posicion = _posicion;
        }
    }

}