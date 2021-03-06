﻿using HCI_projekat2.Model;
using HCI_projekat2.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using HCI_projekat2.Tabels;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using HCI_projekat2.Help;

namespace HCI_projekat2
{
    public partial class MainWindow : Window
    {
        public static Dictionary<string, ResourceModel> _resursi;
        public static Dictionary<string, ResourceModel> Resursi
        {
            get
            {
                return _resursi;
            }
            set
            {
                _resursi = value;
            }
        }
        public static Dictionary<string, TypeModel> _tipovi;
        public static Dictionary<string, TypeModel> Tipovi
        {
            get
            {
                return _tipovi;
            }
            set
            {
                _tipovi = value;
            }
        }
        public static Dictionary<string, LabelModel> _etikete;
        public static Dictionary<string, LabelModel> Etikete
        {
            get
            {
                return _etikete;
            }
            set
            {
                _etikete = value;
            }

        }
        public static Dictionary<Rect, ResourceModel> resursiNaMapi
        {
            get;
            set;
        }
        private static List<UserModel> _korisnici;
        public static List<UserModel> Korisnici
        {
            get
            {
                return _korisnici;
            }
            set
            {
                _korisnici = value;
            }
        }
        private UserModel _currUser;
        public UserModel CurrUser
        {
            get
            {
                return _currUser;
            }
            set
            {
                _currUser = value;
            }
        }

        public string ResursiFajl;
        public string TipoviFajl;
        public string EtiketeFajl;
        public string KorisniciFajl;

        public static ObservableCollection<LabelModel> obsEtikete
        {
            get;
            set;
        }
        public static ObservableCollection<TypeModel> obsTipovi
        {
            get;
            set;
        }
        public static ObservableCollection<ResourceModel> obsResursi
        {
            get;
            set;
        }

        public static ObservableCollection<ResourceModel> ikoniceResursa
        {
            get;
            set;
        }
        public static ObservableCollection<ResourceModel> ikoniceCanvas
        {
            get;
            set;
        }

        Point startPoint = new Point();

        public static List<string> frekvencije { get; set; }

        //frekvencije
        public static string Redak = "Redak";
        public static string Cest = "Čest";
        public static string Univerzalan = "Univerzalan";

        public static List<string> mere { get; set; }
        public bool Collision { get; private set; }

        //jedinice mere
        public static string Merica = "Merica";
        public static string Barel = "Barel";
        public static string Tona = "Tona";
        public static string Kg = "Kilogram";

        public static int imgWidth = 45;
        public static int imgHeight = 45;

        private bool _cntFlag = false; //flag za nastavak rada
        public bool CntFlag
        {
            get
            {
                return _cntFlag;
            }
            set
            {
                _cntFlag = value;
            }
        }

        private bool _usersFlag;
        public bool UsersFlag
        {
            get
            {
                return _usersFlag;
            }
            set
            {
                _usersFlag = value;
            }
        }

        private bool _login = true;
        public bool Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }

        public MainWindow()
        {
            if (!ucitajKorisnike())
            {
                var t = new NewUserDialog(this);
                t.ShowDialog();
            }

                var l = new LoginDialog(this);
                l.ShowDialog();

            if (_cntFlag)
            {
                InitializeComponent();
                Style = (Style)FindResource(typeof(Window));

                ikoniceCanvas = new ObservableCollection<ResourceModel>();
                resursiNaMapi = new Dictionary<Rect, ResourceModel>();

                ucitajPodatke();

                ucitajIkonice();

                obsEtikete = new ObservableCollection<LabelModel>(Etikete.Values);
                obsTipovi = new ObservableCollection<TypeModel>(Tipovi.Values);
                obsResursi = new ObservableCollection<ResourceModel>(Resursi.Values);



                DataContext = this;
            }
            else
            {
                Close();
            }
        }

        private void AddNewRes_Click(object sender, RoutedEventArgs e)
        {
            var s = new NewResourceDialog();
            s.ShowDialog();
        }

        private void NewResType_Click(object sender, RoutedEventArgs e)
        {
            var t = new NewResourceType();
            t.ShowDialog();
        }

