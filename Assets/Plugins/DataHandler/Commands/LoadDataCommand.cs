using System.IO;
using FrostLib.Commands;
using Newtonsoft.Json;
using UnityEngine;

namespace Plugins.DataHandler.Commands
{
    public class LoadDataCommand<T> : ICommand<T>
    {
        public T Execute()
        {
            var containerName = typeof(T).ToString();
            var path = Path.Join(Application.persistentDataPath,  containerName);

            if (!File.Exists(path))
                return default;

            var data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}