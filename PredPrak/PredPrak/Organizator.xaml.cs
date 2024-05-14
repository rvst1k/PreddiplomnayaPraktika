using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Drawing;


namespace PredPrak
{
    /// <summary>
    /// Логика взаимодействия для Organizator.xaml
    /// </summary>
    public partial class Organizator : Window
    {
        predprakEntities2 entities = new predprakEntities2();
        public Organizator(int id)
        {
            InitializeComponent();


            Users users = entities.Users.Where(x => x.id == id).FirstOrDefault();
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            string timeOfDay;
            if (currentHour >= 9 && currentHour < 11)
            {
                timeOfDay = "Доброе утро";
            }
            else if (currentHour >= 11 && currentHour < 18)
            {
                timeOfDay = "Добрый день";
            }
            else
            {
                timeOfDay = "Добрый вечер";
            }
            time.Content = timeOfDay;

            using (predprakEntities2 pred = new predprakEntities2())
            {
                var result = pred.Users.Where(x => x.id == id).FirstOrDefault();

                if (result != null)
                {
                    if (result.GenderId == 1)
                    {
                        name.Content = "Mrs. " + users.Name;
                    }
                    else
                    {
                        name.Content = "Mr. " + users.Name;
                    }
                }
            }
            using (predprakEntities2 pred = new predprakEntities2())
            {              

                var fileName = pred.Users.Where(x => x.id == id).Select(x => x.Photo).FirstOrDefault();

                if (!string.IsNullOrEmpty(fileName))
                {
                    // Добавляем путь к файлу перед именем файла
                    var imagePath = @"C:\Users\zrust\OneDrive\Рабочий стол\Ресурсы\Участники_import\" + fileName;

                    // Создаем новый BitmapImage
                    BitmapImage image = new BitmapImage(new Uri(imagePath));

                    // Устанавливаем BitmapImage в качестве источника для Image
                    pfp.Source = image;
                }


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrationOfJury registration = new RegistrationOfJury();
            registration.Show();
            this.Close();
        }
    }
}

