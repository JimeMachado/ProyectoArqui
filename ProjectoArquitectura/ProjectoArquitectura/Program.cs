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
        private int[] memoria_instruciones;

        private int[] bus_datos;
        private int[] bud_intruciones;

        private int[,] cache_datos_nucleo1;
        private int[,] cache_datos_nucleo2;
        private int[,] cache_datos_nucleo3;

        private int[,] cache_instruciones_nucleo1;
        private int[,] cache_instruciones_nucleo2;
        private int[,] cache_instruciones_nucleo3;

        private int PC1;
        private int PC2;
        private int PC3;

        private long reloj;

        private bool fin_programa;

        static void Main(string[] args)
        {


        }
    }

    class instruccionesP1
    {

    }

    class instruccionesP2
    {

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
