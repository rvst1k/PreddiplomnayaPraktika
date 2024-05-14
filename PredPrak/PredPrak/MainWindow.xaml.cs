using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.ComponentModel;


namespace PredPrak
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Event> events = new List<Event>();
        private List<Directions> directions = new List<Directions>();
        public MainWindow()
        {
            InitializeComponent();

            using (predprakEntities2 entities2 = new predprakEntities2())
            {
                events = entities2.Event.ToList();
                directions = entities2.Directions.ToList();
                Directions.ItemsSource = directions;
                Directions.DisplayMemberPath = "name";

                EventsListView.ItemsSource = entities2.Event.Include(e => e.Directions).ToList();
                
            }
        }


        private void Directions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (predprakEntities2 predprakentities2 = new predprakEntities2())
            {
                if (Directions.SelectedIndex != 0)
                {

                    events = predprakentities2.Event.Include(x => x.Directions).Where(x => x.DirectionId == ((Directions)Directions.SelectedItem).id).ToList();

                    EventsListView.ItemsSource = events;
                }
                else
                {
                    events = predprakentities2.Event.ToList();

                    EventsListView.ItemsSource = events;
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            using (predprakEntities2 predprakentities2 = new predprakEntities2())
            {
                if (Date.SelectedDate != null)
                {
                    DateTime selectedDate = Date.SelectedDate.Value;

                    events = predprakentities2.Event.Include(x => x.Directions).Where(x => x.Date == selectedDate.Date).ToList();

                    EventsListView.ItemsSource = events;
                }
                else
                {
                    events = predprakentities2.Event.ToList();

                    EventsListView.ItemsSource = events;
                }
            }
        }
    }
}
