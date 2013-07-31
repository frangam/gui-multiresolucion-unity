using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GUIMultiresolucion.Core.Fuentes{
	
	[System.Serializable]
	public class Fuente {
		
		/// <summary>
		/// Los simbolos que representan cada letra
		/// </summary>
		private SimboloLetra[] simbolos;
		
		private string nombreFichero;
		private string infoFace;
		private int size;
		private bool bold;
		private bool italic;
		private string charset;
		private bool unicode;
		private int stretchH;
		private bool smooth;
		private bool aa;
		private int[] padding = new int[4]; //arriba, derecha, abajo, izquierda;
		private int[] spacing = new int[2];
		private int commonLineHeight;
		private int _base;
		private int scaleW;
		private int scaleH;
		[SerializeField] private int espacioSize = 30;
		
		#region Getters
		public SimboloLetra[] Chars{
			get {return simbolos;}
		}
		public string NombreFichero{
			get{return nombreFichero;}	
		}	
		public string InfoFace{
			get{ return infoFace; }
		}	
		public int Size{
			get { return size;}
		}
		public bool Bold{
			get { return bold;}
		}
		public bool Italic{
			get { return italic; }
		}
		public string Charset{
			get { return charset; }
		}
		public bool Unicode{
			get { return unicode; }
		}
		public int StretchH{
			get { return stretchH; }
		}
		public bool Smooth{
			get { return smooth; }
		}
		public bool AA{
			get { return aa;}
		}
		public int[] Padding{
			get { return padding; }
		}
		public int[] Spacing{
			get { return spacing; }
		}
		public int CommonLineHeigth{
			get { return commonLineHeight; }
		}
		public int Base{
			get { return _base; }
		}
		public int ScaleW{
			get { return scaleW; }
		}
		public int ScaleH{
			get { return scaleH; }
		}
		public int EspacioSize{
			get { return espacioSize; }
		}
		#endregion
		
		public class SimboloLetra
		{	
			public int charID;
			public int posX;
			public int posY;
			public int w;
			public int h;
			
			/// <summary>
			/// Variable de control para saber desde que fila se parte para
			/// empezar a dibujar el simbolo
			/// </summary>
			public int filaPartidaDibujar = 0;
			
			/// <summary>
			/// Variable de control para saber si la fila es suelo ha terminado de dibujarse
			/// </summary>
			public int filaSuelo = 0;
			
			public int offsetx;
			public int offsety;
			public int xadvance;
			
			public SimboloLetra(){}
			public SimboloLetra(SimboloLetra sim){
				this.charID = sim.charID;
				this.h = sim.h;
				this.offsetx = sim.offsetx;
				this.offsety = sim.offsety;
				this.posX = sim.posX;
				this.posY = sim.posY;
				this.w = sim.w;
				this.xadvance = sim.xadvance;
				this.filaPartidaDibujar =0;
				this.filaSuelo = 0;
			}
		}
	
		public Fuente(string archivo)
		{
			nombreFichero = archivo;
			loadConfigfile(archivo);
		}	
		
		/// <summary>
		/// Parse the fnt file with the font definition.  Font files should be in the Resources folder and have a .txt extension.
		/// Do not inluclude the file extension in the filename!
		/// </summary>
		private void loadConfigfile( string filename )
		{
			simbolos = new SimboloLetra[256];
			for( int i = 0; i < simbolos.Length; i++ )
				simbolos[i] = new SimboloLetra();
			
			var asset = Resources.Load( filename, typeof( TextAsset ) ) as TextAsset;
			if( asset == null )
				Debug.LogError( "Could not find font config file in Resources folder: " + filename );
		
			int idNum = 0;
			
			foreach( var input in asset.text.Split( new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries ) )
			{
				//first split line into "space" chars
	       		string[] words = input.Split(' ');
				foreach( string word in words )
	        	{
					//then split line into "=" sign to get the values for each component
					string[] wordsSplit = word.Split( '=' );
					foreach( string word1 in wordsSplit )
	       	 		{
						//atributos iniciales del archivo
						//info face="Impact" size=64 bold=1 italic=0 charset="" unicode=0 stretchH=100 smooth=1 aa=1 padding=0,0,0,0 spacing=0,0
						//common lineHeight=79 base=65 scaleW=512 scaleH=512 pages=1 packed=0
						if(string.Equals (word1, "face")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							infoFace = tmp;
						}
						if(string.Equals (word1, "size")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							size = System.Int32.Parse( tmp );
						}
						if(string.Equals (word1, "bold")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							bold = (tmp == "1")? true: false;
						}
						if(string.Equals (word1, "italic")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							italic = (tmp == "1")? true: false;
						}
						if(string.Equals (word1, "charset")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							charset = tmp;
						}
						if(string.Equals (word1, "unicode")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							unicode = (tmp == "1")? true: false;
						}
						if(string.Equals (word1, "stretchH")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							stretchH = System.Int32.Parse( tmp );
						}
						if(string.Equals (word1, "smooth")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							smooth = (tmp == "1")? true: false;
						}
						if(string.Equals (word1, "aa")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							aa = (tmp == "1")? true: false;
						}
						if(string.Equals (word1, "padding")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							string[] tmpString = tmp.Split(',');
							for(int i = 0; i < tmpString.Length; i++){
								padding[i] = System.Int32.Parse(tmpString[i]);
							}
						}
						if(string.Equals (word1, "spacing")){
							string tmp = wordsSplit[1].Substring( 0, 3 );
							string[] tmpString = tmp.Split(',');
							for(int i = 0; i < tmpString.Length; i++){
								spacing[i] = System.Int32.Parse(tmpString[i]);
							}
						}
						if(string.Equals (word1, "lineHeight")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							commonLineHeight = System.Int32.Parse( tmp );
						}
						if(string.Equals (word1, "base")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							_base = System.Int32.Parse( tmp );
						}
						if(string.Equals (word1, "scaleW")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							scaleW = System.Int32.Parse( tmp );
						}
						if(string.Equals (word1, "scaleH")){
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							scaleH = System.Int32.Parse( tmp );
						}
						
						//atributos de cada char
						if( string.Equals( word1, "id" ) )
						{	
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							idNum = System.Int32.Parse( tmp );
							simbolos[idNum].charID = new int();
							simbolos[idNum].charID = idNum;
						}
						else if( string.Equals( word1, "x" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].posX = new int();
							simbolos[idNum].posX = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "y" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].posY = new int();
							simbolos[idNum].posY = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "width" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].w = new int();
							//preguntamos si el simbolo es el espacio, si lo es le ponemos el tamaño configurado para el espacio sino el que diga el archivo fnt.
							simbolos[idNum].w = (idNum==32)? EspacioSize : System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "height" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].h = new int();
							simbolos[idNum].h = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "xoffset" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].offsetx = new int();
							simbolos[idNum].offsetx = System.Int32.Parse(tmp);
						}
						else if( string.Equals( word1, "yoffset" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].offsety = new int();
							simbolos[idNum].offsety = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "xadvance" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							simbolos[idNum].xadvance = new int();
							simbolos[idNum].xadvance = System.Int32.Parse( tmp );
						}
					} // end foreach
				} // end foreach
			} // end while
		}	
		
		/// <summary>
		/// Devuelve un Array de SimbolosLetra de un cadena de texto
		/// </summary>
		/// <returns>
		/// El array con los simbolos de la caneda
		/// </returns>
		/// <param name='text'>
		/// Cadena
		/// </param>
		public List<SimboloLetra> GetCharsOfString(string text)
		{
			char[] charOfText = text.ToCharArray();
			List<SimboloLetra> charsCode = new List<SimboloLetra>();
			for(int i = 0; i<charOfText.Length; i++)
			{
//				charsCode[i] = new SimboloLetra();
//				int code = (int)charOfText[i];
//				charsCode[i] = chars[code];
				int code = (int)charOfText[i];
				charsCode.Add(new SimboloLetra(simbolos[code]));
			}
			
			return charsCode;
		}
		
		/// <summary>
		/// Divide un texto segun un tamaño dado
		/// </summary>
		/// <returns>
		/// Una lista de SimbolosLetras con todos los trozos que se ha dividido.
		/// cada elemento de la lista es una linea de texto.
		/// </returns>
		/// <param name='texto'>
		/// Texto a separar.
		/// </param>
		/// <param name='tamaño'>
		/// Tamaño.
		/// </param>
		public List<List<SimboloLetra>> TextoATrozos(string texto, int size)
		{
			// actualmente solo seapra por espacio.
			List<List<SimboloLetra>> res = new List<List<SimboloLetra>>();
			string[] chunk = texto.Split(',');
			
			foreach(string s in chunk)
			{
				res.Add(GetCharsOfString(s));
			}
			return res;
		}
	}
}