using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Services
{
    public class ServiceResult<T>
    {
		public T SuccessResult { get; set; }
		public string[] Errors { get; set; }
		public bool IsError { get { return Errors?.Length > 0; } }

		public ServiceResult(T successResult)
		{
			Errors = null;
			SuccessResult = successResult;
		}
		public ServiceResult(params string[] errors)
		{
			Errors = errors;
		}
	}
}
