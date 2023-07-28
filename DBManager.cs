using Congratulator.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Congratulator
{
    public class DBManager : DbContext
    {
        private SqliteConnection sqliteConnection;

        public DBManager()
        {
            sqliteConnection = new SqliteConnection("Data Source=database.db");
            sqliteConnection.Open();
        }

        public Person GetPerson(int id)
        {
            var result = sqliteConnection.Query<Person>($"SELECT * FROM People WHERE ID='{id}'", new DynamicParameters()).Select(p => new Person(p.ID, p.Name.ToString(), p.BirthdayDate, p.PhotoName)).ToList()[0];

            return result;
        }

        public List<Person> GetPeople()
        {
            var result = sqliteConnection.Query<Person>("SELECT * FROM People", new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            return result;
        }

        public List<Person> GetPeopleSortByDate(bool isLess)
        {
            string format = isLess ? "DESC" : "ASC";

            var result = sqliteConnection.Query<Person>($"SELECT * FROM People ORDER BY BirthdayDate {format}", new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            return result;
        }

        public List<Person> GetPeopleByCurrentDate()
        {
            string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd");

            var result = sqliteConnection.Query<Person>($"SELECT * FROM People WHERE BirthdayDate='{dateTimeString}'", new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            return result;
        }

        public List<Person> GetPeopleByNerarerDate(DateTime dateTime)
        {
            string todayDateTimeString = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string maxDateTimeString = dateTime.ToString("yyyy-MM-dd");

            var result = sqliteConnection.Query<Person>($"SELECT * FROM People WHERE BirthdayDate BETWEEN '{todayDateTimeString}' " +
                $"AND '{maxDateTimeString}' ORDER BY BirthdayDate ASC",
                new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            return result;
        }

        public List<Person> GetPeopleByLessDateFromToday()
        {
            string todayDateTimeString = DateTime.Now.ToString("yyyy-MM-dd");

            var result = sqliteConnection.Query<Person>($"SELECT * FROM People WHERE BirthdayDate<'{todayDateTimeString}' ORDER BY BirthdayDate DESC",
                new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            return result;
        }

        public async void CreatePerson(Person data)
        {
            await sqliteConnection.ExecuteAsync($"INSERT INTO People (Name, BirthdayDate, PhotoName) VALUES (@Name, @BirthdayDate, @PhotoName)", 
                new { data.Name, data.BirthdayDate, data.PhotoName});
        }

        public async void SavePerson(int id, Person data)
        {
            await sqliteConnection.ExecuteAsync($"UPDATE People SET Name='{data.Name}', BirthdayDate='{data.BirthdayDate}', PhotoName='{data.PhotoName}' WHERE ID='{id}'");
        }

        public async void DeletePerson(int id)
        {
            await sqliteConnection.ExecuteAsync($"DELETE FROM People WHERE ID='{id}'");
        }

        public bool IsUserHas(int id)
        {
            var result = sqliteConnection.Query<Person>($"SELECT ID FROM People WHERE ID='{id}'", new DynamicParameters())
                .Select(p => new Person(p.ID, p.Name, p.BirthdayDate, p.PhotoName)).ToList();

            if (result.Count <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
