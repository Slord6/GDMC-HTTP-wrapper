using GDMCHttp;
using GDMCHttp.Analysis;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;

namespace GDMC_Debugging.Demos
{
    public class PlaceStructuresBySizeDemo : Demo
    {
        private string dir;
        private bool paintFlatness;

        public PlaceStructuresBySizeDemo(string structuresDir, bool paintFlatness = false)
        {
            dir = structuresDir;
            this.paintFlatness = paintFlatness;
        }

        public override void Run(McWorld world)
        {
            Surveyor surveyor = new Surveyor(world);

            if (paintFlatness)
            {
                surveyor.PaintHeightLandscape();
                world.Flush();
            }

            Block[][] plots = surveyor.BuildablePlots().Where(p => p.Length > 10).ToArray();
            Structure[] allStructures = new StructureProcessing(world.Connection).LoadAllStructures(dir);
            Structure[] selectedStructures = new Structure[plots.Length];

            for (int i = 0; i < plots.Length; i++)
            {
                Block[] plot = plots[i];
                Vec3Int[] plotPositions = plot.Select(b => b.Position).ToArray();
                Vec3Int centreish = Vec3Int.MedianPosition(plotPositions);

                Structure strcuture = surveyor.ClosestSize(plot.Length, allStructures);
                Structure copy = new Structure(strcuture);
                copy.MoveTo(centreish);
                selectedStructures[i] = copy;
            }

            world.PushStructures(selectedStructures);
        }
    }
}
