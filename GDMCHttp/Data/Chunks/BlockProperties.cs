using Cyotek.Data.Nbt;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data.Chunks
{
    public class BlockProperties
    {
        private string name;
        private Dictionary<BlockProperty, string> properties;

        public string Name { get => name; }
        public Dictionary<BlockProperty, string> Properties { get => properties; }

        public BlockProperties(string name, Dictionary<BlockProperty, string> properties)
        {
            this.name = name;
            this.properties = properties;
        }

        public BlockProperties(TagCompound blockData)
        {
            name = blockData.GetString("Name").Value;
            properties = new Dictionary<BlockProperty, string>();

            TagCompound propertiesTag = blockData.GetCompound("Properties");

            foreach (Tag property in propertiesTag.Value)
            {
                TagString prop = (TagString)property;

                BlockProperty parsedProperty;
                if(Enum.TryParse<BlockProperty>(prop.Name, out parsedProperty))
                {
                    properties.Add(parsedProperty, prop.Value);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");
            foreach (KeyValuePair<BlockProperty, string> propertyPair in Properties)
            {
                stringBuilder.Append($"{propertyPair.Key}={propertyPair.Value},");
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
}
