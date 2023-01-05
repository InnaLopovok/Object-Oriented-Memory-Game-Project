namespace Ex1_Inna_Adam
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Board
    {
        ////attributes
        private int m_Rows;
        private int m_Cols;
        private char[,] m_BoardMatrix;
        private bool[,] m_ExposedCardsMatrix;
        private BoardLists m_Lists;

        ////ctor
        public Board(int GameRows, int GameCols)
        {
            m_Lists = new BoardLists(GameRows, GameCols);
            m_Rows = GameRows;
            m_Cols = GameCols;
            m_BoardMatrix = new char[m_Rows, m_Cols];
            m_ExposedCardsMatrix = new bool[m_Rows, m_Cols];

            FillMatrixFromRandList(Lists.MatrixCardsList);
        }

        ////properties
        public int Rows
        {
            get
            {
                return this.m_Rows;
            }

            set
            {
                this.m_Rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return this.m_Cols;
            }

            set
            {
                this.m_Cols = value;
            }
        }

        public char[,] BoardMatrix
        {
            get
            {
                return this.m_BoardMatrix;
            }

            set
            {
                this.m_BoardMatrix = value;
            }
        }

        public bool[,] ExposedCardsMatrix
        {
            get
            {
                return this.m_ExposedCardsMatrix;
            }

            set
            {
                this.m_ExposedCardsMatrix = value;
            }
        }

        public BoardLists Lists
        {
            get
            {
                return this.m_Lists;
            }
        }

        ////methods
        public static bool CheckUserChoiceDimensionsValidity(ref string io_UserChoiceDimensions, ref UserInterface.e_Error io_Error)
        {
            bool isDimensionsLegit = false;
            int temp_row, temp_col;

            if (io_UserChoiceDimensions.Length != 3)
            {
                isDimensionsLegit = false;
                io_Error = UserInterface.e_Error.WrongLength;
            }
            else
            {
                if (int.TryParse(io_UserChoiceDimensions[0].ToString(), out temp_row) == false || int.TryParse(io_UserChoiceDimensions[2].ToString(), out temp_col) == false || (io_UserChoiceDimensions[1].Equals('x') == false && io_UserChoiceDimensions[1].Equals('X') == false))
                {
                    isDimensionsLegit = false;
                    io_Error = UserInterface.e_Error.WrongFormat;
                }
                else
                {
                    if ((temp_col * temp_row) % 2 == 1)
                    {
                        isDimensionsLegit = false;
                        io_Error = UserInterface.e_Error.WrongDimensionsParity;
                    }
                    else
                    {
                        if (temp_row < 4 || temp_row > 6 || temp_col < 4 || temp_col > 6)
                        {
                            isDimensionsLegit = false;
                            io_Error = UserInterface.e_Error.WrongDimensionsRange;
                        }
                        else
                        {
                            isDimensionsLegit = true;
                        }
                    }
                }
            }

            return isDimensionsLegit;
        }

        public char CardStrToChar(string i_Card)
        {
            int moveRow, moveCol;
            moveRow = int.Parse(i_Card[1].ToString()) - 1; ////Analyizing card 1's content
            moveCol = (int)char.ToUpper(i_Card[0]) - (int)'A';

            return m_BoardMatrix[moveRow, moveCol];
        }

        public void FillMatrixFromRandList(List<char> i_CharList)
        {
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    BoardMatrix[i, j] = Lists.GetRandItemFromList(i_CharList);
                    i_CharList.Remove(m_BoardMatrix[i, j]);
                }
            }
        }

        public void PrintBoard()
        {
            Console.Write("    ");

            for (int i = 0; i < this.m_Cols; i++)
            {
                Console.Write("{0}   ", (char)('A' + i));
            }

            Console.Write("{0}  ", System.Environment.NewLine);

            for (int k = 0; k < (5 + 4 * (m_Cols - 1)); k++)
            {
                Console.Write("=");
            }

            for (int i = 0; i < m_Rows; i++)
            {
                Console.WriteLine();
                Console.Write("{0} |", i + 1);

                for (int j = 0; j < m_Cols; j++)
                {
                    Console.Write(" ");

                    if (IsCardExposed(i, j) == true)
                    {
                        Console.Write("{0}", m_BoardMatrix[i, j]);
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    Console.Write(" |");
                }

                Console.Write("{0}  ", System.Environment.NewLine);

                for (int k = 0; k < (5 + (4 * (this.m_Cols - 1))); k++)
                {
                    Console.Write("=");
                }
            }
        }

        public bool IsCardExposed(int i_CheckRow, int i_CheckCol)
        {
            if (this.m_ExposedCardsMatrix[i_CheckRow, i_CheckCol] == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ExposeCard(string i_Card)
        {
            int moveRow, moveCol;
            moveRow = int.Parse(i_Card[1].ToString()) - 1; ////Analyizing card 1's content
            moveCol = (int)char.ToUpper(i_Card[0]) - (int)'A';

            this.ExposedCardsMatrix[moveRow, moveCol] = true;
        }
    }
}