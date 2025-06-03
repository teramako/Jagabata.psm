using System.Globalization;
using System.Net;

namespace APITest
{
    public class Util
    {
        public static void DumpResponse(IRestAPIResponse res)
        {
            Console.WriteLine();
            Console.WriteLine("{0} {1} (HTTP {2})", res.Method, res.RequestUri, res.HttpVersion);
            Console.WriteLine("Status: {0:d} {1}", res.StatusCode, res.ReasonPhrase);
            Console.WriteLine("Response Header:");
            foreach (var header in res.ContentHeaders)
            {
                Console.WriteLine("    {0}: {1}", header.Key, string.Join(", ", header.Value));
            }
            foreach (var header in res.ResponseHeaders)
            {
                Console.WriteLine("    {0}: {1}", header.Key, string.Join(", ", header.Value));
            }
            Console.WriteLine("Request Header:");
            if (res.RequestHeaders is null) return;
            foreach (var header in res.RequestHeaders)
            {
                Console.WriteLine("    {0}: {1}", header.Key, string.Join(", ", header.Value));
            }
        }
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        public static void DumpObject(object json)
        {
            Console.WriteLine($"-- {json.GetType().FullName} --");
            Console.WriteLine(JsonSerializer.Serialize(json, jsonSerializerOptions));
            Console.WriteLine("--------------");
        }
        public static void DumpSummary(SummaryFieldsDictionary summary)
        {
            Console.WriteLine("-----SummaryFields-----");
            foreach (var kv in summary)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }

    }
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void Test01DefaultFile()
        {
            var path = ApiConfig.DefaultConfigPath;
            Console.WriteLine(path);
            Assert.IsNotNull(path);
            Assert.IsTrue(File.Exists(path));
        }
        [TestMethod]
        public void Test02DefaultConfig()
        {
            var config = ApiConfig.Instance;
            Assert.IsNotNull(config);
            Console.WriteLine(config.File);
            Console.WriteLine(config.Origin);
            Assert.IsInstanceOfType<Uri>(config.Origin);
            Assert.IsNotNull(config.File);
            Assert.IsNotNull(config.Token);

            config.Save();
            Assert.IsNotNull(config.LastSaved);
            Console.WriteLine(config.LastSaved?.ToString("o"));
        }
        public static readonly DirectoryInfo? projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent;
        [TestMethod]
        public void Test03LoadConfig()
        {
            var file = Path.Join(projectDirectory?.ToString(), ".ansible_psm_config.json");
            Console.WriteLine(file);
            Assert.IsTrue(File.Exists(file));
            var fileInfo = new FileInfo(file);
            var config = ApiConfig.Load(fileInfo);
            Assert.AreSame(fileInfo, config.File);
        }
    }
    [TestClass]
    public class TestApiClient
    {
        public TestApiClient()
        {
            var configFile = Path.Join(ConfigTest.projectDirectory?.ToString(), ".ansible_psm_config.json");
            RestAPI.SetClient(ApiConfig.Load(new FileInfo(configFile)));
        }
        [TestMethod]
        public async Task Error404AsJsonResponse1()
        {
            var ex = await Assert.ThrowsExactlyAsync<RestAPIException>(static () => RestAPI.GetAsync<User>("/api/v2/users/0/"));
            Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            Console.WriteLine(ex.ToString());
            Console.WriteLine("====================");
            Assert.IsNull(ex.InnerException);
            Assert.IsTrue(ex.Message.IndexOf("{\"detail\":", StringComparison.Ordinal) > 0);
            /*
            var ex = await Assert.ThrowsExceptionAsync<RestAPIException>(() => RestAPI.GetAsync<User>("/api/v2/users/0/"));
            var apiResponse = await RestAPI.GetAsync<User>("/api/v2/users/0/");
            Assert.IsFalse(apiResponse.IsSuccess);
            Assert.IsNotNull(apiResponse.Exception);
            Console.WriteLine(apiResponse.Exception.ToString());
            Assert.IsInstanceOfType<RestAPIException>(apiResponse.Exception);
            Assert.IsTrue(apiResponse.Exception.Message.IndexOf("{\"detail\":") > 0);
            */
        }
        [TestMethod]
        public async Task Error404AsHtmlResponse2()
        {
            var ex = await Assert.ThrowsExactlyAsync<RestAPIException>(static () => RestAPI.GetAsync<User>("/404NotFound/"));
            Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            Console.WriteLine(ex.ToString());
            Console.WriteLine("====================");
            Assert.IsNull(ex.InnerException);
            Assert.IsTrue(ex.Message.IndexOf("text/html", StringComparison.Ordinal) > 0);
            /*
            var apiResponse = await RestAPI.GetAsync<User>("/404NotFound/");
            Assert.IsFalse(apiResponse.IsSuccess);
            Assert.IsNotNull(apiResponse.Exception);
            Console.WriteLine(apiResponse.Exception.ToString());
            Assert.IsInstanceOfType<RestAPIException>(apiResponse.Exception);
            Assert.IsTrue(apiResponse.Exception.Message.IndexOf("text/html") > 0);
            */
        }
        [TestMethod]
        public async Task GetJson()
        {
            var apiResult = await RestAPI.GetAsync<Ping>("/api/v2/ping/");
            Assert.IsNotNull(apiResult);
            Assert.AreEqual(HttpStatusCode.OK, apiResult.Response.StatusCode);
            Assert.IsTrue(apiResult.Response.IsSuccessStatusCode);
            Assert.IsTrue(apiResult.Response.ContentLength > 0);
            Assert.AreSame(RestAPI.JsonContentType, apiResult.Response.ContentType);
            var res = apiResult.Contents;
            Assert.IsNotNull(res);
            Assert.IsFalse(res.HA);
            Assert.IsFalse(string.IsNullOrEmpty(res.Version));
            Console.WriteLine($"Version: {res.Version}");
            Assert.IsFalse(string.IsNullOrEmpty(res.ActiveNode));
            Assert.IsFalse(string.IsNullOrEmpty(res.InstallUuid));
            Assert.IsTrue(res.Instances.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(res.Instances[0].Node));
            Assert.IsFalse(string.IsNullOrEmpty(res.Instances[0].NodeType));
            Assert.IsFalse(string.IsNullOrEmpty(res.Instances[0].Uuid));
            Assert.IsFalse(string.IsNullOrEmpty(res.Instances[0].Heartbeat));
            Assert.IsTrue(res.Instances[0].Capacity > 0);
            Assert.IsFalse(string.IsNullOrEmpty(res.Instances[0].Version));
            Assert.IsTrue(res.InstanceGroups.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(res.InstanceGroups[0].Name));
            Assert.IsTrue(res.InstanceGroups[0].Capacity > 0);
            Assert.IsTrue(res.InstanceGroups[0].Instances.Length > 0);
            Assert.IsFalse(string.IsNullOrEmpty(res.InstanceGroups[0].Instances[0]));
            Util.DumpObject(res);
            Util.DumpResponse(apiResult.Response);
        }
        [TestMethod]
        public async Task OptionsJson()
        {
            var apiResult = await RestAPI.OptionsJsonAsync<ApiHelp>("/api/v2/ping/");
            Assert.IsNotNull(apiResult);
            var help = apiResult.Contents;
            Assert.IsNotNull(help);
            Util.DumpObject(help);
            Util.DumpResponse(apiResult.Response);
            Assert.IsInstanceOfType<ApiHelp>(help);
            Assert.IsTrue(help.Name.Length > 0);
            Assert.IsTrue(help.Description.Length > 0);
        }


        [TestMethod]
        public async Task GetText()
        {
            var jobResult = await RestAPI.GetAsync<string>("/api/v2/jobs/4/stdout/?format=txt", AcceptType.Text);
            Assert.IsTrue(jobResult.Response.IsSuccessStatusCode);
            Console.WriteLine(jobResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(RestAPI.TextContentType, jobResult.Response.ContentType);
            Assert.IsInstanceOfType<string>(jobResult.Contents);
            Util.DumpResponse(jobResult.Response);
            Console.WriteLine("----------------");
            Console.WriteLine(jobResult.Contents);
        }
        [TestMethod]
        public async Task GetHtml()
        {
            var jobResult = await RestAPI.GetAsync<string>("/api/v2/jobs/4/stdout/?format=html", AcceptType.Html);
            Assert.IsTrue(jobResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(RestAPI.HtmlContentType, jobResult.Response.ContentType);
            Assert.IsInstanceOfType<string>(jobResult.Contents);
            Util.DumpResponse(jobResult.Response);
            Console.WriteLine("----------------");
            Console.WriteLine(jobResult.Contents);
        }
        [TestMethod]
        public async Task GetResultSet()
        {
            await foreach (var apiResult in RestAPI.GetResultSetAsync<User>("/api/v2/me/"))
            {
                var resultSet = apiResult.Contents;
                Assert.IsNotNull(resultSet);
                Assert.IsNull(resultSet.Previous);
                Assert.IsNull(resultSet.Next);
                Assert.IsTrue(resultSet.Count > 0);
                foreach (var user in resultSet.Results)
                {
                    Assert.IsTrue(user.Id > 0);
                    Assert.AreEqual(ResourceType.User, user.Type);
                    Assert.IsFalse(string.IsNullOrEmpty(user.Username));
                    Util.DumpObject(user);
                }
            }
        }
    }

    [TestClass]
    public class TestActivityStream
    {
        private readonly HttpQuery singleQuery = new("order_by=id&page_size=1");

        private static void DumpResource(ActivityStream a)
        {
            Console.WriteLine($"{a.Id} {a.Type} {a.Timestamp}");
            Console.WriteLine($"Operation: {a.Operation}");
            Console.WriteLine($"Object   : 1:{a.Object1}, 2:{a.Object2}");
        }
        [TestMethod("[ActivityStream] 01 Simple Find and Get")]
        public void Get01FindAndGet()
        {
            var activites = ActivityStream.Find(singleQuery);
            Assert.AreEqual(1, activites.Length);
            var activity = ActivityStream.Get(activites[0].Id);
            Assert.IsInstanceOfType<ActivityStream>(activity);
            Assert.AreEqual(ResourceType.ActivityStream, activity.Type);
            Assert.IsInstanceOfType<ActivityStreamOperation>(activity.Operation);
            DumpResource(activity);
            Util.DumpSummary(activity.SummaryFields);
        }
        [TestMethod("[ActivityStream] 02 List from Application")]
        public void Get02ListFromApplication()
        {
            var app = Application.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{app.Id}][{app.Type}] {app.Name})");
            foreach (var activity in ActivityStream.Find(app))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 03 List from Token")]
        public void Get03ListFromToken()
        {
            var token = OAuth2AccessToken.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{token.Id}][{token.Type}] {token.Description})");
            foreach (var activity in ActivityStream.Find(token))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 04 List from Organization")]
        public void Get04ListFromOrganization()
        {
            var org = Organization.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{org.Id}][{org.Type}] {org.Name})");
            foreach (var activity in ActivityStream.Find(org))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 05 List from User")]
        public void Get05ListFromUser()
        {
            var user = User.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{user.Id}][{user.Type}] {user.Username})");
            foreach (var activity in ActivityStream.Find(user))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 06 List from Project")]
        public void Get06ListFromProject()
        {
            var proj = Project.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{proj.Id}][{proj.Type}] {proj.Name})");
            foreach (var activity in ActivityStream.Find(proj))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 07 List from Team")]
        public void Get07ListFromTeam()
        {
            var team = Team.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{team.Id}][{team.Type}] {team.Name})");
            foreach (var activity in ActivityStream.Find(team))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 08 List from Credential")]
        public void Get08ListFromCredential()
        {
            var cred = Credential.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{cred.Id}][{cred.Type}] {cred.Name})");
            foreach (var activity in ActivityStream.Find(cred))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 09 List from CredentialType")]
        public void Get09ListFromCredentialType()
        {
            var credType = CredentialType.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{credType.Id}][{credType.Type}] {credType.Name})");
            foreach (var activity in ActivityStream.Find(credType))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 10 List from Inventory")]
        public void Get10ListFromInventory()
        {
            var inventory = Inventory.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{inventory.Id}][{inventory.Type}] {inventory.Name})");
            foreach (var activity in ActivityStream.Find(inventory))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 11 List from InventorySource")]
        public void Get11ListFromInventorySource()
        {
            var inventorySource = InventorySource.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{inventorySource.Id}][{inventorySource.Type}] {inventorySource.Name})");
            foreach (var activity in ActivityStream.Find(inventorySource))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 12 List from Group")]
        public void Get12ListFromGroup()
        {
            var group = Group.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{group.Id}][{group.Type}] {group.Name})");
            foreach (var activity in ActivityStream.Find(group))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 13 List from Host")]
        public void Get13ListFromHost()
        {
            var host = Host.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{host.Id}][{host.Type}] {host.Name})");
            foreach (var activity in ActivityStream.Find(host))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 14 List from JobTemplate")]
        public void Get14ListFromJobTemplate()
        {
            var jt = JobTemplate.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{jt.Id}][{jt.Type}] {jt.Name})");
            foreach (var activity in ActivityStream.Find(jt))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 15 List from JobTemplateJob")]
        public void Get15ListFromJobTemplateJob()
        {
            var job = JobTemplateJob.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{job.Id}][{job.Type}] {job.Name})");
            foreach (var activity in ActivityStream.Find(job))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 16 List from AdHocCommand")]
        public void Get16ListFromAdHoCommand()
        {
            var cmd = AdHocCommand.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{cmd.Id}][{cmd.Type}] {cmd.Name})");
            foreach (var activity in ActivityStream.Find(cmd))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 17 List from WorkflowJobTemplate")]
        public void Get17ListFromWorkflowJobTemplate()
        {
            var wjt = WorkflowJobTemplate.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{wjt.Id}][{wjt.Type}] {wjt.Name})");
            foreach (var activity in ActivityStream.Find(wjt))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 18 List from WorkflowJob")]
        public void Get18ListFromWorkflowJob()
        {
            var wjt = WorkflowJob.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{wjt.Id}][{wjt.Type}] {wjt.Name})");
            foreach (var activity in ActivityStream.Find(wjt))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
        [TestMethod("[ActivityStream] 19 List from ExecutionEnvironment")]
        public void Get19ListFromExecutionEnvironment()
        {
            var ee = ExecutionEnvironment.Find(singleQuery).Single();
            Console.WriteLine($"ActivityStream for ([{ee.Id}][{ee.Type}] {ee.Name})");
            foreach (var activity in ActivityStream.Find(ee))
            {
                Assert.IsInstanceOfType<ActivityStream>(activity);
                Console.WriteLine($"[{activity.Timestamp}] {activity.Operation} [{activity.Object1}, {activity.Object2}]");
            }
        }
    }

    [TestClass]
    public class TestApplication
    {
        private readonly HttpQuery singleQuery = new("order_by=id&page_size=1");

        [TestMethod("[Application] 01 Simple Find and Get")]
        public void Get01Single()
        {
            var apps = Application.Find(singleQuery);
            Assert.AreEqual(1, apps.Length);
            Assert.IsInstanceOfType<Application>(apps[0]);
            var app = Application.Get(apps[0].Id);
            Assert.IsInstanceOfType<Application>(app);
            Console.WriteLine($"Id           : {app.Id}");
            Console.WriteLine($"Name         : {app.Name}");
            Console.WriteLine($"Description  : {app.Description}");
            Console.WriteLine($"Created      : {app.Created}");
            Console.WriteLine($"Modified     : {app.Modified}");
            Console.WriteLine($"AuthGrantType: {app.AuthorizationGrantType}");
            Console.WriteLine($"ClientId     : {app.ClientId}");
            Console.WriteLine($"ClientType   : {app.ClientType}");
            Console.WriteLine($"ClientSecret : {app.ClientSecret ?? "(null)"}");
            Console.WriteLine($"Redirect     : {app.RedirectUris}");
            Console.WriteLine($"SkipAuth     : {app.SkipAuthorization}");
            Console.WriteLine($"Organization : {app.Organization}");

            Util.DumpSummary(app.SummaryFields);
        }
        [TestMethod("[Application] 02 List from Organization")]
        public void Get02ListFromOrganization()
        {
            var orgId = Application.Find(new HttpQuery("organization__gt=0&order_by=-id&page_size=1"))
                                   .Single()
                                   .Organization;
            var resource = new Resource(ResourceType.Organization, orgId);
            foreach (Application app in Application.Find(resource))
            {
                Assert.IsInstanceOfType<Application>(app);
                Console.WriteLine($"{app.Id,5:d}: {app.Name} {app.Description}");
            }
        }
        [TestMethod("[Application] 03 List from User")]
        public void Get03ListFromUser()
        {
            var resource = new Resource(ResourceType.User, 1);
            foreach (var app in Application.Find(resource))
            {
                Assert.IsInstanceOfType<Application>(app);
                Console.WriteLine($"{app.Id,5:d}: {app.Name} {app.Description}");
            }
        }
    }

    [TestClass]
    public class TestOAuth2AccessToken
    {
        private static void DumpToken(OAuth2AccessToken token)
        {
            Console.WriteLine($"{token.Id} {token.Token} - {token.Description}");
            Console.WriteLine($"Application : {token.Application}");
            Console.WriteLine($"Socpe       : {token.Scope}");
            Console.WriteLine($"Expires     : {token.Expires}");
            Console.WriteLine($"Created     : {token.Created}");
            Console.WriteLine($"Modified    : {token.Modified?.ToString("o") ?? "(null)"}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var token = await OAuth2AccessToken.GetAsync(1);
            Assert.IsInstanceOfType<OAuth2AccessToken>(token);
            DumpToken(token);
            Util.DumpSummary(token.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var token in OAuth2AccessToken.FindAsync())
            {
                DumpToken(token);
                Util.DumpSummary(token.SummaryFields);
                Console.WriteLine();
            }
        }
        [TestMethod]
        public async Task Get03ListFromApplication()
        {
            await foreach (var token in OAuth2AccessToken.FindFromApplication(1))
            {
                Assert.IsInstanceOfType<OAuth2AccessToken>(token);
                Console.WriteLine($"[{token.Id}] {token.Scope} User:[{token.User}]" +
                    (token.Application > 0 ? $" App:[{token.Application}]" : ""));
            }
        }
        [TestMethod]
        public async Task Get04ListFromUser()
        {
            await foreach (var token in OAuth2AccessToken.FindFromUser(1))
            {
                Assert.IsInstanceOfType<OAuth2AccessToken>(token);
                Console.WriteLine($"[{token.Id}] {token.Scope} User:[{token.User}]" +
                    (token.Application > 0 ? $" App:[{token.Application}]" : ""));
            }
        }
        [TestMethod]
        public async Task Get05ListPersonalTokensFromUser()
        {
            await foreach (var token in OAuth2AccessToken.FindPersonalTokensFromUser(1))
            {
                Assert.IsInstanceOfType<OAuth2AccessToken>(token);
                Console.WriteLine($"[{token.Id}] {token.Scope} User:[{token.User}]" +
                    (token.Application > 0 ? $" App:[{token.Application}]" : ""));
            }
        }
        [TestMethod]
        public async Task Get06ListAuthorizedTokensFromUser()
        {
            await foreach (var token in OAuth2AccessToken.FindAuthorizedTokensFromUser(1))
            {
                Assert.IsInstanceOfType<OAuth2AccessToken>(token);
                Console.WriteLine($"[{token.Id}] {token.Scope} User:[{token.User}]" +
                    (token.Application > 0 ? $" App:[{token.Application}]" : ""));
            }
        }
    }

    [TestClass]
    public class TestInstance
    {
        private static void DumpInstance(Instance instance)
        {
            Console.WriteLine($"Id                : {instance.Id}");
            Console.WriteLine($"Type              : {instance.Type}");
            Console.WriteLine($"Hostname          : {instance.Hostname}");
            Console.WriteLine($"UUID              : {instance.Uuid}");
            Console.WriteLine($"Created           : {instance.Created}");
            Console.WriteLine($"Modified          : {instance.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"LastSeen          : {instance.LastSeen}");
            Console.WriteLine($"HelthCheckStarted : {instance.HealthCheckStarted?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"HelthCheckPending : {instance.HealthCheckPending}");
            Console.WriteLine($"LastHealthCheck   : {instance.LastHealthCheck?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Errors            : {instance.Errors}");
            Console.WriteLine($"CapacityAdjustment: {instance.CapacityAdjustment}");
            Console.WriteLine($"Version           : {instance.Version}");
            Console.WriteLine($"Capacity          : {instance.Capacity}");
            Console.WriteLine($"ConsumedCapacity  : {instance.ConsumedCapacity}");
            Console.WriteLine($"PercentCapacityRemaining: {instance.PercentCapacityRemaining}");
            Console.WriteLine($"JobRunning        : {instance.JobsRunning}");
            Console.WriteLine($"JobTotal          : {instance.JobsTotal}");
            Console.WriteLine($"CPU               : {instance.Cpu}");
            Console.WriteLine($"Memory            : {instance.Memory}");
            Console.WriteLine($"CPU Capacity      : {instance.CpuCapacity}");
            Console.WriteLine($"Mem Capacity      : {instance.MemCapacity}");
            Console.WriteLine($"Enabled           : {instance.Enabled}");
            Console.WriteLine($"ManagedByPolicy   : {instance.ManagedByPolicy}");
            Console.WriteLine($"NodeType          : {instance.NodeType}");
            Console.WriteLine($"NodeState         : {instance.NodeState}");
            Console.WriteLine($"IpAddress         : {instance.IpAddress}");
            Console.WriteLine($"Listener Port     : {instance.ListenerPort}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var instance = await Instance.GetAsync(1);
            Assert.IsInstanceOfType<Instance>(instance);
            DumpInstance(instance);
            Util.DumpSummary(instance.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var expectCount = 2;
            var c = 0;
            var query = new HttpQuery($"page_size={expectCount}");

            await foreach (var instance in Instance.FindAsync(query))
            {
                c++;
                Assert.IsInstanceOfType<Instance>(instance);
                DumpInstance(instance);
                Util.DumpSummary(instance.SummaryFields);
                Console.WriteLine();
            }
            Assert.IsTrue(c <= expectCount);
        }
        [TestMethod]
        public async Task Get03ListFromInstanceGroup()
        {
            await foreach (var inst in Instance.FindAsync(1))
            {
                Assert.IsInstanceOfType<Instance>(inst);
                Console.WriteLine($"[{inst.Id}] {inst.Hostname} {inst.NodeType} {inst.NodeState}");
            }
        }
    }
    [TestClass]
    public class TestInstanceGroup
    {
        private static void DumpResource(InstanceGroup ig)
        {
            Console.WriteLine($"Id                : {ig.Id}");
            Console.WriteLine($"Type              : {ig.Type}");
            Console.WriteLine($"Created           : {ig.Created}");
            Console.WriteLine($"Modified          : {ig.Modified}");
            Console.WriteLine($"Name              : {ig.Name}");
            Console.WriteLine($"Capacity          : {ig.Capacity}");
            Console.WriteLine($"ConsumedCapacity  : {ig.ConsumedCapacity}");
            Console.WriteLine($"PercentCapacityRemaining: {ig.PercentCapacityRemaining}");
            Console.WriteLine($"JobsRunning       : {ig.JobsRunning}");
            Console.WriteLine($"MaxConcurrentJobs : {ig.MaxConcurrentJobs}");
            Console.WriteLine($"MaxForkcs         : {ig.MaxForks}");
            Console.WriteLine($"JobsTotal         : {ig.JobsTotal}");
            Console.WriteLine($"Instances         : {ig.Instances}");
            Console.WriteLine($"IsContainerGroup  : {ig.IsContainerGroup}");
            Console.WriteLine($"Credential        : {ig.Credential?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Console.WriteLine($"PolicyInstancePercentage: {ig.PolicyInstancePercentage}");
            Console.WriteLine($"PolicyInstanceMinimum   : {ig.PolicyInstanceMinimum}");
            Console.WriteLine($"PolicyInstanceList      : {ig.PolicyInstanceList}");
            Console.WriteLine($"PodSpecOverride   : {ig.PodSpecOverride}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var ig = await InstanceGroup.GetAsync(1);
            Assert.IsInstanceOfType<InstanceGroup>(ig);
            DumpResource(ig);
            Util.DumpSummary(ig.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var expectCount = 2;
            var c = 0;
            var query = new HttpQuery($"page_size={expectCount}");

            await foreach (var ig in InstanceGroup.FindAsync(query))
            {
                c++;
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                DumpResource(ig);
                Util.DumpSummary(ig.SummaryFields);
                Console.WriteLine();
            }
            Assert.IsTrue(c <= expectCount);
        }
        [TestMethod]
        public async Task Get03ListFromInstance()
        {
            var resource = new Resource(ResourceType.Instance, 1);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }

        }
        [TestMethod]
        public async Task Get04ListFromOranization()
        {
            var resource = new Resource(ResourceType.Organization, 2);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }

        }
        [TestMethod]
        public async Task Get05ListFromInventory()
        {
            var resource = new Resource(ResourceType.Inventory, 2);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }
        }
        [TestMethod]
        public async Task Get06ListFromJobTemplate()
        {
            var resource = new Resource(ResourceType.Inventory, 7);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }
        }
        [TestMethod]
        public async Task Get07ListFromSchedule()
        {
            var resource = new Resource(ResourceType.Schedule, 8);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }
        }
        [TestMethod]
        public async Task Get08ListFromWorkflowJobTemplateNode()
        {
            var resource = new Resource(ResourceType.WorkflowJobTemplateNode, 4);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }
        }
        [TestMethod]
        public async Task Get09ListFromWorkflowJobNode()
        {
            var resource = new Resource(ResourceType.WorkflowJobNode, 7);
            await foreach (var ig in InstanceGroup.FindAsync(resource))
            {
                Assert.IsInstanceOfType<InstanceGroup>(ig);
                Console.WriteLine($"[{ig.Id}] {ig.Name} Instances = {ig.Instances}");
            }
        }
    }
    [TestClass]
    public class TestOrganization
    {
        private static void DumpResource(Organization org)
        {
            Console.WriteLine($"Id                : {org.Id}");
            Console.WriteLine($"Type              : {org.Type}");
            Console.WriteLine($"Created           : {org.Created}");
            Console.WriteLine($"Modified          : {org.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name              : {org.Name}");
            Console.WriteLine($"Description       : {org.Description}");
            Console.WriteLine($"MaxHosts          : {org.MaxHosts}");
            Console.WriteLine($"DefaultEnvironment: {org.DefaultEnvironment?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var org = await Organization.GetAsync(1);
            Assert.IsInstanceOfType<Organization>(org);
            DumpResource(org);
            Util.DumpSummary(org.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var expectCount = 2;
            var c = 0;
            var query = new HttpQuery($"page_size={expectCount}");

            await foreach (var org in Organization.FindAsync(query))
            {
                c++;
                Assert.IsInstanceOfType<Organization>(org);
                DumpResource(org);
                Util.DumpSummary(org.SummaryFields);
            }
            Assert.IsTrue(c <= expectCount);
        }
        [TestMethod]
        public async Task Get03ListAdministeredFromUser()
        {
            await foreach (var org in Organization.FindAsync(8, admin: true))
            {
                Assert.IsInstanceOfType<Organization>(org);
                Console.WriteLine($"[{org.Id}] {org.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromUser()
        {
            await foreach (var org in Organization.FindAsync(8, admin: false))
            {
                Assert.IsInstanceOfType<Organization>(org);
                Console.WriteLine($"[{org.Id}] {org.Name}");
            }
        }

    }
    [TestClass]
    public class TestUser
    {
        [TestMethod("既存ユーザーの作成を試行")]
        public async Task UserCreateError1()
        {
            var user = new UserData()
            {
                Username = "teramako",
                Email = "teramako@gmail.com",
                Password = "P@ssw0rd"
            };
            var ex = await Assert.ThrowsExactlyAsync<RestAPIException>(() => RestAPI.PostJsonAsync<User>("/api/v2/users/", user));
            Console.WriteLine(ex.ToString());
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            /*
            var res = await RestAPI.PostJsonAsync<User>("/api/v2/users/", jt);
            Assert.IsFalse(res.IsSuccess);
            Assert.IsNotNull(res.Exception);
            Console.WriteLine(res.Exception.ToString());
            Assert.IsInstanceOfType<RestAPIException>(res.Exception);
            Assert.IsTrue(res.Exception.Message.IndexOf("{\"username\":") > 0);
            */
        }
        public async Task UserCreateAndDelete()
        {
            var user = new UserData()
            {
                Username = "Test_User_1",
                FirstName = "User 1",
                LastName = "Test",
                Email = "",
                IsSuperuser = false,
                IsSystemAuditor = false,
                Password = "Password",
            };
            Console.WriteLine("================= Create =================");
            var apiResult = await RestAPI.PostJsonAsync<User>("/api/v2/users/", user);
            Assert.IsNotNull(apiResult.Contents);
            var createdUser = apiResult.Contents;
            Assert.IsInstanceOfType<User>(createdUser);
            Util.DumpObject(createdUser);
            Util.DumpResponse(apiResult.Response);

            Console.WriteLine("================= Deleate =================");
            var id = createdUser.Id;
            var deleteResult = await RestAPI.DeleteAsync($"/api/v2/users/{id}/");
            Assert.AreEqual(0, deleteResult.Response.ContentLength);
            if (deleteResult.Contents is not null)
                Util.DumpObject(deleteResult.Contents);
            else
                Console.WriteLine($"{nameof(deleteResult.Contents)} is null");
            Util.DumpResponse(deleteResult.Response);
        }
        private static void DumpResource(User user)
        {
            Console.WriteLine($"ID   : {user.Id}");
            Console.WriteLine($"Type : {user.Type}");
            Console.WriteLine($"Created  : {user.Created}");
            Console.WriteLine($"Modifed  : {user.Modified}");
            Console.WriteLine($"Username : {user.Username}");
            Console.WriteLine($"FirstName: {user.FirstName}");
            Console.WriteLine($"LastName : {user.LastName}");
            Console.WriteLine($"Email    : {user.Email}");
            Console.WriteLine($"LastLogin: {user.LastLogin?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Auth     : {user.Auth}");
            Console.WriteLine($"Password : {user.Password}");
            Console.WriteLine($"LdapDn   : {user.LdapDn}");
            Console.WriteLine($"IsSuperuser      : {user.IsSuperuser}");
            Console.WriteLine($"IsSystemAutoditor: {user.IsSystemAuditor}");
            Console.WriteLine($"ExternalAccount  : {user.ExternalAccount}");
            Util.DumpSummary(user.SummaryFields);
        }
        [TestMethod]
        public void Get01Single()
        {
            var user = User.Get(2);
            Assert.IsInstanceOfType<User>(user);
            DumpResource(user);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=2");
            foreach (var user in User.Find(query))
            {
                DumpResource(user);
            }
        }
        [TestMethod]
        public void Get03Me()
        {
            var user = User.GetMe();
            Assert.IsInstanceOfType<User>(user);
            DumpResource(user);
        }
        [TestMethod]
        public void Get04ListFromOrganization()
        {
            var resource = new Resource(ResourceType.Organization, 2);
            foreach (var user in User.Find(resource))
            {
                Assert.IsInstanceOfType<User>(user);
                Console.WriteLine($"[{user.Id}] {user.Username} {user.Email}");
            }
        }
        [TestMethod]
        public void Get05ListFromTeam()
        {
            var resource = new Resource(ResourceType.Team, 1);
            foreach (var user in User.Find(resource))
            {
                Assert.IsInstanceOfType<User>(user);
                Console.WriteLine($"[{user.Id}] {user.Username} {user.Email}");
            }
        }
        [TestMethod]
        public void Get6ListOwnersFromCredential()
        {
            var resource = new Resource(ResourceType.Credential, 1);
            foreach (var user in User.Find(resource))
            {
                Assert.IsInstanceOfType<User>(user);
                Console.WriteLine($"[{user.Id}] {user.Username} {user.Email}");
            }
        }
        [TestMethod]
        public void Get07ListFromRole()
        {
            var resource = new Resource(ResourceType.Role, 1);
            foreach (var user in User.Find(resource))
            {
                Assert.IsInstanceOfType<User>(user);
                Console.WriteLine($"[{user.Id}] {user.Username} {user.Email}");
            }
        }
    }
    [TestClass]
    public class TestProject
    {
        private static void DumpResource(Project proj)
        {
            Console.WriteLine($"Id                   : {proj.Id}");
            Console.WriteLine($"Type                 : {proj.Type}");
            Console.WriteLine($"Created              : {proj.Created}");
            Console.WriteLine($"Modified             : {proj.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name                 : {proj.Name}");
            Console.WriteLine($"Description          : {proj.Description}");
            Console.WriteLine($"LocalPath            : {proj.LocalPath}");
            Console.WriteLine($"ScmType              : {proj.ScmType}");
            Console.WriteLine($"ScmUrl               : {proj.ScmUrl}");
            Console.WriteLine($"ScmBranch            : {proj.ScmBranch}");
            Console.WriteLine($"ScmRefspac           : {proj.ScmRefspec}");
            Console.WriteLine($"ScmClean             : {proj.ScmClean}");
            Console.WriteLine($"ScmTrackSubmodules   : {proj.ScmTrackSubmodules}");
            Console.WriteLine($"ScmDeleteOnUpdate    : {proj.ScmDeleteOnUpdate}");
            Console.WriteLine($"Credential           : {proj.Credential?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Console.WriteLine($"Timeout              : {proj.Timeout}");
            Console.WriteLine($"ScmRevision          : {proj.ScmRevision}");
            Console.WriteLine($"LastJobRun           : {proj.LastJobRun?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"LastJobFailed        : {proj.LastJobFailed}");
            Console.WriteLine($"NextJobFun           : {proj.NextJobRun?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Status               : {proj.Status}");
            Console.WriteLine($"Organization         : {proj.Organization}");
            Console.WriteLine($"ScmUpdateOnLaunch    : {proj.ScmUpdateOnLaunch}");
            Console.WriteLine($"ScmUpdateCacheTimeout: {proj.ScmUpdateCacheTimeout}");
            Console.WriteLine($"AllowOverride        : {proj.AllowOverride}");
            Console.WriteLine($"CustomVirtualenv     : {proj.CustomVirtualenv ?? "(null)"}");
            Console.WriteLine($"DefaultEnvironment   : {proj.DefaultEnvironment?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Console.WriteLine($"LastUpdateFailed     : {proj.LastUpdateFailed}");
            Console.WriteLine($"LastUpdated          : {proj.LastUpdated?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"SignatureValidateionCredential: {proj.SignatureValidationCredential?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Util.DumpSummary(proj.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var proj = await Project.GetAsync(8);
            Assert.IsInstanceOfType<Project>(proj);
            DumpResource(proj);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=2");
            await foreach (var proj in Project.FindAsync(query))
            {
                DumpResource(proj);
            }
        }
        [TestMethod]
        public async Task Get03ListFromOrganization()
        {
            var resource = new Resource(ResourceType.Organization, 1);
            await foreach (var proj in Project.FindAsync(resource))
            {
                Assert.IsInstanceOfType<Project>(proj);
                Console.WriteLine($"[{proj.Id}] {proj.Name} {proj.ScmType}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromUser()
        {
            var resource = new Resource(ResourceType.User, 1);
            await foreach (var proj in Project.FindAsync(resource))
            {
                Assert.IsInstanceOfType<Project>(proj);
                Console.WriteLine($"[{proj.Id}] {proj.Name} {proj.ScmType}");
            }

        }
        [TestMethod]
        public async Task Get05ListFromTeam()
        {
            var resource = new Resource(ResourceType.Team, 1);
            await foreach (var proj in Project.FindAsync(resource))
            {
                Assert.IsInstanceOfType<Project>(proj);
                Console.WriteLine($"[{proj.Id}] {proj.Name} {proj.ScmType}");
            }
        }
        [TestMethod]
        public async Task Get06GetInventoryFiles()
        {
            var files = await Project.GetInventoryFiles(8);
            Console.WriteLine(string.Join('\n', files));
        }
    }

    [TestClass]
    public class TestProjectUpdate
    {
        private static void DumpResource(IProjectUpdateJob job)
        {
            Console.WriteLine($"ID         : {job.Id}");
            Console.WriteLine($"Name       : {job.Name}");
            Console.WriteLine($"Description: {job.Description}");
            Console.WriteLine($"LocalPath  : {job.LocalPath}");
            Console.WriteLine($"ScmType    : {job.ScmType}");
            Console.WriteLine($"ScmUrl     : {job.ScmUrl}");
            Console.WriteLine($"ScmBranch  : {job.ScmBranch}");
            Console.WriteLine($"ScmRefspec : {job.ScmRefspec}");
            Console.WriteLine($"ScmClean   : {job.ScmClean}");
            Console.WriteLine($"ScmTrackSubmodules: {job.ScmTrackSubmodules}");
            Console.WriteLine($"ScmDeleteOnUpdate : {job.ScmDeleteOnUpdate}");
            Console.WriteLine($"Credential : {job.Credential?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Console.WriteLine($"Timeout    : {job.Timeout}");
            Console.WriteLine($"Project    : {job.Project}");
        }

        [TestMethod]
        public async Task Get01Single()
        {
            var job = await ProjectUpdateJob.GetAsync(5);
            Assert.IsInstanceOfType<ProjectUpdateJob.Detail>(job);
            DumpResource(job);
            Console.WriteLine($"JobArgs    : {job.JobArgs}");
            Console.WriteLine($"JobCwd     : {job.JobCwd}");
            Console.WriteLine($"JobEnv     : {job.JobEnv.Count}");
            foreach (var (k, v) in job.JobEnv)
            {
                Console.WriteLine($"   {k}: {v}");
            }
            Util.DumpSummary(job.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=2&order_by=-id");
            await foreach (var job in ProjectUpdateJob.FindAsync(query))
            {
                DumpResource(job);
                Util.DumpSummary(job.SummaryFields);
            }
        }
        [TestMethod]
        public async Task Get03ListFromProject()
        {
            await foreach (var job in ProjectUpdateJob.FindAsync(8))
            {
                Assert.IsInstanceOfType<ProjectUpdateJob>(job);
                Console.WriteLine($"[{job.Id}] {job.Name} {job.Finished}");
                Console.WriteLine($"  {job.ScmType} {job.ScmRevision}");
            }
        }
    }
    [TestClass]
    public class TestTeam
    {
        private static void DumpResource(Team team)
        {
            Console.WriteLine($"Id          : {team.Id}");
            Console.WriteLine($"Type        : {team.Type}");
            Console.WriteLine($"Created     : {team.Created}");
            Console.WriteLine($"Modified    : {team.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {team.Name}");
            Console.WriteLine($"Description : {team.Description}");
            Console.WriteLine($"Organization: {team.Organization}");
            Util.DumpSummary(team.SummaryFields);
        }
        private static void DumpObjectRoles(Team team)
        {
            if (team.SummaryFields.TryGetValue<Dictionary<string, ObjectRoleSummary>>("ObjectRoles", out var roles))
            {
                foreach (var kv in roles)
                {
                    Console.WriteLine($"{kv.Key}:  {kv.Value.Name} - {kv.Value.Description}");
                }
            }
        }

        [TestMethod]
        public void Get01Single()
        {
            var team = Team.Get(1);
            Assert.IsInstanceOfType<Team>(team);
            DumpResource(team);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=2");
            foreach (var team in Team.Find(query))
            {
                DumpResource(team);
            }
        }
        [TestMethod]
        public void Get03ListFromOrganization()
        {
            var resource = new Resource(ResourceType.Organization, 2);
            foreach (var team in Team.Find(resource))
            {
                Assert.IsInstanceOfType<Team>(team);
                Console.WriteLine($"[{team.Id}] {team.Name}");
                DumpObjectRoles(team);
            }
        }
        [TestMethod]
        public void Get04ListFromUser()
        {
            var resource = new Resource(ResourceType.User, 2);
            foreach (var team in Team.Find(resource))
            {
                Assert.IsInstanceOfType<Team>(team);
                Console.WriteLine($"[{team.Id}] {team.Name}");
                DumpObjectRoles(team);
            }
        }
        [TestMethod]
        public void Get05ListFromProject()
        {
            var resource = new Resource(ResourceType.Project, 8);
            foreach (var team in Team.Find(resource))
            {
                Assert.IsInstanceOfType<Team>(team);
                Console.WriteLine($"[{team.Id}] {team.Name}");
                DumpObjectRoles(team);
            }
        }
        [TestMethod]
        public void Get06FindOwnerFromCredential()
        {
            var resource = new Resource(ResourceType.Credential, 2);
            foreach (var team in Team.Find(resource))
            {
                Assert.IsInstanceOfType<Team>(team);
                Console.WriteLine($"[{team.Id}] {team.Name}");
                DumpObjectRoles(team);
            }
        }
        [TestMethod]
        public void Get07FindFromRole()
        {
            var resource = new Resource(ResourceType.Role, 73);
            foreach (var team in Team.Find(resource))
            {
                Assert.IsInstanceOfType<Team>(team);
                Console.WriteLine($"[{team.Id}] {team.Name}");
                DumpObjectRoles(team);
            }

        }
    }
    [TestClass]
    public class TestCredential
    {
        private static void DumpResource(Credential cred)
        {
            Console.WriteLine($"Id            : {cred.Id}");
            Console.WriteLine($"Type          : {cred.Type}");
            Console.WriteLine($"Created       : {cred.Created}");
            Console.WriteLine($"Modified      : {cred.Modified}");
            Console.WriteLine($"Name          : {cred.Name}");
            Console.WriteLine($"Description   : {cred.Description}");
            Console.WriteLine($"Organization  : {cred.Organization?.ToString(CultureInfo.InvariantCulture) ?? "(null)"}");
            Console.WriteLine($"CredentialType: {cred.CredentialType}");
            Console.WriteLine($"Managed       : {cred.Managed}");
            Console.WriteLine($"Kind          : {cred.Kind}");
            Console.WriteLine($"Cloud         : {cred.Cloud}");
            Console.WriteLine($"Kubernetes    : {cred.Kubernetes}");
            Util.DumpSummary(cred.SummaryFields);
        }

        [TestMethod]
        public async Task Get01Single()
        {
            var cred = await Credential.GetAsync(2);
            Assert.IsInstanceOfType<Credential>(cred);
            DumpResource(cred);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=id");
            await foreach (var cred in Credential.FindAsync(query))
            {
                DumpResource(cred);
            }
        }
        [TestMethod]
        public async Task Get03ListFromOrganization()
        {
            var res = new Resource(ResourceType.Organization, 2);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListGalaxyFromOrganization()
        {
            await foreach (var cred in Credential.FindGalaxyAsync(1))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get05ListFromUser()
        {
            var res = new Resource(ResourceType.User, 1);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get06ListFromTeam()
        {
            var res = new Resource(ResourceType.Team, 1);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get07ListFromCredentialType()
        {
            var res = new Resource(ResourceType.CredentialType, 1);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get08ListFromInventorySource()
        {
            var res = new Resource(ResourceType.InventorySource, 17);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get09ListFromInventoryUpdate()
        {
            var res = new Resource(ResourceType.InventoryUpdate, 526);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get10ListFromJobTemplate()
        {
            var res = new Resource(ResourceType.JobTemplate, 7);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get11ListFromJob()
        {
            var res = new Resource(ResourceType.Job, 565);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get12ListFromSchedule()
        {
            var res = new Resource(ResourceType.Schedule, 6);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get13ListFromWorkflowJobTemplateNode()
        {
            var res = new Resource(ResourceType.WorkflowJobTemplateNode, 1);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
        [TestMethod]
        public async Task Get14ListFromWorkflowJobNode()
        {
            var res = new Resource(ResourceType.WorkflowJobNode, 131);
            await foreach (var cred in Credential.FindAsync(res))
            {
                Assert.IsInstanceOfType<Credential>(cred);
                Console.WriteLine($"[{cred.Id}][{cred.CredentialType}] {cred.Name}");
            }
        }
    }
    [TestClass]
    public class TestCredentialType
    {
        private static void DumpResource(CredentialType ct)
        {
            Console.WriteLine($"Id          : {ct.Id}");
            Console.WriteLine($"Type        : {ct.Type}");
            Console.WriteLine($"Created     : {ct.Created}");
            Console.WriteLine($"Modified    : {ct.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {ct.Name}");
            Console.WriteLine($"Description : {ct.Description}");
            Console.WriteLine($"Kind        : {ct.Kind}");
            Console.WriteLine($"Namespace   : {ct.Namespace}");
            Console.WriteLine($"Managed     : {ct.Managed}");
            Console.WriteLine($"==== Inputs ({ct.Inputs.Count})======");
            if (ct.Inputs.Count > 0)
                Util.DumpObject(ct.Inputs);
            Console.WriteLine($"==== Injectors ({ct.Injectors})===");
            Util.DumpObject(ct.Injectors);
            Util.DumpSummary(ct.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var ct = await CredentialType.GetAsync(1);
            Assert.IsInstanceOfType<CredentialType>(ct);
            DumpResource(ct);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=20&order_by=id");
            await foreach (var ct in CredentialType.FindAsync(query))
            {
                DumpResource(ct);
            }
        }
    }

    [TestClass]
    public class TestInventory
    {
        private static void DumpResource(Inventory inventory)
        {
            Console.WriteLine($"Id          : {inventory.Id}");
            Console.WriteLine($"Type        : {inventory.Type}");
            Console.WriteLine($"Created     : {inventory.Created}");
            Console.WriteLine($"Modified    : {inventory.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {inventory.Name}");
            Console.WriteLine($"Description : {inventory.Description}");
            Console.WriteLine($"Kind        : {inventory.Kind}");
            Console.WriteLine($"HostFilter  : {inventory.HostFilter}");
            Console.WriteLine($"Variables   : {inventory.Variables}");
            Util.DumpSummary(inventory.SummaryFields);
        }

        [TestMethod]
        public async Task Get01Single()
        {
            var inventory = await Inventory.GetAsync(1);
            Assert.IsInstanceOfType<Inventory>(inventory);
            DumpResource(inventory);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=20&order_by=id");
            await foreach (var inventory in Inventory.FindAsync(query))
            {
                DumpResource(inventory);
            }
        }
        [TestMethod]
        public async Task Get03ListFromOrganization()
        {
            var resource = new Resource(ResourceType.Organization, 2);
            await foreach (var inventory in Inventory.FindAsync(resource))
            {
                Assert.IsInstanceOfType<Inventory>(inventory);
                Console.WriteLine($"[{inventory.Id}] {inventory.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListInputInventires()
        {
            Console.WriteLine("Inventory [4]'s Inpput Inventories:");
            var resource = new Resource(ResourceType.Inventory, 4);
            await foreach (var inventory in Inventory.FindAsync(resource))
            {
                Assert.IsInstanceOfType<Inventory>(inventory);
                Console.WriteLine($"[{inventory.Id}] {inventory.Name}");
            }
        }
    }
    [TestClass]
    public class TestConstructedInventory
    {
        private static void DumpResource(ConstructedInventory inventory)
        {
            Console.WriteLine($"Id          : {inventory.Id}");
            Console.WriteLine($"Type        : {inventory.Type}");
            Console.WriteLine($"Created     : {inventory.Created}");
            Console.WriteLine($"Modified    : {inventory.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {inventory.Name}");
            Console.WriteLine($"Description : {inventory.Description}");
            Console.WriteLine($"Kind        : {inventory.Kind}");
            Console.WriteLine($"Variables   : {inventory.Variables}");
            Console.WriteLine($"Sourcevars  : {inventory.SourceVars}");
            Util.DumpSummary(inventory.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var inventory = await ConstructedInventory.GetAsync(4);
            Assert.IsInstanceOfType<ConstructedInventory>(inventory);
            Assert.AreEqual("constructed", inventory.Kind);
            DumpResource(inventory);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var inventory in ConstructedInventory.FindAsync())
            {
                Assert.AreEqual("constructed", inventory.Kind);
                DumpResource(inventory);
            }
        }
    }
    [TestClass]
    public class TestInventorySource
    {
        private static void DumpResource(InventorySource res)
        {
            Console.WriteLine($"Id          : {res.Id}");
            Console.WriteLine($"Type        : {res.Type}");
            Console.WriteLine($"Name        : {res.Name}");
            Console.WriteLine($"Status      : {res.Status}");
            Console.WriteLine($"Source      : {res.Source}");
            Console.WriteLine($"SourcePath  : {res.SourcePath}");
            Console.WriteLine($"SourceVars  : {res.SourceVars}");
            Console.WriteLine($"Enabled  Var: {res.EnabledVar}, Value: {res.EnabledValue}");
            Console.WriteLine($"Overwrite   : {res.Overwrite}, Vars: {res.OverwriteVars}");
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await InventorySource.GetAsync(11);
            Assert.IsInstanceOfType<InventorySource>(res);
            DumpResource(res);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var res in InventorySource.FindAsync(new HttpQuery("order_by=id")))
            {
                DumpResource(res);
            }
        }
        [TestMethod]
        public async Task Get03ListFromProject()
        {
            var proj = await Project.GetAsync(8);
            Console.WriteLine($"Scm InventorySources for ([{proj.Type}][{proj.Id}] {proj.Name})");
            await foreach (var res in InventorySource.FindAsync(proj))
            {
                Assert.IsInstanceOfType<InventorySource>(res);
                Console.WriteLine($"[{res.Id}] {res.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromInventory()
        {
            var inventory = await Inventory.GetAsync(4);
            Console.WriteLine($"InventorySources for ([{inventory.Type}][{inventory.Id}] {inventory.Name})");
            await foreach (var res in InventorySource.FindAsync(inventory))
            {
                Assert.IsInstanceOfType<InventorySource>(res);
                Console.WriteLine($"[{res.Id}] {res.Name}");
            }
        }
        [TestMethod]
        public async Task Get05ListFromGroup()
        {
            var group = await Group.GetAsync(4);
            Console.WriteLine($"InventorySources for ([{group.Type}][{group.Id}] {group.Name})");
            await foreach (var res in InventorySource.FindAsync(group))
            {
                Assert.IsInstanceOfType<InventorySource>(res);
                Console.WriteLine($"[{res.Id}] {res.Name}");
            }
        }
        [TestMethod]
        public async Task Get06ListFromHost()
        {
            var host = await Host.GetAsync(3);
            Console.WriteLine($"InventorySources for ([{host.Type}][{host.Id}] {host.Name})");
            await foreach (var res in InventorySource.FindAsync(host))
            {
                Assert.IsInstanceOfType<InventorySource>(res);
                Console.WriteLine($"[{res.Id}] {res.Name}");
            }
        }
    }
    [TestClass]
    public class TestInventoryUpdate
    {
        private static void DumpResource(InventoryUpdateJobBase res)
        {
            Console.WriteLine($"Id          : {res.Id}");
            Console.WriteLine($"Type        : {res.Type}");
            Console.WriteLine($"Name        : {res.Name}");
            Console.WriteLine($"Status      : {res.Status}");
            Console.WriteLine($"Source      : {res.Source}");
            Console.WriteLine($"SourcePath  : {res.SourcePath}");
            Console.WriteLine($"SourceVars  : {res.SourceVars}");
            Console.WriteLine($"Enabled  Var: {res.EnabledVar}, Value: {res.EnabledValue}");
            Console.WriteLine($"Overwrite   : {res.Overwrite}, Vars: {res.OverwriteVars}");
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await InventoryUpdateJob.GetAsync(46);
            Assert.IsInstanceOfType<InventoryUpdateJob.Detail>(res);
            Assert.IsInstanceOfType<IUnifiedJob>(res);
            DumpResource(res);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var res in InventoryUpdateJob.FindAsync(new HttpQuery("order_by=id")))
            {
                DumpResource(res);
            }
        }
        [TestMethod]
        public async Task Get03ListFromProjectUpdate()
        {
            var projectUpdateJob = await ProjectUpdateJob.GetAsync(76);
            Console.WriteLine($"InventoryUpdateJobs for ([{projectUpdateJob.Id}][{projectUpdateJob.Type}] {projectUpdateJob.Name})");
            await foreach (var res in InventoryUpdateJob.FindAsync(projectUpdateJob))
            {
                Assert.IsInstanceOfType<InventoryUpdateJob>(res);
                Console.WriteLine($"[{res.Id}] {res.Name} {res.Status} {res.Finished}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromInventorySource()
        {
            var inventorySource = await InventorySource.GetAsync(11);
            Console.WriteLine($"InventoryUpdateJobs for ([{inventorySource.Id}][{inventorySource.Type}] {inventorySource.Name})");
            await foreach (var res in InventoryUpdateJob.FindAsync(inventorySource))
            {
                Assert.IsInstanceOfType<InventoryUpdateJob>(res);
                Console.WriteLine($"[{res.Id}] {res.Name} {res.Status} {res.Finished}");
            }
        }
    }

    [TestClass]
    public class TestGroup
    {
        private static void DumpResource(Group group)
        {
            Console.WriteLine($"Id          : {group.Id}");
            Console.WriteLine($"Type        : {group.Type}");
            Console.WriteLine($"Created     : {group.Created}");
            Console.WriteLine($"Modified    : {group.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {group.Name}");
            Console.WriteLine($"Description : {group.Description}");
            Console.WriteLine($"Inventory   : {group.Inventory}");
            Console.WriteLine($"Variables   : {group.Variables}");
            Util.DumpSummary(group.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var group = await Group.GetAsync(1);
            Assert.IsInstanceOfType<Group>(group);
            DumpResource(group);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=20");
            await foreach (var group in Group.FindAsync(query))
            {
                DumpResource(group);
            }
        }
        [TestMethod]
        public async Task Get03ListFromInventory()
        {
            var inventory = await Inventory.GetAsync(2);
            Console.WriteLine($"Groups in [{inventory.Type}][{inventory.Id}] {inventory.Name}");
            await foreach (var group in Group.FindAsync(inventory))
            {
                Assert.IsInstanceOfType<Group>(group);
                Console.WriteLine($"[{group.Id}] {group.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListOnlyRootFromInventory()
        {
            var inventory = await Inventory.GetAsync(2);
            Console.WriteLine($"Groups in [{inventory.Type}][{inventory.Id}] {inventory.Name}");
            await foreach (var group in Group.FindAsync(inventory, all: false))
            {
                Assert.IsInstanceOfType<Group>(group);
                Console.WriteLine($"[{group.Id}] {group.Name}");
            }
        }
        [TestMethod]
        public async Task Get05ListFromInventorySource()
        {
            var inventorySource = await InventorySource.GetAsync(11);
            Console.WriteLine($"Groups in [{inventorySource.Type}][{inventorySource.Id}] {inventorySource.Name}");
            await foreach (var group in Group.FindAsync(inventorySource))
            {
                Assert.IsInstanceOfType<Group>(group);
                Console.WriteLine($"[{group.Id}] {group.Name}");
            }
        }
        [TestMethod]
        public async Task Get06ListAllFromHost()
        {
            var host = await Host.GetAsync(3);
            Console.WriteLine($"Groups in [{host.Type}][{host.Id}] {host.Name}");
            await foreach (var group in Group.FindAsync(host, all: true))
            {
                Assert.IsInstanceOfType<Group>(group);
                Console.WriteLine($"[{group.Id}] {group.Name}");
            }
        }
        [TestMethod]
        public async Task Get07ListFromHost()
        {
            var host = await Host.GetAsync(3);
            Console.WriteLine($"Groups in [{host.Type}][{host.Id}] {host.Name}");
            await foreach (var group in Group.FindAsync(host, all: false))
            {
                Assert.IsInstanceOfType<Group>(group);
                Console.WriteLine($"[{group.Id}] {group.Name}");
            }
        }
    }
    [TestClass]
    public class TestHost
    {
        private static void DumpResource(Host host)
        {
            Console.WriteLine($"Id          : {host.Id}");
            Console.WriteLine($"Type        : {host.Type}");
            Console.WriteLine($"Created     : {host.Created}");
            Console.WriteLine($"Modified    : {host.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {host.Name}");
            Console.WriteLine($"Description : {host.Description}");
            Console.WriteLine($"Inventory   : {host.Inventory}");
            Console.WriteLine($"Enabled     : {host.Enabled}");
            Console.WriteLine($"InstanceId  : {host.InstanceId}");
            Console.WriteLine($"Variables   : {host.Variables}");
            Util.DumpSummary(host.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var host = await Host.GetAsync(1);
            Assert.IsInstanceOfType<Host>(host);
            DumpResource(host);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=20");
            await foreach (var host in Host.FindAsync(query))
            {
                DumpResource(host);
            }
        }
        [TestMethod]
        public async Task Get03ListFromInventory()
        {
            var inventory = await Inventory.GetAsync(2);
            Console.WriteLine($"Hosts in [{inventory.Type}][{inventory.Id}] {inventory.Name}");
            await foreach (var host in Host.FindAsync(inventory))
            {
                Assert.IsInstanceOfType<Host>(host);
                Console.WriteLine($"[{host.Id}] {host.Name}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromInventorySource()
        {
            var inventorySource = await InventorySource.GetAsync(11);
            Console.WriteLine($"Hosts in [{inventorySource.Type}][{inventorySource.Id}] {inventorySource.Name}");
            await foreach (var host in Host.FindAsync(inventorySource))
            {
                Assert.IsInstanceOfType<Host>(host);
                Console.WriteLine($"[{host.Id}] {host.Name}");
            }
        }
        [TestMethod]
        public async Task Get05ListAllFromHost()
        {
            var group = await Group.GetAsync(1);
            Console.WriteLine($"Groups in [{group.Type}][{group.Id}] {group.Name}");
            await foreach (var host in Host.FindAsync(group, all: true))
            {
                Assert.IsInstanceOfType<Host>(host);
                Console.WriteLine($"[{host.Id}] {host.Name}");
            }
        }
        [TestMethod]
        public async Task Get06ListFromHost()
        {
            var group = await Group.GetAsync(1);
            Console.WriteLine($"Groups in [{group.Type}][{group.Id}] {group.Name}");
            await foreach (var host in Host.FindAsync(group, all: false))
            {
                Assert.IsInstanceOfType<Host>(host);
                Console.WriteLine($"[{host.Id}] {host.Name}");
            }
        }
    }

    [TestClass]
    public class TestHostMetrics
    {
        private static void DumpResource(HostMetric metric)
        {
            Console.WriteLine($"Id              : {metric.Id}");
            Console.WriteLine($"Type            : {metric.Type}");
            Console.WriteLine($"Hostname        : {metric.Hostname}");
            Console.WriteLine($"FirstAutomation : {metric.FirstAutomation?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"LastAutomation  : {metric.LastAutomation?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"LastDeleted     : {metric.LastDeleted?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"AutomatedCounter: {metric.AutomatedCounter}");
            Console.WriteLine($"DeletedCounter  : {metric.DeletedCounter}");
            Console.WriteLine($"Deleted         : {metric.Deleted}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var metric = await HostMetric.GetAsync(1);
            Assert.IsInstanceOfType<HostMetric>(metric);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var metric in HostMetric.FindAsync(new QueryBuilder().SetPageSize(1)
                                                                                .SetOrderBy("id")
                                                                                .Build()))
            {
                DumpResource(metric);
            }
        }
    }

    [TestClass]
    public class TestJobTemplate
    {
        private static void DumpResource(JobTemplate jt)
        {
            Console.WriteLine($"Id          : {jt.Id}");
            Console.WriteLine($"Type        : {jt.Type}");
            Console.WriteLine($"Created     : {jt.Created}");
            Console.WriteLine($"Modified    : {jt.Modified?.ToString("o") ?? "(null)"}");
            Console.WriteLine($"Name        : {jt.Name}");
            Console.WriteLine($"Description : {jt.Description}");
            Console.WriteLine($"Inventory   : {jt.Inventory}");
            Console.WriteLine($"Project     : {jt.Project}");
            Console.WriteLine($"Playbook    : {jt.Playbook}");
            Console.WriteLine($"ExtraVars   : {jt.ExtraVars}");
            Util.DumpSummary(jt.SummaryFields);
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var jt = await JobTemplate.GetAsync(9);
            Assert.IsInstanceOfType<JobTemplate>(jt);
            Assert.IsInstanceOfType<IUnifiedJobTemplate>(jt);
            DumpResource(jt);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=20&order_by=id");
            await foreach (var jt in JobTemplate.FindAsync(query))
            {
                DumpResource(jt);
            }
        }
        [TestMethod]
        public async Task Get03ListFromOrganization()
        {
            var org = await Organization.GetAsync(2);
            Console.WriteLine($"JobTemplates in ({org.Type})[{org.Id}] {org.Name}");
            await foreach (var jt in JobTemplate.FindAsync(org))
            {
                Assert.IsInstanceOfType<JobTemplate>(jt);
                Console.WriteLine($"[{jt.Id}] {jt.Name} {jt.Status}");
            }
        }
        [TestMethod]
        public async Task Get04listFromInventory()
        {
            var inv = await Inventory.GetAsync(2);
            Console.WriteLine($"JobTemplates in ({inv.Type})[{inv.Id}] {inv.Name}");
            await foreach (var jt in JobTemplate.FindAsync(inv))
            {
                Assert.IsInstanceOfType<JobTemplate>(jt);
                Console.WriteLine($"[{jt.Id}] {jt.Name} {jt.Status}");
            }
        }
    }
    [TestClass]
    public class TestJob
    {
        private const ulong jobId = 4;

        private static void DumpResource(JobTemplateJob.Detail job)
        {
            Console.WriteLine($"[{job.Id}] {job.Name} - {job.Description}");
            Console.WriteLine($"JobArgs: {job.JobArgs}");
            Console.WriteLine($"JobCwd : {job.JobCwd}");
            Console.WriteLine($"JobEnv : {string.Join(", ", job.JobEnv.Keys)}");
            Console.WriteLine($"EventProcessingFinished: {job.EventProcessingFinished}");
            foreach (var kv in job.HostStatusCounts)
            {
                Console.WriteLine($"HostStatus: {kv.Key}: {kv.Value}");
            }
            foreach (var kv in job.PlaybookCounts)
            {
                Console.WriteLine($"PlaybookCounts: {kv.Key}: {kv.Value}");
            }
        }
        private static void DumpResource(JobTemplateJobBase job)
        {
            Console.WriteLine("===== Job =====");
            Console.WriteLine($"[{job.Id}] {job.Name} - {job.Description}");
            Console.WriteLine($"Created   : {job.Created}");
            Console.WriteLine($"Modified  : {job.Modified}");
            Console.WriteLine($"Finished  : {job.Finished}");
            Assert.IsInstanceOfType<JobType>(job.JobType);
            Console.WriteLine($"JobType   : {job.JobType}");
            Assert.IsInstanceOfType<JobLaunchType>(job.LaunchType);
            Console.WriteLine($"LaunchType: {job.LaunchType}");
            Assert.IsInstanceOfType<JobStatus>(job.Status);
            Console.WriteLine($"Status    : {job.Status}");

            Assert.IsInstanceOfType<JobVerbosity>(job.Verbosity);
            Console.WriteLine($"Verbosity : {job.Verbosity}");
            Assert.IsInstanceOfType<JobVerbosity>(job.Verbosity);
            Console.WriteLine("=== Launched By ===");
            Console.WriteLine($"  [{job.LaunchedBy.Type}]{job.LaunchedBy.Name} [{job.LaunchedBy.Id}] {job.LaunchedBy.Url}");
            Assert.AreEqual($"[{job.LaunchedBy.Type}]{job.LaunchedBy.Name}", job.LaunchedBy.ToString());
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var job = await JobTemplateJob.GetAsync(jobId);
            Assert.IsInstanceOfType<JobTemplateJob.Detail>(job);
            DumpResource(job);
            Console.WriteLine($"JobArgs   : {job.JobArgs}");
            Console.WriteLine($"JobCwd    : {job.JobCwd}");
            Util.DumpSummary(job.SummaryFields);
        }

        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=2&order_by=-id");
            await foreach (var job in JobTemplateJob.FindAsync(query))
            {
                DumpResource(job);
                Util.DumpSummary(job.SummaryFields);
            }
        }
        [TestMethod]
        public async Task Get03ListFromJobtemplate()
        {
            var jt = await JobTemplate.GetAsync(9);
            Console.WriteLine($"Jobs in ({jt.Type})[{jt.Id}] {jt.Name}");
            await foreach (var job in JobTemplateJob.FindAsync(jt.Id))
            {
                Assert.IsInstanceOfType<JobTemplateJob>(job);
                Console.WriteLine($"[{job.Id}] {job.Status} {job.Finished} {job.LaunchedBy}");
            }
        }

        [TestMethod]
        public async Task JobLogTestText()
        {
            var apiResult = await RestAPI.GetAsync<string>($"/api/v2/jobs/{jobId}/stdout/", AcceptType.Text);
            Assert.IsTrue(apiResult.Response.IsSuccessStatusCode);
            Assert.IsInstanceOfType<string>(apiResult.Contents);
            var jobLog = apiResult.Contents;
            Console.WriteLine(jobLog);
        }
        [TestMethod]
        public async Task JobLogTestAnsi()
        {
            var apiResult = await RestAPI.GetAsync<string>($"/api/v2/jobs/{jobId}/stdout/?format=ansi", AcceptType.Text);
            Assert.IsTrue(apiResult.Response.IsSuccessStatusCode);
            Assert.IsInstanceOfType<string>(apiResult.Contents);
            var jobLog = apiResult.Contents;
            Console.WriteLine(jobLog);
        }

        [TestMethod]
        public async Task JobLogTestHtml()
        {
            var apiResult = await RestAPI.GetAsync<string>($"/api/v2/jobs/{jobId}/stdout/?format=html", AcceptType.Html);
            Assert.IsTrue(apiResult.Response.IsSuccessStatusCode);
            Assert.IsInstanceOfType<string>(apiResult.Contents);
            var jobLog = apiResult.Contents;
            Console.WriteLine(jobLog);
        }
        [TestMethod]
        public async Task JobLogTestJson()
        {
            var apiResult = await RestAPI.GetAsync<JobLog>($"/api/v2/jobs/{jobId}/stdout/?format=json");
            Assert.IsTrue(apiResult.Response.IsSuccessStatusCode);
            Assert.IsInstanceOfType<JobLog>(apiResult.Contents);
            var jobLog = apiResult.Contents;
            Assert.IsInstanceOfType<JobLog.JobLogRange>(jobLog.Range);
            Assert.AreEqual<uint>(0, jobLog.Range.Start);
            Assert.IsInstanceOfType<string>(jobLog.Content);
            Console.WriteLine(jobLog.Content);
        }
    }

    [TestClass]
    public class TestJobEvent
    {
        private readonly HttpQuery singleQuery = new("order_by=-id&page_size=1");
        private readonly HttpQuery orderByCounter = new("order_by=counter");

        [TestMethod("[JobEvent] 01 JobEvents from Job")]
        public void Get01FindFromJob()
        {
            var job = JobTemplateJob.Find(singleQuery).Single();
            foreach (var je in JobEvent.Find(job, orderByCounter))
            {
                Assert.IsInstanceOfType<JobEvent>(je);
                Console.WriteLine($"[{je.Id}][{je.Counter}] {je.EventLevel} {je.EventDisplay} {je.Task}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 02 JobEvents from Group")]
        public void Get02FindFromGroup()
        {
            var group = new Resource(ResourceType.Group, 1);
            foreach (var je in JobEvent.Find(group, orderByCounter))
            {
                Assert.IsInstanceOfType<JobEvent>(je);
                Console.WriteLine($"{je.Job} [{je.Id}][{je.Counter}] {je.EventLevel} {je.EventDisplay} {je.Task}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 03 JobEvents from Host")]
        public void Get03FindFromHost()
        {
            var host = new Resource(ResourceType.Host, 2);
            foreach (var je in JobEvent.Find(host, orderByCounter))
            {
                Assert.IsInstanceOfType<JobEvent>(je);
                Console.WriteLine($"{je.Job} [{je.Id}][{je.Counter}] {je.EventLevel} {je.EventDisplay} {je.Task}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 04 ProjectUpdateEvents")]
        public void Get04ProjectUpdate()
        {
            var job = ProjectUpdateJob.Find(singleQuery).Single();
            Console.WriteLine($"JobEvents in ({job.Type})[{job.Id}] {job.Name}");
            foreach (var je in ProjectUpdateJobEvent.Find(job.Id, orderByCounter))
            {
                Assert.IsInstanceOfType<IJobEventBase>(je);
                Assert.IsInstanceOfType<ProjectUpdateJobEvent>(je);
                Console.WriteLine($"[{je.Id}][{je.Counter}] {je.EventLevel} {je.EventDisplay} {je.Task}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 05 InventoryUpdateEvents")]
        public void Get05InventoryUpdate()
        {
            var job = InventoryUpdateJob.Find(singleQuery).Single();
            Console.WriteLine($"JobEvents in ({job.Type})[{job.Id}] {job.Name}");
            foreach (var je in InventoryUpdateJobEvent.Find(job.Id, orderByCounter))
            {
                Assert.IsInstanceOfType<IJobEventBase>(je);
                Assert.IsInstanceOfType<InventoryUpdateJobEvent>(je);
                Console.WriteLine($"[{je.Id}][{je.Counter}] {je.EventDisplay}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 06 SystemJobEvents")]
        public void Get06SystemJob()
        {
            var job = SystemJob.Find(singleQuery).Single();
            foreach (var je in SystemJobEvent.Find(job.Id, orderByCounter))
            {
                Assert.IsInstanceOfType<IJobEventBase>(je);
                Assert.IsInstanceOfType<SystemJobEvent>(je);
                Console.WriteLine($"[{je.Id}][{je.Counter}] {je.EventDisplay}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
        [TestMethod("[JobEvent] 07 AdHocCommandJobEvent")]
        public void Get07AdHocCommandEvent()
        {
            var cmd = AdHocCommand.Find(singleQuery).Single();
            foreach (var je in AdHocCommandJobEvent.Find(cmd.Id, orderByCounter))
            {
                Assert.IsInstanceOfType<IJobEventBase>(je);
                Assert.IsInstanceOfType<AdHocCommandJobEvent>(je);
                Console.WriteLine($"[{je.Id}][{je.Counter}] {je.EventDisplay}");
                if (!string.IsNullOrEmpty(je.Stdout))
                {
                    Console.WriteLine(je.Stdout);
                }
            }
        }
    }

    [TestClass]
    public class TestJobHostSummary
    {
        private static void DumpResource(JobHostSummary res)
        {
            Console.WriteLine($"{res.Id} {res.Type} [{res.Host}]{res.HostName} [Job:{res.Job}]");
            Console.WriteLine($"  OK        : {res.OK}");
            Console.WriteLine($"  Changed   : {res.Changed}");
            Console.WriteLine($"  Failed    : {res.Failures}");
            Console.WriteLine($"  Skiped    : {res.Skipped}");
            Console.WriteLine($"  Ignored   : {res.Ignored}");
            Console.WriteLine($"  Proecessed: {res.Processed}");
            Console.WriteLine($"  Dark      : {res.Dark}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await JobHostSummary.GetAsync(1);
            Assert.IsInstanceOfType<JobHostSummary>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02ListFromGroup()
        {
            var group = await Group.GetAsync(1);
            Console.WriteLine($"JobHostSummaries in ({group.Type})[{group.Id}] {group.Name}");
            await foreach (var summary in JobHostSummary.FindAsync(group))
            {
                Assert.IsInstanceOfType<JobHostSummary>(summary);
                Console.WriteLine($"{summary.Job} [{summary.Id}][{summary.Host}] {summary.HostName}");
                Console.WriteLine($"  OK={summary.OK} Changed={summary.Changed} Failures={summary.Failures}");
                Console.WriteLine($"  Rescued{summary.Rescued} Skipped={summary.Skipped}");
            }
        }
        [TestMethod]
        public async Task Get03ListFromHost()
        {
            var host = await Host.GetAsync(2);
            Console.WriteLine($"JobHostSummaries in ({host.Type})[{host.Id}] {host.Name}");
            await foreach (var summary in JobHostSummary.FindAsync(host))
            {
                Assert.IsInstanceOfType<JobHostSummary>(summary);
                Console.WriteLine($"{summary.Job} [{summary.Id}][{summary.Host}] {summary.HostName}");
                Console.WriteLine($"  OK={summary.OK} Changed={summary.Changed} Failures={summary.Failures}");
                Console.WriteLine($"  Rescued{summary.Rescued} Skipped={summary.Skipped}");
            }
        }
        [TestMethod]
        public async Task Get04ListFromJob()
        {
            var job = await JobTemplateJob.GetAsync(4);
            Console.WriteLine($"JobHostSummaries in ({job.Type})[{job.Id}] {job.Name}");
            await foreach (var summary in JobHostSummary.FindAsync(job))
            {
                Assert.IsInstanceOfType<JobHostSummary>(summary);
                Console.WriteLine($"{summary.Job} [{summary.Id}][{summary.Host}] {summary.HostName}");
                Console.WriteLine($"  OK={summary.OK} Changed={summary.Changed} Failures={summary.Failures}");
                Console.WriteLine($"  Rescued{summary.Rescued} Skipped={summary.Skipped}");
            }
        }
    }

    [TestClass]
    public class TestAdHocCommand
    {
        private readonly HttpQuery singleQuery = new("order_by=id&page_size=1");

        private static void DumpResource(AdHocCommandBase res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name}");
            Console.WriteLine($"  {res.JobType} {res.Created} {res.Modified}");
            Console.WriteLine($"  {res.ModuleName} {res.ModuleArgs}");
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod("[AdHocCommand] 01 Simple Find and Get")]
        public void Get01FindAndGet()
        {
            var adhocCommands = AdHocCommand.Find(singleQuery);
            Assert.AreEqual(1, adhocCommands.Length);
            Assert.IsInstanceOfType<AdHocCommand>(adhocCommands[0]);
            var detail = AdHocCommand.Get(adhocCommands[0].Id);
            Assert.IsInstanceOfType<AdHocCommand.Detail>(detail);
            DumpResource(detail);
        }
        [TestMethod("[AdHocCommand] 02 List from Inventoroy")]
        public void Get02ListFromInventory()
        {
            var inventory = Inventory.Find(singleQuery).Single();
            foreach (var cmd in AdHocCommand.Find(inventory))
            {
                Assert.IsInstanceOfType<AdHocCommand>(cmd);
                Console.WriteLine($"[{cmd.Id}] {cmd.Name}[{cmd.Status}] {cmd.Finished}");
            }
        }
        [TestMethod("[AdHocCommand] 03 List from Group")]
        public void Get03ListFromFroup()
        {
            var group = Group.Find(singleQuery).Single();
            foreach (var cmd in AdHocCommand.Find(group))
            {
                Assert.IsInstanceOfType<AdHocCommand>(cmd);
                Console.WriteLine($"[{cmd.Id}] {cmd.Name}[{cmd.Status}] {cmd.Finished}");
            }
        }
        [TestMethod("[AdHocCommand] 04 List from Host")]
        public void Get04ListFromHost()
        {
            var host = Host.Find(singleQuery).Single();
            foreach (var cmd in AdHocCommand.Find(host))
            {
                Assert.IsInstanceOfType<AdHocCommand>(cmd);
                Console.WriteLine($"[{cmd.Id}] {cmd.Name}[{cmd.Status}] {cmd.Finished}");
            }
        }
    }

    [TestClass]
    public class TestSystemJobTemplate
    {
        private static void DumpResource(SystemJobTemplate res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"  {res.JobType} {res.Created} {res.Modified}");
            Console.WriteLine($"  {res.LastJobRun} {res.NextJobRun}");
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public void Get01Single()
        {
            var res = SystemJobTemplate.Get(1);
            Assert.IsInstanceOfType<SystemJobTemplate>(res);
            DumpResource(res);
        }
        [TestMethod]
        public void Get02List()
        {
            foreach (var res in SystemJobTemplate.Find())
            {
                DumpResource(res);
            }

        }
    }
    [TestClass]
    public class TestSystemJob
    {
        private static void DumpResource(SystemJobBase res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"UnifiedJT     : {res.UnifiedJobTemplate}");
            Console.WriteLine($"LaunchType    : {res.LaunchType}");
            Console.WriteLine($"Status        : {res.Status}");
            Console.WriteLine($"EV            : {res.ExecutionEnvironment}");
            Console.WriteLine($"Failed        : {res.Failed}");
            Console.WriteLine($"Started       : {res.Started}");
            Console.WriteLine($"Fnished       : {res.Finished}");
            Console.WriteLine($"CancledOn     : {res.CanceledOn}");
            Console.WriteLine($"Elapsed       : {res.Elapsed}");
            Console.WriteLine($"JobExplain    : {res.JobExplanation}");
            Console.WriteLine($"ExecutionNode : {res.ExecutionNode}");
            Console.WriteLine($"LaunchedBy    : {res.LaunchedBy}");
            Console.WriteLine($"SystemJT      : {res.SystemJobTemplate}");
            Console.WriteLine($"JobType       : {res.JobType}");
            Console.WriteLine($"ExtraVars     : {res.ExtraVars}");
            Console.WriteLine($"ResultStdout  : {res.ResultStdout}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var job = SystemJob.Find(new QueryBuilder().SetOrderBy("id").SetPageSize(1).Build()).FirstOrDefault();
            Assert.IsNotNull(job);
            var res = SystemJob.Get(job.Id);
            Assert.IsInstanceOfType<SystemJob.Detail>(res);
            Assert.IsInstanceOfType<IUnifiedJob>(res);
            DumpResource(res);
            Console.WriteLine($"JobArgs   : {res.JobArgs}");
            Console.WriteLine($"JobCwd    : {res.JobCwd}");
            Console.WriteLine($"JobEnv    : ({res.JobEnv.Count})");
            foreach (var (k, v) in res.JobEnv)
            {
                Console.WriteLine($"  {k}: {v}");
            }
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("order_by=id");
            foreach (var res in SystemJob.Find(query))
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public void Get03ListSystemJobTemplate()
        {
            var resource = new Resource(ResourceType.SystemJobTemplate, 1);
            var jobs = SystemJob.Find(resource.Id);
            Assert.IsTrue(jobs.Length > 0);
            Assert.IsInstanceOfType<SystemJob[]>(jobs);
        }
    }

    [TestClass]
    public class TestSchedule
    {
        private static void DumpResource(Schedule res)
        {
            Console.WriteLine($"{res.Id} [{res.Type}] {res.Name} - {res.Description}");
            Console.WriteLine($"RRule   : {res.Rrule}");
            Console.WriteLine($"Job     : {res.UnifiedJobTemplate}");
            Console.WriteLine($"Start   : {res.DtStart}");
            Console.WriteLine($"NextRun : {res.NextRun}");
            Console.WriteLine($"End     : {res.DtEnd}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await Schedule.GetAsync(1);
            Assert.IsInstanceOfType<Schedule>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("order_by=id");
            await foreach (var res in Schedule.FindAsync(query))
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public async Task Get03ListSystemJobTemplate()
        {
            var resource = new Resource(ResourceType.SystemJobTemplate, 1);
            await foreach (var res in Schedule.FindAsync(resource))
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }
    [TestClass]
    public class TestRole
    {
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await Role.GetAsync(1);
            Assert.IsInstanceOfType<Role>(res);
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            var summary = res.SummaryFields;
            Console.WriteLine($"  Resource: {summary.ResourceId} {summary.ResourceType} {summary.ResourceName}");
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("order_by=id");
            await foreach (var res in Role.FindAsync(query))
            {
                Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
                var summary = res.SummaryFields;
                Console.WriteLine($"  Resource: {summary.ResourceId} {summary.ResourceType} {summary.ResourceName}");
            }
        }
        [TestMethod]
        public async Task Get03ListUser()
        {
            var resource = new Resource(ResourceType.User, 1);
            await foreach (var res in Role.FindAsync(resource))
            {
                Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
                var summary = res.SummaryFields;
                Console.WriteLine($"  Resource: {summary.ResourceId} {summary.ResourceType} {summary.ResourceName}");
            }
        }
        [TestMethod]
        public async Task Get03ListOrganizationObjectRoles()
        {
            var resource = new Resource(ResourceType.Organization, 1);
            await foreach (var res in Role.FindObjectRolesAsync(resource))
            {
                Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
                var summary = res.SummaryFields;
                Console.WriteLine($"  Resource: {summary.ResourceId} {summary.ResourceType} {summary.ResourceName}");
            }
        }
    }

    [TestClass]
    public class TestNotificationTemplate
    {
        private static void DumpResource(NotificationTemplate res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"Origanization    : {res.Organization}");
            Console.WriteLine($"NotificationType : {res.NotificationType}");
            Console.WriteLine($"NotificationConfig:");
            Util.DumpObject(res.NotificationConfiguration);
            if (res.Messages is not null)
                Util.DumpObject(res.Messages);

        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await NotificationTemplate.GetAsync(1);
            Assert.IsInstanceOfType<NotificationTemplate>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("order_by=id");
            await foreach (var res in NotificationTemplate.FindAsync(query))
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }
    [TestClass]
    public class TestNotification
    {
        private static void DumpResource(Notification res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.NotificationType}");
            Console.WriteLine($"{res.Created} {res.Modified}");
            Console.WriteLine($"NotificationTemplate : {res.NotificationTemplate}");
            Console.WriteLine($"Error                : {res.Error}");
            Console.WriteLine($"Status               : {res.Status}");
            Console.WriteLine($"NotificationSent     : {res.NotificationsSent}");
            Console.WriteLine($"Recipients           : {res.Recipients}");
            Console.WriteLine($"Subject              : {res.Subject}");
            Console.WriteLine($"Body                 : {res.Body ?? "(null)"}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await Notification.GetAsync(1);
            Assert.IsInstanceOfType<Notification>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("order_by=id");
            await foreach (var res in Notification.FindAsync(query))
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }

    [TestClass]
    public class TestLabel
    {
        private static void DumpResource(Label res)
        {
            Console.WriteLine($"{res.Id} {res.Name} {res.Url}");
            Console.WriteLine($"Organization: {res.Organization}");
            Console.WriteLine($"Created: {res.Created} Modified: {res.Modified}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await Label.GetAsync(1);
            Assert.IsInstanceOfType<Label>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var res in Label.FindAsync())
            {
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }

    [TestClass]
    public class TestUnifiedJobTemplate
    {
        private static void DumpResource(IUnifiedJobTemplate jt)
        {
            Console.WriteLine($"---- Type: {jt.GetType().Name} ----");
            Console.WriteLine($"{jt.Id} [{jt.Type}] {jt.Name}");
            Console.WriteLine($"  Status: {jt.Status}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await UnifiedJobTemplate.GetAsync(1);
            Console.WriteLine($"{res.Id} {res.Type} {res.Name}");
            Assert.IsInstanceOfType<IUnifiedJobTemplate>(res);
            DumpResource(res);
        }
        [TestMethod]
        public async Task Get02List()
        {
            await foreach (var res in UnifiedJobTemplate.GetAsync([1, 6, 9, 11, 13]))
            {
                DumpResource(res);
                switch (res)
                {
                    case JobTemplate jt:
                        Console.WriteLine($"  {jt.Playbook}");
                        break;
                    case SystemJobTemplate systemjt:
                        Console.WriteLine($"  {systemjt.JobType} {systemjt.Name}");
                        break;
                    case Project pj:
                        Console.WriteLine($"  {pj.ScmType} {pj.ScmUrl} {pj.ScmBranch}");
                        break;
                    case InventorySource inv:
                        Console.WriteLine($"  {inv.SourceProject} {inv.SourcePath}");
                        break;
                    case WorkflowJobTemplate wjt:
                        Console.WriteLine($"  {wjt.Url}");
                        break;
                    default:
                        Assert.Fail($"Unkown type: {res.Type}");
                        break;
                }
            }
        }

        [TestMethod]
        public async Task Get03ListJobTemplate()
        {
            var query = new HttpQuery("type=job_template&order_by=-id&page_size=2");
            await foreach (var res in UnifiedJobTemplate.FindAsync(query))
            {
                DumpResource(res);
                Assert.IsInstanceOfType<JobTemplate>(res);
            }
        }
        [TestMethod]
        public async Task Get04ListProject()
        {
            var query = new HttpQuery("type=project&order_by=-id&page_size=2");
            await foreach (var res in UnifiedJobTemplate.FindAsync(query))
            {
                DumpResource(res);
                Assert.IsInstanceOfType<Project>(res);
            }
        }
        [TestMethod]
        public async Task Get05ListInventorySource()
        {
            var query = new HttpQuery("type=inventory_source&order_by=-id&page_size=2");
            await foreach (var res in UnifiedJobTemplate.FindAsync(query))
            {
                DumpResource(res);
                Assert.IsInstanceOfType<InventorySource>(res);
            }
        }
        [TestMethod]
        public async Task Get06ListSystemJobTemplate()
        {
            var query = new HttpQuery("type=system_job_template&order_by=-id&page_size=2");
            await foreach (var res in UnifiedJobTemplate.FindAsync(query))
            {
                DumpResource(res);
                Assert.IsInstanceOfType<SystemJobTemplate>(res);
            }
        }
        [TestMethod]
        public async Task Get07ListWorkflowJobTemplate()
        {
            var query = new HttpQuery("type=workflow_job_template&order_by=-id&page_size=2");
            await foreach (var res in UnifiedJobTemplate.FindAsync(query))
            {
                DumpResource(res);
                Assert.IsInstanceOfType<WorkflowJobTemplate>(res);
            }
        }
    }


    [TestClass]
    public class TestUnifiedJob
    {
        private static void DumpResource(IUnifiedJob job)
        {
            Console.WriteLine($"---- Type: {job.GetType().Name} ----");
            Console.WriteLine($"{job.Id} [{job.Type}] {job.Name}");
            Console.WriteLine($"  Start: {job.Started} - {job.Finished} ({job.Elapsed})");
            Console.WriteLine($"  Status: {job.Status}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var job = await UnifiedJob.GetAsync(20);
            Console.WriteLine($"{job.Id} {job.Type} {job.Name}");
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
            }
        }
        [TestMethod]
        public async Task Get03JobTemplateJob()
        {
            var query = new HttpQuery("type=job&page_size=2&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
                Assert.IsInstanceOfType<JobTemplateJob>(job);
            }
        }
        [TestMethod]
        public async Task Get04ProjectUpdateJob()
        {
            var query = new HttpQuery("type=project_update&page_size=2&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
                Assert.IsInstanceOfType<ProjectUpdateJob>(job);
            }
        }
        [TestMethod]
        public async Task Get05InventoryUpdate()
        {
            var query = new HttpQuery("type=inventory_update&page_size=2&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
                Assert.IsInstanceOfType<InventoryUpdateJob>(job);
            }
        }
        [TestMethod]
        public async Task Get06WorkflobJob()
        {
            var query = new HttpQuery("type=workflow_job&page_size=2&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
                Assert.IsInstanceOfType<WorkflowJob>(job);
            }
        }
        [TestMethod]
        public async Task Get07SystemJob()
        {
            var query = new HttpQuery("type=system_job&page_size=2&order_by=-id");
            await foreach (var job in UnifiedJob.FindAsync(query))
            {
                DumpResource(job);
                Assert.IsInstanceOfType<SystemJob>(job);
            }
        }
    }

    [TestClass]
    public class TestWorkflowJobTemplate
    {
        private static void DumpResource(WorkflowJobTemplate res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name}");
            Console.WriteLine($"Description : {res.Description}");
            Console.WriteLine($"Status      : {res.Status}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var res = WorkflowJobTemplate.Get(13);
            Assert.IsInstanceOfType<WorkflowJobTemplate>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=-id");
            foreach (var res in WorkflowJobTemplate.Find(query))
            {
                Assert.IsInstanceOfType<WorkflowJobTemplate>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public void Get03ListFromOrganization()
        {
            var org = new Resource(ResourceType.Organization, 2);
            foreach (var wjt in WorkflowJobTemplate.Find(org.Id))
            {
                Assert.IsInstanceOfType<WorkflowJobTemplate>(wjt);
                Console.WriteLine($"[{wjt.Id}] {wjt.Name} [{wjt.Status}]");
            }
        }
    }

    [TestClass]
    public class TestWorkflowJob
    {
        private static void DumpResource(WorkflowJob res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name}");
            Console.WriteLine($"Description : {res.Description}");
            Console.WriteLine($"Status      : {res.Status}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var job = WorkflowJob.Find(new HttpQuery("order_by=-id&page_size=1")).Single();
            var detail = WorkflowJob.Get(job.Id);
            Assert.IsInstanceOfType<WorkflowJob.Detail>(detail);
            Util.DumpSummary(detail.SummaryFields);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=-id");
            foreach (var res in WorkflowJob.Find(query))
            {
                Assert.IsInstanceOfType<WorkflowJob>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public void Get03ListFromWorkflowJobTemplate()
        {
            var wjt = new Resource(ResourceType.WorkflowJobTemplate, 13);
            foreach (var job in WorkflowJob.Find(wjt.Id))
            {
                Assert.IsInstanceOfType<WorkflowJob>(job);
                Console.WriteLine($"[{job.Id}] {job.Name} [{job.Status}] [{job.Finished}]");
            }
        }
    }

    [TestClass]
    public class TestWorkflowJobTemplateNode
    {
        private static void DumpResource(WorkflowJobTemplateNode res)
        {
            Console.WriteLine($"{res.Id} {res.Type}");
            Console.WriteLine($"WorkflowJobTemplate : {res.WorkflowJobTemplate}");
            Console.WriteLine($"UnifiedJobTemplate  : {res.UnifiedJobTemplate}");
            Console.WriteLine($"SuccessNodes        : {string.Join(", ", res.SuccessNodes)}");
            Console.WriteLine($"FailureNodes        : {string.Join(", ", res.FailureNodes)}");
            Console.WriteLine($"AlwaysNodes         : {string.Join(", ", res.AlwaysNodes)}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var res = WorkflowJobTemplateNode.Get(1);
            Assert.IsInstanceOfType<WorkflowJobTemplateNode>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=-id");
            foreach (var res in WorkflowJobTemplateNode.Find(query))
            {
                Assert.IsInstanceOfType<WorkflowJobTemplateNode>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public void Get03ListFromWorkflowJobTemplate()
        {
            var wjt = WorkflowJobTemplate.Find(new HttpQuery("order_by=-id&page_size=1")).Single();
            foreach (var node in WorkflowJobTemplateNode.Find(wjt.Id))
            {
                Assert.IsInstanceOfType<WorkflowJobTemplateNode>(node);
            }
        }
    }
    [TestClass]
    public class TestWorkflowJobNode
    {
        private static void DumpResource(WorkflowJobNode res)
        {
            Console.WriteLine($"{res.Id} {res.Type}");
            Console.WriteLine($"Job                 : {res.Job}");
            Console.WriteLine($"UnifiedJobTemplate  : {res.UnifiedJobTemplate}");
            Console.WriteLine($"SuccessNodes        : {string.Join(", ", res.SuccessNodes)}");
            Console.WriteLine($"FailureNodes        : {string.Join(", ", res.FailureNodes)}");
            Console.WriteLine($"AlwaysNodes         : {string.Join(", ", res.AlwaysNodes)}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var res = WorkflowJobNode.Get(131);
            Assert.IsInstanceOfType<WorkflowJobNode>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public void Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=-id");
            foreach (var res in WorkflowJobNode.Find(query))
            {
                Assert.IsInstanceOfType<WorkflowJobNode>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public void Get03ListFromWorkflowJob()
        {
            var job = WorkflowJob.Find(new HttpQuery("order_by=-id&page_size=1")).Single();
            foreach (var res in WorkflowJobNode.Find(job.Id))
            {
                Assert.IsInstanceOfType<WorkflowJobNode>(res);
            }
        }
    }

    [TestClass]
    public class TestCredentialInputSource
    {
        private static void DumpResource(CredentialInputSource res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Description}");
            Console.WriteLine($"  InputFieldName  : {res.InputFieldName}");
            Console.WriteLine($"  SourceCredential: {res.SourceCredential}");
            Console.WriteLine($"  TargetCredential: {res.TargetCredential}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await CredentialInputSource.GetAsync(1);
            Assert.IsInstanceOfType<CredentialInputSource>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=id");
            await foreach (var res in CredentialInputSource.FindAsync(query))
            {
                Assert.IsInstanceOfType<CredentialInputSource>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public async Task Get03ListFromCredential()
        {
            var cred = await Credential.GetAsync(7);
            Console.WriteLine($"Credential for ([{cred.Id}][{cred.Type}] {cred.Name})");
            await foreach (var cis in CredentialInputSource.FindAsync(cred.Id))
            {
                Assert.IsInstanceOfType<CredentialInputSource>(cis);
                Console.WriteLine($"[{cis.Id}] Source:{cis.SourceCredential} Target:{cis.TargetCredential}");
            }

        }
    }

    [TestClass]
    public class TestExecutionEnvironment
    {
        private static void DumpResource(ExecutionEnvironment res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"Image   : {res.Image}");
            Console.WriteLine($"Managed : {res.Managed}");
        }
        [TestMethod]
        public async Task Get01Single()
        {
            var res = await ExecutionEnvironment.GetAsync(1);
            Assert.IsInstanceOfType<ExecutionEnvironment>(res);
            DumpResource(res);
            Util.DumpSummary(res.SummaryFields);
        }
        [TestMethod]
        public async Task Get02List()
        {
            var query = new HttpQuery("page_size=10&order_by=id");
            await foreach (var res in ExecutionEnvironment.FindAsync(query))
            {
                Assert.IsInstanceOfType<ExecutionEnvironment>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
        [TestMethod]
        public async Task Get03ListFromOrganization()
        {
            var org = await Organization.GetAsync(2);
            Console.WriteLine($"ActivityStream for ([{org.Id}][{org.Type}] {org.Name})");
            await foreach (var exeEnv in ExecutionEnvironment.FindAsync(org.Id))
            {
                Assert.IsInstanceOfType<ExecutionEnvironment>(exeEnv);
                Console.WriteLine($"[{exeEnv.Id}] {exeEnv.Name} {exeEnv.Image}");
            }
        }
    }

    [TestClass]
    public class TestMetrics
    {
        [TestMethod]
        public async Task GetMetrics()
        {
            var apiResult = await RestAPI.GetAsync<Metrics>(Metrics.PATH);
            foreach (var (key, value) in apiResult.Contents)
            {
                Console.WriteLine($"{key}:");
                Console.WriteLine($"    {value}");
            }
        }
    }

    [TestClass]
    public class TestWorkflowApprovalTemplate
    {
        private static void DumpResource(WorkflowApprovalTemplate res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"Timeout: {res.Timeout}");
        }
        [TestMethod]
        public void Get01Single()
        {
            foreach (var approval in WorkflowApproval.Find(new HttpQuery("order_by=-id&page_size=1")))
            {
                Console.WriteLine($"WorkflowApproval: [{approval.Id}]{approval.Name}");
                Assert.IsNotNull(approval.UnifiedJobTemplate);
                var res = WorkflowApprovalTemplate.Get((ulong)approval.UnifiedJobTemplate);
                Assert.IsInstanceOfType<WorkflowApprovalTemplate>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }

    [TestClass]
    public class TestWorkflowApproval
    {
        private static void DumpResource(WorkflowApprovalBase res)
        {
            Console.WriteLine($"{res.Id} {res.Type} {res.Name} {res.Description}");
            Console.WriteLine($"  {res.Status} {res.Finished}");
        }
        [TestMethod]
        public void Get01Single()
        {
            var query = new HttpQuery("order_by=-id&page_size=1");
            foreach (var res in WorkflowApproval.Find(query))
            {
                var detail = WorkflowApproval.Get(res.Id);
                Assert.IsInstanceOfType<WorkflowApproval.Detail>(detail);
                DumpResource(detail);
                Util.DumpSummary(detail.SummaryFields);
            }
        }
        [TestMethod]
        public void Get02FindFromWorkflowApprovalTemplate()
        {
            var approval = WorkflowApproval.Find(new HttpQuery("order_by=-id&page_size=1")).Single();
            Assert.IsNotNull(approval.UnifiedJobTemplate);
            var query = new HttpQuery("order_by=-id&page_size=2");
            foreach (var res in WorkflowApproval.Find((ulong)approval.UnifiedJobTemplate, query))
            {
                Assert.IsInstanceOfType<WorkflowApproval>(res);
                DumpResource(res);
                Util.DumpSummary(res.SummaryFields);
            }
        }
    }

    [TestClass]
    public class TestConfig
    {
        [TestMethod]
        public async Task ConfigGet()
        {
            var apiResult = await RestAPI.GetAsync<Config>("/api/v2/config/");
            Assert.IsNotNull(apiResult);
            var config = apiResult.Contents;
            Assert.IsNotNull(config);
            Util.DumpObject(config);
            Util.DumpResponse(apiResult.Response);
            Assert.IsInstanceOfType<Config>(config);
            Assert.AreEqual("UTC", config.TimeZone);
            Assert.AreEqual("open", config.LicenseInfo.LicenseType);

            Assert.AreEqual("config", config.AnalyticsCollectors["config"].Name);
        }
    }

    [TestClass]
    public class TestSettings
    {
        [TestMethod]
        public void ListSlugs()
        {
            var slugs = Setting.ListSlugs();
            Assert.IsInstanceOfType<Setting[]>(slugs);
            Assert.IsTrue(slugs.Length > 0);
            Util.DumpObject(slugs);
        }
        [TestMethod]
        public void GetGithub()
        {
            var setting = Setting.Get("github");
            Assert.IsInstanceOfType<Dictionary<string, object?>>(setting);
            Assert.IsTrue(setting.Count > 0);
            Util.DumpObject(setting);
        }
    }

}
