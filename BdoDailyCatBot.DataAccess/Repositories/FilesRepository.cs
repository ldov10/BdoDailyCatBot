using BdoDailyCatBot.DataAccess.Entities;
using BdoDailyCatBot.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Resources;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    public class FilesReposiroty : IFilesRepository
    {
        private readonly Dictionary<FileTypes, string> filePath = new Dictionary<FileTypes, string>()
        {
            [FileTypes.ChannelsToReg] = "",
            [FileTypes.CurrentRaids] = ""
        };

        public FilesReposiroty(ResourceManager Files)
        {
            var channelsDirectoryPath = Files.GetString("ChannelsDirectoryPath");
            var raidsDirectoryPath = Files.GetString("RaidsDirectoryPath");

            filePath[FileTypes.ChannelsToReg] = channelsDirectoryPath + Files.GetString("ChannelsToReg");
            filePath[FileTypes.CurrentRaids] = raidsDirectoryPath + Files.GetString("CurrentRaids");

            CheckDirectory(channelsDirectoryPath);
            CheckDirectory(raidsDirectoryPath);
        }

        private void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public async Task<bool> Delete<T>(T item, FileTypes fileTypes) where T : ToFile
        {
            if (!filePath.ContainsKey(fileTypes))
            {
                return false;
            }

            using (FileStream fs = new FileStream(filePath[fileTypes], FileMode.OpenOrCreate)) ;

            ListItems<T> listItems = new ListItems<T>();

            using (StreamReader sr = new StreamReader(filePath[fileTypes]))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    listItems = JsonConvert.DeserializeObject<ListItems<T>>(json);
                }

            }

            using (StreamWriter fs = new StreamWriter(filePath[fileTypes], false))
            {
                bool flag = false;
                if (listItems.listItems.Contains(item))
                {
                    listItems.listItems.Remove(item);
                    flag = true;
                }

                var json = JsonConvert.SerializeObject(listItems, Formatting.Indented);

                await fs.WriteAsync(json);
                return flag;
            }
        }

        public async Task<List<T>> GetAll<T>(FileTypes FileTypes) where T: ToFile
        {
            if (!filePath.ContainsKey(FileTypes))
            {
                return (new List<T>());
            }

            using (FileStream fs = new FileStream(filePath[FileTypes], FileMode.OpenOrCreate));

            ListItems<T> listItems = new ListItems<T>();

            using (StreamReader sr = new StreamReader(filePath[FileTypes]))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    var temp = JsonConvert.DeserializeObject<ListItems<T>>(json);
                    listItems = temp;
                }
            }

            return listItems.listItems;
        }

        public async Task<bool> Update<T>(T item, FileTypes fileType) where T : ToFile
        {
            if (!filePath.ContainsKey(fileType))
            {
                return false;
            }

            using (FileStream fs = new FileStream(filePath[fileType], FileMode.OpenOrCreate));

            ListItems<T> listItems = new ListItems<T>();

            using (StreamReader sr = new StreamReader(filePath[fileType]))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    listItems = JsonConvert.DeserializeObject<ListItems<T>>(json);
                }

            }

            using (StreamWriter fs = new StreamWriter(filePath[fileType], false))
            {
                bool flag = false;

                T itemToUpdate = listItems.listItems.Find(x => x.Id == item.Id);
                listItems.listItems.Remove(itemToUpdate);
                listItems.listItems.Add(item);

                var json = JsonConvert.SerializeObject(listItems, Formatting.Indented);

                await fs.WriteAsync(json);
                return flag;
            }
        }


        public async Task<bool> Add<T>(T item, FileTypes FileTypes) where T: ToFile
        {
            if (!filePath.ContainsKey(FileTypes))
            {
                return false;
            }

            using (FileStream fs = new FileStream(filePath[FileTypes], FileMode.OpenOrCreate));

            ListItems<T> listItems = new ListItems<T>();

            using (StreamReader sr = new StreamReader(filePath[FileTypes]))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    listItems = JsonConvert.DeserializeObject<ListItems<T>>(json);
                }

            }

            using (StreamWriter fs = new StreamWriter(filePath[FileTypes], false))
            {
                bool flag = false;
                if (!listItems.listItems.Contains(item))
                {
                    listItems.listItems.Add(item);
                    flag = true;
                }

                var json = JsonConvert.SerializeObject(listItems, Formatting.Indented);

                await fs.WriteAsync(json);
                return flag;
            }
        }
    }
}
