using System;
using System.Configuration;
using System.Diagnostics;
using NUnit.Framework;
using ServiceStack;

namespace OpsGenieApi.UnitTests
{
    [TestFixture]
    public class OpsGenieClientTests
    {

        private OpsGenieClient CreateClient()
        {
            return new OpsGenieClient(new OpsGenieClientConfig
            {
                ApiKey = ConfigurationManager.AppSettings["OpsGenieApiKey"],
                ApiUrl = "https://api.opsgenie.com/v1/json/alert"
            });
        }


        [Test]
        public void CheckSetting()
        {
            var apikey = ConfigurationManager.AppSettings["OpsGenieApiKey"];

            Assert.IsNotNullOrEmpty(apikey,"Please set apikey in config");
            Assert.That(apikey, Is.Not.SameAs("Your-Secret-Api-Key"), "Please replace default api key");
        }

        [Test]
        public void GetLast()
        {
            var client = CreateClient();

            var response = client.GetLastOpenAlerts();
            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);
        }


        [Test]
        public void Raise()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = new Guid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);
        }


        [Test]
        public void CloseByAlertId()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = Guid.NewGuid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);

            var responseClose = client.Close(response.AlertId, null, "Closing alert, all ok");

            Trace.WriteLine(responseClose.ToJson());
            Assert.IsTrue(responseClose.Ok);

        }

        [Test]
        public void CloseByAlias()
        {
            var client = CreateClient();

            string alias = Guid.NewGuid().ToString();
            var response = client.Raise(new Alert
            {
                Alias = alias,
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);

            var responseClose = client.Close(null, alias, "Closing alert, all ok");

            Trace.WriteLine(responseClose.ToJson());
            Assert.IsTrue(responseClose.Ok);

        }



        [Test]
        public void AckAndClose()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = Guid.NewGuid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok, "Should succesfully create new alert");

            var responseAck = client.Acknowledge(response.AlertId, null, "Ack, working on it");
            
            Trace.WriteLine(responseAck.ToJson());
            Assert.IsTrue(responseAck.Ok, "Should succesfully ackowledge");

            var responseClose = client.Close(response.AlertId, null, "Closing alert, all ok");

            Trace.WriteLine(responseClose.ToJson());
            Assert.IsTrue(responseClose.Ok, "Should succesfully close");

        }


        public void x()
        {
            var opsClient = new OpsGenieClient(new OpsGenieClientConfig
            {
                ApiKey = ".. your api key",
                ApiUrl = "https://api.opsgenie.com/v1/json/alert"
            });

            var response = opsClient.Raise(new Alert {Alias = "alert2", Source = "Test", Message = "All systems down"});

            if (response.Ok)
            {
                var respAck =  opsClient.Acknowledge(response.AlertId, null, "Working on it!");

                var respClose = opsClient.Close(response.AlertId, null, "Fixed by ..");
            }
        }
    }
}
