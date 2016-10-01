using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArqui
{
    class Procesador
    {

        private int[] memoria_datos;
        private int[] memoria_instrucciones;

        private int[] bus_datos;
        private int[] bus_instrucciones;

        private int[,] cache_datos_nucleo1;
        private int[,] cache_datos_nucleo2;
        private int[,] cache_datos_nucleo3;

        private int[,] cache_instrucciones_nucleo1;
        private int[,] cache_instrucciones_nucleo2;
        private int[,] cache_instrucciones_nucleo3;

        private int PC1;
        private int PC2;
        private int PC3;

        private List<int> cola_contexto;

        private long reloj;

        private bool fin_programa;

        public void inicializacion(){
            
            cola_contexto = new List<int>(); // cola de contextos 
          
            memoria_datos = new int[96]; 
            memoria_instrucciones = new int[640];

            bus_datos = new int[1];
            bus_instrucciones = new int[1];

            cache_instrucciones_nucleo1 = new int[1, 1];
            cache_instrucciones_nucleo2 = new int[1, 1];
            cache_instrucciones_nucleo3 = new int[1, 1];
          
            cache_datos_nucleo1 = new int[6, 4]; 
            cache_datos_nucleo2 = new int[6, 4];
            cache_datos_nucleo3 = new int[6, 4];

        }
        



        static void Main(string[] args)
        {


        }
    }

   
}
