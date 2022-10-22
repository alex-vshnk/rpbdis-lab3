using Microsoft.Extensions.Caching.Memory;
using LanguageClasses.Models;
using LanguageClasses.Data;
using System.Threading.Tasks;

namespace LanguageClassesWebApp.Services
{
    public class CachedListenersService : ICachedListenersService
    {
        private readonly LanguageClassesContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedListenersService(LanguageClassesContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddListeners(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Listener> listeners = _dbContext.Listeners.Take(rowsNumber).ToList();
            if (listeners != null)
            {
                _memoryCache.Set(cacheKey, listeners, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(258)
                });
            }
        }

        public IEnumerable<Listener> GetListeners(int rowsNumber = 20)
        {
            return _dbContext.Listeners.Take(rowsNumber).ToList();
        }

        public IEnumerable<Listener> GetListeners(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Listener> listeners;
            if (!_memoryCache.TryGetValue(cacheKey, out listeners))
            {
                listeners = _dbContext.Listeners.Take(rowsNumber).ToList();
                if (listeners != null)
                {
                    _memoryCache.Set(cacheKey, listeners,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(248)));
                }
            }
            return listeners;
        }
    }
}
