using AvocoBackend.Data.Models;
using AvocoBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvocoBackend.Repository
{
	public static class DbInitializer
	{
		public static void Seed(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();
			if (!context.Interests.Any())
			{
				context.Interests.AddRange(
					new Interest { InterestName = "Filmy" },
					new Interest { InterestName = "Zwierzęta" },
					new Interest { InterestName = "Gotowanie" },
					new Interest { InterestName = "Fotografia" },
                    new Interest { InterestName = "Wojskowosc" },
                    new Interest { InterestName = "Muzyka" },
                    new Interest { InterestName = "Programiowanie" },
                    new Interest { InterestName = "Podroze" },
                    new Interest { InterestName = "Motoryzacja" },
                    new Interest { InterestName = "Zielarstwo" },
                    new Interest { InterestName = "Bieganie" });

                context.SaveChanges();
			}
			if (!context.Groups.Any())
			{
				context.Groups.AddRange(
                    new Group { GroupName = "Gotowanie w lesie" },
                    new Group { GroupName = "Biegacze w Olsztynie" },
                    new Group { GroupName = "Poczatkujacy programisci" },
                    new Group { GroupName = "Fotografowie przyrody" });
                context.SaveChanges();
			}
			if (!context.UsersInterests.Any())
			{
				var ui = new UserInterest
				{
					Interest = context.Interests.First(i => i.Id == 1),
					User = context.Users.First(u => u.Id == 6)
				};
				context.UsersInterests.Add(ui);
				context.SaveChanges();
			}
			if (!context.Friends.Any())
			{
				context.Friends.Add(new Friend { User1Id = 6, User2Id = 1 });
				context.Friends.Add(new Friend { User1Id = 6, User2Id = 8 });
			}
			context.SaveChanges();
			if (!context.GroupsJoinedUsers.Any())
			{
                context.GroupsJoinedUsers.AddRange(
                    new GroupJoinedUser { UserId = 1, GroupId = 17 }, //ty nie wiem czy tutaj juz nie lepiej na sztywno bylo Id wpisywac xDD no teraz chyba lepiej
                    new GroupJoinedUser { UserId = 6, GroupId = 17 },
                    new GroupJoinedUser { UserId = 6, GroupId = 12 },
                    new GroupJoinedUser { UserId = 6, GroupId = 13 },
                    new GroupJoinedUser { UserId = 6, GroupId = 14 },
                    new GroupJoinedUser { UserId = 6, GroupId = 15 });
				context.SaveChanges();
			}
			if (!context.Events.Any())
			{
                var group = new Group { GroupName = "Militaria", GroupDescription = "Grupa zrzeszająca miłośników militarii, wojska, bronii oraz szeroko pojętego survivalu.", GroupPicture = "Images\\Groups\\1.png" };
                context.Events.Add(new Event { Group = group, EventName = "ASG w terenie", EventDescription = "Strzelanie z kulek do ludzi", EventDateTime = new DateTime(2018, 5, 29, 16, 0, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Defilada", EventDateTime = new DateTime(2018, 7, 17, 12, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Pokaz broni", EventDateTime = new DateTime(2018, 8, 1, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Maraton filmów wojennych", EventDateTime = new DateTime(2018, 7, 11, 20, 0, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Maraton filmów wojennych 2", EventDateTime = new DateTime(2018, 7, 12, 20, 0, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Amatorski trening polowy", EventDateTime = new DateTime(2018, 6, 19, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Spotkanie z weteranami", EventDateTime = new DateTime(2018, 8, 20, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Strzelanie do kaczek", EventDateTime = new DateTime(2018, 7, 19, 7, 00, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Strzelanie do dzików", EventDateTime = new DateTime(2018, 9, 8, 7, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Wyscigi czołgów", EventDateTime = new DateTime(2018, 6, 11, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Wspólne ostrzenie bagnetów", EventDateTime = new DateTime(2018, 8, 11, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { Group = group, EventName = "Wywiad z członkiem SWAT", EventDateTime = new DateTime(2018, 8, 16, 10, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                /*context.Events.Add(new Event { GroupId = 17, EventName = "Maraton filmowy", EventDateTime = new DateTime(2018, 6, 21, 22, 20, 0), EventLocationLat = 53.477367, EventLocationLng = 20.284959 });
                context.Events.Add(new Event { GroupId = 17, EventName = "Szkolenie z pythona", EventDateTime = new DateTime(2018, 7, 11, 11, 0, 0), EventLocationLat = 53.977367, EventLocationLng = 20.384959 });
                context.Events.Add(new Event { GroupId = 17, EventName = "Sesja zdjeciowa", EventDateTime = new DateTime(2018, 6, 1, 8, 0, 0), EventLocationLat = 53.577367, EventLocationLng = 20.584959 });
                context.Events.Add(new Event { GroupId = 17, EventName = "Bieg wokol jeziora", EventDateTime = new DateTime(2018, 7, 20, 14, 0, 0), EventLocationLat = 52.777367, EventLocationLng = 21.484959 });
                context.Events.Add(new Event { GroupId = 17, EventName = "Wspolne granie na stadionie", EventDateTime = new DateTime(2018, 7, 11, 20, 0, 0), EventLocationLat = 53.447367, EventLocationLng = 20.224959 });
                context.Events.Add(new Event { GroupId = 17, EventName = "Prelekcja o ziolach bagiennych", EventDateTime = new DateTime(2018, 6, 20, 13, 0, 0), EventLocationLat = 53.677367, EventLocationLng = 20.784959 });*/
                context.SaveChanges();
            }
            if (!context.PostsComments.Any())
            {
				var post1 = new Post { GroupId = 17, UserId = 3, Content = "Hej :) Ostatnio zainteresowałem się okresem II WŚ. Mogłby ktoś polecic mi jakieś materiały na ten temat? Moze jakieś ksiażki?" };
				context.PostsComments.Add(new PostComment { Post = post1, UserId = 1, Content = "Polecam ksiaże: 'My Battle'. Jest bardzo dokładna i szczegółowa. Na pewno nie pożałujesz:)" });
                var post2 = new Post { GroupId = 17, UserId = 6, Content = "Witam zapaleńców militarii! Ostatnio na posiedzeniu wpadłem na genialny pomysł! Postanowiłem stworzyć petycje o dostęp do bronii dla 60+ latków. Dzięki temu starsi ludzie nie mieliby juz problemu z opryskliwą młodzieżą która nie chce im ustepować miejsc w komunikacji miejskiej. Dodatkowo, lunety snajperskie mogłyby pomagać babciom w przeglądaniu terenu. Kto wie, może jakaś starsza pani obezwładniła by jakieś przestepce dzieki swojemu AWP :) Piszie co o tym myślicie milki" };
                context.PostsComments.Add(new PostComment { Post = post2, UserId = 1, Content = "To najgłupszy pomysł o jakim słyszałem. Pozdrawiam" });
                context.PostsComments.Add(new PostComment { Post = post2, UserId = 6, Content = "Masz ty rozum i godność człowieka??? Bój sie Boga takie rzeczy pisać. WSTYD!!!!!" });
                var post3 = new Post { GroupId = 17, UserId = 3, Content = "Wybiera sie ktos na defilade do Olsztyna? Mój przyjaciel w ostatniej chwili niestety zrezygnował i teraz nie mam z kim iść, a z kimś zawsze weselej. Chetni niech piszą w komentarzach, może zbierzemy jakaś grupe na wspólny wypad!" };
                context.PostsComments.Add(new PostComment { Post = post3, UserId = 6, Content = "Z chęcią wybiorę sie z miłą Panią na defilade! Całuje rączki i już szykuje mojego passata na wyjazd!" });
                context.PostsComments.Add(new PostComment { Post = post3, UserId = 1, Content = "Ja też chetnie się wybiore!" });
                context.PostsComments.Add(new PostComment { Post = post3, UserId = 6, Content = "Halo halo! Ty młody to jeszcze całe zycie przed tobą to sie jeszcze nachodzisz. Ja tu sie wybieram z Panią sam!" });
                context.PostsComments.Add(new PostComment { Post = post3, UserId = 1, Content = "Przepraszam panie Janie, ale słyszałem ze pani Joanna jest meżata wiec chyba nici z wspólnego spotkania:)" });
                context.PostsComments.Add(new PostComment { Post = post3, UserId = 6, Content = "Dobra młody nie interesuj sie. Dorosniesz to zmądrzejesz hehe" });
                context.SaveChanges();
            }
		}
	}
}

// id sie zmieniaja przy kazdym seedzie wiec gl, nie wiem jak teraz to zrobic jak sie moga zmieniac skoro w bazie jest zapisane tak i tak i zaden kurwa inny 
// bowiem jak cos usuniesz  z tabeli i potem znowu dodasz to id jest wieksze
//kurwa caly seed poszedl w pizdu w takim razie wtf
//bo nie mozemy sie do groupid i userid odwolywac