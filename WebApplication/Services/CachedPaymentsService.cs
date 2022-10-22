using Microsoft.Extensions.Caching.Memory;
using LanguageClasses.Models;
using LanguageClasses.Data;
using System.Threading.Tasks;

namespace LanguageClassesWebApp.Services
{
    public class CachedPaymentsService : ICachedPaymentsService
    {
        private readonly LanguageClassesContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedPaymentsService(LanguageClassesContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddPayments(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Payment> payments = _dbContext.Payments.Take(rowsNumber).ToList();
            if (payments != null)
            {
                _memoryCache.Set(cacheKey, payments, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(258)
                });
            }
        }

        public IEnumerable<Payment> GetPayments(int rowsNumber = 20)
        {
            return _dbContext.Payments.Take(rowsNumber).ToList();
        }

        public IEnumerable<Payment> GetPayments(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Payment> payments;
            if (!_memoryCache.TryGetValue(cacheKey, out payments))
            {
                payments = _dbContext.Payments.Take(rowsNumber).ToList();
                if (payments != null)
                {
                    _memoryCache.Set(cacheKey, payments,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(248)));
                }
            }
            return payments;
        }
    }
}
