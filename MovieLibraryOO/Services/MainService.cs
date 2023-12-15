using ConsoleTables;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dao;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers;
using System;
using System.Linq;
using System.Numerics;

namespace MovieLibraryOO.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMovieMapper _movieMapper;
        private readonly IRepository _repository;
        private readonly IFileService _fileService;

        public MainService(ILogger<MainService> logger, IMovieMapper movieMapper, IRepository repository, IFileService fileService)
        {
            _logger = logger;
            _movieMapper = movieMapper;
            _repository = repository;
            _fileService = fileService;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseAction();

                switch (menuChoice)
                {
                    case Menu.MenuOptions.Add:
                        _logger.LogInformation("Adding a new movie");
                        {
                            Console.WriteLine("Please enter the movie you are adding:");
                            var userMovie = Console.ReadLine();
                            Console.WriteLine("Please enter the release date of the movie: ");
                            DateTime realeaseDay = Convert.ToDateTime(Console.ReadLine());
                            var newMovie = new Movie { Title = userMovie, ReleaseDate = realeaseDay };

                            _repository.AddMovie(newMovie);
                            Console.WriteLine($"{userMovie} has been saved!");

                        }
                        break;
                    case Menu.MenuOptions.Update:
                        _logger.LogInformation("Updating an existing movie");
                        var movie = _repository.GetAll();

                        // list of movies
                        Console.WriteLine("List of Movies:");
                        foreach (var i in movie)
                        {
                            Console.WriteLine($"{i.Id} - {i.Title} ({i.ReleaseDate})");
                        }
                        Console.WriteLine("Please select the movie Id you will like to update: ");
                        int movieId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("What would you like the new title to be: ");
                        var newTitle = Console.ReadLine();
                        Console.WriteLine("What is the realease date ");
                        DateTime realeaseDate = Convert.ToDateTime( Console.ReadLine());
                        var updatedMovie = new Movie { Title = newTitle, ReleaseDate = realeaseDate};                        
                        _repository.UpdateMovie(updatedMovie, movieId);
                        Console.WriteLine($"{newTitle} had been updated!");
                        break;
                    case Menu.MenuOptions.ListFromDb:
                        _logger.LogInformation("Listing movies from database");
                        var allMovies = _repository.GetAll();
                        var movies = _movieMapper.Map(allMovies);
                        ConsoleTable.From<MovieDto>(movies).Write();
                        break;
                    /*case Menu.MenuOptions.ListFromDbGenres:
                        _logger.LogInformation("Listing movies from database with genres");
                        _repository.GetMovies();
                        break;*/
                    case Menu.MenuOptions.Search:
                        _logger.LogInformation("Searching for a movie");
                        var userSearchTerm = menu.GetUserResponse("Enter your", "search string:", "green");
                        _repository.Search(userSearchTerm);
                        
                        break;                                        
                    case Menu.MenuOptions.Delete:
                        _logger.LogInformation("Deleting a movie");
                        break;
                    case Menu.MenuOptions.AddUser:
                        _logger.LogInformation("Adding User");
                        Console.WriteLine("Please enter user age less than 100:");
                        int userAge = Convert.ToInt32(Console.ReadLine());                                            
                        Console.WriteLine("Please enter the user Gender M or F: ");
                        var userGender = Console.ReadLine();                                          
                        Console.WriteLine("Please enter the user's Zip Code: ");
                        var userZipCode = Console.ReadLine();
                        Console.WriteLine("List of Occupations");
                        var occupations = (_repository.AllOccupations().ToList());
                        foreach(var occupation in occupations)
                        {
                            Console.WriteLine($"{occupation.Id}. {occupation.Name}");
                        }
                        Console.WriteLine("Please enter the user occupation from above: ");
                        
                        int userOccupation = Convert.ToInt32(Console.ReadLine());

                        _repository.AddUser(userAge,userGender, userZipCode, userOccupation);
                        
                                              
                        Console.WriteLine($"{userAge}, {userGender}, {userZipCode},{userOccupation} has been saved!");
                        break;
                                            
                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}