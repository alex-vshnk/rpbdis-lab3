using LanguageClasses.Models;

namespace LanguageClassesWebApp.Services
{
    public interface ICachedPaymentsService
    {
        public IEnumerable<Payment> GetPayments(int rowsNumber = 20);
        public void AddPayments(string cacheKey, int rowNumber = 20);
        public IEnumerable<Payment> GetPayments(string cacheKey, int rowsNumber = 20);
    }
}
