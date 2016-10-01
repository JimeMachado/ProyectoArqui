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


        public void inicializacion() {

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
    }

    class instrucciones
    {
        private void instruccionesP1(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8:
                    break;

                case 32:
                    break;

                case 34:
                    break;

                case 12:
                    break;

                case 14:
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 3:
                    break;
                case 2:
                    break;

                case 35:
                    break;

                case 43:
                    break;

                case 63:
                    break;
            }
        }

        private void instruccionesP2(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8:
                    break;

                case 32:
                    break;

                case 34:
                    break;

                case 12:
                    break;

                case 14:
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 3:
                    break;
                case 2:
                    break;

                case 35:
                    break;

                case 43:
                    break;

                case 63:
                    break;
            }
        }

        private void instruccionesP3(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8:
                    break;

                case 32:
                    break;

                case 34:
                    break;

                case 12:
                    break;

                case 14:
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 3:
                    break;
                case 2:
                    break;

                case 35:
                    break;

                case 43:
                    break;

                case 63:
                    break;
            }
        }
    }
}
