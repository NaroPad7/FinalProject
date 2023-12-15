using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
        void AddUser(int Age, string Gender, string ZipCode, int OccupationId);
        List<Occupation> AllOccupations();
        void GetMovies();
        void AddMovie(Movie movie);
        void UpdateMovie(Movie updatedMovie, int id);

    }
}
