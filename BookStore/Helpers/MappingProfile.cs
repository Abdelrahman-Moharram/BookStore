using AutoMapper;
using BookStore.DTOs.Author;
using BookStore.DTOs.Book;
using BookStore.DTOs.Category;
using BookStore.Models;

namespace BookStore.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, AddBookDTO>().ReverseMap();
            CreateMap<Book, UpdateBookDTO>().ReverseMap();


            CreateMap<Book, BookDTO>()
                .ForMember(dest=>dest.BookId, src=>src.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.BookName, src=>src.MapFrom(src=>src.Name))
                .ForPath(dest=>dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForPath(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.UserName))
                .ForPath(dest => dest.CategoryName, opt => opt.MapFrom(src => (src.Category)))
                .ReverseMap();

            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();

        }

    }
}
