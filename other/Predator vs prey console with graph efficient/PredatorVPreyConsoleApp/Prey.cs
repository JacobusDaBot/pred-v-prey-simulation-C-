using static MyApp.EnvironmentThings;

namespace MyApp
{

    internal class Prey
    {
        readonly Random rand = new Random();
        public string PreyASCII = ((char)215).ToString();
        public int xpos, ypos;
        private double RealXpos, RealYpos;
        int iMoveToX = 0, iMoveToY = 0;
        double dSpeed = 1d;
        string CurrentState = "idle";
        readonly int iGen;//1=Male;0=Female
        public int iHunger, iThirst, iDetection, iSatisfaction;//out of 100
        readonly int iDirection;
        int idelay = 0;
        readonly int FrmWidth, FrmHeight;
        readonly string ImgString = @"\Image\bunny.png";
        public bool locatedtarget = false;
        public bool AtTargetDestination = false;
        public bool BCanMove = true;
        public int iDetectionRange = 6;
        public int iLocatingRange = 1000;
        public int iInteractionRange = 1;
        public ConsoleColor PreyColor = ConsoleColor.Cyan;
        int tTimeToRand = 0;
        public int iAge;

        public Prey(int formHeight, int formWidth)//creates a new prey
        {
            FrmWidth = formWidth;
            FrmHeight = formHeight;
            xpos = rand.Next(formWidth);
            ypos = rand.Next(formHeight);
            RealXpos = xpos;
            RealYpos = ypos;
            iHunger = rand.Next(30, 50);
            iThirst = rand.Next(30, 50);
            iSatisfaction = rand.Next(-100, 20); ;
            iDetection = 0;
            iGen = rand.Next(0, 2);
            iAge = rand.Next(1, 50);
        }
        public void setState(string state) { CurrentState = state; }
        public void setHunger(int hunger) { iHunger = hunger; }
        public void setThirst(int thirst) { iThirst = thirst; }
        public void setDetection(int detection) { iDetection = detection; }
        public void setSatisfacion(int satisfaction) { iSatisfaction = satisfaction; }
        public void setSpeed(double speed) { dSpeed = speed; }
        public void setMoveTo(int MoveToX, int MoveToY) { iMoveToX = MoveToX; iMoveToY = MoveToY; }





        public string getState() { return this.CurrentState; }
        public int getSatisfacion() { return this.iSatisfaction; }
        public int getHunger() { return this.iHunger; }
        public int getThirst() { return this.iThirst; }
        public int getDetection() { return this.iDetection; }
        public int getGen() { return this.iGen; }
        public double getSpeed() { return this.dSpeed; }
        public int getXpos() { return this.xpos; }
        public int getYpos() { return this.ypos; }
        public string getImg() { return this.ImgString; }
        static double Distance(int X1, int Y1, int X2, int Y2)
        {

            double dDistance = Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
            return dDistance;
        }
        public void decrease_stats()
        {
            iHunger--;
            iAge++;
            // iSatisfaction--;
            iThirst--;
            if ((iHunger > 60) && (iThirst > 60)) { iSatisfaction += 25; }
        }

