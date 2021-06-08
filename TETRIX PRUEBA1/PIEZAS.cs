﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tetris
{
    class PIEZAS
    {
        public static int[,] I = new int[1, 4] { { 1, 1, 1, 1 } };
        public static int[,] O = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        public static int[,] T = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        public static int[,] S = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        public static int[,] Z = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        public static int[,] J = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        public static int[,] L = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        public static List<int[,]> tetrimino = new List<int[,]>() { I, O, T, S, Z, J, L };

        private bool isRecto = false;
        private int[,] figura;
        public List<int[]> posicion = new List<int[]>();

        public PIEZAS()
        {
         
            Random random = new Random();
            figura = tetrimino[random.Next(0, 7)];
            for (int i = 23; i < 33; ++i)
            {
                for (int j = 3; j < 10; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }

            }

            Program.pintarAJuego();

            for (int i = 0; i < figura.GetLength(0); i++)
            {
                for (int j = 0; j < figura.GetLength(1); j++)
                {
                    if (figura[i, j] == 1)
                    {
                        Console.SetCursorPosition(((10 - figura.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
                        Console.Write(Program.cuadrado);
                    }
                }
            }
        }

        public void Aparecer()
        {
            for (int i = 0; i < figura.GetLength(0); i++)
            {
                for (int j = 0; j < figura.GetLength(1); j++)
                {
                    if (figura[i, j] == 1)
                    {
                        posicion.Add(new int[] { i, (10 - figura.GetLength(1)) / 2 + j });
                    }
                }
            }
            Actualizar();
        }

        public void Soltar()
        {

            if (Hayalgoabajo())
            {
                for (int i = 0; i < 4; i++)
                {
                    Program.Caída[posicion[i][0], posicion[i][1]] = 1;
                }
                Program.dejarCaer = true;

            }
            else
            {
                for (int numCount = 0; numCount < 4; numCount++)
                {
                    posicion[numCount][0] += 1;
                }
                Actualizar();
            }
        }

        public void Rotar()
        {
            List<int[]> posicionTemporal = new List<int[]>();
            for (int i = 0; i < figura.GetLength(0); i++)
            {
                for (int j = 0; j < figura.GetLength(1); j++)
                {
                    if (figura[i, j] == 1)
                        posicionTemporal.Add(new int[] { i, (10 - figura.GetLength(1)) / 2 + j });

                }
            }

            if (figura == tetrimino[0])
            {
                if (isRecto == false)
                {
                    for (int i = 0; i < posicion.Count; i++)
                    {
                        posicionTemporal[i] = TransformMatrix(posicion[i], posicion[2], "AgujasReloj");
                    }
                }
                else
                {
                    for (int i = 0; i < posicion.Count; i++)
                    {
                        posicionTemporal[i] = TransformMatrix(posicion[i], posicion[2], "Antihorario");
                    }
                }
            }

            else if (figura == tetrimino[3])
            {
                for (int i = 0; i < posicion.Count; i++)
                {
                    posicionTemporal[i] = TransformMatrix(posicion[i], posicion[3], "AgujasReloj");
                }
            }

            else if (figura == tetrimino[1]) return;
            else
            {
                for (int i = 0; i < posicion.Count; i++)
                {
                    posicionTemporal[i] = TransformMatrix(posicion[i], posicion[2], "AgujasReloj");
                }
            }


            for (int count = 0; SuperponerIZQ(posicionTemporal) != false | SuperPonerDER(posicionTemporal) != false | suporponer(posicionTemporal) != false; count++)
            {
                if (SuperponerIZQ(posicionTemporal) == true)
                {
                    for (int i = 0; i < posicion.Count; i++)
                    {
                        posicionTemporal[i][1] += 1;
                    }
                }

                if (SuperPonerDER(posicionTemporal) == true)
                {
                    for (int i = 0; i < posicion.Count; i++)
                    {
                        posicionTemporal[i][1] -= 1;
                    }
                }
                if (suporponer(posicionTemporal) == true)
                {
                    for (int i = 0; i < posicion.Count; i++)
                    {
                        posicionTemporal[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            posicion = posicionTemporal;

        }

        public int[] TransformMatrix(int[] coord, int[] axis, string dir)
        {
            int[] pcoord = { coord[0] - axis[0], coord[1] - axis[1] };
            if (dir == "Antihorario")
            {
                pcoord = new int[] { -pcoord[1], pcoord[0] };
            }
            else if (dir == "AgujasReloj")
            {
                pcoord = new int[] { pcoord[1], -pcoord[0] };
            }

            return new int[] { pcoord[0] + axis[0], pcoord[1] + axis[1] };
        }

        public bool Hayalgoabajo()
        {
            for (int i = 0; i < 4; i++)
            {
                if (posicion[i][0] + 1 >= 20)
                    return true;
                if (posicion[i][0] + 1 < 20)
                {
                    if (Program.Caída[posicion[i][0] + 1, posicion[i][1]] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool? suporponer(List<int[]> location)
        {
            List<int> coordenadas = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordenadas.Add(location[i][0]);
                if (location[i][0] >= 20)
                    return true;
                if (location[i][0] < 0)
                    return null;
                if (location[i][1] < 0)
                    return null;
                if (location[i][1] > 9)
                    return null;

            }
            for (int i = 0; i < 4; i++)
            {
                if (coordenadas.Max() - coordenadas.Min() == 3)
                {
                    if (coordenadas.Max() == location[i][0] | coordenadas.Max() - 1 == location[i][0])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordenadas.Max() == location[i][0])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool HayAlgoIZQ()
        {
            for (int i = 0; i < 4; i++)
            {
                if (posicion[i][1] == 0)
                {
                    return true;
                }
                else if (Program.Caída[posicion[i][0], posicion[i][1] - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool? SuperponerIZQ(List<int[]> location)
        {
            List<int> coordenadas = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordenadas.Add(location[i][1]);
                if (location[i][1] < 0)
                {
                    return true;
                }
                if (location[i][1] > 9)
                {
                    return false;
                }
                if (location[i][0] >= 20)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (coordenadas.Max() - coordenadas.Min() == 3)
                {
                    if (coordenadas.Min() == location[i][1] | coordenadas.Min() + 1 == location[i][1])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordenadas.Min() == location[i][1])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool HayAlgoDER()
        {
            for (int i = 0; i < 4; i++)
            {
                if (posicion[i][1] == 9)
                {
                    return true;
                }
                else if (Program.Caída[posicion[i][0], posicion[i][1] + 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool? SuperPonerDER(List<int[]> location)
        {
            List<int> coordenadas = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordenadas.Add(location[i][1]);
                if (location[i][1] > 9)
                {
                    return true;
                }
                if (location[i][1] < 0)
                {
                    return false;
                }
                if (location[i][0] >= 20)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (coordenadas.Max() - coordenadas.Min() == 3)
                {
                    if (coordenadas.Max() == location[i][1] | coordenadas.Max() - 1 == location[i][1])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordenadas.Max() == location[i][1])
                    {
                        if (Program.Caída[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void Actualizar()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Program.AJuego[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Program.AJuego[posicion[i][0], posicion[i][1]] = 1;
            }
            Program.Dibujar();
        }

    }
}














































