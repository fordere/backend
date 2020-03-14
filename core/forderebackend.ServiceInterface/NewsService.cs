using System.Collections.Generic;
using System.Linq;
using System.Net;

using Fordere.RestService.Entities;
using Fordere.RestService.Extensions;
using Fordere.RestService.Properties;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.News;

using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    
    public class NewsService : BaseService
    {
        public object Get(GetAllNewsRequest request)
        {
            var newsQuery = Db.From<News>().Where(x => x.DivisionId == DivisionId || x.DivisionId == null).OrderByDescending(p => p.PostDate);

            if (this.IsAdmin == false)
            {
                newsQuery = newsQuery.Where(p => p.IsPublished);
            }

            if (request.PagingRequested)
            {
                newsQuery = newsQuery.Limit(request.Offset, request.PageSize);
            }

            var newsEntities = this.Db.Select(newsQuery);

            var users = this.Db.SelectByIds<UserAuth>(newsEntities.Select(s => s.UserAuthId)).ToDictionary(k => k.Id);

            var dtos = new List<NewsDto>(newsEntities.Count);

            foreach (var news in newsEntities)
            {
                if (string.IsNullOrEmpty(news.Summary))
                {
                    // TODO use Humanizer
                    news.Summary = news.Content.Substring(0, 200);  //news.Content.Truncate(30, Truncator.FixedNumberOfWords);
                }

                var dto = news.ConvertTo<NewsDto>();
                dto.User = users[news.UserAuthId].ToDto();

                dtos.Add(dto);
            }

            return dtos;
        }

        public object Get(GetNewsByIdRequest request)
        {
            var news = this.Db.SingleById<News>(request.Id);

            if (news.IsPublished == false && this.IsAdmin == false)
            {
                news = null;
            }

            news.Throw404NotFoundIfNull("News not found");

            var author = this.Db.SingleById<UserAuth>(news.UserAuthId);

            var dto = news.ConvertTo<NewsDto>();
            dto.User = author.ToDto();

            return dto;
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateNewsRequest request)
        {
            var news = this.Db.SingleById<News>(request.Id);

            news.Throw404NotFoundIfNull("News not found");

            news.PopulateWith(request);
            news.UserAuthId = request.User.Id;

            this.Db.Update(news);

            return this.Get(new GetNewsByIdRequest { Id = request.Id });
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddNewsRequest request)
        {
            var news = request.ConvertTo<News>();

            var id = (int)this.Db.Insert(news, true);

            var dto = this.Get(new GetNewsByIdRequest { Id = id });

            return new HttpResult(dto, HttpStatusCode.Created)
            {
                Location = this.Request.AbsoluteUri + "/news/" + id
            };
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Patch(UpdateIsPublishedRequest request)
        {
            var news = this.Db.SingleById<News>(request.Id);

            news.Throw404NotFoundIfNull("News not found");

            this.Db.Update<News>(new { request.IsPublished }, p => p.Id == request.Id);

            return new HttpResult(null, HttpStatusCode.OK);
        }
    }
}