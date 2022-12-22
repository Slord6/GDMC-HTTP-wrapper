using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace GDMCHttp.Data.Blocks.Structures
{
    public class Structure
    {
        public Area Position { get; set; }
        public Rotation Rotation { get; set; }
        public Vec3Int Pivot { get; set; }
        public bool Entities { get; set; }
        public Mirror Mirror { get; set; }
        public byte[] Data { get; set; }

        public Structure()
        {
            Position = new Area(Vec3Int.Zero, Vec3Int.Zero);
            Pivot = Vec3Int.Zero;
            Rotation = Rotation.None;
            Entities = false;
            Mirror = Mirror.None;
            Data = new byte[0];
        }

        public Structure(byte[] data) : this()
        {
            Data = data;
        }

        public Structure(Area position, byte[] data): this()
        {
            UpdatePosition(position);
            Data = data;
        }

        public Structure(Structure existing) : this(existing.Position, existing) { }

        /// <summary>
        /// Duplicate a structure but in a new position. The new structure pivot is also updated.
        /// </summary>
        /// <param name="position">New position</param>
        /// <param name="existing">Existing structure to copy data from</param>
        public Structure(Area position, Structure existing) : this(position, existing.Rotation, existing.Entities, existing.Mirror, existing.Data)
        {

        }

        public Structure(Area position, Rotation rotation, bool entities, Mirror mirror, byte[] data, Vec3Int pivot = null)
        {
            Rotation = rotation;
            if(pivot == null)
            {
                UpdatePosition(position, true);
            }
            else
            {
                UpdatePosition(position, false);
                Pivot = pivot;
            }
            Entities = entities;
            Mirror = mirror;
            Data = data;
        }

        public void UpdatePosition(Area position, bool updatePivotToCenter = true)
        {
            Position = position;
            if(updatePivotToCenter)
            {
                Pivot = Position.CentreOffset.ToAbsolute();
            }
        }

        public void Translate(Vec3Int offset, bool updatePivotToCenter = true)
        {
            UpdatePosition(
                new Area(
                    Position.CornerA + offset,
                    Position.CornerB + offset
                ),
                updatePivotToCenter
            );
        }

        public void MoveTo(Vec3Int position, bool updatePivotToCenter = true)
        {
            Vec3Int offset = position - Position.CornerA;
            Translate(offset, updatePivotToCenter);
        }

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <param name="filePath">The file path to write the object instance to.</param>
        public void WriteToXmlFile(string filePath)
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Structure));
                writer = new StreamWriter(filePath, false);
                serializer.Serialize(writer, this);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// </summary>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static Structure ReadFromXmlFile(string filePath)
        {
            TextReader reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Structure));
                reader = new StreamReader(filePath);
                return (Structure)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
