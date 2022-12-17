using CinemaWithMVVM.Command;
using CinemaWithMVVM.Model;
using CinemaWithMVVM.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CinemaWithMVVM.ViewModel
{
    public class InformationFilmViewModel:BaseViewModel
    {
        private Movie movie;

        public RelayCommand YoutubeCommand { get; set; }


        

        public Movie Movie
        {
            get { return movie; }
            set
            {
                movie = value;
                OnPropertyChanged();
            }
        }


        public InformationFilmViewModel()
        {
            

            YoutubeCommand = new RelayCommand((o) =>
            {
                

                WebBrowser webBrowser=new WebBrowser();

                webBrowser.Navigate($"https://www.youtube.com/results?search_query={Movie!.Title}+trailer");
               
            });
        }

    }
}
