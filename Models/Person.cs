using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace Congratulator.Models
{
    public partial class Person
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Дата рождения")]
        public string BirthdayDate { get; set; }

        [DisplayName("Фото")]
        public string PhotoName { get; set; }

        [NotMapped]
        [DisplayName("Фото")]
        public IFormFile UploadPhoto { get; set; }

        public Person()
        {

        }

        public Person(int ID, string name, string birthdayDate, string photoName)
        {
            this.ID = ID;
            Name = name;
            BirthdayDate = birthdayDate;
            PhotoName = photoName;
        }
    }
}
