using System;
using System.Data;
using System.Drawing;
using System.Text;


namespace MyApp
{
    class ProgramMain
    {       
        static public int formHeight = 30;
        static public int formWidth = 70;           
        static public string WaterASCII = "W";
        static public string PlantASCII = "P";
        static public string VerticalSeparator = "+";
        static public string HorizontalSeraparator = " ";
        const ConsoleColor PreyColor = ConsoleColor.Cyan;
        const ConsoleColor PredatorColor = ConsoleColor.Red;
        const ConsoleColor WaterColor = ConsoleColor.Blue;
        const ConsoleColor PlantColor = ConsoleColor.Green;
        const ConsoleColor SeparatorColor = ConsoleColor.DarkGray;

        
        const int initialPrey = 80;
        const int initialPredator = 15;
        const int initialWater = 15;
        const int initialPlant = 15;
        private static int idelay =1;
        public static bool ibreak = false;

        public string startupPath = Environment.CurrentDirectory;
        readonly Random rand = new Random();
        
        public static Prey[] arrayOfPrey = new Prey[initialPrey];

        public static Predator[] arrayOfPredator = new Predator[initialPredator];

        public static EnvironmentThings.WaterPlace[] arrayOfWater = new EnvironmentThings.WaterPlace[initialWater];

        public static EnvironmentThings.PlantThings[] arrayOfPlant = new EnvironmentThings.PlantThings[initialPlant];        

        static void DrawAll()
        {
            Console.ForegroundColor = SeparatorColor;
            Console.SetCursorPosition(0,0);
            string sLine = "";
            for (int i = 0; i < formWidth; i++)
            {
                sLine = sLine + VerticalSeparator+ HorizontalSeraparator;
            }
            for (int i = 0; i < formHeight; i++)
            {
                Console.WriteLine(sLine);
            }  
        }
        static  void CreatePrey()
        {
            for (int i = 0; i < initialPrey; i++)
            {
                ProgramMain.arrayOfPrey[i] = new Prey(ProgramMain.formHeight, ProgramMain.formWidth);
                Console.SetCursorPosition(2 * arrayOfPrey[i].xpos + 1, arrayOfPrey[i].ypos);
                Console.Write(value: arrayOfPrey[i].PreyASCII);                
            }
        }
        static void CreatePredator()
        {
            for (int i = 0; i < initialPredator; i++)
            {
                arrayOfPredator[i] = new Predator(ProgramMain.formHeight, ProgramMain.formWidth);
                Console.SetCursorPosition(2 * arrayOfPredator[i].xpos + 1, arrayOfPredator[i].ypos);
                Console.Write(value: arrayOfPredator[i].PredatorASCII);
            }
        }
        static void CreateWaterPlace(int iNum)
        {           
            for (int i = 0; i < iNum; i++)
            {
                arrayOfWater[i] = new EnvironmentThings.WaterPlace(ProgramMain.formHeight, ProgramMain.formWidth, i);
                Console.SetCursorPosition(2 * arrayOfWater[i].xpos + 1, arrayOfWater[i].ypos);
                Console.Write(value: arrayOfWater[i].WaterASCII);
            }
        }
        static void CreatePlants(int iNum)
        {            
            for (int i = 0; i < iNum; i++)
            {
                arrayOfPlant[i] = new EnvironmentThings.PlantThings(ProgramMain.formHeight, ProgramMain.formWidth);
                Console.SetCursorPosition(2 * arrayOfPlant[i].xpos + 1, arrayOfPlant[i].ypos);
                Console.Write(value: ProgramMain.PlantASCII);
            }
        }

