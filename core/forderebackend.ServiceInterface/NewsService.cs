using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Extensions;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages.News;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;

namespace forderebackend.ServiceInterface
{
    public class NewsService : BaseService
    {
        public object Get(GetAllNewsRequest request)
        {
            var newsQuery = Db.From<News>().Where(x => x.DivisionId == DivisionId || x.DivisionId == null)
                .OrderByDescending(p => p.PostDate);

            if (IsAdmin == false)
            {
                newsQuery = newsQuery.Where(p => p.IsPublished);
            }

            if (request.PagingRequested)
            {
                newsQuery = newsQuery.Limit(request.Offset, request.PageSize);
            }

            var newsEntities = Db.Select(newsQuery);

            var users = Db.SelectByIds<UserAuth>(newsEntities.Select(s => s.UserAuthId)).ToDictionary(k => k.Id);

            var dtos = new List<NewsDto>(newsEntities.Count);

            foreach (var news in newsEntities)
            {
                if (string.IsNullOrEmpty(news.Summary))
                {
                    // TODO use Humanizer
                    news.Summary =
                        news.Content.Substring(0, Math.Min(200, news.Content.Length)); //news.Content.Truncate(30, Truncator.FixedNumberOfWords);
                }

                var dto = news.ConvertTo<NewsDto>();
                dto.User = users[news.UserAuthId].ToDto();

                dtos.Add(dto);
            }

            return dtos;
        }

        public object Get(GetNewsByIdRequest request)
        {
            var news = Db.SingleById<News>(request.Id);

            if (news.IsPublished == false && IsAdmin == false)
            {
                news = null;
            }

            news.Throw404NotFoundIfNull("News not found");

            var author = Db.SingleById<UserAuth>(news.UserAuthId);

            var dto = news.ConvertTo<NewsDto>();
            dto.User = author.ToDto();

            return dto;
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Put(UpdateNewsRequest request)
        {
            var news = Db.SingleById<News>(request.Id);

            news.Throw404NotFoundIfNull("News not found");

            news.PopulateWith(request);
            news.UserAuthId = request.User.Id;

            Db.Update(news);

            return Get(new GetNewsByIdRequest {Id = request.Id});
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Post(AddNewsRequest request)
        {
            var news = request.ConvertTo<News>();

            var id = (int) Db.Insert(news, true);

            var dto = Get(new GetNewsByIdRequest {Id = id});

            return new HttpResult(dto, HttpStatusCode.Created)
            {
                Location = Request.AbsoluteUri + "/news/" + id
            };
        }

        [Authenticate]
        [RequiredRole(RoleNames.Admin)]
        public object Patch(UpdateIsPublishedRequest request)
        {
            var news = Db.SingleById<News>(request.Id);

            news.Throw404NotFoundIfNull("News not found");

            Db.Update<News>(new {request.IsPublished}, p => p.Id == request.Id);

            return new HttpResult(null, HttpStatusCode.OK);
        }
    }
}