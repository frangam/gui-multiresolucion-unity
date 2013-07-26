using UnityEngine;
using System;
using System.Collections;

namespace GUIMultiresolucion.Core.Fuentes{
	public class Fuente {
		
		private SimboloLetra[] chars;
		private string nombreFichero;
		
		public string NombreFichero{
			get{return nombreFichero;}	
		}
		
		public class SimboloLetra : IComparable
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
			
			#region implementacion del IComparable
			/// <summary>
			/// Compara por la altura del simbolo
			/// </summary>
			/// <returns>
			/// The to.
			/// </returns>
			/// <param name='otroSimbolo'>
			/// Otro simbolo.
			/// </param>
			public int CompareTo(System.Object otroSimbolo){
				SimboloLetra simbolo = (SimboloLetra) otroSimbolo;
				
				return this.h.CompareTo(simbolo.h);
			}
			#endregion	
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
			chars = new SimboloLetra[256];
			for( int i = 0; i < chars.Length; i++ )
				chars[i] = new SimboloLetra();
			
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
						if( string.Equals( word1, "id" ) )
						{	
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							idNum = System.Int32.Parse( tmp );
							chars[idNum].charID = new int();
							chars[idNum].charID = idNum;
						}
						else if( string.Equals( word1, "x" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].posX = new int();
							chars[idNum].posX = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "y" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].posY = new int();
							chars[idNum].posY = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "width" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].w = new int();
							chars[idNum].w = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "height" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].h = new int();
							chars[idNum].h = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "xoffset" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
	//						chars[idNum].offsetx = new int();
							chars[idNum].offsetx = System.Int32.Parse(tmp);
						}
						else if( string.Equals( word1, "yoffset" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].offsety = new int();
							chars[idNum].offsety = System.Int32.Parse( tmp );
						}
						else if( string.Equals( word1, "xadvance" ) )
						{
							string tmp = wordsSplit[1].Substring( 0, wordsSplit[1].Length );
							chars[idNum].xadvance = new int();
							chars[idNum].xadvance = System.Int32.Parse( tmp );
						}
					} // end foreach
				} // end foreach
			} // end while
		}
		
		
		public SimboloLetra[] GetCharsOfString(string text)
		{
			char[] charOfText = text.ToCharArray();
			SimboloLetra[] charsCode = new SimboloLetra[charOfText.Length];
			for(int i = 0; i<charsCode.Length; i++)
			{
				charsCode[i] = new SimboloLetra();
				int code = (int)charOfText[i];
				charsCode[i] = chars[code];
			}
			
			return charsCode;
		}
		
			
	}
}