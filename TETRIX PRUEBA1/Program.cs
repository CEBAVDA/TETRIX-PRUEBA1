using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;

namespace Tetris
{
    class Program
    {

        public static int[,] AJuego = new int[20, 10];
        public static int[,] Caída = new int[20, 10];
        public static string cuadrado = "■";
        public static Stopwatch temporizador = new Stopwatch();
        //temporizador de la caida: temporizadorC
        public static Stopwatch temporizadorC = new Stopwatch();
        //temporizador de la entrada: temporizadorE
        public static Stopwatch temporizadorE = new Stopwatch();
        public static int tiempoC, velocidadC = 300;
        public static bool dejarCaer = false;
        static PIEZAS tetrimino;
        static PIEZAS proximoTetrimino;
        public static ConsoleKeyInfo tecla;
        public static bool teclaP = false;
        public static int lineasConseguidas = 0, puntos = 0, nivel = 1;

        static void Main()
        {
            pintarAJuego();

            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Pulsa una tecla");
            Console.ReadKey(true);
           //aqui va la música
            temporizador.Start();
            temporizadorC.Start();
            long time = temporizador.ElapsedMilliseconds;
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Nivel " + nivel);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Puntos " + puntos);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("Lineas " + lineasConseguidas);
            proximoTetrimino = new PIEZAS();
            tetrimino = proximoTetrimino;
            //aparece el bloque
            tetrimino.Aparecer();
            //sale nuevo bloque
            proximoTetrimino = new PIEZAS();

            Actualizar();

            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over \n Otra partida? (Y/N)");
            string teclaPulsada = Console.ReadLine();

            if (teclaPulsada.ToUpper() == "Y")
            {
                int[,] AJuego = new int[20, 10];
                Caída = new int[20, 10];
                temporizador = new Stopwatch();
                temporizadorC = new Stopwatch();
                temporizadorE = new Stopwatch();
                velocidadC = 300;
                dejarCaer = false;
                Program.teclaP = false;
                lineasConseguidas = 0;
                puntos = 0;
                nivel = 1;
                GC.Collect();
                Console.Clear();
                Main();
            }
            else return;

        }

        private static void Actualizar()
        {
            while (true)
            {
                tiempoC = (int)temporizadorC.ElapsedMilliseconds;
                if (tiempoC > velocidadC)
                {
                    tiempoC = 0;
                    temporizadorC.Restart();
                    tetrimino.Soltar();
                }
                if (dejarCaer == true)
                {
                    tetrimino = proximoTetrimino;
                    proximoTetrimino = new PIEZAS();
                    tetrimino.Aparecer();

                    dejarCaer = false;
                }
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (Caída[0, j] == 1)
                        return;
                }

                Pulsacion();
                moverBloque();
            }
        }
        private static void moverBloque()
        {
            int combo = 0;
            for (int i = 0; i < 20; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (Caída[i, j] == 0)
                        break;
                }
                if (j == 10)
                {
                    lineasConseguidas++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        Caída[i, j] = 0;
                    }
                    int[,] newdroppedtetrominoeLocationGrid = new int[20, 10];
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            newdroppedtetrominoeLocationGrid[k + 1, l] = Caída[k, l];
                        }
                    }
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            Caída[k, l] = 0;
                        }
                    }
                    for (int k = 0; k < 20; k++)
                        for (int l = 0; l < 10; l++)
                            if (newdroppedtetrominoeLocationGrid[k, l] == 1)
                                Caída[k, l] = 1;
                    Dibujar();
                }
            }
            if (combo == 1)
                puntos += 40 * nivel;
            else if (combo == 2)
                puntos += 100 * nivel;
            else if (combo == 3)
                puntos += 300 * nivel;
            else if (combo > 3)
                puntos += 300 * combo * nivel;

            if (lineasConseguidas < 5) nivel = 1;
            else if (lineasConseguidas < 10) nivel = 2;
            else if (lineasConseguidas < 15) nivel = 3;
            else if (lineasConseguidas < 25) nivel = 4;
            else if (lineasConseguidas < 35) nivel = 5;
            else if (lineasConseguidas < 50) nivel = 6;
            else if (lineasConseguidas < 70) nivel = 7;
            else if (lineasConseguidas < 90) nivel = 8;
            else if (lineasConseguidas < 110) nivel = 9;
            else if (lineasConseguidas < 150) nivel = 10;


            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Nivel " + nivel);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Puntos " + puntos);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("Lineas Conseguidas " + lineasConseguidas);
            }

            velocidadC = 300 - 22 * nivel;

        }
        private static void Pulsacion()
        {
            if (Console.KeyAvailable)
            {
                tecla = Console.ReadKey();
                teclaP = true;
            }
            else
                teclaP = false;

            if (Program.tecla.Key == ConsoleKey.A & !tetrimino.HayAlgoIzquierda() & teclaP)
            {
                for (int i = 0; i < 4; i++)
                {
                    tetrimino.posicion[i][1] -= 1;
                }
                tetrimino.Actualizar();

            }
            else if (Program.tecla.Key == ConsoleKey.D & !tetrimino.HayAlgoDerecha() & teclaP)
            {
                for (int i = 0; i < 4; i++)
                {
                    tetrimino.posicion[i][1] += 1;
                }
                tetrimino.Actualizar();
            }
            if (Program.tecla.Key == ConsoleKey.S & teclaP)
            {
                tetrimino.Soltar();
            }
            if (Program.tecla.Key == ConsoleKey.S & teclaP)
            {
                for (; tetrimino.HayAlgoDebajo() != true;)
                {
                    tetrimino.Soltar();
                }
            }
            if (Program.tecla.Key == ConsoleKey.W & teclaP)
            {

                tetrimino.Rotar();
                tetrimino.Actualizar();
            }
        }
        public static void Dibujar()
        {
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if (AJuego[i, j] == 1 | Caída[i, j] == 1)
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(cuadrado);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

            }
        }

        public static void pintarAJuego()
        {

            Console.ForegroundColor = ConsoleColor.Cyan;

            for (int lengthCount = 0; lengthCount <= 22; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("#");
                Console.SetCursorPosition(21, lengthCount);
                Console.Write("#");
            }
            Console.SetCursorPosition(0, 20);
            for (int widthCount = 0; widthCount <= 10; widthCount++)
            {
                Console.Write("*-");
            }


            Console.ForegroundColor = ConsoleColor.Magenta;

        }

    }


}
