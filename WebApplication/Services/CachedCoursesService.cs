using Microsoft.Extensions.Caching.Memory;
using LanguageClasses.Models;
using LanguageClasses.Data;
using System.Threading.Tasks;

namespace LanguageClassesWebApp.Services
{
    public class CachedCoursesService : ICachedCoursesService
    {
        private readonly LanguageClassesContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCoursesService(LanguageClassesContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddCourses(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Course> courses = _dbContext.Courses.Take(rowsNumber).ToList();
            if (courses != null)
            {
                _memoryCache.Set(cacheKey, courses, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(258)
                });
            }
        }

        public IEnumerable<Course> GetCourses(int rowsNumber = 20)
        {
            return _dbContext.Courses.Take(rowsNumber).ToList();
        }

        public IEnumerable<Course> GetCourses(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Course> courses;
            if (!_memoryCache.TryGetValue(cacheKey, out courses))
            {
                courses = _dbContext.Courses.Take(rowsNumber).ToList();
                if (courses != null)
                {
                    _memoryCache.Set(cacheKey, courses,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(248)));
                }
            }
            return courses;
        }
    }
}
