using BookStore.Constants;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace BookStore.Filters
{
    public class BookPublisherOrAdminAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var val = context.HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Role).ToList().Where(i=>i.Value == Roles.SuperAdmin.ToString()).ToList();
            if (val.Count() > 0)
                return;

            var svc = context.HttpContext.RequestServices;
            var _bookRepository = svc.GetService<IBaseRepository<Book>>();

            // TODO Handle casting error
            string bookId = (string) context.RouteData.Values["bookId"];
            var vl = context.HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Role).ToList();
            
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
            if(userId != book.PublisherId)
            {
                context.Result =  new UnauthorizedResult() ;
            }
            return;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        
    }
}
