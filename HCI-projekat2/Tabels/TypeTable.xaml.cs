﻿using System.Collections.ObjectModel;
using System.Windows;
using HCI_projekat2.Model;
using static HCI_projekat2.MainWindow;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System;
using HCI_projekat2.Dialogs;

namespace HCI_projekat2.Tabels
{

    public partial class TypeTable : Window
    {
        public TypeModel mod = new TypeModel();


        public ObservableCollection<TypeModel> tipovi
        {
            get;
            set;
        }

        public ObservableCollection<TypeModel> tipoviContainer
        {
            get;
            set;
        }


        public TypeTable()
        {
            InitializeComponent();
            tipovi = new ObservableCollection<TypeModel>(Tipovi.Values);
            tipoviContainer = new ObservableCollection<TypeModel>(Tipovi.Values);
            DataContext = this;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dgrMain.SelectedItem == null)
            {
                MessageBox.Show(this, "Morate izabrati jedan tip iz tabele.", "Operacija neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(this, "Jeste li sigurni da želite obrisati selektovani tip?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TypeModel model = (TypeModel)dgrMain.SelectedItem;
                foreach (ResourceModel r in Resursi.Values)
                {
                    if (r.Type.ID.Equals(model.ID))
                    {
                        MessageBoxResult mbr = MessageBox.Show("Ne može se obrisati tip kojem pripadaju neke vrste. Molimo Vas, izmenite resurse pre brisanja tipa.", "Nedozvoljena operacija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }

                Tipovi.Remove(model.ID);
                tipovi.Clear();
                foreach (TypeModel t in Tipovi.Values)
                {
                    tipovi.Add(t);
                }
                MessageBox.Show(this, "Tip resursa je obrisan.", "Operacija uspešna", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dgrMain.SelectedItem == null)
            {
                MessageBox.Show(this, "Morate izabrati jedan tip iz tabele.", "Operacija neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TypeModel model = (TypeModel)dgrMain.SelectedItem;
            ChangeTypeDialog val = new ChangeTypeDialog(model);
            val.Show();
        }

        private void resetFilter_Click(object sender, RoutedEventArgs e)
        {
            idTextBox.Text = "";
            opisTextBox.Text = "";
            imeTextBox.Text = "";

            tipovi.Clear();

            foreach(TypeModel tip in tipoviContainer)
            {
                tipovi.Add(tip);
            }
        }

        private void filtrirajTabelu(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tipovi.Clear();
            bool uslov;

            foreach (TypeModel tip in tipoviContainer)
            {
                uslov = true;
                if (!idTextBox.Text.Equals(""))
                {
                    if (!tip.ID.Contains(idTextBox.Text))
                    {
                        uslov = false;
                    }
                }

                if (!opisTextBox.Text.Equals(""))
                {
                    if (!tip.Desc.Contains(opisTextBox.Text))
                    {
                        uslov = false;
                    }
                }

                if (!imeTextBox.Text.Equals(""))
                    if (!tip.Name.Contains(imeTextBox.Text))
                        uslov = false;

                if (uslov)
                    tipovi.Add(tip);
            }
        }
    }
}
