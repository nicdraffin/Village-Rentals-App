using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Village_Rentals_App.Data;
using Village_Rentals_App.Model;

namespace Utility
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public InventoryViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Inventory> _inventorys = new();

        [ObservableProperty]
        private Inventory _operatingInventory = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadInventoryAsync()
        {
            await ExecuteAsync(async () =>
            {
                var inventorys = await _context.GetAllAsync<Inventory>();
                if (inventorys is not null && inventorys.Any())
                {
                    Inventorys ??= new ObservableCollection<Inventory>();

                    foreach (var inventory in inventorys)
                    {
                        Inventorys.Add(inventory);
                    }
                }
            }, "Fetching Inventory...");
        }

        [RelayCommand]
        private void SetOperatingInventory(Inventory inventory) => OperatingInventory = inventory ?? new();

        [RelayCommand]
        public async Task SaveInventoryAsync()
        {
            if (OperatingInventory is null)
                return;

            var (isValid, errorMessage) = OperatingInventory.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "OK");
                return;
            }

            var busyText = OperatingInventory.Id == 0 ? "Creating Rental..." : "Updating Rental...";
            await ExecuteAsync(async () =>
            {
                if (OperatingInventory.Id == 0)
                {
                    await _context.AddItemAsync<Inventory>(OperatingInventory);
                    Inventorys.Add(OperatingInventory);
                }
                else
                {
                    // Update Inventory
                    if (await _context.UpdateItemAsync<Inventory>(OperatingInventory))
                    {
                        var rentalCopy = OperatingInventory.Clone();

                        var index = Inventorys.IndexOf(OperatingInventory);
                        Inventorys.RemoveAt(index);

                        Inventorys.Insert(index, rentalCopy);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Rental updation error", "OK");
                        return;
                    }
                }
                SetOperatingInventoryCommand.Execute(new());
            }, busyText);
        }

        [RelayCommand]
        private async Task DeleteInventoryAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<Inventory>(id))
                {
                    var inventory = Inventorys.FirstOrDefault(p => p.Id == id);
                    Inventorys.Remove(inventory);
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
