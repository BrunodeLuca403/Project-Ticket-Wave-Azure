using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketAzure.application
{
    public class Result<T>
    {
        public bool Success;

        public T? Value;

        public List<string>? Errors;

        public ErrorType ErrorType;

        public static Result<T> Fail(Error error)
        {
            return new Result<T>
            {
                Success = false,
                Errors = [error.Message],
                ErrorType = error.Type
            };
        }

        public static Result<T> Fail(List<string> errors)
        {
            return new Result<T>
            {
                Success = false,
                Errors = errors,
                ErrorType = ErrorType.Invalid
            };
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>
            {
                Success = true,
                Value = value
            };
        }

        public TResult Match<TResult>(
            Func<T, TResult> onSuccess,
            Func<(ErrorType ErrorType, List<string>? Errors), TResult> onFailure)
        {
            if (!Success)
            {
                return onFailure((ErrorType, Errors));
            }

            return onSuccess(Value ?? default!);
        }
    }

    public abstract class Error
    {
        public string Message { get; set; }
        public ErrorType Type { get; set; }
        public Error(string message, ErrorType type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum ErrorType
    {
        NotFound,
        Invalid,
        Unauthorized,
        Forbidden,
        Conflict,
        Problem,
        InternalServerError
    }
}

