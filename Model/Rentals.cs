using SQLite;

namespace Village_Rentals_App.Model
{
    public class Rental
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EquipmentID { get; set; }
        public int RentalID { get; set; }
        public int RentalAmount { get; set; }
        public string RentalStartdate { get; set; }
        public string RentalEnddate { get; set; }
        public string LastName { get; set; }
        


        public Rental Clone() => MemberwiseClone() as Rental;

        public (bool IsValid, string? ErrorMessage) Validate()
        {

            if (string.IsNullOrWhiteSpace(RentalStartdate))
            {
                return (false, $"{nameof(RentalStartdate)} is required.");
            }

            else if (string.IsNullOrWhiteSpace(LastName))
            {
                return (false, $"{nameof(LastName)} is required.");
            }

            else if (string.IsNullOrWhiteSpace(RentalEnddate))
            {
                return (false, $"{nameof(RentalEnddate)} is required.");
            }

            else if (EquipmentID == 0)
            {
                return (false, $"{nameof(EquipmentID)} is required.");
            }
            else if (RentalID == 0)
            {
                return (false, $"{nameof(EquipmentID)} is required.");
            }

            return (true, null);
        }
    }
}