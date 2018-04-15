using AvocoBackend.Data.DTOs;
using Microsoft.AspNetCore.Http;

namespace AvocoBackend.Services.Interfaces
{
	public interface IUserService
	{
		ServiceResult<UserDTO> GetUserInfo(int userId);
		ServiceResult<UserDTO> SetUserInfo(UserDTO userInfo, HttpContext httpContext);
		ServiceResult<InterestDTO[]> GetInterests(int userId);
		ServiceResult<InterestDTO[]> SearchInterests(string searchText);
		ServiceResult<InterestDTO> AddInterest(HttpContext httpContext, int? interestId, string interestName);
		ServiceResult<GroupDTO[]> GetGroups(int userId);
		ServiceResult<UserDTO[]> GetFriends(HttpContext httpContext);
		ServiceResult<UserDTO[]> AddFriend(int user2Id, HttpContext httpContext); // chyba zwraca wszystkich znajomych
		ServiceResult<UserDTO[]> Unfriend(int user2Id, HttpContext httpContext); // j.w.
		//jeszcze zdjecia, raczej rozdzielone miedzy ten service i oddzielny
 	}
}
