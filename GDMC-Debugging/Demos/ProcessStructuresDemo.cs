using GDMCHttp;
using GDMCHttp.Analysis;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;

namespace GDMC_Debugging.Demos
{
    public class ProcessStructuresDemo : Demo
    {
        private string dir;

        public ProcessStructuresDemo(string structuresDir)
        {
            dir = structuresDir;
        }

        public override void Run(McWorld world)
        {
            StructureProcessing processing = new StructureProcessing(world.Connection);
            processing.PrepareStructuresInteractive(dir);
        }
    }
}
