using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Village_Rentals_App.Data;
using Village_Rentals_App.Model;

namespace Utility
{
    public partial class RentalsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public RentalsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Rental> _rentals = new();

        [ObservableProperty]
        private Rental _operatingRental = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadrentalsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var rentals = await _context.GetAllAsync<Rental>();
                if (rentals is not null && rentals.Any())
                {
                    Rentals ??= new ObservableCollection<Rental>();

                    foreach (var rental in rentals)
                    {
                        Rentals.Add(rental);
                    }
                }
            }, "Fetching rentals...");
        }

        [RelayCommand]
        private void SetOperatingRental(Rental rental) => OperatingRental = rental ?? new();

        [RelayCommand]
        public async Task SaveRentalAsync()
        {
            if (OperatingRental is null)
                return;

            var (isValid, errorMessage) = OperatingRental.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                return;
            }

            var busyText = OperatingRental.Id == 0 ? "Creating Rental..." : "Updating Rental...";
            await ExecuteAsync(async () =>
            {
                if (OperatingRental.Id == 0)
                {
                    await _context.AddItemAsync<Rental>(OperatingRental);
                    Rentals.Add(OperatingRental);
                }
                else
                {
                    // Update Rental
                    if (await _context.UpdateItemAsync<Rental>(OperatingRental))
                    {
                        var rentalCopy = OperatingRental.Clone();

                        var index = Rentals.IndexOf(OperatingRental);
                        Rentals.RemoveAt(index);

                        Rentals.Insert(index, rentalCopy);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Rental updation error", "OK");
                        return;
                    }
                }
                SetOperatingRentalCommand.Execute(new());
            }, busyText);
        }

        [RelayCommand]
        private async Task DeleteRentalAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<Rental>(id))
                {
                    var rental = Rentals.FirstOrDefault(p => p.Id == id);
                    Rentals.Remove(rental);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Rental was not deleted", "OK");
                }
            }, "Deleting Rental...");
        }

        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}
