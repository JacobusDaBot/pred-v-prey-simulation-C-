using static MyApp.EnvironmentThings;

namespace MyApp
{
    internal class Predator
    {
        readonly Random rand = new Random();
        public string PredatorASCII = ((char)216).ToString();
        public int xpos, ypos;
        private double RealXpos, RealYpos;
        int iMoveToX, iMoveToY;
        double dSpeed = 1.4d;
        string CurrentState = "idle";
        readonly int iGen;//1=Male;0=Female
        public int iHunger, iThirst, iDetection, iSatisfaction;//out of 100
        readonly int iDirection;
        int idelay = 0;
        readonly string ImgString = @"\Image\cat.png";
        readonly int FrmWidth, FrmHeight;
        public bool locatedtarget = false;
        public bool BCanMove = true;
        int tTimeToRand = 0;
        public int iDetectionRange = 3;
        public int iLocatingRange = 1000;
        public int iInteractionRange = 1;
        public ConsoleColor PredatorColor = ConsoleColor.Red;
        public int iAge;
        public Predator(int formHeight, int formWidth)//creates a new predator
        {
            locatedtarget = false;
            FrmWidth = formWidth;
            FrmHeight = formHeight;
            xpos = rand.Next(formWidth);
            ypos = rand.Next(formHeight);
            RealXpos = xpos;
            RealYpos = ypos;
            iHunger = rand.Next(30, 50);
            iThirst = rand.Next(30, 50);
            iSatisfaction = rand.Next(-20, 20);
            iDetection = 0;
            iGen = rand.Next(0, 2);
            iAge = 0;// rand.Next(1, 50);

        }

        public void setState(string state) { CurrentState = state; }
        public void setHunger(int hunger) { iHunger = hunger; }
        public void setThirst(int thirst) { iThirst = thirst; }
        public void setDetection(int detection) { iDetection = detection; }
        public void setSatisfacion(int satisfaction) { iSatisfaction = satisfaction; }
        public void setSpeed(double speed) { dSpeed = speed; }
        public void setMoveTo(int MoveToX1, int MoveToY1) { iMoveToX = MoveToX1; iMoveToY = MoveToY1; }

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
        public void decrease_stats()
        {

            iHunger -= 2;

            iAge++;
            iThirst--;
            if ((iHunger > 60) && (iThirst > 60)) { iSatisfaction += 29; }
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

                //var diffx = (Math.Floor(Math.Cos(angle / 180 * Math.PI) * 10000) / 10000) * dSpeed;
                //var diffy = (Math.Floor(Math.Sin(angle / 180 * Math.PI) * 10000) / 10000) * dSpeed;


                if (((RealXpos + diffx1) > 0) && ((RealXpos + diffx1) < FrmWidth))
                {
                    RealXpos = RealXpos + diffx1;
                    xpos = (int)RealXpos;
                }
                else
                {
                    RealXpos = RealXpos - diffx1;
                    xpos = (int)RealXpos;
                }
                if (((RealYpos + diffy1) > 0) && ((RealYpos + diffy1) < FrmHeight))
                {
                    RealYpos = RealYpos + diffy1;
                    ypos = (int)RealYpos;
                }
                else
                {
                    RealYpos = RealYpos - diffy1;
                    ypos = (int)RealYpos;
                }
            }
        }


