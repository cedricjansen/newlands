
namespace New_Lands
{
    public struct Grid 
    {
        public int min, max, current;
        public Grid(int min, int max, int current)
        {
            this.min = min;
            this.max = max;
            this.current = current;
        }
    }

    public struct Forest 
    {
        public int minTree, maxTree, currentTrees;

        public Forest(int min, int max, int current)
        {
            this.minTree = min;
            this.maxTree = max / 1023;
            this.currentTrees = current;
        }
    }

    public class Generator
    {
        
        public int EstimatedUseableTiles;
        public int IslandCounts;

        public World.MapBias WorldBias;
        public GeneratorBias NewBias;
        public GeneratorSizes NewSizes;
        public GeneratorType  GenType;

        public float RiverStep = 2f;
        public float RiverJitter = 1f;

        public float minIslandArea = 64f;
        public float minCircleArea = 80f;

        public int EdgeHole = 5;
        public float WaterFreq = 10f;

        public float WaterMod = 50f;


        public enum GeneratorType { Official, SmallIslands, LargeIslands, Natural, Fractual };
        public enum GeneratorSizes { Small, Medium, Large, Huge, Insane };
        public enum GeneratorBias { Land, Island };
        public Generator()
        {
            this.GenType = GeneratorType.Official;
        }

