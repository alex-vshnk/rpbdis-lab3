using LanguageClasses.Models;

namespace LanguageClassesWebApp.Services
{
    public interface ICachedListenersService
    {
        public IEnumerable<Listener> GetListeners(int rowsNumber = 20);
        public void AddListeners(string cacheKey, int rowNumber = 20);
        public IEnumerable<Listener> GetListeners(string cacheKey, int rowsNumber = 20);
    }
}
