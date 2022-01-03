using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Models
{
    public interface IDatabaseRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employeeChanges);
        Employee DeleteEmployee(int id);

        Article AddArticle(Article employee);
        Article DeleteArticle(int id);
        IEnumerable<Article> GetAllArticles();
        Article GetArticle(int id);
        Article UpdateArticle(Article employeeChanges);

        Playlist AddPlaylist(Playlist playlist);
        Playlist DeletePlaylist(int id);
        IEnumerable<Playlist> GetAllPlaylists();
        Playlist GetPlaylist(int id);
        Playlist UpdatePlaylist(Playlist playlistChanges);

        Blueprint AddBlueprint(Blueprint playlist);
        Blueprint DeleteBlueprint(Guid id);
        IEnumerable<Blueprint> GetAllBlueprints();
        Blueprint GetBlueprint(Guid id);
        Blueprint UpdateBlueprint(Blueprint playlistChanges);
    }
}