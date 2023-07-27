namespace Congratulator.Models
{
    public class HomeViewModel
    {
        public List<Person> peopleCurrentBirthday { get; set; }
        public List<Person> peopleNearerBirthday { get; set; }

        public HomeViewModel(List<Person> peopleCurrentBirthday, List<Person> peopleNearerBirthday)
        {
            this.peopleCurrentBirthday = peopleCurrentBirthday;
            this.peopleNearerBirthday = peopleNearerBirthday;
        }
    }
}