        public void SetGenType( int value)
        {
            switch(value)
            {
                case 0: // Official
                    this.GenType = GeneratorType.Official;
                    this.RiverStep = 2f;
                    this.RiverJitter = 1f;
                    this.minIslandArea = 64f;
                    this.minCircleArea = 80f;

                    this.EdgeHole = 5;
                    this.WaterFreq = 10f;
                    break;
                case 1: // Small Islands
                    this.GenType = GeneratorType.SmallIslands;
                    this.NewBias = GeneratorBias.Island;
                    switch( this.NewSizes )
                    {
                        case GeneratorSizes.Small:
                            SetSimpleVals(2f, 1f, 2f, 4f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Medium: // > 70
                            SetSimpleVals(2f, 1f, 4f, 8f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Large: // > 100
                            SetSimpleVals(2f, 1f, 6f, 20f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Huge: // > 130
                            SetSimpleVals(2f, 1f, 8f, 20f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Insane: // > 160
                            SetSimpleVals(2f, 1f, 10f, 20f, 5, WaterMod);
                            break;
                    }  
                    break;
                case 2: // LargeIslands
                    this.GenType = GeneratorType.LargeIslands;
                    this.NewBias = GeneratorBias.Island;
                    switch (this.NewSizes)
                    {
                        case GeneratorSizes.Small:
                            SetSimpleVals(2f, 1f, 30f, 25f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Medium: // > 70
                            SetSimpleVals(2f, 1f, 50f, 35f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Large: // > 100
                            SetSimpleVals(2f, 1f, 80, 100, 5, WaterMod);
                            break;
                        case GeneratorSizes.Huge: // > 130
                            SetSimpleVals(2f, 1f, 150, 150f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Insane: // > 160
                            SetSimpleVals(2f, 1f, 200f, 150f, 5, WaterMod);
                            break;
                    }
                    break;
                case 3: //Natural
                    this.GenType = GeneratorType.Natural;
                    this.NewBias = GeneratorBias.Land;
                    this.RiverStep = 2f;
                    this.RiverJitter = 1f;
                    
                    this.EdgeHole = 5;
                    this.WaterFreq = 10f;
                    switch (this.NewSizes)
                    {
                        case GeneratorSizes.Small:
                            SetSimpleVals(2f, 1f, 30f, 10f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Medium: // > 70
                            SetSimpleVals(2f, 1f, 50f, 35f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Large: // > 100
                            SetSimpleVals(2f, 1f, 130f, 100f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Huge: // > 130
                            SetSimpleVals(2f, 1f, 200f, 150f, 5, WaterMod);
                            break;
                        case GeneratorSizes.Insane: // > 160
                            SetSimpleVals(2f, 1f, 200f, 150f, 5, WaterMod);
                            break;
                    }
                    break;
                case 4: // Fractual
                    this.GenType = GeneratorType.Fractual;
                    this.NewBias = GeneratorBias.Land;
                    
                    this.RiverStep = 2f;
                    this.RiverJitter = 1f;

                    this.EdgeHole = 5;
                    
                    switch (this.NewSizes)
                    {
                        case GeneratorSizes.Small:
                            SetSimpleVals(2f, 1f, 2f, 4f, 5, 10);
                            break;
                        case GeneratorSizes.Medium: // > 70
                            SetSimpleVals(2f, 1f, 4f, 8f, 5, 10);
                            break;
                        case GeneratorSizes.Large: // > 100
                            SetSimpleVals(2f, 1f, 4f, 8, 5, 10);
                            break;
                        case GeneratorSizes.Huge: // > 130
                            SetSimpleVals(2f, 1f, 4, 20f, 5, 10);
                            break;
                        case GeneratorSizes.Insane: // > 160
                            SetSimpleVals(2f, 1f, 4, 20f, 5, 10);
                            break;
                    }
                    break;
            }
        }

        public void SetSimpleVals(float Step, float Jitter, float IslandArea, float CircleArea, int Edge, float Freq)
        {
           
            this.RiverStep = Step;
            this.RiverJitter = Jitter;
            this.minIslandArea = IslandArea;
            this.minCircleArea = CircleArea;

            this.EdgeHole = Edge;
            this.WaterFreq = Freq;
        }

        public void UpdateValues()
        {
            this.WorldBias = World.inst.generatedMapsBias;

            SetBias();
            SetSizes();
            SetIslandCount();
            SetUseableTiles();

        }

        private void SetBias()
        {

            if( this.GenType == GeneratorType.Official)
            {
                switch( this.WorldBias )
                {
                    case World.MapBias.Island:
                        this.NewBias = GeneratorBias.Island;
                        break;
                    case World.MapBias.Land:
                        this.NewBias = GeneratorBias.Land;
                        break;
                    case World.MapBias.Random:
                        if (SRand.CoinFlip())
                            this.NewBias = GeneratorBias.Land;
                        else
                            this.NewBias = GeneratorBias.Island;
                        break;
                }
            }
            else
            {
                SetGenType(UserInterface.GeneratorDropValue);
            }
        }

        private void SetSizes()
        {
            if (Main.Grid.current < 70)
                this.NewSizes = GeneratorSizes.Small;
            else if (Main.Grid.current < 100 && Main.Grid.current >= 70)
                this.NewSizes = GeneratorSizes.Medium;
            else if (Main.Grid.current < 130 && Main.Grid.current >= 100)
                this.NewSizes = GeneratorSizes.Large;
            else if (Main.Grid.current < 160 && Main.Grid.current >= 130)
                this.NewSizes = GeneratorSizes.Huge;
            else
                this.NewSizes = GeneratorSizes.Insane;
        }


        private void SetIslandCount()
        {
            switch (this.NewBias)
            {
                case GeneratorBias.Land:
                    this.IslandCounts = 1;
                    break;
                case GeneratorBias.Island:
                    this.NewBias = GeneratorBias.Island;
                    DetermineIslands(this.NewSizes);
                    break;
            }
        }

        private void DetermineIslands( GeneratorSizes mapSize)
        {
            switch( mapSize )
            {
                case GeneratorSizes.Small:
                    this.IslandCounts = 3;
                    break;
                case GeneratorSizes.Medium:
                    this.IslandCounts = 7;
                    break;
                case GeneratorSizes.Large:
                    this.IslandCounts = 10;
                    break;
                case GeneratorSizes.Huge:
                    this.IslandCounts = 12;
                    break;
                case GeneratorSizes.Insane:
                    this.IslandCounts = 50;
                    break;
            }
        }

        public void SetUseableTiles()
        {
            switch (this.NewSizes)
            {
                case GeneratorSizes.Small:
                    this.EstimatedUseableTiles = (int)10 * ((Main.Grid.current * Main.Grid.current) / (int)22);
                    break;
                case GeneratorSizes.Medium:
                    this.EstimatedUseableTiles = (int)10 * ((Main.Grid.current * Main.Grid.current) / (int)19);
                    break;
                case GeneratorSizes.Large:
                    this.EstimatedUseableTiles = (int)10 * ((Main.Grid.current * Main.Grid.current) / (int)16);
                    break;
                case GeneratorSizes.Huge:
                    this.EstimatedUseableTiles = (int) 10* ((Main.Grid.current * Main.Grid.current) / (int) 13);
                    break;
                case GeneratorSizes.Insane:
                    this.EstimatedUseableTiles = (int)(Main.Grid.current * Main.Grid.current) - 1;
                    break;
            }
        }
    }

    
}
