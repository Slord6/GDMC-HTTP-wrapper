using Cyotek.Data.Nbt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data
{
    public class BlockProperties
    {
        private string name;
        private Dictionary<BlockProperty, string> properties;
        private Tags tags;

        public string Name { get => name; }
        public Dictionary<BlockProperty, string> Properties { get => properties; }

        public BlockProperties(BlockName name, Dictionary<BlockProperty, string> properties)
        {
            this.name = "minecraft:" + name.ToString();
            this.properties = properties;
            this.tags = new Tags();
        }

        public BlockProperties(string name, Dictionary<BlockProperty, string> properties)
        {
            this.name = name;
            this.properties = properties;
            this.tags = new Tags();
        }

        public BlockProperties(string nameWithProperties)
        {
            string[] sections = nameWithProperties.Split(new char[] { '[' });
            name = sections[0];
            properties = PropertiesFromString(sections[1]);
        }

        public BlockProperties(TagCompound blockData)
        {
            name = blockData.GetString("Name").Value;
            properties = new Dictionary<BlockProperty, string>();

            TagCompound propertiesTag = blockData.GetCompound("Properties");
            if (propertiesTag == null) return;

            foreach (Tag property in propertiesTag.Value)
            {
                TagString prop = (TagString)property;

                BlockProperty parsedProperty;
                if (Enum.TryParse<BlockProperty>(prop.Name, out parsedProperty))
                {
                    properties.Add(parsedProperty, prop.Value);
                }
            }
        }

        private Dictionary<BlockProperty, string> PropertiesFromString(string propertyString)
        {
            propertyString = propertyString.Replace("[", "").Replace("]", "");
            string[] pairs = propertyString.Split(new char[] { ',' });

            Dictionary<BlockProperty, string> parsedPairs = new Dictionary<BlockProperty, string>();
            for (int i = 0; i < pairs.Length; i++)
            {
                string[] splitPair = pairs[i].Split(new char[] { '=' });
                BlockProperty blockProperty;
                if (Enum.TryParse<BlockProperty>(splitPair[0], out blockProperty))
                {
                    parsedPairs.Add(blockProperty, splitPair[1]);
                }
                else
                {
                    throw new ArgumentException("Invalid block property name - " + splitPair[0]);
                }
            }
            return parsedPairs;
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
