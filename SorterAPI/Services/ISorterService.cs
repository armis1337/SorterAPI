using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SorterAPI.Services
{
    public interface ISorterService
    {
        public void SortAndSave(string numbers);
        public string LoadLatestFile();
    }
}
