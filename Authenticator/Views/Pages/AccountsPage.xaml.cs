﻿using Authenticator_for_Windows.Events;
using Authenticator_for_Windows.Storage;
using Authenticator_for_Windows.Utilities;
using Authenticator_for_Windows.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Authenticator_for_Windows.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountsPage : Page
    {
        private EntryStorage entryStorage;
        private Dictionary<Entry, EntryBlock> mappings;

        public AccountsPage()
        {
            InitializeComponent();

            entryStorage = new EntryStorage();
            mappings = new Dictionary<Entry, EntryBlock>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<Entry> entries = await entryStorage.GetEntriesAsync();

            foreach (Entry entry in entries)
            {
                EntryBlock code = new EntryBlock(entry);
                code.DeleteRequested += Code_DeleteRequested;

                Codes.Children.Add(code);
                mappings.Add(entry, code);
            }

            StrechProgress.Begin();
            StrechProgress.Seek(new TimeSpan(0, 0, 30 - TOTPUtilities.RemainingSeconds));
        }

        private async void Code_DeleteRequested(object sender, DeleteRequestEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Account verwijderen",
                Content = "Weet u zeker dat u dit accoubt wilt verwijderen?\nLet op: Het verwijderen van dit account deactiveert tweestapsauthenticatie op uw account niet!",
                PrimaryButtonText = "Verwijderen",
                SecondaryButtonText = "Annuleren"
            };

            dialog.PrimaryButtonClick += async delegate
            {
                KeyValuePair<Entry, EntryBlock> entry = mappings.FirstOrDefault(m => m.Key == e.Entry);

                await entryStorage.RemoveAsync(entry.Key);

                entry.Value.Remove();
            };

            await dialog.ShowAsync();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            foreach (EntryBlock code in Codes.Children.Where(c => c.GetType() == typeof(EntryBlock)))
            {
                code.InEditMode = !code.InEditMode;
            }
        }
    }
}