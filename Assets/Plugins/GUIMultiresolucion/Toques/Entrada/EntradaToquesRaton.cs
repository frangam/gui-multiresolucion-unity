using System;
using UnityEngine;

namespace GUIMultiresolucion.Toques.Entrada{
	/// <summary>
    /// Entrada de toques para simular los clicks del raton como un toque
    /// </summary>
    [AddComponentMenu("GUIMultiresolucion/Entrada de toques/Raton")]
	public class EntradaToquesRaton: EntradaToques{
		
		#region atributos privados

        private int idToqueRaton = -1;
        private int fakeIdToqueRaton = -1;
        private Vector3 posicionToqueRaton = Vector3.zero;

        #endregion

        #region Unity

        /// <inheritdoc />
        protected override void Start()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    Destroy(this);
                    return;
            }
            base.Start();
        }

        /// <inheritdoc />
        protected override void Update()
        {
            base.Update();

            var upHandled = false;
            if (Input.GetMouseButtonUp(0))
            {
                if (idToqueRaton != -1)
                {
                    finalizarToque(idToqueRaton);
                    idToqueRaton = -1;
                    upHandled = true;
                }
            }

            if (fakeIdToqueRaton > -1 && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
            {
                finalizarToque(fakeIdToqueRaton);
                fakeIdToqueRaton = -1;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var pos = Input.mousePosition;
                if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && fakeIdToqueRaton == -1)
                {
                    fakeIdToqueRaton = iniciarToque(new Vector2(pos.x, pos.y));
                } else
                {
                    idToqueRaton = iniciarToque(new Vector2(pos.x, pos.y));
					Debug.Log("posicion toque raton: "+ posicionToqueRaton + " idToque: " +idToqueRaton);
                }
				
            } else if (Input.GetMouseButton(0))
            {
                var pos = Input.mousePosition;
                if (posicionToqueRaton != pos)
                {
                    posicionToqueRaton = pos;
                    if (fakeIdToqueRaton > -1 && idToqueRaton == -1)
                    {
                        moverToque(fakeIdToqueRaton, new Vector2(pos.x, pos.y));
                    } else
                    {
                        moverToque(idToqueRaton, new Vector2(pos.x, pos.y));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && !upHandled)
            {
                finalizarToque(idToqueRaton);
                idToqueRaton = -1;
            }
        }

        #endregion
	}
}

