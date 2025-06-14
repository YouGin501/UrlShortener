using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Base.Handlers
{
	public abstract class BaseHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
		where TRequest : IRequest<TResult>
	{
		protected readonly ILogger _logger;
		private readonly string _name;
		protected virtual bool ShouldLogInput => true;
		protected virtual bool ShouldLogOutput => true;

		protected BaseHandler(ILogger logger)
		{
			_logger = logger;
			_name = GetType().Name;
		}

		public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Start to execute {Name}. Input: {Request}", _name, ShouldLogInput ? FormatJson(request) : "[Redacted]");

				TResult result = await HandleInternal(request, cancellationToken);

				_logger.LogInformation("Executed {name}. Output: {result}", _name, ShouldLogInput ? FormatJson(result) : "[Redacted]");

				return result;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Exception raised. Handler: {name}. Request: {request}", _name, FormatJson(request));
				throw;
			}
		}

		protected abstract Task<TResult> HandleInternal(TRequest request, CancellationToken cancellationToken);

		private string FormatJson(object obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}

			try
			{
				var options = new JsonSerializerOptions
				{
					WriteIndented = true
				};

				return JsonSerializer.Serialize(obj, options);
			}
			catch
			{
				return obj.ToString() ?? string.Empty;
			}
		}
	}
}