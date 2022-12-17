using CinemaWithMVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaWithMVVM.ViewModel
{
    internal class MovieUCViewModel:BaseViewModel
    {
        private Movie? movie;
        public Movie? Movie
        {
            get { return movie; }
            set
            {
                movie = value;
                OnPropertyChanged();
            }
        }
        public MovieUCViewModel()
        {

        }
    }
}
