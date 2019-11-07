using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

[XmlRoot("dictionary")]
public class SDic<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
{
    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
            return;

        //bool keyIsString = typeof(TKey) == typeof(string);
        bool keyIsString = typeof(TKey).FindInterfaces((n, k) => n.ToString() == k.ToString(), "System.IConvertible").Length > 0;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {
            if (keyIsString)
            {
                /*
                var ms = new MemoryStream(Encoding.Unicode.GetBytes(reader.GetAttribute("key")));
                TKey key = (TKey)keySerializer.Deserialize(ms);
                */
                string at = reader.GetAttribute("key");
                TKey key = (TKey)Convert.ChangeType(at, typeof(TKey));
                reader.ReadStartElement("item");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                this.Add(key, value);
            }
            else
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                
                this.Add(key, value);
            }
            reader.ReadEndElement();
            reader.MoveToContent();
        }
        reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        //bool keyIsString = typeof(TKey) == typeof(string);
        bool keyIsString = typeof(TKey).FindInterfaces((n, k) => n.ToString() == k.ToString(), "System.IConvertible").Length > 0;

        foreach (TKey key in this.Keys)
        {
            writer.WriteStartElement("item");

            if (keyIsString)
            {

                writer.WriteAttributeString("key", key.ToString());

                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
            }
            else
            {
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
    #endregion
}

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
{
    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }
 
    public void ReadXml(System.Xml.XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
 
        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();
 
        if (wasEmpty)
            return;
 
        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {
            reader.ReadStartElement("item");
 
            reader.ReadStartElement("key");
            TKey key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();
 
            reader.ReadStartElement("value");
            TValue value = (TValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();
 
            this.Add(key, value);
 
            reader.ReadEndElement();
            reader.MoveToContent();
        }
        reader.ReadEndElement();
    }
 
    public void WriteXml(System.Xml.XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
 
        foreach (TKey key in this.Keys)
        {
            writer.WriteStartElement("item");
 
            writer.WriteStartElement("key");
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();
 
            writer.WriteStartElement("value");
            TValue value = this[key];
            valueSerializer.Serialize(writer, value);
            writer.WriteEndElement();
 
            writer.WriteEndElement();
        }
    }
    #endregion
}