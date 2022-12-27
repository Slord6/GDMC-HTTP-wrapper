using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMC_Debugging
{
    internal class StructureProcessing
    {
        private Connection connection;

        public StructureProcessing(Connection connection)
        {
            this.connection = connection;
        }

        private string Announce(string msg)
        {
            Console.WriteLine(msg);
            return connection.SendCommandSync(new Say(msg));
        }

        public void ExportStructuresInteractive()
        {
            while (true)
            {
                Announce("Any key in console when world bounds are ready");
                Console.ReadKey();

                Announce("Loading world cache");
                McWorld _world = new McWorld(connection);
                Announce("Cache loaded");

                Structure structure = _world.ToStructure();

                Announce("Waiting on name");
                Console.Write("Structure name: ");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string name = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                Announce("Exporting...");
                structure.WriteToXmlFile("./" + name + ".xml");
                Announce("Exported");

                Console.WriteLine();
            }
        }

        private void PlaceAllStructures(string dirPath = "./")
        {
            IEnumerable<string> structureFilepaths = Directory.EnumerateFiles(dirPath, "*.xml");
            List<Structure> structures = new List<Structure>(structureFilepaths.Count());
            foreach (string filepath in structureFilepaths)
            {
                structures.Add(Structure.ReadFromXmlFile(filepath));
            }


            McWorld world = new McWorld(connection);
            Vec3Int offset = Vec3Int.Zero;
            Vec3Int movement = new Vec3Int(30, 0, 0);
            Vec3Int root = Vec3Int.MergeToMin(world.BuildArea.CornerA, world.BuildArea.CornerB);
            int height = -61;
            root.Y = height;
            foreach (Structure structure in structures)
            {
                Vec3Int position = root + offset;
                structure.MoveTo(position);
                offset += movement;
            }

            world.PushStructures(structures.ToArray());
        }

        public bool IsOk(string q)
        {
            Console.WriteLine(q);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string res = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return res.StartsWith("Y");
        }

        public void PrepareStructuresInteractive()
        {
            List<string> structureFilepaths = Directory.EnumerateFiles("./", "*.xml").ToList();
            List<Structure> structures = new List<Structure>(structureFilepaths.Count());
            foreach (string filepath in structureFilepaths)
            {
                structures.Add(Structure.ReadFromXmlFile(filepath));
                Console.WriteLine(filepath);
            }


            McWorld world = new McWorld(connection);
            world.ReplaceBlocks(world.GetBlocks(BlockName.grass_block), BlockName.gold_block);
            world.RefreshCache();

            int index = 0;
            foreach (Structure structure in structures)
            {
                Vec3Int min = Vec3Int.MergeToMin(structure.Position.CornerA, structure.Position.CornerB);
                Vec3Int max = Vec3Int.MergeToMax(structure.Position.CornerA, structure.Position.CornerB);
                structure.UpdatePosition(new Area(min, max));

                Announce(structureFilepaths[index]);
                Vec3Int centre = world.BuildArea.Centre;
                int height = -61;
                centre.Y = height;
                Announce("Centre is " + centre);
                world.ReplaceBlock(world.GetBlock(centre), new Block(BlockName.glass, centre));
                world.Flush();

                structure.MoveTo(centre);
                world.PushStructures(new Structure[] { structure });

                while (!IsOk("Is structure pointing North (or rotation is unimportant)?"))
                {
                    world.Restore();
                    try
                    {
                        structure.Rotation = (Rotation)((int)structure.Rotation) + 1;
                    }
                    catch
                    {
                        structure.Rotation = Rotation.None;
                    }
                    world.PushStructures(new Structure[] { structure });
                }


                int change = 7;

                while (!IsOk("Is Structure flush with ground?"))
                {
                    change--;
                    world.Restore();
                    structure.MovementYOffset = change;
                    structure.MoveTo(centre);
                    world.PushStructures(new Structure[] { structure });
                }

                structure.WriteToXmlFile(structureFilepaths[index]);
                index++;
                world.Restore();
            }
        }

    }
}