        public void Move()
        {
            //trig stuf
            if (BCanMove)
            {
                var ytemp = -ypos + iMoveToY;
                var xtemp = -xpos + iMoveToX;
                var angle = Math.Atan2(ytemp, xtemp) / Math.PI * 180;
                var diffx1 = Math.Cos(angle / 180 * Math.PI) * dSpeed;
                var diffy1 = Math.Sin(angle / 180 * Math.PI) * dSpeed;
                if (CurrentState == "Flee")
                {
                    if (((RealXpos - diffx1) > 0) && ((RealXpos - diffx1) < FrmWidth))
                    {
                        RealXpos = RealXpos - diffx1;
                        xpos = (int)RealXpos;
                    }
                    if (((RealYpos - diffy1) > 0) && ((RealYpos - diffy1) < FrmHeight))
                    {
                        RealYpos = RealYpos - diffy1;
                        ypos = (int)RealYpos;
                    }
                }
                else
                {
                    if (((RealXpos + diffx1) > 0) && ((RealXpos + diffx1) < FrmWidth))
                    {
                        RealXpos = RealXpos + diffx1;
                        xpos = (int)RealXpos;
                    }
                    if (((RealYpos + diffy1) > 0) && ((RealYpos + diffy1) < FrmHeight))
                    {
                        RealYpos = RealYpos + diffy1;
                        ypos = (int)RealYpos;
                    }
                }
            }
        }
        public void SearchForThing()
        {
            if ((!locatedtarget) && (tTimeToRand <= 0))
            {
                setMoveTo(xpos + (rand.Next(100, 500) * rand.Next(-1, 2)), ypos + (rand.Next(100, 500) * rand.Next(-1, 2)));
                tTimeToRand = 5;
            }
        }
        public void SearchWater(WaterPlace[] arrayOfWater)
        {
            var SearchForArray = arrayOfWater;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);
            foreach (var x in SearchForArray)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if (CurrentDistance <= iLocatingRange)
                {
                    if (DistToClosestSearch > CurrentDistance)
                    {
                        ClosestSearch = x;
                        DistToClosestSearch = CurrentDistance;
                    }
                }
            }
            if (DistToClosestSearch <= iLocatingRange)
            {
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = true;
            }
            else { locatedtarget = false; }
            if (DistToClosestSearch <= iInteractionRange)
            {
                iThirst += 40;
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = false;
            }
        }
        public void SearchFood(PlantThings[] arrayOfPlant)
        {
            var SearchForArray = arrayOfPlant;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);
            foreach (var x in SearchForArray)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if (CurrentDistance <= iLocatingRange)
                {
                    if (DistToClosestSearch > CurrentDistance)
                    {
                        ClosestSearch = x;
                        DistToClosestSearch = CurrentDistance;
                    }
                }
            }
            if (DistToClosestSearch <= iLocatingRange)
            {
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = true;
            }
            else { locatedtarget = false; }
            if (DistToClosestSearch <= iInteractionRange)
            {
                iHunger += 40;
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = false;
            }
        }
        public void DetectEnemyAndFlee(Predator[] arrayOfPredator)
        {
            var ClosestPredator = ProgramMain.arrayOfPredator[0];
            var DistToClosestPredator = Distance(ClosestPredator.xpos, ClosestPredator.ypos, this.xpos, this.ypos);
            foreach (var x in ProgramMain.arrayOfPredator)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if (CurrentDistance <= iDetectionRange)
                {
                    if (DistToClosestPredator > CurrentDistance)
                    {
                        ClosestPredator = x;
                        DistToClosestPredator = CurrentDistance;
                    }
                }

            }
            if (DistToClosestPredator <= iDetectionRange)
            {
                CurrentState = "Flee";
                setMoveTo(ClosestPredator.xpos, ClosestPredator.ypos);
                locatedtarget = true;
            }
            else
            {
                locatedtarget = false;
            }
        }
        public void SearchPartner(Prey[] arrayOfPrey)
        {
            var SearchForArray = arrayOfPrey;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);
            foreach (var x in SearchForArray)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if ((CurrentDistance <= iLocatingRange) &&/* (x.getGen() != this.getGen()) && */(x.CurrentState == "SearchPartner"))
                {
                    if (DistToClosestSearch > CurrentDistance)
                    {
                        ClosestSearch = x;
                        DistToClosestSearch = CurrentDistance;
                    }
                }
            }
            if ((DistToClosestSearch <= iLocatingRange) &&/* (ClosestSearch.getGen() != this.getGen()) &&*/ (ClosestSearch.CurrentState == "SearchPartner"))
            {
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = true;
                if ((DistToClosestSearch <= iInteractionRange) &&/* (ClosestSearch.getGen() != this.getGen()) &&*/ (ClosestSearch.CurrentState == "SearchPartner"))
                {
                    iSatisfaction = 0;
                    iHunger = 20;
                    iThirst = 20;
                    ClosestSearch.iSatisfaction = 0;
                    ClosestSearch.iHunger = 20;
                    ClosestSearch.iThirst = 20;
                    Array.Resize(ref ProgramMain.arrayOfPrey, ProgramMain.arrayOfPrey.Length + 1);
                    ProgramMain.arrayOfPrey[ProgramMain.arrayOfPrey.Length - 1] = new Prey(ProgramMain.formHeight, ProgramMain.formWidth);
                    locatedtarget = false;
                }
            }
            else { locatedtarget = false; }
        }
        public void KillThis()
        {
            if (this != null)
            {
                /*Console.SetCursorPosition(2 * this.xpos + 1, this.ypos);
                Console.Write(value: ProgramMain.HorizontalSeraparator);*/
                BCanMove = false; PreyASCII = "_";
                PreyColor = ConsoleColor.Black;
                Prey[] NewArray = new Prey[ProgramMain.arrayOfPrey.Length - 1];
                uint i = 0;
                foreach (var x in ProgramMain.arrayOfPrey)
                {
                    if ((x != this) && (x != null) && (i <= NewArray.Length - 1))
                    {

                        NewArray[i] = x;


                        i++;
                    }
                }
                Array.Resize(ref ProgramMain.arrayOfPrey, ProgramMain.arrayOfPrey.Length - 1);
                ProgramMain.arrayOfPrey = NewArray;
            }
        }
        public void Update(Prey[] arrayOfPrey, WaterPlace[] arrayOfWater, Predator[] arrayOfPredator, PlantThings[] arrayOfPlant)
        {
            idelay--;
            if (idelay < 0)
            {
                decrease_stats();
                idelay = 2;
            }
            if (iThirst < 60) { CurrentState = "SearchWater"; }
            else if (iHunger < 60) { CurrentState = "SearchFood"; }
            else if (iSatisfaction > 75) { CurrentState = "SearchPartner"; }



            DetectEnemyAndFlee(ProgramMain.arrayOfPredator);
            switch (CurrentState)
            {
                case "Flee":
                    break;
                case "SearchWater":
                    SearchWater(ProgramMain.arrayOfWater);
                    break;
                case "SearchFood":
                    SearchFood(ProgramMain.arrayOfPlant);
                    break;
                case "SearchPartner":
                    SearchPartner(ProgramMain.arrayOfPrey);
                    break;
            }
            if (!locatedtarget) { SearchForThing(); }
            tTimeToRand--;
            Move();
            if (iThirst > 100) { iThirst = 100; }
            if (iHunger > 100) { iHunger = 100; }
            if ((iThirst <= 0) || (iHunger <= 0)) { KillThis(); }
            // if (iAge >= 100) { KillThis(); }
            //if (iSatisfaction > 100) { iSatisfaction = 100; }
        }

    }
}
