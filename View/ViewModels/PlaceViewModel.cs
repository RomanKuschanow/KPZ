#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace View.ViewModels;
public partial class PlaceViewModel : ObservableObject
{
    private readonly DBContext context;

    [ObservableProperty]
    private IEnumerable<Place> places;

    [ObservableProperty]
    private IEnumerable<Owner> owners;

    [ObservableProperty]
    private IEnumerable<string> cities;

    [ObservableProperty]
    private IEnumerable<string> addresses;

    [ObservableProperty]
    private IEnumerable<string> types = new List<string>()
    {
        "Парк",
        "Ресторан",
        "Магазин",
        "Подія",
        "Торговий центр"
    };

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    private string city;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    private string address;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private string editCity;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private string editAddress;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private Owner owner;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private string type;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private string name;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlaceCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditPlaceCommand))]
    private string description;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string newItemName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string newItemValue;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemEditCommand))]
    private string editItemName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemEditCommand))]
    private string editItemValue;

    [ObservableProperty]
    private ObservableCollection<NameValueItem> newItems = new();

    [ObservableProperty]
    private ObservableCollection<NameValueItem> editItems = new();

    [ObservableProperty]
    private Place selectedItem;

    public PlaceViewModel(DBContext context)
    {
        this.context = context;

        Places = context.Places.Include(p => p.Owner).Include(p => p.Address).ThenInclude(a => a.City).ToList();
        Owners = context.Owners.ToList();
        Cities = context.Cities.Select(c => c.Name).ToList();
    }

    [RelayCommand]
    private void CityChanged() => Addresses = context.Addresses.Include(a => a.City).Where(a => a.City.Name == City).Select(a => a.Name).ToList();

    [RelayCommand]
    private void NewLine(object obj)
    {
        TextBox textBox = obj as TextBox;
        int caret = textBox.CaretIndex;
        Description = Description.Insert(textBox.CaretIndex, Environment.NewLine);
        textBox.CaretIndex = caret + 1;
    }

    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemName) && !string.IsNullOrWhiteSpace(NewItemValue);

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        NewItems.Add(new NameValueItem { Name = NewItemName, Value = NewItemValue });
        NewItemName = string.Empty;
        NewItemValue = string.Empty;
    }

    [RelayCommand]
    private void RemoveItem(NameValueItem item)
    {
        if (item != null)
        {
            NewItems.Remove(item);
        }
    }

    private bool CanAddItemEdit() => !string.IsNullOrWhiteSpace(EditItemName) && !string.IsNullOrWhiteSpace(EditItemValue);

    [RelayCommand(CanExecute = nameof(CanAddItemEdit))]
    private void AddItemEdit()
    {
        EditItems.Add(new NameValueItem { Name = EditItemName, Value = EditItemValue });
        EditItemName = string.Empty;
        EditItemValue = string.Empty;
    }

    [RelayCommand]
    private void RemoveItemEdit(NameValueItem item)
    {
        if (item != null)
        {
            EditItems.Remove(item);
        }
    }

    private bool CanAddPlace() => !string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Address) && Owner is not null && !string.IsNullOrWhiteSpace(Type) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Description);

    [RelayCommand(CanExecute = nameof(CanAddPlace))]
    private void AddPlace()
    {
        if (!context.Cities.Select(c => c.Name).Contains(City))
        {
            context.Add(new City() { Name = City });
            context.SaveChanges();
        }

        if (!context.Addresses.Select(a => a.Name).Contains(Address))
        {
            context.Add(new Address() { CityId = context.Cities.Single(c => c.Name == City).Id, Name = Address });
            context.SaveChanges();
        }

        string addinfo = "";

        if (NewItems.Count > 0)
        {
            addinfo = JsonConvert.SerializeObject(NewItems, Formatting.Indented);
        }

        context.Add(new Place() { AddressId = context.Addresses.Include(a => a.City).Single(a => a.City.Name == City && a.Name == Address).Id, OwnerId = Owner.Id, Type = Type, Name = Name, Description = Description, IsOpen = true, AddInfo = addinfo });
        context.SaveChanges();

        City = "";
        Address = "";
        Type = null;
        Owner = null;
        Name = "";
        Description = "";
        NewItems = new();

        Places = context.Places.Include(p => p.Owner).Include(p => p.Address).ThenInclude(a => a.City).ToList();
    }

    private bool CanEditPlace() => !string.IsNullOrWhiteSpace(EditCity) && !string.IsNullOrWhiteSpace(EditAddress) && SelectedItem.Owner is not null && !string.IsNullOrWhiteSpace(SelectedItem.Type) && !string.IsNullOrWhiteSpace(SelectedItem.Name) && !string.IsNullOrWhiteSpace(SelectedItem.Description);

    [RelayCommand(CanExecute = nameof(CanEditPlace))]
    private void EditPlace()
    {
        if (!context.Cities.Select(c => c.Name).Contains(EditCity))
        {
            context.Add(new City() { Name = City });
            context.SaveChanges();
        }

        if (!context.Addresses.Select(a => a.Name).Contains(EditAddress))
        {
            context.Add(new Address() { CityId = context.Cities.Single(c => c.Name == City).Id, Name = Address });
            context.SaveChanges();
        }

        SelectedItem.AddressId = context.Addresses.Include(a => a.City).Single(a => a.City.Name == EditCity && a.Name == EditAddress).Id;
        context.SaveChanges();

        SelectedItem = null;

        Places = context.Places.Include(p => p.Owner).Include(p => p.Address).ThenInclude(a => a.City).ToList();
    }

    [RelayCommand]
    private void DeletePlace()
    {
        if (SelectedItem is not null)
        {
            context.Remove(SelectedItem);
            context.SaveChanges();
            Places = context.Places.Include(p => p.Owner).Include(p => p.Address).ThenInclude(a => a.City).ToList();
        }
    }

    partial void OnSelectedItemChanged(Place value)
    {
        if (value is null)
            return;

        EditCity = value.Address.City.Name;
        EditAddress = value.Address.Name;
        EditItems = new(JsonConvert.DeserializeObject<List<NameValueItem>>(value.AddInfo));
    }

    public class NameValueItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}