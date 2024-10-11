using System.Collections.Specialized;

namespace Cmdlet_Test
{
    /// <summary>
    /// Test for
    /// <list type="bullet">
    ///     <item><see cref="GetPingCommand"/></item>
    /// </list>
    /// </summary>
    [TestClass]
    public class PingCmdlet
    {
        [TestMethod]
        public void Get_Ping()
        {
            var cmdlet = new GetPingCommand();
            var ping = cmdlet.Invoke<Ping>().Single();
            Assert.IsNotNull(ping);
            Console.WriteLine(ping.Version);
            Console.WriteLine("=== Instances ===");
            foreach (var instance in ping.Instances)
            {
                Console.WriteLine($"{instance.Node}: {instance.NodeType} {instance.Version} {instance.Uuid}");
            }
            Console.WriteLine("=== InstanceGroups ===");
            foreach (var group in ping.InstanceGroups)
            {
                Console.WriteLine($"{group.Name}: ({group.Capacity}) {string.Join(", ", group.Instances)}");
            }
        }
    }
    /// <summary>
    /// Test for
    /// <list type="bullet">
    ///     <item><see cref="GetActivityStreamCommand"/></item>
    ///     <item><see cref="FindActivityStreamCommand"/></item>
    /// </list>
    /// </summary>
    [TestClass]
    public class ActivityStreamCmdlet
    {
        [TestMethod]
        public void Get()
        {
            var cmdlet = new GetActivityStreamCommand()
            {
                Id = [1]
            };
            var act = cmdlet.Invoke<ActivityStream>().Single();
            Console.WriteLine($"{act.Id} {act.Timestamp}@{act.ActionNode} {act.Operation}");
            Console.WriteLine($"[{act.ObjectType}] 1:{act.Object1} 2:{act.Object2} Associate:{act.ObjectAssociation}");
            foreach (var kv in act.Changes)
            {
                Console.WriteLine($"{kv.Key} {kv.Value}");
            }
        }
        [TestMethod]
        public void Find()
        {
            ushort reqCount = 10;
            var cmdlet = new FindActivityStreamCommand()
            {
                Count = reqCount
            };
            var c = 0;
            foreach (var act in cmdlet.Invoke<ActivityStream>())
            {
                c++;
                Console.WriteLine("===================");
                Console.WriteLine($"{act.Id} {act.Timestamp}@{act.ActionNode} {act.Operation}");
                Console.WriteLine($"[{act.ObjectType}] 1:{act.Object1} 2:{act.Object2} Associate:{act.ObjectAssociation}");
                foreach (var kv in act.Changes)
                {
                    Console.WriteLine($"  {kv.Key} {kv.Value}");
                }
            }
            Assert.AreEqual(reqCount, c, $"ActivityStream count is not {reqCount}");
        }
    }


    [TestClass]
    public class SettingCmdlet
    {
        [TestMethod]
        public void GetList()
        {
            var cmdlet = new GetSettingCommand();
            foreach (var setting in cmdlet.Invoke<Setting>())
            {
                Assert.IsNotNull(setting);
                Console.WriteLine($"{setting.Name} {setting.Slug} {setting.Url}");
            }
        }
        [TestMethod]
        public void Get()
        {
            var cmdlet = new GetSettingCommand()
            {
                Name = "github"
            };
            var github = cmdlet.Invoke<OrderedDictionary>().Single();
            Assert.IsNotNull(github);
            foreach (string key in github.Keys)
            {
                var val = github[key];
                Console.WriteLine($"{key}: {(val == null ? "null" : "\"" + val + '"')}");
            }
        }
    }
    [TestClass]
    public class ConfigCmdlet
    {
        [TestMethod]
        public void Get()
        {
            var cmdlet = new GetConfigCommand();
            var config = cmdlet.Invoke<Config>().Single();
            Assert.IsNotNull(config);
            Console.WriteLine($"Version: {config.Version}");
            Console.WriteLine($"Eula   : {config.Eula}");
            Console.WriteLine($"ConfigAnalyticsStatus: {config.ConfigAnalyticsStatus}");
            Console.WriteLine("=== BecomeMethods ===");
            foreach (var becomMethods in config.BecomeMethods)
            {
                Console.WriteLine($"  [{string.Join(", ", becomMethods)}]");
            }
            Console.WriteLine($"UiNext : {config.UiNext}");
            Console.WriteLine($"ProjectBaseDir : {config.ProjectBaseDir}");
            Console.WriteLine($"ProjectLocalPaths: [{string.Join(", ", config.ProjectLocalPaths)}]");
            Console.WriteLine("=== CustomVirtualEnvs ===");
            foreach (var envs in config.CustomVirtualenvs)
            {
                Console.WriteLine($"  Envs: {envs}");
            }
        }
    }
}