        private void NewResLabel_Click(object sender, RoutedEventArgs e)
        {
            var l = new NewLabelDialog();
            l.ShowDialog();
        }

        private void ShowLabels_Click(object sender, RoutedEventArgs e)
        {
            var l = new LabelTable();
            l.ShowDialog();
        }

        private void ShowTypes_Click(object sender, RoutedEventArgs e)
        {
            var l = new TypeTable(this);
            l.ShowDialog();
        }

        private void ShowRes_Click(object sender, RoutedEventArgs e)
        {
            var l = new ResourceTable(this);
            l.Show();
        }

        private bool TipoveSacuvaj()
        {
            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();
            bool retVal = false;
            try
            {
                stream = File.Open(CurrUser.Name + "tipovi.nkvd", FileMode.OpenOrCreate);
                bf.Serialize(stream, _tipovi);
            }
            catch
            {
                //nista
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    retVal = true;
                }
            }
            return retVal;
        }

        private bool EtiketeSacuvaj()
        {

            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();
            bool retVal = false;
            try
            {
                stream = File.Open(CurrUser.Name + "etikete.nkvd", FileMode.OpenOrCreate);
                bf.Serialize(stream, _etikete);
            }
            catch
            {
                //nista
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    retVal = true;
                }
            }
            return retVal;
        }

        private bool ResurseSacuvaj()
        {

            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();
            bool retVal = false;
            try
            {
                stream = File.Open(CurrUser.Name + "resursi.nkvd", FileMode.OpenOrCreate);
                bf.Serialize(stream, _resursi);
            }
            catch
            {
                //nista
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    retVal = true;
                }
            }
            return retVal;
        }

        private void Sacuvaj_Sve_Click(object sender, RoutedEventArgs e)
        {
            if (EtiketeSacuvaj() && ResurseSacuvaj() && TipoveSacuvaj() && sacuvajKorisnike())
            {
                MessageBoxResult msg;
                msg = MessageBox.Show(this, "Podaci su uspešno sačuvani.", "Operacija uspešna", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult msg;
                msg = MessageBox.Show(this, "Došlo je do greške prilikom čuvanja!", "Operacija neuspešna", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (CntFlag)
            {
                MessageBoxResult msb = MessageBox.Show("Želite li da sačuvate unete izmene?", "Čuvanje pre zatvaranja", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (msb == MessageBoxResult.Yes)
                {
                    EtiketeSacuvaj();
                    TipoveSacuvaj();
                    ResurseSacuvaj();
                    sacuvajKorisnike();
                }
                else if (msb == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            base.OnClosing(e);
        }

        /*ucitava resurse, etikete i tipove za trenutno aktivnog korisnika, ako nema tog korisnika, napravi nove fajlove*/
        private void ucitajPodatke()
        {
            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();

            TipoviFajl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CurrUser.Name + "tipovi.nkvd");
            if (File.Exists(TipoviFajl))
            {
                try
                {
                    stream = File.Open(TipoviFajl, FileMode.Open);
                    Tipovi = (Dictionary<string, TypeModel>)bf.Deserialize(stream);
                }
                catch
                {
                    //nista
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
            {
                Tipovi = new Dictionary<string, TypeModel>();
            }
            stream = null;
            EtiketeFajl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CurrUser.Name + "etikete.nkvd");
            if (File.Exists(EtiketeFajl))
            {
                try
                {
                    stream = File.Open(EtiketeFajl, FileMode.Open);
                    Etikete = (Dictionary<string, LabelModel>)bf.Deserialize(stream);
                }
                catch
                {

                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
            {

                Etikete = new Dictionary<string, LabelModel>();
            }
            stream = null;
            ResursiFajl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CurrUser.Name + "resursi.nkvd");
            if (File.Exists(ResursiFajl))
            {
                try
                {
                    stream = File.Open(ResursiFajl, FileMode.Open);
                    Resursi = (Dictionary<string, ResourceModel>)bf.Deserialize(stream);
                }
                catch
                {

                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
            {
                Resursi = new Dictionary<string, ResourceModel>();

            }
        }

        private bool sacuvajKorisnike()
        {
            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();
            bool retVal = false;
            try
            {
                stream = File.Open("korisnici.nkvd", FileMode.OpenOrCreate);
                bf.Serialize(stream, _korisnici);
            }
            catch
            {
                //nista
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    retVal = true;
                }
            }
            return retVal;
        }

        //ucitava korisnike, ako ima njihov fajl
        private bool ucitajKorisnike()
        {
            bool retVal = false;
            FileStream stream = null;
            BinaryFormatter bf = new BinaryFormatter();

            KorisniciFajl = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "korisnici.nkvd");
            if (File.Exists(KorisniciFajl))
            {
                retVal = true;
                try
                {
                    stream = File.Open(KorisniciFajl, FileMode.Open);
                    Korisnici = (List<UserModel>)bf.Deserialize(stream);

                }
                catch
                {
                    //nista
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
            {
                Korisnici = new List<UserModel>();
            }
            return retVal;
        }

        public void ucitajIkonice()
        {
            ikoniceResursa = new ObservableCollection<ResourceModel>();
            foreach (ResourceModel model in Resursi.Values)
            {

                if (model.Point.X != 0 || model.Point.Y != 0)
                {
                    Image cpy = new Image();
                    cpy.Width = imgWidth;
                    cpy.Height = imgHeight;
                    cpy.Source = new BitmapImage(new Uri(model.IconPath, UriKind.RelativeOrAbsolute));
                    cpy.Cursor = Cursors.Hand;
                    cpy.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Image_PreviewMouseLeftButtonDown);
                    cpy.MouseMove += new MouseEventHandler(Image_MouseMove);

                    cpy.Tag = model;

                    cpy.ContextMenu = createContextMenu();
                    cpy.ToolTip = addTooltip(model);

                    Canvas.SetLeft(cpy, model.Point.X - cpy.Width / 2);
                    Canvas.SetTop(cpy, model.Point.Y - cpy.Height / 2);
                    Canvas.Children.Add(cpy);

                    resursiNaMapi.Add(new Rect(model.Point.X - cpy.Width / 2, model.Point.Y - cpy.Height / 2, imgWidth, imgHeight), model);
                }
                else
                {
                    ikoniceResursa.Add(model);
                }
            }
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                Image image = e.Source as Image;
                DataObject data = new DataObject("ikonica", image);
                DragDrop.DoDragDrop(image, data, DragDropEffects.Move);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ikonica"))
            {
                Point p = e.GetPosition(Canvas);
                Image img = e.Data.GetData("ikonica") as Image;
                RemovePreview();
                if (preklapanje(p, img))
                {
                    return;
                }

                ikoniceResursa.Remove((ResourceModel)img.Tag);

                listaIkonica.SelectedIndex = -1;
                listaIkonica.UpdateLayout();

                Canvas.Children.Remove(img);

                Image cpy = new Image();
                cpy.Width = imgWidth;
                cpy.Height = imgHeight;
                cpy.Source = img.Source;
                cpy.Cursor = Cursors.Hand;
                cpy.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Image_PreviewMouseLeftButtonDown);
                cpy.MouseMove += new MouseEventHandler(Image_MouseMove);

                cpy.Tag = img.Tag;

                cpy.ToolTip = addTooltip(((ResourceModel)img.Tag));

                cpy.ContextMenu = createContextMenu();
                
                Resursi[((ResourceModel)img.Tag).ID].Point = p;

                double x = p.X - cpy.Width / 2;
                double y = p.Y - cpy.Height / 2;
                Canvas.SetLeft(cpy, x);
                Canvas.SetTop(cpy, y);
                Canvas.Children.Add(cpy);

                resursiNaMapi.Add(new Rect(x, y, imgWidth, imgHeight), (ResourceModel)img.Tag);
            }
        }

        private ContextMenu createContextMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem editMenuItem = new MenuItem();
            editMenuItem.Header = "Izmeni";
            editMenuItem.Click += new RoutedEventHandler(EditElement);

            MenuItem deleteMenuItem = new MenuItem();
            deleteMenuItem.Header = "Obriši";
            deleteMenuItem.Click += new RoutedEventHandler(DeleteElement);
            

            cm.Items.Add(editMenuItem);
            cm.Items.Add(deleteMenuItem);

            return cm;
        }

        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            Image image = ((ContextMenu)menuItem.Parent).PlacementTarget as Image;
            ResourceModel model = ((ResourceModel)image.Tag);
            foreach (ResourceModel m in Resursi.Values)
            {
                if (m.ID.Equals(model.ID))
                {
                    Resursi.Remove(m.ID);
                    break;
                }
            }
            removeResourceFromMap(model);
            iscrtajOpet();
            Canvas.Children.Remove(image);
        }

        private void EditElement(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            Image image = ((ContextMenu)menuItem.Parent).PlacementTarget as Image;
            ChangeResourceDialog res = new ChangeResourceDialog((ResourceModel)image.Tag, this);
            res.ShowDialog();
        }

        private Rectangle preview = null;

        private void RemovePreview()
        {
            Canvas.Children.Remove(preview);
            preview = null;
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ikonica"))
            {
                if (preview == null)
                {
                    preview = new Rectangle();
                    preview.Height = imgHeight;
                    preview.Width = imgWidth;

                    Canvas.Children.Add(preview);
                }

                Point p = e.GetPosition(Canvas);
                preview.StrokeThickness = 2;
                preview.Stroke = new SolidColorBrush(Colors.Black);

                //preklapanje sa vec postojecom ikonicom
                if (preklapanje(p, e.Data.GetData("ikonica") as Image))
                {
                    preview.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                    preview.Fill = new SolidColorBrush(Colors.CadetBlue);

                Canvas.SetLeft(preview, p.X - imgWidth / 2);
                Canvas.SetTop(preview, p.Y - imgHeight / 2);
            }
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("ikonica") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private bool preklapanje(Point p, Image img)
        {
            foreach (Rect r in resursiNaMapi.Keys)
            {
                if (!resursiNaMapi[r].ID.Equals(((ResourceModel)img.Tag).ID) && r.IntersectsWith(new Rect(p.X - imgWidth / 2, p.Y - imgHeight / 2, imgWidth, imgHeight)))
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    return true;
                }
            }

            return false;
        }

        public void iscrtajOpet()
        {
            listaIkonica.SelectedItem = -1;
            listaIkonica.UpdateLayout();
        }

        public void removeResourceFromMap(ResourceModel model)
        {
            foreach (Rect r in resursiNaMapi.Keys)
            {
                if (resursiNaMapi[r].Equals(model))
                {
                    resursiNaMapi.Remove(r);
                    break;
                }
            }
        }

        private System.Windows.Forms.ContextMenu createIconContextMenu()
        {
            System.Windows.Forms.ContextMenu cm = new System.Windows.Forms.ContextMenu();

            return cm;
        }

        internal void doThings(string param)
        {
            throw new NotImplementedException();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpProvider.ShowHelp("main", this);
        }

        public string addTooltip(ResourceModel model)
        {
            return model.Name + Environment.NewLine + model.Date.ToShortDateString() + Environment.NewLine + model.Price + "$"
                + Environment.NewLine + model.Type.Name;
        }

        private void Help_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelpProvider.ShowHelp("main", this);
        }

        private void New_Resource_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewRes_Click(sender, e);
        }

        private void New_Type_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewResType_Click(sender, e);
        }

        private void New_Label_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewResLabel_Click(sender, e);
        }

        private void Show_ResTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowRes_Click(sender, e);
        }

        private void Show_LabTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowLabels_Click(sender, e);
        }

        private void Show_TypeTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowTypes_Click(sender, e);
        }

        private void Save_Project_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sacuvaj_Sve_Click(sender, e);
        }

        private void ReturnToPanel(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ikonica"))
            {
                Point p = new Point(0, 0);
                Image img = e.Data.GetData("ikonica") as Image;
                RemovePreview();
                ((ResourceModel)img.Tag).Point = p;
                ikoniceResursa.Add((ResourceModel)img.Tag);
                Canvas.Children.Remove(img);
                iscrtajOpet();

            }
        }
    }
}

