using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace BookStore.Filters
{
    public class BookPublisherAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var svc = context.HttpContext.RequestServices;
            var _bookRepository = svc.GetService<IBaseRepository<Book>>();

            // TODO Handle casting error
            string bookId = (string) context.RouteData.Values["bookId"];
            string userId = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "userId")?.Value;


            if (bookId == null)
            {
                context.Result = new BadRequestResult();
                return;
            }
            else if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var book = _bookRepository?.GetByIdAsync(bookId)?.Result;
            if(book == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            context.Result = userId != book.PublisherId ? new UnauthorizedResult() : null;
            return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        
    }
}
