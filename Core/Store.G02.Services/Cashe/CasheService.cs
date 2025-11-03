using Store.G02.Domain.Contracts;
using Store.G02.Services.Abstractions.Cashe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Cashe
{
    public class CasheService(ICasheRepository _cashRepository) : ICasheService
    {

        public async Task<string?> GetAsync(string key)
        {
           var result = await _cashRepository.GetAsync(key);
           return result;
        }

        public async Task setAsync(string key, object value, TimeSpan duration)
        {
            await _cashRepository.SetAsync(key,value,duration);
        }
    }
}
