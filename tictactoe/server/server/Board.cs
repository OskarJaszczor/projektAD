using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Board
    {
        public char[,] cells;
        public int size;
        public Board()
        {
            size = 3;
            cells = new char[size, size];
            InitializeBoard();
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cells[i, j] = ' ';
                }
            }
        }
        public bool PlaceMark(int index, char mark)
        {
            int row = 0;
            int col = 0; ;
            if (index >= 1 && index <= 9)
            {
                row = (index - 1) / 3;
                col = (index - 1) % 3;
                //Console.WriteLine($"Wartość w komórce [{row}, {col}]: {cells[row, col]}");
            }

            if (row < 0 || row >= size || col < 0 || col >= size || cells[row, col] != ' ')
            {
                // Niepoprawna pozycja lub zajęta komórka
                return false;
            }

            cells[row, col] = mark;
            return true;
        }
        // Metoda sprawdzająca, czy plansza jest pełna (remis)
        public bool IsFull()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (cells[i, j] == ' ')
                    {
                        return false; 
                    }
                }
            }
            return true; 
        }

        // Metoda sprawdzająca, czy ktoś wygrał
        public bool CheckWin(char mark)
        {
            // Sprawdzanie poziomych i pionowych linii oraz obu przekątnych
            for (int i = 0; i < size; i++)
            {
                if (CheckRow(i, mark) || CheckColumn(i, mark))
                {
                    return true;
                }
            }

            if (CheckDiagonal(mark) || CheckAntiDiagonal(mark))
            {
                return true;
            }

            return false;
        }
        // Metoda sprawdzająca, czy istnieje wygrywająca linia w danym wierszu
        private bool CheckRow(int row, char mark)
        {
            for (int j = 0; j < size; j++)
            {
                if (cells[row, j] != mark)
                {
                    return false;
                }
            }
            return true;
        }

        // Metoda sprawdzająca, czy istnieje wygrywająca linia w danej kolumnie
        private bool CheckColumn(int col, char mark)
        {
            for (int i = 0; i < size; i++)
            {
                if (cells[i, col] != mark)
                {
                    return false;
                }
            }
            return true;
        }

        // Metoda sprawdzająca, czy istnieje wygrywająca przekątna
        private bool CheckDiagonal(char mark)
        {
            for (int i = 0; i < size; i++)
            {
                if (cells[i, i] != mark)
                {
                    return false;
                }
            }
            return true;
        }

        // Metoda sprawdzająca, czy istnieje wygrywająca antyprzekątna
        private bool CheckAntiDiagonal(char mark)
        {
            for (int i = 0; i < size; i++)
            {
                if (cells[i, size - 1 - i] != mark)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

