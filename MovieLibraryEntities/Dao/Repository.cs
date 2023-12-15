using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(MovieContext dbContext)
        {
            _context = dbContext;
        }
        /*public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }*/

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
            
        }
        public IEnumerable<MovieGenre> GetAllGenre()
        {
            return _context.MovieGenres.ToList();
        }

        public void GetMovies()
        {
            var allMovies = from movie in _context.Movies
                          join genre in _context.Genres on movie.Id equals genre.Id
                          select new
                          {
                              MovieId = movie.Id,
                              MovieTitle = movie.Title,
                              Genre = genre.Name
                          }; 
            var listOfMovies =  allMovies.ToList();

            foreach (var movie in listOfMovies)
            {
                Console.WriteLine($"MovieId: {movie.MovieId}, Title: {movie.MovieTitle}, Genre: {movie.Genre}");
            }
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }
        public void AddUser(int Age, string Gender, string ZipCode, int OccupationId)
        {
            var newUser = new User
            {
                Age = Age,
                Gender = Gender,
                ZipCode = ZipCode,
                Occupation = _context.Occupations.FirstOrDefault(x => x.Id == OccupationId)/*Occupation.ReferenceEquals(OccupationId, 0) ? 0 : OccupationId;   */

            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            
            
        }
        public List<Occupation> AllOccupations()
        {
            return _context.Occupations.ToList();
        }
        public void AddMovie(Movie movie)
        {
            var newMovie = new Movie
            {
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate

            };

            _context.Movies.Add(newMovie);
            _context.SaveChanges();
        }
        public void UpdateMovie(Movie updatedMovie, int id)
        {
            var newMovie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (newMovie != null)
            {
                newMovie.Title = updatedMovie.Title;
                newMovie.ReleaseDate = updatedMovie.ReleaseDate;

            };
            _context.Movies.Update(updatedMovie);
            _context.SaveChanges();
        }
    }
}
