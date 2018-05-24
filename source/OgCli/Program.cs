using System;
using System.IO;
using System.Linq;
using CommandLine;
using OpsGenieApi.Model;

namespace OpsGenieCli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {

                if (!File.Exists(options.Config))
                {
                    Console.WriteLine("Config file not found.");                   
                    return;
                }

                var opsGenieClient = OpsGenieHelper.CreateOpsGenieClient(OpsGenieHelper.GetOpsGenieConfig(options.Config));
                
                switch (options.Action)
                {
                    case Action.Raise:
                        opsGenieClient.Raise(
                            new Alert
                            {
                                Alias = options.Alias,
                                Message = options.Message,
                                Source = options.Source,
                                Description = options.Description,
                                Recipients = !string.IsNullOrWhiteSpace(options.Recipients) 
                                             ? options.Recipients.Split(new []{','},StringSplitOptions.RemoveEmptyEntries).ToList()
                                             : null
                            }
                            );
                        break;
                    case Action.Acknowledge:
                        opsGenieClient.Acknowledge(null, options.Alias, options.Note);
                        break;
                    case Action.Resolve:
                        opsGenieClient.Close(null, options.Alias, options.Note);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }          
        }

        internal class Options
        {
            [Option('c', "config", DefaultValue = "OpsGenie.config")]
            public string Config { get; set; }

            [Option('s', "source", DefaultValue = "Sourcet")]
            public string Source { get; set; }

            [Option('m', "message", DefaultValue = "")]
            public string Message { get; set; }

            [Option('a', "action", DefaultValue = Action.Raise)]
            public Action Action { get; set; }

            [Option('i', "alias", DefaultValue = null)]
            public string Alias { get; set; }

            [Option('d', "Description", DefaultValue = null)]
            public string Description { get; set; }

            [Option('n', "Note", DefaultValue = null)]
            public string Note { get; set; }
           
            [Option('r', "Recipients", DefaultValue = null)]
            public string Recipients { get; set; }

        }

        internal enum Action
        {
            Raise,
            Acknowledge,
            Resolve
        }


    }
}