        static void Start()
        {
            //Console.SetWindowSize((2 *formWidth) +2, formHeight+2+20 );
            Console.ForegroundColor = SeparatorColor;
            DrawAll();
            Console.ForegroundColor = PredatorColor;
            CreatePredator();
            Console.ForegroundColor = PreyColor;
            CreatePrey();
            Console.ForegroundColor = WaterColor;
            CreateWaterPlace(initialWater);
            Console.ForegroundColor = PlantColor;
            CreatePlants(initialPlant);            
        }
        static void Update()
        {           
            CheckEmptyArrays();
            if (ibreak != true)
            {
                for (int i = 0; i < arrayOfPrey.Length-1; i++)
                {
                    if (arrayOfPrey[i] != null)
                    {
                        Console.ForegroundColor = SeparatorColor;
                        Console.SetCursorPosition((2 * arrayOfPrey[i].xpos) + 1, arrayOfPrey[i].ypos);
                        Console.Write(value: HorizontalSeraparator);                      
                        arrayOfPrey[i].Update(arrayOfPrey, arrayOfWater, arrayOfPredator, arrayOfPlant);                        
                        Console.ForegroundColor = arrayOfPrey[i].PreyColor;
                        Console.SetCursorPosition((2 * arrayOfPrey[i].xpos) + 1, arrayOfPrey[i].ypos);
                        Console.Write(value: arrayOfPrey[i].PreyASCII);                       
                    }
                }
            }
            CheckEmptyArrays();
            if (ibreak != true)
            {
                foreach (var x in arrayOfPredator)
                {
                    if ((x != null))
                    {
                        Console.ForegroundColor = SeparatorColor;
                        Console.SetCursorPosition(2 * x.xpos + 1, x.ypos);
                        Console.Write(value: HorizontalSeraparator);                                            
                        x.Update(arrayOfPrey, arrayOfWater, arrayOfPredator);                        
                        Console.ForegroundColor = x.PredatorColor;
                        Console.SetCursorPosition((2 * x.xpos) + 1, x.ypos);
                        Console.Write(value: x.PredatorASCII);      
                    }
                }
            }
           
            Console.ForegroundColor = PlantColor;
            for (int i = 0; i < arrayOfPlant.Length; i++)
            {              
                Console.SetCursorPosition(2 * arrayOfPlant[i].xpos + 1, arrayOfPlant[i].ypos);
                Console.Write(value: ProgramMain.PlantASCII);
            }
            Console.ForegroundColor = WaterColor;
            for (int i = 0; i < arrayOfWater.Length; i++)
            {
                Console.SetCursorPosition(2 * arrayOfWater[i].xpos + 1, arrayOfWater[i].ypos);
                Console.Write(value: arrayOfWater[i].WaterASCII);
            }
            
        }
        static void ClearObjects()
        {            
            Console.SetCursorPosition(0, formHeight+2); 
            Console.WriteLine("                 ");
            Console.WriteLine("                 ");
            Console.WriteLine("                 ");
        }
        public static void CheckEmptyArrays()
        {
            if ((arrayOfPredator.Length<=1)||(arrayOfPrey.Length <= 1)) { ibreak= true; }            
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize((2 * formWidth) + 4, formHeight + 2 + 20);
            Console.ReadLine();
            Start();
            DataTable table = new DataTable();
            table.Columns.Add("NO", typeof(string));
            table.Columns.Add("Predator", typeof(string));
            table.Columns.Add("Prey", typeof(string));
            int icount = 0;
            while (true)
            {
                CheckEmptyArrays();
                if (ibreak == true) { break; }
                ClearObjects();
                Console.SetCursorPosition(0, formHeight +2);
                Console.WriteLine(Convert.ToString(arrayOfPredator.Length));
                Console.WriteLine(Convert.ToString(arrayOfPrey.Length));
                Console.WriteLine(Convert.ToString(icount));
                Update();
                CheckEmptyArrays();
                if (ibreak == true) { break; }


                idelay--;
                if (idelay < 0)
                {
                    table.Rows.Add(icount, arrayOfPredator.Length, arrayOfPrey.Length);
                    idelay = 10;
                    icount++;
                }
               // if (icount >= 500) { break; }

            }
            Console.Clear();
            StringBuilder sb = new StringBuilder();

            string[] columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in table.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.AppendLine(string.Join(",", fields));
            }           
            File.WriteAllText("test.csv", sb.ToString());
            Console.ReadLine();
        }
    }
}