        public void SearchForThing()
        {
            if (!locatedtarget && (tTimeToRand <= 0))
            {
                switch (rand.Next(0, 2))
                {
                    case >= 1:

                        setMoveTo(ProgramMain.formWidth / 2, ProgramMain.formHeight / 2);
                        tTimeToRand = 3;
                        break;
                    case 0:
                        setMoveTo(xpos + (rand.Next(100, 500) * rand.Next(-1, 2)), ypos + (rand.Next(100, 500) * rand.Next(-1, 2)));
                        tTimeToRand = 7;
                        break;
                }

            }

        }
        static double Distance(int X1, int Y1, int X2, int Y2)
        {

            double dDistance = Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
            return dDistance;
        }
        public void SearchWater(WaterPlace[] arrayOfWater)
        {
            var SearchForArray = arrayOfWater;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);
            /*if (!locatedtarget)
            {*/
            foreach (var x in SearchForArray)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if (CurrentDistance <= iLocatingRange)
                {
                    if (DistToClosestSearch >= CurrentDistance)
                    {
                        ClosestSearch = x;
                        DistToClosestSearch = CurrentDistance;
                    }
                }
                //}
            }
            if (DistToClosestSearch <= iLocatingRange)
            {
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = true;
            }
            else { locatedtarget = false; }
            if (DistToClosestSearch <= iInteractionRange)
            {
                iThirst += 10;
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = false;
            }
        }
        public void SearchFood(Prey[] arrayOfPrey)//the first one cant die???
        {
            var SearchForArray = arrayOfPrey;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);

            foreach (var x in SearchForArray)
            {
                if (x != null)
                {
                    var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                    if ((CurrentDistance <= iDetectionRange) && (x.BCanMove))
                    {
                        if (DistToClosestSearch > CurrentDistance)
                        {
                            ClosestSearch = x;
                            DistToClosestSearch = CurrentDistance;
                        }
                    }
                }
            }
            if ((DistToClosestSearch <= iDetectionRange) && (ClosestSearch.BCanMove))
            {
                setMoveTo(ClosestSearch.xpos, ClosestSearch.ypos);
                locatedtarget = true;
            }
            else { locatedtarget = false; }
            if ((DistToClosestSearch <= iInteractionRange) && (ClosestSearch.BCanMove))
            {
                ClosestSearch.BCanMove = false;
                ClosestSearch.PreyASCII = "_";
                ClosestSearch.PreyColor = ConsoleColor.Black;
                ClosestSearch.KillThis();
                iHunger += 50;
                locatedtarget = false;
            }
        }
        public void SearchPartner(Predator[] arrayOfPredator)
        {
            var SearchForArray = arrayOfPredator;
            var ClosestSearch = SearchForArray[0];
            var DistToClosestSearch = Distance(ClosestSearch.xpos, ClosestSearch.ypos, this.xpos, this.ypos);
            foreach (var x in SearchForArray)
            {
                var CurrentDistance = Distance(x.xpos, x.ypos, this.xpos, this.ypos);
                if ((CurrentDistance <= iLocatingRange) && /*(x.getGen() != this.getGen()) &&*/ (x.CurrentState == "SearchPartner"))
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
                if ((DistToClosestSearch <= iInteractionRange) && /*(ClosestSearch.getGen() != this.getGen()) &&*/ (ClosestSearch.CurrentState == "SearchPartner"))
                {
                    iSatisfaction = 0;
                    iHunger = 40;
                    iThirst = 40;
                    ClosestSearch.iSatisfaction = 10;
                    ClosestSearch.iHunger = 40;
                    ClosestSearch.iThirst = 40;
                    Array.Resize(ref ProgramMain.arrayOfPredator, ProgramMain.arrayOfPredator.Length + 1);
                    ProgramMain.arrayOfPredator[ProgramMain.arrayOfPredator.Length - 1] = new Predator(ProgramMain.formHeight, ProgramMain.formWidth);
                    locatedtarget = false;
                }
            }
            else { locatedtarget = false; }
        }
        public void KillThis()
        {
            /*Console.SetCursorPosition(2 * this.xpos + 1, this.ypos);
            Console.Write(value: ProgramMain.HorizontalSeraparator);*/
            BCanMove = false; PredatorASCII = "_";
            PredatorColor = ConsoleColor.Black;
            Predator[] NewArray = new Predator[ProgramMain.arrayOfPredator.Length - 1];
            uint i = 0;
            foreach (var x in ProgramMain.arrayOfPredator)
            {
                if ((x != this) && (x != null) && (i <= NewArray.Length - 1))
                {
                    NewArray[i] = x;
                    i++;
                }
            }
            Array.Resize(ref ProgramMain.arrayOfPredator, ProgramMain.arrayOfPredator.Length - 1);
            ProgramMain.arrayOfPredator = NewArray;
        }

        public void Update(Prey[] arrayOfPrey, WaterPlace[] arrayOfWater, Predator[] arrayOfPredator)
        {

            idelay--;
            if (idelay < 0)
            {
                decrease_stats();
                idelay = 2;
            }
            if (iThirst < 60) { CurrentState = "SearchWater"; }
            else if (iHunger < 60) { CurrentState = "SearchFood"; }
            else if (iSatisfaction > 55) { CurrentState = "SearchPartner"; }


            switch (CurrentState)
            {
                case "SearchFood"://increase stats and kill bunny if damage and 
                    SearchFood(ProgramMain.arrayOfPrey);
                    break;
                case "SearchWater"://increase stats and reset located target if full
                    SearchWater(ProgramMain.arrayOfWater);
                    break;
                case "SearchPartner"://reset located target if finished                    
                    //locatedtarget = false;
                    SearchPartner(ProgramMain.arrayOfPredator);
                    break;
            }
            if (!locatedtarget) { SearchForThing(); }
            tTimeToRand--;
            Move();
            if (iThirst > 100) { iThirst = 100; }
            if (iHunger > 100) { iHunger = 100; }
            if ((iThirst <= 0) || (iHunger <= 0)) { KillThis(); }
            // if (iAge >= 120) { KillThis(); }
            //if (iSatisfaction > 100) { iSatisfaction = 100; }

        }
    }
}