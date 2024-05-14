using EasyCaptcha.Wpf;
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


namespace PredPrak
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private int nullCount = 0;

        public Authorization()
        {
            InitializeComponent();
            RemadeCaptcha();
        }

        private void RemadeCaptcha()
        {
            Captcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Answer.Text != Captcha.CaptchaText)
            {
                MessageBox.Show("Неверная CAPTCHA");
            }
            else
            {
                using (predprakEntities2 entities1 = new predprakEntities2())
                {
                    int id = int.Parse(idNumber.Text);
                    var result = entities1.Users.Where(x => x.id == id && x.Password == Password.Password).FirstOrDefault();
                    if (result != null)
                    {

                        if (result.RolesId == 4)
                        {
                            ParticipantWindow participant = new ParticipantWindow();
                            participant.Show();
                            this.Close();
                        }
                        else if (result.RolesId == 2)
                        {
                            ModeratorWindow moderator = new ModeratorWindow();
                            moderator.Show();
                            this.Close();
                        }
                        else if (result.RolesId == 1)
                        {
                            ZhuriWindow jury = new ZhuriWindow();
                            jury.Show();
                            this.Close();
                        }
                        else if (result.RolesId == 3)
                        {                           
                            Organizator organaizer = new Organizator(id);
                            organaizer.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!");
                        nullCount++;
                        if (nullCount >= 3)
                        {
                            MessageBox.Show("Вы ввели неправильные данные слишком много раз. В целях безопасности система заблокирована на 10 секунд");

                            nullCount = 0;

                            button1.IsEnabled = false; // Деактивируем кнопку
                            Task.Delay(10000).ContinueWith(t =>
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    button1.IsEnabled = true; // Активируем кнопку после 10 секунд
                                });
                            });
                        }
                    }                                            
                }
            }
        }
    }
}
