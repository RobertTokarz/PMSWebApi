using AutoMapper;
using PMSWebApi.DTOEntities;
using PMSWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSWebApi.Data
{
    public class AutoMapping : Profile
    {
       public AutoMapping()
        {
            CreateMap<Project, ProjectModel>().ReverseMap();
            CreateMap<SubProject, SubProjectModel >().ReverseMap();
        }
    }
}
