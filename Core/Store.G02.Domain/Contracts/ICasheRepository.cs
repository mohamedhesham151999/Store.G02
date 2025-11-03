using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Contracts
{
    public interface ICasheRepository
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value, TimeSpan duration);
    }
}
