using System;
using System.Linq;

using Fordere.RestService.Entities.Forum;

using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages.Forum;

using ServiceStack;
using ServiceStack.OrmLite;

namespace Fordere.RestService
{
    
    public class ForumService : BaseService
    {
        public object Get(GetAllThreadsRequest request)
        {
            var allThreads = Db.LoadSelect<ForumThread>().ToList();

            // TODO: Is there really no better option to do this?
            foreach (ForumThread thread in allThreads)
            {
                foreach (ForumPost post in thread.ForumPosts)
                {
                    Db.LoadReferences(post);
                }
            }

            return allThreads.Select(thread => thread.ToDto()).OrderByDescending(x => x.LastActivityDate);
        }

        public object Get(GetPostsOfThreadRequest request)
        {
            var allPosts = Db.LoadSelect<ForumPost>(sql => sql.ForumThreadId == request.ForumThreadId);
            return allPosts.Select(post => post.ConvertTo<ForumPostDto>());
        }

        [Authenticate]
        public object Post(CreatePostRequest request)
        {
            var post = request.ConvertTo<ForumPost>();
            post.Date = DateTime.Now;
            post.UserAuthId = this.SessionUserId;

            var id = Db.Insert(post, true);

            var insertedPost = Db.LoadSingleById<ForumPost>(id);
            return insertedPost.ConvertTo<ForumPostDto>();
        }

        [Authenticate]
        public object Post(CreateThreadRequest request)
        {
            var thread = request.ConvertTo<ForumThread>();
            var threadId = Db.Insert(thread, true);

            return Post(new CreatePostRequest { ForumThreadId = (int)threadId, Text = request.Text });
        }

        public object Get(GetSingleThreadRequest request)
        {
            return Db.Single<ForumThread>(sql => sql.Id == request.Id).ConvertTo<ForumThread>();
        }
    }
}
