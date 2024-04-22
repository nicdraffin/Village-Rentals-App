using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Village_Rentals_App.Model
{

    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }


        public Category Clone() => MemberwiseClone() as Category;


        public (bool IsValid, string? ErrorMessage) Validate()
        {

            if (CategoryID == 0)
            {
                return (false, $"{nameof(CategoryID)} is required.");
            }

            else if (string.IsNullOrWhiteSpace(Name))
            {
                return (false, $"{nameof(Name)} is required.");
            }

            

            return (true, null);
        }
    }
}