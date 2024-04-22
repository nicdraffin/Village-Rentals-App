using SQLite;

namespace Village_Rentals_App.Model
{
    public class Inventory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EquipmentID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double RentalCost { get; set; }
        public bool Working { get; set; }
        public bool Rented { get; set; }



        public Inventory Clone() => MemberwiseClone() as Inventory;

        public (bool IsValid, string? ErrorMessage) Validate()
        {

            if (string.IsNullOrWhiteSpace(Name))
            {
                return (false, $"{nameof(Name)} is required.");
            }

            else if (string.IsNullOrWhiteSpace(Description))
            {
                return (false, $"{nameof(Description)} is required.");
            }

            else if (RentalCost == 0)
            {
                return (false, $"{nameof(RentalCost)} is required.");
            }

            else if (EquipmentID == 0)
            {
                return (false, $"{nameof(EquipmentID)} is required.");
            }
            else if (string.IsNullOrWhiteSpace(CategoryName))
            {
                return (false, $"{nameof(CategoryName)} is required.");
            }
            else if (Working == null)
            {
                return (false, $"{nameof(Working)} is required.");
            }
            else if (Rented == null)
            {
                return (false, $"{nameof(Rented)} is required.");
            }

            return (true, null);
        }
    }
}