﻿using HCI_projekat2.Dialogs;
using HCI_projekat2.Model;
using System.Collections.ObjectModel;
using System.Windows;
using static HCI_projekat2.MainWindow;

namespace HCI_projekat2.Tabels
{
    /// <summary>
    /// Interaction logic for LabelTable.xaml
    /// </summary>
    public partial class LabelTable : Window
    {
        public ObservableCollection<LabelModel> etikete
        {
            get;
            set;
        }


        public LabelTable()
        {
            InitializeComponent();
            etikete = new ObservableCollection<LabelModel>();
            foreach (LabelModel s in Etikete.Values)
            {
                etikete.Add(s);
            }
            DataContext = this;
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(this, "Jeste li sigurni da želite obrisati selektovanu etiketu?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LabelModel model = (LabelModel)dgrMain.SelectedItem;
                Etikete.Remove(model.ID);
                etikete.Clear();
                foreach (LabelModel t in Etikete.Values)
                {
                    etikete.Add(t);
                }
                MessageBox.Show(this, "Etiketa je obrisana.", "Operacija uspešna", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Izadji_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            LabelModel model = (LabelModel)dgrMain.SelectedItem;
            ChangeLabelDialog val = new ChangeLabelDialog(model);
            val.Show();
        }
    }
}
