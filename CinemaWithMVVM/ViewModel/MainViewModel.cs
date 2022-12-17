using CinemaWithMVVM.Command;
using CinemaWithMVVM.Model;
using CinemaWithMVVM.UserControls;
using CinemaWithMVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.IO;

namespace CinemaWithMVVM.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        string jsonStr;
        private List<string> _movieDataBase;
        HttpClient HttpClient=new HttpClient();


        public RelayCommand SearchCommand { get; set; }
        public RelayCommand KeyDownCommand { get; set; }

        public UniformGrid Uniform { get; set; }

        public Movie movie { get; set; }


        private void Uc_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var uc = sender as MovieUC;
            var ucVm = new MovieUCViewModel();

            var something = uc!.DataContext;
            var cinema = something as MovieUCViewModel;
            ucVm.Movie = cinema!.Movie;

            uc!.DataContext = ucVm;


            InformationFilm more = new();
            var moreVm = new InformationFilmViewModel();
            moreVm.Movie = ucVm.Movie;
            more.DataContext = moreVm;

            more.ShowDialog();
        }

        private async Task Search_Film(UniformGrid uniformGrid, object o)
        {
            var TextBox_Search = o as TextBox;

            if (string.IsNullOrWhiteSpace(TextBox_Search!.Text))
            {
                MessageBox.Show("Please Enter Movie Name", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            jsonStr = await HttpClient.GetStringAsync($@"http://www.omdbapi.com/?apikey=82bcd4c7&t={TextBox_Search.Text}");

            if (!jsonStr.Contains("Error"))
            {
                var movie = JsonSerializer.Deserialize<Movie>(jsonStr);

                var uc = new MovieUC ();
                var ucVm = new MovieUCViewModel();
                ucVm.Movie = movie;
                uc.DataContext = ucVm;

                movie = movie!;

                uniformGrid!.Children.Add(uc);

                uc.MouseDoubleClick += Uc_MouseDoubleClick;

                _movieDataBase!.Add(movie?.Title!);
                string str = JsonSerializer.Serialize(_movieDataBase);
                File.WriteAllText("../../../DataBase/MovieDataBase.json", str);
                return;
            }
            else
                MessageBox.Show("No Result Found", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        


        public MainViewModel(UniformGrid uniformGrid)
        {
            _movieDataBase = new List<string>();
            movie = new Movie();


            _movieDataBase = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("../../../DataBase/MovieDataBase.json"))!;


            Uniform = uniformGrid;
            foreach (var m in _movieDataBase)
            {
                jsonStr = HttpClient.GetStringAsync($"http://www.omdbapi.com/?apikey=82bcd4c7&t={m}&plot=full").Result;

                if (!jsonStr.Contains("Error"))
                {
                    var movie = JsonSerializer.Deserialize<Movie>(jsonStr);
                    var uc = new MovieUC();
                    var ucVm = new MovieUCViewModel();
                    ucVm.Movie = movie;
                    uc.DataContext = ucVm;

                     //movie = ucVm.Movie!;

                    uc.MouseDoubleClick += Uc_MouseDoubleClick;

                    uniformGrid!.Children.Add(uc);
                }
            }

            SearchCommand = new RelayCommand(async (o) =>
            {
                await Search_Film(uniformGrid, o);
            });
        }
    }
}
