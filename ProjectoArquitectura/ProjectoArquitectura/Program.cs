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
        private int[32] registros;

        private int[] bus_datos;
        private int[] bus_instrucciones;

        private int[,] cache_datos_nucleo1;
        private int[,] cache_datos_nucleo2;
        private int[,] cache_datos_nucleo3;

        private int[,] cache_instrucciones_nucleo1;
        private int[,] cache_instrucciones_nucleo2;
        private int[,] cache_instrucciones_nucleo3;

        private int PC;

        private List<int> cola_contexto;
        private Dictionary<int, int> contexto;

        private WaitHandle[] administrador_banderas;

        // primer bloque de parada en banderas
        private AutoResetEvent bandera_administrador_nucleo1;
        private AutoResetEvent bandera_administrador_nucleo2;
        private AutoResetEvent bandera_administrador_nucleo3;

        // segundo bloque de para en banderas
        private AutoResetEvent bandera_nucleo1_administradpr;
        private AutoResetEvent bandera_nucleo2_administrador;
        private AutoResetEvent bandera_nucleo3_administrador;

        private int numero_hilillos;
        private int quantum;
        private long reloj;

        private bool fin_programa;

        public void inicializacion() {

            // cola de contextos 
            cola_contexto = new List<int>();
            // id del proceso
            contexto = new Dictionary<int, int>();

            // memoria de datos 0-384 por ende tenemos 384 / 4 = 96
            memoria_datos = new int[96];
            // memoria de instruciones de 384 - 1020 por ende tenemos 1020-384=636
            memoria_instrucciones = new int[636];
            registros = new int[32];

            bus_datos = new int[1];
            bus_instrucciones = new int[1];

            cache_instrucciones_nucleo1 = new int[1, 1];
            cache_instrucciones_nucleo2 = new int[1, 1];
            cache_instrucciones_nucleo3 = new int[1, 1];

            // 6 campos: 4 de palabras, 1 de bit de validez, 1 de direccion
            // 4 bloques
            cache_datos_nucleo1 = new int[6, 4];
            cache_datos_nucleo2 = new int[6, 4];
            cache_datos_nucleo3 = new int[6, 4];

            //BANDERAS
            administrador_banderas = new WaitHandle[3];

            bandera_nucleo1 = new AutoResetEvent(false);
            bandera_nucleo2 = new AutoResetEvent(false);
            bandera_nucleo3 = new AutoResetEvent(false);

            administrador_banderas[0] = bandera_nucleo1;
            administrador_banderas[1] = bandera_nucleo2;
            administrador_banderas[2] = bandera_nucleo3;


        }
        
        public void consola()
        {
            Console.WriteLine("Bienvanido al Programa de Simulación de Procesador MIPS.");
            Console.WriteLine("Por favor ingrese el numero de Hilollos que desea correr (máximo de 8)");
                // parse para asegurar que se un integer
                numero_hilillos = int.Parse(Console.ReadLine());
            Console.WriteLine("Por favor ingrese el numero de Quantum para cada hilillo.");
                // parse para asegurar que se un integer
                quantum = int.Parse(Console.ReadLine());
            
        }
        
        public void administradorHilos()
        {
    
            int contador = 0;
            
            // creacion de hilos
            Thread hilonucleo1 = new Thread(new ThreadStart(/*falta pasar el metodo*/));
            hilonucleo1.Name = "HiloNucleo1";
            hilonucleo1.Start();
            Thread hilonucleo2 = new Thread(new ThreadStart());
            hilonucleo2.Name = "HiloNucleo2";
            hilonucleo2.Start();
            Thread hilonucleo3 = new Thread(new ThreadStart());
            hilonucleo3.Name = "HiloNucleo3";
            hilonucleo3.Start();
    
            reloj = -1;
    
        }
        
        public void instrucciones(int[] inst, int hilillos)
        {
            switch (inst[0])
            {
                case 8: //DADDI Ry,Rx,n : Ry+n->rx
                    registros[inst[2]] = registros[inst[1]] + inst[3];
                    break;

                case 32: //DADD Ry,Rz,Rx : Ry+Rz->Rx
                    registros[inst[3]] = registros[inst[1]] + registros[inst[2]];
                    break;

                case 34: //DSUB Ry,Rz,Rx : Ry-Rz->Rx
                    registros[inst[3]] = registros[inst[1]] + registros[inst[2]];
                    break;

                case 12: //DMUL Ry,Rz,Rx : Ry*Rz->Rx
                    registros[inst[inst[3]] = registros[inst[1]] * registros[inst[2]];
                    break;

                case 14: //DDIV Ry,Rz,Rx : Ry/Rz->Rx
                    registros[inst[3]] = registros[inst[1]] / registros[inst[2]];
                    break;

                case 4: //BEQZ Rx,-,Etiq : si Rx==0, salta a Etiq
                    if(registros[inst[1]] == 0)
                        PC == inst[3];
                    break;

                case 5: //BNEZ Rx,-,Etiq : si Rx!=0, salta a Etiq
                    if(registros[inst[1]] != 0)
                        PC == inst[3];
                    break;

                case 3: //JAL -,-,n : PC->R31, PC->PC+n
                    registros[inst[1]] = 
                    PC += inst[3];
                    break;

                case 2: //JR Rx,-,- : PC->Rx
                    break;

                case 35: //LW Ry,Rx,n) : M(n+Ry)->Rx
                    break;

                case 43: //SW Ry,Rx,n) : Rx->M(n+Ry)
                    break;

                case 63: //FIN -,-,- : detiene el programa
                    break;
            }
        }
        
    }

}
