using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace TGEditor
{
    /// <summary>
    /// Prepares a given list to be serialized by the list serializer class
    /// </summary>
    [Serializable]
    public class StorableList
    {
        public List<string> ModelList
        {
            get { return storableObjects; }
        }

        List<string> storableObjects;
        public StorableList(List<string> objects)
        {
            storableObjects = objects;
            ListSerializer listSerializer = new ListSerializer(this);
        }
    }

    /// <summary>
    /// Serializes a list via binary serialization
    /// </summary>
    public class ListSerializer
    {
        public ListSerializer(StorableList storableLObject)
        {
            Stream stream = null;
            IFormatter formatter = new BinaryFormatter();
            stream = new FileStream(EngineStart.engine.Content.RootDirectory + "/bin/TestList", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, storableLObject);
            stream.Close();
        }
    }
}
