using System;
using System.Collections.Generic;
using System.Text;

namespace AvocoBackend.Data.DTOs
{
    public class TokenDTO
    {
		public string Token { get; set; }

		public TokenDTO(string token)
		{
			Token = token;
		}
	}
}
