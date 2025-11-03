using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Abstractions.Cashe
{
    public interface ICasheService
    {
       Task setAsync(string key , object value ,TimeSpan duration);
       Task <string?> GetAsync(string key);
    }
}
