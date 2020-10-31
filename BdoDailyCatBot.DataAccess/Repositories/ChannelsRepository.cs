using BdoDailyCatBot.DataAccess.Entities;
using BdoDailyCatBot.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    public class ChannelsRepository : IChannels
    {
        private readonly string directoryPath;

        private readonly Dictionary<FilesType, string> fileNames = new Dictionary<FilesType, string>() 
        {
            [FilesType.ChannelsToReg] = "/ChannelsToReg.json"
        };

        public ChannelsRepository(string directoryPath)
        {
            this.directoryPath = directoryPath;

            CheckDirectory(directoryPath);
        }

        private void CheckDirectory(string path)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public async Task<List<Channels>> GetAll(FilesType fileType) // TODO: ...
        {
            if (!fileNames.ContainsKey(fileType))
            {
                return (new List<Channels>());
            }

            ListChannels channels = new ListChannels();

            using (StreamReader sr = new StreamReader((directoryPath + fileNames[fileType])))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    var temp = JsonConvert.DeserializeObject<ListChannels>(json);
                    channels = temp;
                }
            }

            return channels.channels;
        }

        public async Task<(bool, string)> Add(Channels channel, FilesType fileType)
        {
            if (!fileNames.ContainsKey(fileType))
            {
                return (false, "Invalid file type");
            }

            ListChannels channels = new ListChannels();

            using (StreamReader sr = new StreamReader((directoryPath + fileNames[fileType])))
            {
                string json = await sr.ReadToEndAsync();

                if (json != "")
                {
                    var temp = JsonConvert.DeserializeObject<ListChannels>(json);
                    channels = temp;
                }

            }

            using (StreamWriter fs = new StreamWriter((directoryPath + fileNames[fileType]), false))
            {
                bool flag = false;
                if (!channels.channels.Contains(channel))
                {
                    channels.channels.Add(channel);
                    flag = true;
                }

                var json = JsonConvert.SerializeObject(channels, Formatting.Indented);

                await fs.WriteAsync(json);
                return (flag, "");
            }
        }


    }
}
