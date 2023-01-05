namespace Ex1_Inna_Adam
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GameManagement
    {
        ////attributes
        private Player[] m_Players;
        private Board m_Board;

        ////properties
        public Board Board
        {
            get
            {
                return this.m_Board;
            }

            set
            {
                this.m_Board = value;
            }
        }

        public Player[] Players
        {
            get
            {
                return this.m_Players;
            }

            set
            {
                this.m_Players = value;
            }
        }

        ////methods
        public void ComputerPlayerMove(out string o_Card1)
        {
            ////The computer first checks if it's possible to choose from a smaller list of choices (New List = Legit Moves - Last Moves)
            if (this.Board.Lists.LastTwoMovesList.Count > 0 && this.Board.Lists.LegitMovesList.Count > this.Board.Lists.LastTwoMovesList.Count)
            {
                o_Card1 = this.Board.Lists.GetRandItemFromList(this.Board.Lists.List1WithoutList2(this.Board.Lists.LegitMovesList, this.Board.Lists.LastTwoMovesList));
            }
            else
            {
                ////if the Last Moves list is empty or equal in size to the Legit Move list, then pick a random card
                o_Card1 = this.Board.Lists.GetRandItemFromList(this.Board.Lists.LegitMovesList);
            }

            this.Board.Lists.LegitMovesList.Remove(o_Card1);
            this.Board.ExposeCard(o_Card1);
        } ////move 1

        public void ComputerPlayerMove(out string o_Card2, string i_Card1)
        {
            char valueOfCardOne;
            bool isFoundMatchingCard = false;

            valueOfCardOne = this.Board.CardStrToChar(i_Card1);

            o_Card2 = this.Board.Lists.GetRandItemFromList(this.Board.Lists.LegitMovesList);

            for (int i = 0; i < this.Board.Lists.LastTwoMovesList.Count; i++)
            {
                ////if there is a matching card in the LastMoves list then the computer chooses it
                if (i_Card1.Equals(this.Board.Lists.LastTwoMovesList[i]) == false && this.Board.CardStrToChar(this.Board.Lists.LastTwoMovesList[i]).Equals(valueOfCardOne) == true)
                {
                    o_Card2 = this.Board.Lists.LastTwoMovesList[i];
                    this.Board.Lists.LastTwoMovesList.Remove(o_Card2);
                    isFoundMatchingCard = true;
                    this.Board.ExposeCard(o_Card2);
                    break;
                }
            }

            if (isFoundMatchingCard == false)
            {
                ////if there isn't a matching card, pick a new random one (and try to avoid last used cards if possible)
                this.ComputerPlayerMove(out o_Card2);
            }
        } ////move 2   

        public void InitiatePlayers(List<string> i_PlayersNamesList)
        {
            int numOfPlayers = i_PlayersNamesList.Count;

            this.m_Players = new Player[numOfPlayers];

            for (int i = 0; i < numOfPlayers; i++)
            {
                this.m_Players[i] = new Player();
            }

            for (int i = 0; i < numOfPlayers; i++)
            {
                this.m_Players[i].PlayerName = i_PlayersNamesList[i];

                if (i_PlayersNamesList[i].Contains("Computer"))
                {
                    this.m_Players[i].IsPlayerComputer = true;
                }
            }
        }

        public void InitiateGameBoard(int i_GameRows, int i_GameCols)
        {
            this.m_Board = new Board(i_GameRows, i_GameCols);
        }

        public bool CheckAmountOfPlayersValidity(string i_AmountOfPlayers)
        {
            bool isAmountLegit = false;
            int strToInt;

            if (i_AmountOfPlayers.Length == 1)
            {
                if (int.TryParse(i_AmountOfPlayers, out strToInt) == true)
                {
                    if (strToInt == 1 || strToInt == 2)
                    {
                        isAmountLegit = true;
                    }
                }
            }

            return isAmountLegit;
        }

        public bool CheckDimensions(ref string io_UserChoiceDimensions, ref UserInterface.e_Error io_Error)
        {
            return Board.CheckUserChoiceDimensionsValidity(ref io_UserChoiceDimensions, ref io_Error);
        }

        public bool CheckUserChoiceCardValidity(ref string io_UserChoiceCard, bool[,] i_ExposedCardsMatrix, ref UserInterface.e_Error io_Error)
        {
            bool isCardLegit = false;
            int rows = i_ExposedCardsMatrix.GetLength(0), cols = i_ExposedCardsMatrix.GetLength(1);

            if (io_UserChoiceCard.Length != 2 || char.IsLetter(io_UserChoiceCard[0]) == false || char.IsDigit(io_UserChoiceCard[1]) == false)
            {
                isCardLegit = false;
                io_Error = UserInterface.e_Error.WrongFormat;
            }
            else
            {
                if ((char.ToUpper(io_UserChoiceCard[0]) - 'A') + 1 > cols || int.Parse(io_UserChoiceCard[1].ToString()) > rows || int.Parse(io_UserChoiceCard[1].ToString()) < 1)
                {
                    isCardLegit = false;
                    io_Error = UserInterface.e_Error.OutOfBounds;
                }
                else
                {
                    if (i_ExposedCardsMatrix[int.Parse(io_UserChoiceCard[1].ToString()) - 1, char.ToUpper(io_UserChoiceCard[0]) - 'A'] == true)
                    {
                        isCardLegit = false;
                        io_Error = UserInterface.e_Error.AlreadyExposed;
                    }
                    else
                    {
                        isCardLegit = true;
                    }
                }
            }

            return isCardLegit;
        }

        public bool CheckAnotherGameValidity(ref string io_UserInput, ref UserInterface.e_Error io_Error)
        {
            bool isUserInputLegit = false;

            if ((io_UserInput.Length != 1) || (char.IsLetter(io_UserInput[0]) == false))
            {
                isUserInputLegit = false;
                io_Error = UserInterface.e_Error.WrongFormat;
            }
            else
            {
                if (io_UserInput[0].Equals('y') == false && io_UserInput[0].Equals('Y') == false && io_UserInput[0].Equals('n') == false && io_UserInput[0].Equals('N') == false)
                {
                    isUserInputLegit = false;
                    io_Error = UserInterface.e_Error.WrongCharacter;
                }
                else
                {
                    isUserInputLegit = true;
                }
            }

            return isUserInputLegit;
        }
    }
}