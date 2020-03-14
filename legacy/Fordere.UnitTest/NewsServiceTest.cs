//using System;
//using System.Linq;
//using System.Net;
//using Fordere.Common.Pcl.Dtos;
//using Fordere.RestService.Extensions;
//using Fordere.ServiceInterface.Messages.News;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Fordere.UnitTest
//{
//    [TestClass]
//    public class NewsServiceTest
//    {
//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            ServiceStarter.EnsureServiceIsRunning();
//        }           

//        [TestMethod]
//        public void AddNews()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new AddNewsRequest
//            {
//                Title = "some title here",
//                Content = "some content here",
//                IsPublished = true,
//                PostDate = new DateTime(2000, 2, 3),
//                UserId = "users/1"
//            };

//            var id = 0;
      
//            client.ResponseFilter = delegate(HttpWebResponse webResponse)
//            {
//                Assert.AreEqual(HttpStatusCode.Created, webResponse.StatusCode);
//                var location = webResponse.GetResponseHeader("Location");

//                Assert.IsFalse(string.IsNullOrEmpty(location));

//                id = location.Split('/').Last().StripIdPrefix();
//            };

//            client.Post(request);

//            client.ResponseFilter = null;

//            Assert.AreNotEqual(0, id);

//            var news = client.Get(new GetNewsByIdRequest { Id = id });

//            Assert.IsNotNull(news, "expected news not returned");
//            Assert.AreEqual(request.Content, news.Content);
//            Assert.AreEqual(request.Title, news.Title);
//            Assert.AreEqual(request.Content, news.Content);
//        }

//        [TestMethod]
//        public void EditNews()
//        {
//            var client = ClientHelper.GetClient();
            
//            var request = new UpdateNewsRequest
//            {
//                Id = "news/2",
//                Title = "some title here3",
//                Content = "some content here3",
//                IsPublished = true,
//                PostDate = new DateTime(2000, 2, 3),
//                User = new NameDto { Id = "users/1" }
//            };

//            client.Put(request);

//            var news = client.Get(new GetNewsByIdRequest { Id = 2 });

//            Assert.IsNotNull(news, "expected news not returned");
//            Assert.AreEqual(request.Content, news.Content);
//            Assert.AreEqual(request.Title, news.Title);
//            Assert.AreEqual(request.Content, news.Content);
//        }

//        [TestMethod]
//        public void NewsById()
//        {
//            var client = ClientHelper.GetClient();

//            var request = new GetNewsByIdRequest
//                {
//                    Id = 1
//                };

//            var news = client.Get(request);

//            Assert.AreEqual("news/1", news.Id);
//        }

//        [TestMethod]
//        public void NewsList()
//        {
//            var client = ClientHelper.GetClient();

//            var response = client.Get(new GetAllNewsRequest());

//            var news1 = response.FirstOrDefault(p => p.Id == "news/1");
//            var news2 = response.FirstOrDefault(p => p.Id == "news/2");

//            Assert.IsNotNull(news1, "Seeded news not found with id 'news/1'");
//            Assert.IsNotNull(news2, "Seeded news not found with id 'news/2'");
//        }

//        [TestMethod]
//        public void NewsWithLimit()
//        {
//            var client = ClientHelper.GetClient();

//            var response = client.Get(new GetAllNewsRequest{Limit = 3});

//            Assert.AreEqual(3, response.Count, "Limit did not work for news");
//        }
//    }
//}
