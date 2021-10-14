using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kermis
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleKeyInfo userInput;
            TaxGuy taxGuy = new TaxGuy();
            Attractie[] attracties = new Attractie[] { new BouncingCars(), new Spin(), new MirrorPalace(), new HauntedHouse(), new Hawaii(), new LadderClimbing() };
            CashRegister cashRegister = new CashRegister(attracties);
            Console.WriteLine("Welcome to the carnival controll panel! Please press the following numbers to visit one of the rides! \n" +
            "1 - Bouncing cars\n" +
            "2 - Spider\n" +
            "3 - MirrorPalace\n" +
            "4 - Haunted House\n" +
            "5 - Hawaii\n" +
            "6 - Ladder climbing\n" +
            "o - See total revenue of the park\n" +
            "k - See total amount of tickets sold\n\n");



            do
            {
                userInput = Console.ReadKey();
                switch (userInput.KeyChar.ToString())
                {
                    case "o":
                        cashRegister.ShowTotalRevenue();
                        break;
                    case "k":
                        cashRegister.ShowTotalTicketsSold();
                        break;
                    case "m":
                        for (int i = 0; i < attracties.Length; i++)
                        {
                            if(attracties[i] is HighMaintananceRiskAttraction)
                            {
                                ((HighMaintananceRiskAttraction)attracties[i]).Repair();
                            }
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                        attracties[int.Parse(userInput.KeyChar.ToString()) - 1].Run();
                        break;
                    default:
                        Console.WriteLine("\nNot a valid input!");
                        break;
                }

                if(cashRegister.CalculateTicketsSold() - cashRegister.taxGuyLastTicketTime >= 15)
                {
                    cashRegister.taxGuyLastTicketTime = cashRegister.CalculateTicketsSold();
                    taxGuy.TakeTax(attracties);
                }
            } while (userInput.Key != ConsoleKey.Escape);
        }
    }

    interface GambleAttraction
    {
        double PayGambleGameTax();
    }

    class CashRegister
    {
        public CashRegister(Attractie[] attracties)
        {
            this.attracties = attracties;
        }

        private Attractie[] attracties;
        double totalParkRevenue;
        int totalTicketsSold;
        public int taxGuyLastTicketTime = 0;
        private double CalculateTotalParkRevenue()
        {
            totalParkRevenue = 0;
            foreach(Attractie a in attracties)
            {
                totalParkRevenue += a.revenue;
            }
            return totalParkRevenue;
        }
        public int CalculateTicketsSold()
        {
            totalTicketsSold = 0;
            foreach (Attractie a in attracties)
            {
                totalTicketsSold += a.ticketsSold;
            }
            return totalTicketsSold;
        }
        public void ShowTotalTicketsSold()
        {
            Console.WriteLine("\nThe total number of tickets sold is: " + CalculateTicketsSold());
        }
        public void ShowTotalRevenue()
        {
            Console.WriteLine("\nThe total revenue is: " + CalculateTotalParkRevenue());
        }
    }

    public class Attractie
    {
        public string name;
        protected double costs;
        public int idNumber { get; set; }
        public int areaSize;

        public double revenue = 0;
        public int ticketsSold = 0;
        public virtual void Run()
        {
            Console.WriteLine("\n\nThe " + name + " is running!\n");
            revenue += costs;
            ticketsSold++;
        }
    }
    public abstract class HighMaintananceRiskAttraction : Attractie
    {
        public int maxRidesBeforeRepair;
        public int currentAmountOfRides;

        public void Repair()
        {
            currentAmountOfRides = 0;
            Console.WriteLine("\n\nThe " + name + " is repaired!\n");
        }

        override public void Run()
        {
            if(currentAmountOfRides == maxRidesBeforeRepair)
            {
                Console.WriteLine("\n\nThe " + name + " needs to be repaired before running again! Press 'm' to send a mechanic!\n");
                return;
            }

            if (currentAmountOfRides > maxRidesBeforeRepair)
            {
                throw new InvalidOperationException("\n Error! " + name + " is broken and needs to be repaired first! Press 'm' to send a mechanic!");
            }

            Console.WriteLine("\n\nThe " + name + " is running!\n");
            revenue += costs;
            ticketsSold++;
            currentAmountOfRides++;
        }

    }
    public class TaxGuy
    {
        public void TakeTax(Attractie[] a)
        {
            foreach(Attractie attractie in a)
            {
                if(attractie is GambleAttraction)
                {
                    ((GambleAttraction)attractie).PayGambleGameTax();
                    Console.WriteLine("Yes this is a game attraction");
                }
            }
        }
    }
    public class BouncingCars : Attractie
    {
        public BouncingCars()
        {
            name = "Bouncing cars";
            idNumber = 1;
            costs = 2.50;
        }
    }
    public class Spin : HighMaintananceRiskAttraction
    {
        public Spin()
        {
            name = "Spin";
            idNumber = 2;
            costs = 2.25;
            maxRidesBeforeRepair = 5;
        }
    }
    public class MirrorPalace : Attractie
    {
        public MirrorPalace()
        {
            name = "Mirror Palace";
            idNumber = 3;
            costs = 2.75;
        }
    }
    public class HauntedHouse : Attractie
    {
        public HauntedHouse()
        {
            name = "Haunted House";
            idNumber = 4;
            costs = 3.20;
        }
    }
    public class Hawaii : HighMaintananceRiskAttraction
    {
        public Hawaii()
        {
            name = "Hawaii";
            idNumber = 5;
            costs = 2.90;
            maxRidesBeforeRepair = 10;
        }
    }



    public class LadderClimbing : Attractie, GambleAttraction
    {
        public LadderClimbing()
        {
            name = "Ladder Climbing";
            idNumber = 6;
            costs = 5.00;
        }

        public double PayGambleGameTax()
        {
            double tax = (revenue / 100) * 30;
            revenue = revenue - tax;
            return tax;
        }
    }

    public class aBaseClass
    {
        public aBaseClass()
        {

        }
    }

    class aSubClass : aBaseClass
    {
        aSubClass() : base()
        {

        }
    }

}
