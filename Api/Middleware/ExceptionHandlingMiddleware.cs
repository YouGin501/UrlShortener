using Domain.Exceptions;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError("=====================================");
			_logger.LogError(ex, "An unhandled exception occurred.");
			_logger.LogError("=====================================");

			int statusCode;
			string[] errors;

			switch (ex)
			{
				case UnauthorizedAccessException:
					statusCode = StatusCodes.Status401Unauthorized;
					errors = ["Invalid credentials"];
					break;

				case ArgumentException:
				case InvalidOperationException:
				case ApplicationException:
					statusCode = StatusCodes.Status400BadRequest;
					errors = ["Incorrect input"];
					break;

				case KeyNotFoundException:
					statusCode = StatusCodes.Status404NotFound;
					errors = ["Not found"];
					break;

				case CustomException:
					statusCode = StatusCodes.Status400BadRequest;
					errors = ["Wrong Input"]; // TODO 
					break;

				default:
					statusCode = StatusCodes.Status500InternalServerError;
					errors = ["An internal server error occurred"];
					break;
			}

			await HandleExceptionAsync(context, statusCode, errors);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, int statusCode, string[] errors)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;

		var response = new
		{
			errors
		};

		return context.Response.WriteAsJsonAsync(response);
	}
}