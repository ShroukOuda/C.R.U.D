using AutoMapper;
using CRUD.Data;
using CRUD.Dtos;

namespace CRUD.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDetailsDto>();
            CreateMap<BookDto, Book>()
                .ForMember(src => src.Cover, opt => opt.Ignore());
            CreateMap<Category, CategoryDto>();
            CreateMap<Author, AuthorDto>();

        }
    }
}
