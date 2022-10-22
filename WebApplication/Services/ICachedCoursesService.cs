using LanguageClasses.Models;

namespace LanguageClassesWebApp.Services
{
    public interface ICachedCoursesService
    {
        public IEnumerable<Course> GetCourses(int rowsNumber = 20);
        public void AddCourses(string cacheKey, int rowNumber = 20);
        public IEnumerable<Course> GetCourses(string cacheKey, int rowsNumber = 20);
    }
}
