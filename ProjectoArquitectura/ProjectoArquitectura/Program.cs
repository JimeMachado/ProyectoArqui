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
        
        private int[32] registros1;
        private int[32] registros2;
        private int[32] registros3;

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

        private Queue<int> cola_contexto;
        private Dictionary<int, int> mapaContextos;
        private int[,] contextos;

        private WaitHandle[] administrador_banderas;

        // primer bloque de parada en banderas
        private AutoResetEvent bandera_administrador_nucleo1;
        private AutoResetEvent bandera_administrador_nucleo2;
        private AutoResetEvent bandera_administrador_nucleo3;

        // segundo bloque de para en banderas
        private AutoResetEvent bandera_nucleo1_administrador;
        private AutoResetEvent bandera_nucleo2_administrador;
        private AutoResetEvent bandera_nucleo3_administrador;

        private bool[] hilosTerminados;
        
        private int numero_hilillos;
        private int tamQuantum;
        private long reloj;
        private int numero_archivos;
        
        private int quantum1;
        private int quantum2;
        private int quantum3;
        
        private int numHilillo1;
        private int numHilillo2;
        private int numHilillo3;

        private bool fin_programa;

        public Procesador() {

            fin_programa = false;
            
            reloj = -1;
            
            // cola de contextos 
            cola_contexto = new Queue<int>();
            // id del proceso
            contexto = new Dictionary<int, int>();

            // memoria de datos 0 -> 384
            memoria_datos = new int[96];
            // memoria de instruciones 40 bloques * 4 palabras por bloque * 4 bytes por bloque
            memoria_instrucciones = new int[640];
            
            registros = new int[32];
            
            hilosTerminados = new bool[6];

            bus_datos = new int[1];
            bus_instrucciones = new int[1];

            cache_instrucciones_nucleo1 = new int[17, 4];
            cache_instrucciones_nucleo2 = new int[17, 4];
            cache_instrucciones_nucleo3 = new int[17, 4];

            // 6 campos: 4 de palabras, 1 de bit de validez, 1 de direccion
            // 4 bloques
            cache_datos_nucleo1 = new int[6, 4];
            cache_datos_nucleo2 = new int[6, 4];
            cache_datos_nucleo3 = new int[6, 4];

            //"BANDERAS"
            administrador_banderas = new WaitHandle[3];
            
            bandera_administrador_nucleo1 = new AutoResetEvent(false);
            bandera_administrador_nucleo2 = new AutoResetEvent(false);
            bandera_administrador_nucleo3 = new AutoResetEvent(false);

            bandera_nucleo1_administrador = new AutoResetEvent(false);
            bandera_nucleo2_administrador = new AutoResetEvent(false);
            bandera_nucleo3_administrador = new AutoResetEvent(false);

            administrador_banderas[0] = bandera_nucleo1_administrador;
            administrador_banderas[1] = bandera_nucleo2_administrador;
            administrador_banderas[2] = bandera_nucleo3_administrador;

            // INICIALIZACIÓN
            // inicializamos la memoria principal de datos
            for (int i = 0; i < 96; i++){
                memoria_datos[i] =1;
            }
            
            // inicializamos la memoria principal de instruncion
            for (int i = 0; i < 640; i++){
                memoria_instrucciones[1];
            }
            
            // inicializamos las caches de datos de los hillos en cero
            for (int i = 0; i < 4; i++){
                for (int j =0; i < 4; i++){
                    cache_datos_nucleo1[i,j] = 0;
                    cache_datos_nucleo2[i,j] = 0;
                    cache_datos_nucleo3[i,j] = 0;
                }
            }
            
            // inicializamos los valores invalidos de las caches de datos en menos uno
            for (int i = 4; i < 6; i++){
                for (int j =0; i < 4; i++){
                    cache_datos_nucleo1[i,j] = -1;
                    cache_datos_nucleo2[i,j] = -1;
                    cache_datos_nucleo3[i,j] = -1;
                }
            }
            
            for (int i = 0; i < 17; i++){
                for (int j =0; i < 4 ; i++){
                    cache_instrucciones_nucleo1[i,j] = 0;
                    cache_instrucciones_nucleo2[i,j] = 0;
                    cache_instrucciones_nucleo1[i,j] = 0;
                }
                
            }
            
             for (int j = 0; j < 4; j++){
                cache_instrucciones_nucleo1[16,j] = -1;
                cache_instrucciones_nucleo2[16,j] = -1;
                cache_instrucciones_nucleo1[16,j] = -1;
                
            }
            
            for (int i = 0, i < 32, i++){
                registros1[i] = 0;
                registros2[i] = 0;
                registros3[i] = 0;
            }
            
            for(int i = 0; i < 6; i++){
                hilosTerminados[i] = false;
            }
        }
        
        public void pedirDatos()
        {
            Console.WriteLine("Bienvanido al Programa de Simulación de Procesador MIPS.");
            Console.WriteLine("Por favor ingrese el numero de 'hilillos' que desea correr: ");
            // parse para asegurar que se un integer
            numero_hilillos = int.Parse(Console.ReadLine());
            
                while (numero_hilillos < 1 || numero_hilillos > 8){
                    Console.WriteLine("El número de 'hilillos' escogidos no corresponde a un mínimo de 1 y máximo de 8.");
                    Console.WriteLine("Por favor ingrese nuevamente el número de 'hilillos' que desea correr.");
                     numero_hilillos = int.Parse(Console.ReadLine());
                }
            Console.WriteLine("Por favor ingrese el número de Quantum para cada 'hilillo'.");
            // parse para asegurar que se un integer
            tamQuantum = int.Parse(Console.ReadLine());
        }
        
        public void leerArchivos(){
            
            Console.WriteLine("Por favor ingrese el número de archivos que desea ejecutar: ");
            numero_archivos = int.Parse(Console.ReadLine());

                while (numero_archivos < 1 || numero_archivos > 8){
                    Console.WriteLine ("Por favor ingrese un número de archivos entre 1 y 8.")
                    numero_archivos = int.Parse(Console.ReadLine());
                }
                
                while (numero_archivos != numero_hilillos){
                    Console.WriteLine("Su parámetro ingresado para el número de 'hilillos' es diferente del número de archivos.");
                    Console.WriteLine("Por favor ingrese nuevamente un número de archivos igual al número de 'hilillos'.")
                    numero_archivos = int.Parse(Console.ReadLine());
                }
                
                // este puntero nos dice cual es la siguiente posision a guardar en la memoria de instruciones
                int puntero = 0;
                int contador = 0;
                System.IO.StreamReader sr;
            
            for (int i = 0; i < numero_archivos; i++){
                contador ++;
                int punteroNuevo = leerLinea(ref sr, puntero);
                //se agrega a la lista el id del hilo que se usa para cargar la direccionde memoria
                cola_contexto.Enqueue(contador);
                // se agrega el id del hilo y el numero de hilo
                mapaContextos.Add(contador, puntero);
                // se lee el archivo
                sr = new System.IO.StreamReader(file);
                // cargamos el archivo para leerlo linea por linea
                sr.Close();
                puntero = punteroNuevo;
            }
        }
        
         public int leerLinea(ref System.IO.StreamReader sr, int puntero){
             
            String linea;
            int[] temp;
            linea = sr.ReadLine();
            
            while ((linea != null) && (linea != "")){
                temp = linea.Split(' ').Select(int.Parse).ToArray();

                // cargamos las intruciones en la posicion de la memoria de instruciones
                for (int i = 0; i < temp.Count; i++, puntero++){
                    memoria_instrucciones[puntero] = temp[i];
                }
                linea = sr.ReadLine();
            }
            return puntero;
        }
        
        public void administradorHilos()
        {
            int contador = cola_contexto.Count;
            
            // creacion de hilos
            Thread hiloNucleo1 = new Thread(new ThreadStart(nucleo1));
            hiloNucleo1.Name = "hiloNucleo1";
            hiloNucleo1.Start();
            Thread hiloNucleo2 = new Thread(new ThreadStart(nucleo2));
            hiloNucleo2.Name = "hiloNucleo2";
            hiloNucleo2.Start();
            Thread hiloNucleo3 = new Thread(new ThreadStart(nucleo3));
            hiloNucleo3.Name = "hiloNucleo3";
            hiloNucleo3.Start();
            
            while(cola_contexto.Count > 0) {
                if(contador - 1 == 0)
                    contador = cola_contexto.Count;
                else
                    contador--;
                    
                WaitHandle.WaitAll(administrador_banderas);
                
                //revisa si se acabo el hilillo del nucleo 1 y asigna uno nuevo con su contexto
                if(!hilosTerminados[numHilillo1]) {
                    numHilillo1 = cola_contexto.Dequeue();
                    quantum1 = tamQuantum;
                    for(int i = 0; i < 32; i++){
                        registros1[i] = contextos[numHilillo1, i]; 
                    }
                    PC1 = contextos[numHilillo1, 32];
                }
                //sino revisa si se le acabo el quantum guarda contexto y asigna uno nuevo con su contexto
                else if(quantum1 == 0) {
                    for(int n = 0; n < 32, n ++){
                            contextos[numHilillo1, n] = registros2[n];   
                        }
                    contextos[numHilillo1, 32] = PC2;
                    cola_contexto.Enqueue(numHilillo1);
                    numHilillo1 =  cola_contexto.Dequeue();
                    quantum1 = tamQuantum;
                    for(int i = 0; i < 32; i++){
                    registros1[i] = contextos[numHilillo1, i]; 
                    }
                    PC1 = contextos[numHilillo1, 32];
                }
                
                //revisa si se acabo el hilillo del nucleo 2 y asigna uno nuevo
                if(!hilosTerminados[numHilillo2] && (cola_contexto.Count > 0)) {
                    numHilillo2 = cola_contexto.Dequeue();
                    for(int i = 0; i < 32; i++){
                        registros2[i] = contextos[numHilillo2, i]; 
                    }
                    PC2 = contextos[numHilillo2, 32];
                    quantum2 = tamQuantum;
                }
                //sino revisa si se le acabo el quantum y asigna uno nuevo
                else if(quantum2 == 0) {
                    for(int n = 0; n < 32, n ++){
                        contextos[numHilillo2, n] = registros2[n];   
                    }
                    contextos[numHilillo2, 32] = PC2;
                    cola_contexto.Enqueue(numHilillo2);
                    numHilillo2 = cola_contexto.Dequeue();
                    quantum2 = tamQuantum;
                    for(int i = 0; i < 32; i++){
                        registros2[i] = contextos[numHilillo2, i]; 
                    }
                    PC2 = contextos[numHilillo2, 32];
                }
                
                //revisa si se acabo el hilillo del nucleo 3 y asigna uno nuevo
                if(!hilosTerminados[numHilillo3] && (cola_contexto.Count)) {
                    numHilillo3 = cola_contexto.Dequeue();
                    quantum3 = tamQuantum;
                    for(int i = 0; i < 32; i++){
                        registros3[i] = contextos[numHilillo3, i]; 
                    }
                    PC3 = contextos[numHilillo3, 32];
                } 
                //sino revisa si se le acabo el quantum y asigna uno nuevo
                else if(quantum3 == 0){
                    for(int n = 0; n < 32, n ++){
                        contextos[numHilillo3, n] = registros3[n];   
                    }
                    contextos[numHilillo3, 32] = PC3;
                    cola_contexto.Enqueue(numHilillo3);
                    numHilillo3 =  cola_contexto.Dequeue();
                    quantum3 = tamQuantum;
                }
                
                reloj++;
                
                bandera_administrador_nucleo1.Set();
                bandera_administrador_nucleo2.Set();
                bandera_administrador_nucleo3.Set();
            }
        }

        public void nucleo1() {
            int[] instruccionActual = new int[4];
            bool aciertoCache = false;
            bool busOcupado = true;
    
            while(!fin_programa) {
                bandera_administrador_nucleo1.WaitOne();
                for(int i = 0; i < 32; i++){
                    registros1[i] = contextos[numHilillo1, i]; 
                }
                PC1 = contextos[numHilillo1, 32];
                while(quantum1 != 0 && !hilosTerminados[numHilillo1]){
                    int i = 0;
                    int numeroBloque = PC1 / 16;
                    //revisar si esta en cache
                    if(numeroBloque == cache_instrucciones_nucleo1[16, numeroBloque % 4])
                        aciertoCache = true;
                    if(!aciertoCache){
                        while(busOcupado){
                            if(Monitor.TryEnter(bus_instrucciones)){
                                try{
                                    //subir instruccion a cache
                                    cache_instrucciones_nucleo1
                                    busOcupado = false;
                                }
                                finally{
                                    Monitor.Exit(bus_instrucciones);
                                }
                            }
                        }
                    }
                    int numeroInstruccion = PC1 % 4;
                    for(int j = numeroInstruccion * 4; j < numeroInstruccion + 4; j++){
                        instruccionActual[j] = cache_instrucciones_nucleo1[numeroInstruccion, numeroBloque];
                    }
                    PC1 += 4;
                    instruccionesNucleo1(instruccionActual, numHilillo1);
                    quantum1--;
                    bandera_nucleo1_administrador.Set();
                }
            }
        }
        
        public void nucleo2() {
            int[] instruccionActual = new int[4];
            bool aciertoCache = false;
            bool busOcupado = true;
    
            while(!fin_programa) {
                bandera_administrador_nucleo2.WaitOne();
                while(quantum2 != 0 && !hilosTerminados[numHilillo2]){
                    int i = 0;
                    int numeroBloque = PC2 / 16;
                    //revisar si esta en cache
                    if(numeroBloque == cache_instrucciones_nucleo2[16, numeroBloque % 4])
                        aciertoCache = true;
                    if(!aciertoCache){
                        while(busOcupado){
                            if(Monitor.TryEnter(bus_instrucciones)){
                                //subir instruccion a cache
                                cache_instrucciones_nucleo2
                                busOcupado = false;
                                Monitor.Exit(bus_instrucciones);
                            }
                        }
                    }
                    int numeroInstruccion = PC2 % 4;
                    for(int j = numeroInstruccion * 4; j < numeroInstruccion + 4; j++){
                        instruccionActual[j] = cache_instrucciones_nucleo2[numeroInstruccion, numeroBloque];
                    }
                    PC2 += 4;
                    instruccionesNucleo2(instruccionActual, numHilillo2);
                    quantum2--;
                    bandera_nucleo2_administrador.Set();
                }
            }
        }        
                
        public void nucleo3() {
            int[] instruccionActual = new int[4];
            bool aciertoCache = false;
            bool busOcupado = true;
            
            while(!fin_programa) {
                bandera_administrador_nucleo3.WaitOne();
                while(quantum3 != 0 && !hilosTerminados[numHilillo3]){
                    int i = 0;
                    int numeroBloque = PC3 / 16;
                    //revisar si esta en cache
                    if(numeroBloque == cache_instrucciones_nucleo3[16, numeroBloque % 4])
                        aciertoCache = true;
                    if(!aciertoCache){
                        while(busOcupado){
                            if(Monitor.TryEnter(bus_datos)){
                                try{
                                    //subir instruccion a cache
                                    cache_instrucciones_nucleo3
                                    busOcupado = false;
                                }
                                finally{
                                    Monitor.Exit(bus);
                                }
                            }
                        }
                    }
                    int numeroInstruccion = PC3 % 4;
                    for(int j = numeroInstruccion * 4; j < numeroInstruccion + 4; j++){
                        instruccionActual[j] = cache_instrucciones_nucleo3[numeroInstruccion, numeroBloque];
                    }
                    PC3 += 4;
                    instruccionesNucleo3(instruccionActual, numHilillo3);
                    quantum3--;
                    bandera_nucleo3_administrador.Set();
                }
            }
        }
        
        public void instruccionesNucleo1(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8: //DADDI Ry,Rx,n : Ry+n->rx
                    registros1[inst[2]] = registros1[inst[1]] + inst[3];
                    break;

                case 32: //DADD Ry,Rz,Rx : Ry+Rz->Rx
                    registros1[inst[3]] = registros1[inst[1]] + registros1[inst[2]];
                    break;

                case 34: //DSUB Ry,Rz,Rx : Ry-Rz->Rx
                    registros1[inst[3]] = registros1[inst[1]] + registros1[inst[2]];
                    break;

                case 12: //DMUL Ry,Rz,Rx : Ry*Rz->Rx
                    registros1[inst[inst[3]] = registros1[inst[1]] * registros1[inst[2]];
                    break;

                case 14: //DDIV Ry,Rz,Rx : Ry/Rz->Rx
                    registros1[inst[3]] = registros1[inst[1]] / registros1[inst[2]];
                    break;

                case 4: //BEQZ Rx,-,Etiq : si Rx==0, salta a Etiq
                    if(registros1[inst[1]] == 0)
                        PC1 == inst[3];
                    break;

                case 5: //BNEZ Rx,-,Etiq : si Rx!=0, salta a Etiq
                    if(registros1[inst[1]] != 0)
                        PC1 == inst[3];
                    break;

                case 3: //JAL -,-,n : PC->R31, PC->PC+n
                    registros1[31] = PC;
                    PC1 += inst[3];
                    break;

                case 2: //JR Rx,-,- : PC->Rx
                    PC1 = registros1[inst[1]];
                    break;
                
                case 35: //LW Ry,Rx,n : M(n+Ry)->Rx
                
                	int dirMem = registros1[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo1)){
                		
                	}
                	if((cache_datos_nucleo1[4, posicionCache] != bloqueMem) && (cache_datos_nucleo1[5, posicionCache] != 1)) {
                		while(Monitor.TryEnter(bus_datos)) {
                			
                		}
                		int posicionBloque = bloqueMem * 4;
                		for(int i = 0; i < 4; i++) {
                			cache_datos_nucleo1[i, posicionCache] = memoria_datos[posicionBloque];
                			posicionBloque++;
                		}
                		cache_datos_nucleo1[4, posicionCache] = bloqueMem;
                		cache_datos_nucleo1[5, posicionCache] = 1;
                	}
                	Monitor.Exit(bus_datos);
                	registros1[inst[2]] = cache_datos_nucleo1[numPalabra, posicionCache];
                	Monitor.Exit(cache_datos_nucleo1);
                	break;
                
                case 43: //SW Ry,Rx,n : Rx->M(n+Ry)
                	
                	int dirMem = registros1[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo1)) {
                		
                	}
                	while(!Monitor.TryEnter(bus_datos)) {
                		
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo2)) {
                		if(cache_datos_nucleo2[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo2[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo2);
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo3)) {
                		if(cache_datos_nucleo3[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo3[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo3);
                	}
                	if((cache_datos_nucleo1[4, posicionCache] == bloqueMem) && (cache_datos_nucleo1[5, posicionCache] == 1))
                			cache_datos_nucleo1[numPalabra, posicionCache] == registros1[inst[2]]); 
                	memoria_datos[dirMem] = registros1[inst[2]];
                	
                	Monitor.Exit(bus_datos);
                	Monitor.Exit(cache_datos_nucleo1);
                	break;
                	
                case 63: //FIN -,-,- : detiene el programa
                    hilosTerminados[hilillo];
                    break;
            }
        }
        
        public void instruccionesNucleo2(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8: //DADDI Ry,Rx,n : Ry+n->rx
                    registros2[inst[2]] = registros2[inst[1]] + inst[3];
                    break;

                case 32: //DADD Ry,Rz,Rx : Ry+Rz->Rx
                    registros2[inst[3]] = registros2[inst[1]] + registros2[inst[2]];
                    break;

                case 34: //DSUB Ry,Rz,Rx : Ry-Rz->Rx
                    registros2[inst[3]] = registros2[inst[1]] + registros2[inst[2]];
                    break;

                case 12: //DMUL Ry,Rz,Rx : Ry*Rz->Rx
                    registros2[inst[inst[3]] = registros2[inst[1]] * registros2[inst[2]];
                    break;

                case 14: //DDIV Ry,Rz,Rx : Ry/Rz->Rx
                    registros2[inst[3]] = registros2[inst[1]] / registros2[inst[2]];
                    break;

                case 4: //BEQZ Rx,-,Etiq : si Rx==0, salta a Etiq
                    if(registros2[inst[1]] == 0)
                        PC2 == inst[3];
                    break;

                case 5: //BNEZ Rx,-,Etiq : si Rx!=0, salta a Etiq
                    if(registros2[inst[1]] != 0)
                        PC2 == inst[3];
                    break;

                case 3: //JAL -,-,n : PC->R31, PC->PC+n
                    registros2[31] = PC2;
                    PC2 += inst[3];
                    break;

                case 2: //JR Rx,-,- : PC->Rx
                    PC2 = registros2[inst[1]];
                    break;

                case 35: //LW Ry,Rx,n : M(n+Ry)->Rx
                
                	int dirMem = registros2[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo2)){
                		
                	}
                	if((cache_datos_nucleo2[4, posicionCache] != bloqueMem) && (cache_datos_nucleo2[5, posicionCache] != 1)) {
                		while(Monitor.TryEnter(bus_datos)) {
                			
                		}
                		int posicionBloque = bloqueMem * 4;
                		for(int i = 0; i < 4; i++) {
                			cache_datos_nucleo2[i, posicionCache] = memoria_datos[posicionBloque];
                			posicionBloque++;
                		}
                		cache_datos_nucleo2[4, posicionCache] = bloqueMem;
                		cache_datos_nucleo2[5, posicionCache] = 1;
                	}
                	Monitor.Exit(bus_datos);
                	registros2[inst[2]] = cache_datos_nucleo2[numPalabra, posicionCache];
                	Monitor.Exit(cache_datos_nucleo2);
                	break;
                
                case 43: //SW Ry,Rx,n : Rx->M(n+Ry)
                	
                	int dirMem = registros2[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo2)) {
                		
                	}
                	while(!Monitor.TryEnter(bus_datos)) {
                		
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo3)) {
                		if(cache_datos_nucleo3[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo3[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo3);
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo1)) {
                		if(cache_datos_nucleo1[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo1[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo1);
                	}
                	if((cache_datos_nucleo2[4, posicionCache] == bloqueMem) && (cache_datos_nucleo2[5, posicionCache] == 1))
                			cache_datos_nucleo2[numPalabra, posicionCache] == registros2[inst[2]]); 
                	memoria_datos[dirMem] = registros2[inst[2]];
                	
                	Monitor.Exit(bus_datos);
                	Monitor.Exit(cache_datos_nucleo2);
                	break;

                case 63: //FIN -,-,- : detiene el programa
                    hilosTerminados[hilillo];
                    break;
            }
        }
        
        public void instruccionesNucleo2(int[] inst, int hilillo)
        {
            switch (inst[0])
            {
                case 8: //DADDI Ry,Rx,n : Ry+n->rx
                    registros3[inst[2]] = registros3[inst[1]] + inst[3];
                    break;

                case 32: //DADD Ry,Rz,Rx : Ry+Rz->Rx
                    registros3[inst[3]] = registros3[inst[1]] + registros3[inst[2]];
                    break;

                case 34: //DSUB Ry,Rz,Rx : Ry-Rz->Rx
                    registros3[inst[3]] = registros3[inst[1]] + registros3[inst[2]];
                    break;

                case 12: //DMUL Ry,Rz,Rx : Ry*Rz->Rx
                    registros3[inst[inst[3]] = registros3[inst[1]] * registros3[inst[2]];
                    break;

                case 14: //DDIV Ry,Rz,Rx : Ry/Rz->Rx
                    registros3[inst[3]] = registros3[inst[1]] / registros3[inst[2]];
                    break;

                case 4: //BEQZ Rx,-,Etiq : si Rx==0, salta a Etiq
                    if(registros3[inst[1]] == 0)
                        PC3 == inst[3];
                    break;

                case 5: //BNEZ Rx,-,Etiq : si Rx!=0, salta a Etiq
                    if(registros3[inst[1]] != 0)
                        PC3 == inst[3];
                    break;

                case 3: //JAL -,-,n : PC->R31, PC->PC+n
                    registros3[31] = PC3;
                    PC3 += inst[3];
                    break;

                case 2: //JR Rx,-,- : PC->Rx
                    PC3 = registros3[inst[1]];
                    break;

                case 35: //LW Ry,Rx,n : M(n+Ry)->Rx
                
                	int dirMem = registros3[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo2)){
                		
                	}
                	if((cache_datos_nucleo3[4, posicionCache] != bloqueMem) && (cache_datos_nucleo3[5, posicionCache] != 1)) {
                		while(Monitor.TryEnter(bus_datos)) {
                			
                		}
                		int posicionBloque = bloqueMem * 4;
                		for(int i = 0; i < 4; i++) {
                			cache_datos_nucleo3[i, posicionCache] = memoria_datos[posicionBloque];
                			posicionBloque++;
                		}
                		cache_datos_nucleo3[4, posicionCache] = bloqueMem;
                		cache_datos_nucleo3[5, posicionCache] = 1;
                	}
                	Monitor.Exit(bus_datos);
                	registros3[inst[2]] = cache_datos_nucleo3[numPalabra, posicionCache];
                	Monitor.Exit(cache_datos_nucleo3);
                	break;
                
                case 43: //SW Ry,Rx,n : Rx->M(n+Ry)
                	
                	int dirMem = registros3[1] + inst[3];
                	int numPalabra = dirMem % 4;
                	int bloqueMem = dirMem / 4;
                	int posicionCache = bloqueMem % 4;
                	
                	while(!Monitor.TryEnter(cache_datos_nucleo3)) {
                		
                	}
                	while(!Monitor.TryEnter(bus_datos)) {
                		
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo1)) {
                		if(cache_datos_nucleo1[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo1[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo1);
                	}
                	while(!Monitor.TryEnter(cache_datos_nucleo2)) {
                		if(cache_datos_nucleo2[4, posicionCache] == bloqueMem)
                			cache_datos_nucleo2[5, posicionCache] == -1);
                		Monitor.Exit(cache_datos_nucleo2);
                	}
                	if((cache_datos_nucleo3[4, posicionCache] == bloqueMem) && (cache_datos_nucleo3[5, posicionCache] == 1))
                			cache_datos_nucleo3[numPalabra, posicionCache] == registros3[inst[2]]); 
                	memoria_datos[dirMem] = registros3[inst[2]];
                	
                	Monitor.Exit(bus_datos);
                	Monitor.Exit(cache_datos_nucleo3);
                	break;

                case 63: //FIN -,-,- : detiene el programa
                    hilosTerminados[hilillo];
                    break;
            }
        }
    }
}
