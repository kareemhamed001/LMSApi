using AutoMapper;
using LMSApi.App.DTOs;
using LMSApi.App.Requests.Class;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace testApp.helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Class, ClassRequest>();
            CreateMap<ClassRequest, Class>();
            CreateMap<Class, GetStudentOfClassRequest>();
            CreateMap<GetStudentOfClassRequest, Class>();
        }

    }
}
