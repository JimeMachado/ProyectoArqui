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

   
}
