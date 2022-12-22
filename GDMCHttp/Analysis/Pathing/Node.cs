using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Analysis.Pathing
{
    public class Node
    {
        // G
        public int DistanceFromStart {
            get
            {
                if (Parent == null) return 0;
                int distance = Parent.DistanceFromStart + 1;

                // We add a little extra if we have to change heights to favour flat routes
                // The value here is how many blocks we're willing to walk to avoid a height change
                if (Parent.Block.Position.Y != Block.Position.Y) distance += flatnessPreference;

                return distance;
            }
        }
        // H
        public int EstimatedDistanceToEnd { get; set; }
        // F
        public int TotalScore {
            get
            {
                return DistanceFromStart + EstimatedDistanceToEnd;
            }
        }
        public Node Parent { get; set; }

        public Block Block { get; private set; }
        private int flatnessPreference;

        public Node(Block block, int flatnessPreference = 3)
        {
            this.flatnessPreference = flatnessPreference;
            // Default to very big number
            EstimatedDistanceToEnd = 15000;
            Parent = null;
            Block = block;
        }

        public int DistanceTo(Vec3Int position)
        {
            return Vec3Int.TaxiCabDistance(Block.Position, position);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Node)) return false;

            Node other = obj as Node;
            return other == this;
        }

        public override int GetHashCode()
        {
            return Block.Position.GetHashCode();
        }

        public static bool operator ==(Node a, Node b)
        {
            if (a is null && b is null) return true;
            // already checked both, so if one is, this doesn't match
            if (a is null || b is null) return false;
            return a.Block.Position == b.Block.Position;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }
    }
}
