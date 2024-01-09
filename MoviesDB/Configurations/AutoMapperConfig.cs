using AutoMapper;
using MoviesDB.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Film, Film>();    
        }

    }
    
}
