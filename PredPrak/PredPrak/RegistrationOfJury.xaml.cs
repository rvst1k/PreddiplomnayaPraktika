using Microsoft.Win32;
using PredPrak;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PredPrak
{
    public partial class RegistrationOfJury : Window
    {
        private Users newUser = new Users();

        private Regex _phoneNumberRegex = new Regex(@"^\+7\((\d{3})\)\s-\s(\d{3})-(\d{2})-(\d{2})$");

        private string photoName;
        private string filePath;

        public RegistrationOfJury()
        {
            InitializeComponent();
            Random rnd = new Random();
            int randomNumber = rnd.Next(10000); 
            NumberId.Text = randomNumber.ToString(); 

            newUser.DateOfBirth = new DateTime(2000, 1, 1);
            using (predprakEntities2 predprakEntities2 = new predprakEntities2())
            {

                List<Gender> genders = new List<Gender>();
                genders = predprakEntities2.Gender.ToList();
                gender.ItemsSource = genders;
                gender.DisplayMemberPath = "name";

                gender.SelectedItem = 0;

                List<Directions> directions = new List<Directions>();
                directions = predprakEntities2.Directions.ToList();
                
                DirectionComboBox.ItemsSource = directions;
                DirectionComboBox.DisplayMemberPath = "name";
                DirectionComboBox.SelectedItem = 0;

                List<Roles> roles = new List<Roles>();
                roles = predprakEntities2.Roles.Where(x => x.id == 1 || x.id == 2).ToList();
                role.ItemsSource = roles;
                role.DisplayMemberPath= "name";

                DataContext = newUser;
            }


        }

        public bool IsPhoneNumberValid(string phoneNumber)
        {
            return _phoneNumberRegex.IsMatch(phoneNumber);
        }

        public static int ValidatePassword(string x)
        {
            bool hasDigit = false;
            bool hasLower = false;
            bool hasUpper = false;
            bool hasSpeciaChar = false;
            int kol = 0;

            foreach (char c in x)
            {
                kol++;
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsLower(c))
                {
                    hasLower = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                else if ((char.IsSymbol(c)) || (c == '!') || (c == '@') || (c == '$') || (c == '#'))
                {
                    hasSpeciaChar = true;
                }
            }
            if (hasDigit == false)
            {

                return 0;
            }
            else if (hasLower == false)
            {

                return 1;
            }
            else if (hasUpper == false)
            {

                return 2;
            }
            else if (hasSpeciaChar == false)
            {

                return 3;
            }
            else if ((kol < 5) || (kol > 20))
            {

                return 4;
            }
            else
            {
                return 5;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Выберите картинку";
                op.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files(*.gif) | *.gif";
                if (op.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(op.FileName);
                    if (fileInfo.Length > (1024 * 1024 * 2))
                    {
                        throw new Exception("Размер файла должен быть меньше 2Мб");
                    }
                    UserImage.Source = new BitmapImage(new Uri(op.FileName));
                    photoName = op.SafeFileName;
                    filePath = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK);
                filePath = null;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            using (predprakEntities2 predprakEntities2 = new predprakEntities2())
            {
                List<Event> events = new List<Event>();
                events = predprakEntities2.Event.Where(x => x.DirectionId == ((Directions)DirectionComboBox.SelectedItem).id).ToList();
                ActivityComboBox.ItemsSource = events;
                ActivityComboBox.DisplayMemberPath = "EventName";
            }
            ActivityComboBox.SelectedIndex = 0;
            ActivityComboBox.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ActivityComboBox.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FIO.Text != null && Email.Text != null && PhoneNumber.Text != null && UserPasswordBox.Password
                != null && DirectionComboBox.SelectedItem != null && gender.SelectedItem != null && role.SelectedItem != null)
            {
                if (IsPhoneNumberValid(PhoneNumber.Text))
                {
                    MessageBox.Show("Введите действительный номер телефона");
                }
                else if (UserPasswordBox.Password == RepeatPassword.Password)
                {
                    int result = ValidatePassword(UserPasswordBox.Password);
                    if (result != 5)
                    {
                        if (result == 0)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать цифры");
                        }
                        else if (result == 1)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать строчые буквы");
                        }
                        else if (result == 2)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать заглавные буквы");
                        }
                        else if (result == 3)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать специальные символы");
                        }
                        else if (result == 4)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать от 5 до 20 символов");
                        }
                    }
                    else
                    {
                        using (predprakEntities2 predprakEntities2 = new predprakEntities2())
                        {
                            newUser.id = Convert.ToInt32(NumberId.Text);
                            newUser.GenderId = (gender.SelectedItem as Gender).id;
                            newUser.Password = UserPasswordBox.Password;
                            newUser.GenderId = ((Gender)gender.SelectedItem).id;
                            newUser.RolesId = ((Roles)role.SelectedItem).id;
                            newUser.DirectionId = ((Directions)DirectionComboBox.SelectedItem).id;
                            newUser.Mail = Email.Text;

                            if (filePath != null)
                            {
                                string photo = ChangePhotoName();
                                string dest = Directory.GetCurrentDirectory() + @"\Pictures\" + photo;
                                File.Copy(filePath, dest);
                                newUser.Photo = photo;
                            }

                            predprakEntities2.Users.Attach(newUser);
                            predprakEntities2.Entry(newUser).State = System.Data.Entity.EntityState.Added;
                            predprakEntities2.SaveChanges();
                            NumberId.Clear();
                            Random rnd = new Random();
                            int randomNumber = rnd.Next(10000);
                            NumberId.Text = randomNumber.ToString();
                            FIO.Clear();
                            Email.Clear();
                            PhoneNumber.Clear();
                            UserPasswordBox.Clear();
                            RepeatPassword.Clear();
                            gender.SelectedItem = null;
                            role.SelectedItem = null;
                            DirectionComboBox = null;
                            ActivityComboBox = null;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Повторите пароль");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
        string ChangePhotoName()
        {
            string pathToDir = Directory.GetCurrentDirectory() + @"\Pictures\";
            string x = pathToDir + photoName;
            string newPhotoName = photoName;
            int i = 0;
            if (File.Exists(x))
            {
                while (File.Exists(x))
                {
                    i++;
                    x = pathToDir + newPhotoName + i;
                }
                newPhotoName = i.ToString() + newPhotoName;
            }
            return newPhotoName;
        }

        private void ShowPass_Checked(object sender, RoutedEventArgs e)
        {
           
        }
        }
    }