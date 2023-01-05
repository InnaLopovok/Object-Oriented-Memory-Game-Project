namespace Ex1_Inna_Adam
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserInterface
    {
        ////attributes
        private GameManagement m_Game = new GameManagement();
        private List<string> m_PlayerNames = new List<string>(2);
        private int inputRows, inputCols;
        private e_Error error;

        public enum e_Error
        {
            WrongFormat,
            OutOfBounds,
            AlreadyExposed,
            WrongCharacter,
            WrongLength,
            WrongDimensionsRange,
            WrongDimensionsParity
        }

        ////properties
        public GameManagement Game
        {
            get
            {
                return this.m_Game;
            }

            set
            {
                this.m_Game = value;
            }
        }

        ////methods
        public void StartGame()
        {
            this.ParticipantsManager();
            this.BoardManager();
            this.GameProcessManager();
        }

        public void ParticipantsManager()
        {
            string numOfPlayerInput;

            Console.Write("Please enter your name: ");
            this.m_PlayerNames.Add(Console.ReadLine());

            do
            {
                Console.Write("Press '1' for Player vs PC or '2' for Player vs Player: ");
                numOfPlayerInput = Console.ReadLine();

                if (this.Game.CheckAmountOfPlayersValidity(numOfPlayerInput) == false)
                {
                    Console.WriteLine("Wrong input! Please try again.");
                }
            }
            while (this.Game.CheckAmountOfPlayersValidity(numOfPlayerInput) == false);

            if (numOfPlayerInput[0].Equals('1'))
            {
                this.m_PlayerNames.Add("Computer");
            }
            else
            {
                Console.Write("Please enter next player's name: ");
                this.m_PlayerNames.Add(Console.ReadLine());
            }

            this.Game.InitiatePlayers(this.m_PlayerNames); ////PLAYER LIST READY
        } ////initialize players setup

        public void BoardManager()
        {
            string dimensionsInput;
            bool isValid = false;

            Console.Write("Please Enter the dimensions of the game board (min 4X4, max 6X6): ");
            dimensionsInput = Console.ReadLine();

            isValid = Game.CheckDimensions(ref dimensionsInput, ref error);

            while (isValid == false)
            {
                switch (error)
                {
                    case e_Error.WrongLength:
                        Console.Write("{0}Wrong answer length! Please enter dimensions (4X4, 5X6, etc): ", System.Environment.NewLine);
                        break;
                    case e_Error.WrongFormat:
                        Console.Write("{0}Wrong answer format! Please enter dimensions (4X4, 5X6, etc): ", System.Environment.NewLine);
                        break;
                    case e_Error.WrongDimensionsParity:
                        Console.Write("{0}Dimensions' parity error! Please make sure at least one of the dimensions is divisble by 2 (4X4, 5X6, etc): ", System.Environment.NewLine);
                        break;
                    case e_Error.WrongDimensionsRange:
                        Console.Write("{0}The dimensions provided are not within the legal bounds!{0}Please enter dimensions between 4X4 and 6X6: ", System.Environment.NewLine);
                        break;
                }

                dimensionsInput = Console.ReadLine();
                isValid = Game.CheckDimensions(ref dimensionsInput, ref error);
            }

            StrDimensionsToInts(dimensionsInput, out inputRows, out inputCols);

            Game.InitiateGameBoard(inputRows, inputCols); ////GAME BOARD READY

            void StrDimensionsToInts(string i_DimensionsInput, out int o_Rows, out int o_Cols)
            {
                o_Rows = int.Parse(i_DimensionsInput[0].ToString());
                o_Cols = int.Parse(i_DimensionsInput[2].ToString());
            }
        } ////initialize board/game matrix

        public void GameProcessManager()
        {
            string anotherGameChoice, chosenCard1, chosenCard2;
            bool isGameOver = false, isQPressed = false, isValid = false, isCardRemoved = false;

            while (isQPressed == false && isGameOver == false)
            {
                for (int i = 0; i < m_PlayerNames.Count; i++)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    this.Game.Board.PrintBoard();
                    Console.Write("{0}{1}'s turn ({2} points).", System.Environment.NewLine, m_PlayerNames[i], Game.Players[i].PlayerScore);

                    if (this.m_PlayerNames[i].Contains("Computer"))
                    {
                        System.Threading.Thread.Sleep(2000);
                        Game.ComputerPlayerMove(out chosenCard1);
                        Ex02.ConsoleUtils.Screen.Clear();

                        Game.Board.PrintBoard();
                        Console.Write("{0}Computer chose {1} ", System.Environment.NewLine, chosenCard1);
                        System.Threading.Thread.Sleep(2000);

                        Ex02.ConsoleUtils.Screen.Clear();

                        Game.ComputerPlayerMove(out chosenCard2, chosenCard1);

                        Game.Board.PrintBoard();
                        Console.Write("{0}Computer chose {1} ", System.Environment.NewLine, chosenCard2);
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.Write("{0}Please enter a card (A3, D1, etc) or press 'Q' to quit: ", System.Environment.NewLine);
                        chosenCard1 = HumanPlayerMove(Console.ReadLine());

                        if (IsCardQ(chosenCard1) == true)
                        {
                            Console.WriteLine("{0}Thank you for playing!{0}", System.Environment.NewLine);
                            isQPressed = true;
                            break;
                        }

                        Ex02.ConsoleUtils.Screen.Clear();
                        Game.Board.PrintBoard();

                        Console.Write("{0}Please enter a second card (A3, D1, etc) or press 'Q' to quit: ", System.Environment.NewLine);

                        chosenCard2 = HumanPlayerMove(Console.ReadLine());

                        if (IsCardQ(chosenCard2) == true)
                        {
                            Console.WriteLine("{0}Thank you for playing!{0}", System.Environment.NewLine);
                            isQPressed = true;
                            break;
                        }

                        Ex02.ConsoleUtils.Screen.Clear();
                        Game.Board.PrintBoard();
                    }

                    if (IsCardsMatch(i, chosenCard1, chosenCard2) == true)
                    {
                        ////the game is over
                        if (Game.Board.Lists.LegitMovesList.Count == 0)
                        {
                            int maxScoreIndex = 0;
                            int maxScore = 0;

                            for (int j = 0; j < Game.Players.Length; j++)
                            {
                                if (Game.Players[j].PlayerScore > maxScore)
                                {
                                    maxScore = Game.Players[j].PlayerScore;
                                    maxScoreIndex = j;
                                }
                            }

                            Console.Write("Congrats to {0} for winning with {1} points!{2}Would You like to play again? (Y/N) : ", Game.Players[maxScoreIndex].PlayerName, maxScore, System.Environment.NewLine);
                            anotherGameChoice = Console.ReadLine();

                            isValid = Game.CheckAnotherGameValidity(ref anotherGameChoice, ref error);
                            while (isValid == false)
                            {
                                if (error == e_Error.WrongFormat)
                                {
                                    Console.WriteLine("{0}Wrong answer format! Please try again. Would you like to play another game? (Y/N): ", System.Environment.NewLine);
                                }
                                else
                                {
                                    if (error == e_Error.WrongCharacter)
                                    {
                                        Console.WriteLine("{0}Wrong letter typed! Please try again. Would you like to play another game? (Y/N)", System.Environment.NewLine);
                                    }
                                }

                                anotherGameChoice = Console.ReadLine();
                                isValid = Game.CheckAnotherGameValidity(ref anotherGameChoice, ref error);
                            }

                            if (anotherGameChoice[0].Equals('Y') || anotherGameChoice[0].Equals('y'))
                            {
                                Game.InitiatePlayers(m_PlayerNames); ////PLAYER LIST READY
                                Game.InitiateGameBoard(inputRows, inputCols); ////GAME BOARD READY
                                break;
                            }
                            else
                            {
                                Console.WriteLine("{0}Thank you for playing!", System.Environment.NewLine);
                                System.Threading.Thread.Sleep(1500);
                                isGameOver = true;
                                i = m_PlayerNames.Count;
                            }
                        }
                        else
                        {
                            ////cards matched but game isn't over
                            i--;
                            Game.Board.Lists.LegitMovesList.Remove(chosenCard1.ToUpper());
                            Game.Board.Lists.LegitMovesList.Remove(chosenCard2.ToUpper());

                            if (Game.Board.Lists.LastTwoMovesList.Contains(chosenCard1.ToUpper()) == true)
                            {
                                Game.Board.Lists.LastTwoMovesList.Remove(chosenCard1.ToUpper());
                                isCardRemoved = true;
                            }

                            if (Game.Board.Lists.LastTwoMovesList.Contains(chosenCard2.ToUpper()) == true)
                            {
                                Game.Board.Lists.LastTwoMovesList.Remove(chosenCard2.ToUpper());
                                isCardRemoved = true;
                            }

                            if (isCardRemoved == false && Game.Board.Lists.LastTwoMovesList.Count > 0)
                            {
                                Game.Board.Lists.LastTwoMovesList.RemoveAt(0); ////If there is a match outside the list, make PC "forget" 1 move
                            }

                            isCardRemoved = false;
                        }
                    }
                    else
                    {
                        ////cards didn't match
                        Game.Board.Lists.LegitMovesList.Add(chosenCard1.ToUpper());
                        Game.Board.Lists.LegitMovesList.Add(chosenCard2.ToUpper());

                        if (Game.Board.Lists.LastTwoMovesList.Contains(chosenCard1.ToUpper()) == false)
                        {
                            if (Game.Board.Lists.LastTwoMovesList.Count == 4)
                            {
                                Game.Board.Lists.LastTwoMovesList.RemoveAt(0);
                            }

                            Game.Board.Lists.LastTwoMovesList.Add(chosenCard1.ToUpper());
                        }

                        if (Game.Board.Lists.LastTwoMovesList.Contains(chosenCard2.ToUpper()) == false)
                        {
                            if (Game.Board.Lists.LastTwoMovesList.Count == 4)
                            {
                                Game.Board.Lists.LastTwoMovesList.RemoveAt(0);
                            }

                            Game.Board.Lists.LastTwoMovesList.Add(chosenCard2.ToUpper());
                        }
                    }
                }
            }
        } ////the game itself

        public bool IsCardsMatch(int i_CurrPlayerIndex, string i_FirstChosenCard, string i_SecondChosenCard)
        {
            int rows1 = int.Parse(i_FirstChosenCard[1].ToString()) - 1;
            int cols1 = char.ToUpper(i_FirstChosenCard[0]) - 'A';

            int rows2 = int.Parse(i_SecondChosenCard[1].ToString()) - 1;
            int cols2 = char.ToUpper(i_SecondChosenCard[0]) - 'A';

            if (Game.Board.BoardMatrix[rows1, cols1] == Game.Board.BoardMatrix[rows2, cols2])
            {
                Game.Players[i_CurrPlayerIndex].PlayerScore++;
                Console.Write("{0}{0}Correct! ", System.Environment.NewLine);
                System.Threading.Thread.Sleep(2000);
                Ex02.ConsoleUtils.Screen.Clear();

                return true;
            }
            else
            {
                Console.Write("{0}{0}Incorrect! ", System.Environment.NewLine);
                System.Threading.Thread.Sleep(2000);
                Ex02.ConsoleUtils.Screen.Clear();

                int moveRow1 = int.Parse(i_FirstChosenCard[1].ToString()) - 1, moveCol1 = (int)char.ToUpper(i_FirstChosenCard[0]) - (int)'A';
                int moveRow2 = int.Parse(i_SecondChosenCard[1].ToString()) - 1, moveCol2 = (int)char.ToUpper(i_SecondChosenCard[0]) - (int)'A';

                Game.Board.ExposedCardsMatrix[moveRow1, moveCol1] = false;
                Game.Board.ExposedCardsMatrix[moveRow2, moveCol2] = false;

                return false;
            }
        }

        public string HumanPlayerMove(string i_Move)
        {
            bool isValid = false;
            int moveRow, moveCol;

            if (IsCardQ(i_Move) == false)
            {
                isValid = Game.CheckUserChoiceCardValidity(ref i_Move, Game.Board.ExposedCardsMatrix, ref error);
                while (isValid == false)
                {
                    switch (error)
                    {
                        case e_Error.WrongFormat:
                            Console.Write("{0}Wrong card choice format! Please choose a card (B3, C2, F3, etc): ", System.Environment.NewLine);
                            break;
                        case e_Error.OutOfBounds:
                            Console.Write("{0}Error! The card you picked is out of bounds. Please choose a card (C4, F2, etc): ", System.Environment.NewLine);
                            break;
                        case e_Error.AlreadyExposed:
                            Console.Write("{0}Can't choose an already exposed card! Please try again: ", System.Environment.NewLine);
                            break;
                    }

                    i_Move = Console.ReadLine();
                    isValid = Game.CheckUserChoiceCardValidity(ref i_Move, Game.Board.ExposedCardsMatrix, ref error);
                }

                Game.Board.Lists.LegitMovesList.Remove(i_Move.ToUpper());

                moveRow = int.Parse(i_Move[1].ToString()) - 1;
                moveCol = (int)char.ToUpper(i_Move[0]) - (int)'A';
                Game.Board.ExposedCardsMatrix[moveRow, moveCol] = true;
            }

            return i_Move;
        }

        public bool IsCardQ(string i_Move)
        {
            bool isPressed = false;

            if (i_Move.Length == 1)
            {
                if (i_Move.Equals("Q") || i_Move.Equals("q"))
                {
                    isPressed = true;
                }
            }

            return isPressed;
        }
    }
}