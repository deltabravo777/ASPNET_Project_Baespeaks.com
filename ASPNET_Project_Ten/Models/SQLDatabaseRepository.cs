using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Models
{
    public class SQLDatabaseRepository : IDatabaseRepository
    {
        private readonly AppDBContext context;

        public SQLDatabaseRepository(AppDBContext context)
        {
            this.context = context;
        }

        #region Employees
        public Employee AddEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return context.Employees.Find(id);
            // return context.Employees.FirstOrDefault(emp => emp.Id == id);
        }

        public Employee UpdateEmployee(Employee employeeChanges)
        {
            var employee = context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
        #endregion

        #region Articles
        public Article AddArticle(Article article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
            return article;
        }

        public Article DeleteArticle(int id)
        {
            Article article = context.Articles.Find(id);
            if (article != null)
            {
                context.Articles.Remove(article);
                context.SaveChanges();
            }
            return article;
        }

        public IEnumerable<Article> GetAllArticles()
        {
            return context.Articles;
        }

        public Article GetArticle(int id)
        {
            return context.Articles.Find(id);
        }

        public Article UpdateArticle(Article articleChanges)
        {
            var article = context.Articles.Attach(articleChanges);
            article.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return articleChanges;
        }
        #endregion

        #region Playlists
        public Playlist AddPlaylist(Playlist playlist)
        {
            context.Playlists.Add(playlist);
            context.SaveChanges();
            return playlist;
        }

        public Playlist DeletePlaylist(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            if (playlist != null)
            {
                context.Playlists.Remove(playlist);
                context.SaveChanges();
            }
            return playlist;
        }

        public IEnumerable<Playlist> GetAllPlaylists()
        {
            return context.Playlists;
        }

        public Playlist GetPlaylist(int id)
        {
            return context.Playlists.Find(id);
        }

        public Playlist UpdatePlaylist(Playlist playlistChanges)
        {
            var playlist = context.Playlists.Attach(playlistChanges);
            playlist.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return playlistChanges;
        }
        #endregion

        #region Blueprints
        public Blueprint AddBlueprint(Blueprint blueprint)
        {
            context.Blueprints.Add(blueprint);
            context.SaveChanges();
            return blueprint;
        }

        public Blueprint DeleteBlueprint(Guid id)
        {
            Blueprint blueprint = context.Blueprints.Find(id);
            if (blueprint != null)
            {
                context.Blueprints.Remove(blueprint);
                context.SaveChanges();
            }
            return blueprint;
        }

        public IEnumerable<Blueprint> GetAllBlueprints()
        {
            return context.Blueprints;
        }

        public Blueprint GetBlueprint(Guid id)
        {
            return context.Blueprints.Find(id);
        }

        public Blueprint UpdateBlueprint(Blueprint blueprintChanges)
        {
            var blueprint = context.Blueprints.Attach(blueprintChanges);
            blueprint.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return blueprintChanges;
        }
        #endregion
    }
}