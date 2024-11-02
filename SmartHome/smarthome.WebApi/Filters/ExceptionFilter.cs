using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Domain.Exceptions;

namespace smarthome.WebApi.Filters
{
    public sealed class ExceptionFilter : Attribute, IExceptionFilter
    {
        private static readonly Dictionary<Type, Func<Exception, ObjectResult>> _errors = new Dictionary<Type, Func<Exception, ObjectResult>>
        {
            {
                typeof(ArgumentNullException), (Exception exception) => 
                {   var concreteException = (ArgumentNullException)exception;

                    return new ObjectResult(new
                    {
                        InnerCode = "ArgumentNull",
                        Message = $"Argument can not be null or empty. {concreteException.Message}"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
            },
            {
                typeof(ArgumentException), (Exception exception) =>
                {   var concreteException = (ArgumentException)exception;
                               
                    return new ObjectResult(new
                    {
                        InnerCode = "ArgumentError",
                        Message = $"Argument is not valid. {concreteException.Message}"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
            },
            {
                typeof(ForbiddenException), (Exception exception) =>
                {   var concreteException = (ForbiddenException)exception;

                    return new ObjectResult(new
                    {
                    InnerCode = "Forbidden",
                    Message = $"Missing permission. {concreteException.Message}"
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
                }
            },
            {
                typeof(EntityNotFoundException), (Exception exception) =>
                {   var concreteException = (EntityNotFoundException)exception;

                    return new ObjectResult(new
                    {
                    InnerCode = "NotFound",
                    Message = $"Entity not found. {concreteException.Message}"
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
                }
            },
            {
                typeof(ServiceException), (Exception exception) =>
                {   var concreteException = (ServiceException)exception;

                    return new ObjectResult(new
                    {
                    InnerCode = "ServiceException",
                    Message = $"Exception thrown in Service. {concreteException.Message}"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                }
            },
            {
                typeof(ControllerException), (Exception exception) =>
                {   var concreteException = (ControllerException)exception;

                    return new ObjectResult(new
                    {
                    InnerCode = "ControllerException",
                    Message = $"Exception thrown in Controller. {concreteException.Message}"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                }
            }
        };

        public void OnException(ExceptionContext context)
        {
            var  response = _errors.GetValueOrDefault(context.Exception.GetType());
            if (response == null)
            {
                context.Result = new ObjectResult(new
                {
                    InnerCode = "InternalError",
                    Message = "There was an error when processing the request"
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return;
            }
            context.Result = response(context.Exception);
        }
    }
}