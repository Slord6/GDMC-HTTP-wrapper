using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GDMCHttp.Data.Blocks
{
    public class Sign : Block
    {
        private string[] message;
        public string[] Message
        {
            get
            {
                return message;
            }
            set
            {
                if (value.Length != 4) throw new ArgumentException("Signs require 4 lines exactly");
                message = value;
                BlockProperties.BlockStates = MessageToBlockStates(Message);
            }
        }
        public BlockProperty Facing
        {
            set
            {
                BlockProperty[] allowed = new BlockProperty[] { BlockProperty.north, BlockProperty.south, BlockProperty.east, BlockProperty.west };
                if (!allowed.Contains(value)) throw new ArgumentException("Sign direction cannot be " + value);
                BlockProperties.SetProperty(BlockProperty.facing, value.ToString());
            }
        }

        public Sign(BlockProperties properties) : base(properties)
        {
            ThrowOnInvalidSignType(properties.Name);
            Facing = BlockProperty.north;
        }

        public Sign(BlockName signType, string[] message, Vec3Int position) : base(signType, position)
        {
            ThrowOnInvalidSignType(signType);
            Message = message;
            Facing = BlockProperty.north;
        }

        public Sign(string[] message, Vec3Int position) : this(BlockName.oak_sign, message, position)
        {
        }

        private BlockState[] MessageToBlockStates(string[] message)
        {
            //{
            //  Color:"black"
            //  GlowingText:0b,
            //  Text1:'{"text":"One"}',
            //  Text2:'{"text":"Two"}',
            //  Text3:'{"text":"Three"}',
            //  Text4:'{"text":"Four"}'}
            //}
            int lineCount = 4;
            BlockState[] blockStates = new BlockState[lineCount + 2];
            for (int i = 1; i <= lineCount; i++)
            {
                string textTitle = $"Text{i}";

                blockStates[i - 1] = new BlockState(textTitle, new BlockState("text", message[i - 1], true, '"'));
            }
            blockStates[lineCount] = new BlockState("Color", "black", true);
            blockStates[lineCount + 1] = new BlockState("GlowingText", "0b", false);
            return blockStates;
        }

        private void ThrowOnInvalidSignType(BlockName name)
        {

            if (!name.ToString().Contains("sign"))
            {
                throw new ArgumentException("Sign must be a sign block type");
            }
        }
    }
}
