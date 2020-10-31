using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Interfaces
{
    public interface IChannels
    {
        Task<(bool, string)> Add(Entities.Channels channels, Entities.FilesType fileType);
        Task<List<Entities.Channels>> GetAll(Entities.FilesType fileType);
    }
}
