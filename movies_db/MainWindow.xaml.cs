using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
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
using Newtonsoft.Json.Linq;

namespace movies_db
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddEntity()
        {

            string apiKey = "e5df3688"; // API ключ для взятия информации с сайта
            string baseUrl = $"http://www.omdbapi.com/?apikey={apiKey}"; // базовая ссылка сайта, с которого берется информация

            string name = Title.Text.ToLower();

            var sb = new StringBuilder(baseUrl); // обращение к конкретному фильму по названию и API ключу
            sb.Append($"&t={name}"); // добавление имени фильма в ссылку
            var request = WebRequest.Create(sb.ToString()); // организация веб-запроса по ссылке
            request.Timeout = 1000; // задержка ответа с сервера в 1 секунду
            request.Method = "GET"; // тип запроса на сервер, в данном случае GET
            request.ContentType = "application/json"; // в каком виде прислать ответ с сервера

            string result = string.Empty; // обнуляем результат

            // блок обработки исключений при посылке некорректного запроса
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            MessageBox.Show(result);

            dynamic json = JObject.Parse(result); // парсим строчку result в JSON файл
            string title = json.Plot; 

            var option = MessageBox.Show(title, "Add this title to library", MessageBoxButton.YesNo);

            if (option == MessageBoxResult.Yes)
            {
                fillTable();
            }
        }

        public void fillTable()
        {
            using (MoviesEntities context = new MoviesEntities())
            {
                Movie movie = new Movie
                {
                    Name = "hui",
                    director = "whore",
                    Stars = "Sambuka katerina",
                    Rating = (Nullable<short>) 11
                };

                context.Movies.Add(movie);
                context.SaveChanges();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEntity();
        }
    }
}
