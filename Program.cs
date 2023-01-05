namespace Ex1_Inna_Adam
{
    ////includes computer's AI implement - the computer uses a list of the last moves - helps the computer
    ////choose the right matching card
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            UserInterface ui = new UserInterface();
            ui.StartGame();
        }
    }
}