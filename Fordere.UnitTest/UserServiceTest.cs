using Fordere.ServiceInterface.Messages.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fordere.UnitTest
{
    [TestClass]
    public class UserServiceTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceStarter.EnsureServiceIsRunning();
        }

        [TestMethod]
        public void GetUserProfile()
        {
            var client = ClientHelper.GetClient();
            var request = new GetUserProfileRequest { Id = 1336 };

            var response = client.Get(request);
            Assert.IsNotNull(response.Email);
            Assert.IsNotNull(response.FirstName);
            Assert.IsNotNull(response.LastName);
            Assert.IsNotNull(response.CreatedDate);
        }

        [TestMethod]
        public void UpdateUserProfileNoPassword()
        {
            var client = ClientHelper.GetClient();
            var request = new UpdateUserProfileRequest { Id = 1336, Email = "oliver.zuercher@gmail.com", FirstName = "Oliver2", LastName = "Zürcher2" };

            var response = client.Put(request);
            Assert.AreEqual(request.Email, response.Email);
            Assert.AreEqual(request.FirstName, response.FirstName);
            Assert.AreEqual(request.LastName, response.LastName);
        }

        [TestMethod]
        public void UpdateUserProfileWithPassword()
        {
            var client = ClientHelper.GetClient();
            var request = new UpdateUserProfileRequest { Id = 1336, Email = "oliver.zuercher@gmail.com", FirstName = "Oliver2", LastName = "Zürcher2", Password = "blub" };

            var response = client.Put(request);
            Assert.AreEqual(request.Email, response.Email);
            Assert.AreEqual(request.FirstName, response.FirstName);
            Assert.AreEqual(request.LastName, response.LastName);
        }

        [TestMethod]
        public void GetAllPossiblePartners()
        {
            var client = ClientHelper.GetClient();
            var request = new GetAllPossiblePartners { CompetitionId = 2};

            var response = client.Get(request);
        }

        //[TestMethod]
        //public void EmailTakenTest()
        //{
        //    // TODO: bug in servicestack breaks this test
        //    var client = ClientHelper.GetClient();

        //    var request = new EmailTakenRequest { Email = "oliver.zuercher@gmail.com" };

        //    var response = client.Head(request);
        //    var response2 = client.Get(request);
        //    client.Head(request);

        //    Assert.AreSame(HttpStatusCode.OK, response.StatusCode);
        //}

        //[TestMethod]
        //public void EmailNotTakenTest()
        //{
        //    // TODO: bug in servicestack breaks this test
        //    var client = ClientHelper.GetClient();

        //    var request = new EmailTakenRequest { Email = "gibts@nicht.com" };
        //    var response = client.Head(request);

        //    Assert.AreSame(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[TestMethod]
        //public void AddUser()
        //{
        //    var client = ClientHelper.GetClient();

        //    var request = new AddUserRequest
        //        {
        //            EMail = "blubbi@test.com",
        //            FirstName = "First Name",
        //            HasPaid = true,
        //            JoinDate = new DateTime(1986, 1, 1),
        //            LastName = "Last Name",
        //            Password = "pass",
        //            Phone = "1234567890"
        //        };

        //    int id = 0;

        //    client.ResponseFilter = delegate(HttpWebResponse webResponse)
        //    {
        //        Assert.AreEqual(HttpStatusCode.Created, webResponse.StatusCode);
        //        var location = webResponse.GetResponseHeader("Location");

        //        Assert.IsFalse(string.IsNullOrEmpty(location));

        //        id = location.Split('/').Last().StripIdPrefix();
        //    };

        //    client.Post(request);

        //    client.ResponseFilter = null;

        //    Assert.AreNotEqual(0, id);

        //    var user = client.Get(new GetUserByIdRequest { Id = id });

        //    Assert.IsNotNull(user, "expected user not returned");
        //    Assert.AreEqual("users/" + id, user.Id);
        //    Assert.AreEqual(request.EMail, user.EMail);
        //    Assert.AreEqual(request.FirstName, user.FirstName);
        //    Assert.AreEqual(request.LastName, user.LastName);
        //    Assert.AreEqual(request.Phone, user.Phone);
        //}

        //[TestMethod]
        //public void AddUsersFailsWhenEMailExists()
        //{
        //    try
        //    {
        //        var client = ClientHelper.GetClient();

        //        var request = new AddUserRequest
        //        {
        //            EMail = "oliver.zuercher@gmail.com",
        //            FirstName = "First Name",
        //            HasPaid = true,
        //            JoinDate = new DateTime(1986, 1, 1),
        //            LastName = "Last Name",
        //            Password = "pass",
        //            Phone = "1234567890"
        //        };

        //        client.Post(request);

        //        Assert.Fail("Post should throw a WebServiceException");
        //    }
        //    catch (WebServiceException exception)
        //    {
        //        Assert.AreEqual((int)HttpStatusCode.Conflict, exception.StatusCode);
        //    }
        //}

        //[TestMethod]
        //public void EditUserFailsWhenEMailExists()
        //{
        //    try
        //    {
        //        var client = ClientHelper.GetClient();

        //        var request = new UpdateUserRequest
        //        {
        //            Id = 1,
        //            EMail = "opendix@gmail.com",
        //            FirstName = "First Name",
        //            HasPaid = true,
        //            JoinDate = new DateTime(1986, 1, 1),
        //            LastName = "Last Name",
        //            Password = "pass",
        //            Phone = "1234567890"
        //        };

        //        client.Put(request);

        //        Assert.Fail("Put should throw a WebServiceException");
        //    }
        //    catch (WebServiceException exception)
        //    {
        //        Assert.AreEqual((int)HttpStatusCode.Conflict, exception.StatusCode);
        //    }
        //}

        //[TestMethod]
        //public void EditUser()
        //{
        //    var client = ClientHelper.GetClient();

        //    var request = new UpdateUserRequest
        //    {
        //        Id = 5,
        //        EMail = "chefkoch37@gmail.com",
        //        FirstName = "First Name",
        //        HasPaid = true,
        //        LastName = "Last Name",
        //        Phone = "1234567890"
        //    };

        //    client.Put(request);

        //    var updatedUser = client.Get(new GetUserByIdRequest { Id = 5 });

        //    Assert.AreEqual(request.EMail, updatedUser.EMail);
        //    Assert.AreEqual(request.FirstName, updatedUser.FirstName);
        //    Assert.AreEqual(request.HasPaid, updatedUser.HasPaid);
        //    Assert.AreEqual(request.LastName, updatedUser.LastName);
        //    Assert.AreEqual(request.Phone, updatedUser.Phone);
        //}

        //[TestMethod]
        //public void ById()
        //{
        //    var client = ClientHelper.GetClient();

        //    var user = client.Get(new GetUserByIdRequest { Id = 1 });

        //    Assert.IsNotNull(user);
        //    Assert.AreEqual("users/1", user.Id);
        //}

        //[TestMethod]
        //public void List()
        //{
        //    var client = ClientHelper.GetClient();

        //    var request = new GetAllUsersRequest();

        //    var response = client.Get(request);

        //    var user1 = response.Users.FirstOrDefault(p => p.Id == "users/1");
        //    var user2 = response.Users.FirstOrDefault(p => p.Id == "users/2");

        //    Assert.IsNotNull(user1, "Seeded user not found with id 'users/1'");
        //    Assert.IsNotNull(user2, "Seeded user not found with id 'users/2'");
        //}
    }
}
