using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Umbrella.Logging;

namespace Umbrella.Infrastructure.Tests.Logging
{
    public class SerilogServiceCollectionExtensionsTests
    {
        [Test]
        public void AddUmbrellaLogging_ThrowEx_IfServicesIsNull()
        {
            //****** GIVEN
            IServiceCollection services = null;
            IConfiguration config = null;

            //****** WHEN
            TestDelegate testCode = () => services.AddUmbrellaLogging(config, "");

            //****** ASSERT
            ArgumentNullException ex = (ArgumentNullException)Assert.Throws(typeof(ArgumentNullException), testCode);
            Assert.That(ex.ParamName, Is.EqualTo("services"));
            Assert.Pass();
        }

        [Test]
        public void AddUmbrellaLogging_ThrowEx_IfConfigurationIsNull()
        {
            //****** GIVEN
            IServiceCollection services = new ServiceCollection();
            IConfiguration config = null;

            //****** WHEN
            TestDelegate testCode = () => services.AddUmbrellaLogging(config, "");

            //****** ASSERT
            ArgumentNullException ex = (ArgumentNullException)Assert.Throws(typeof(ArgumentNullException), testCode);
            Assert.That(ex.ParamName, Is.EqualTo("config"));
            Assert.Pass();
        }

        // [Test]
        // public void AddUmbrellaLogging_ThrowEx_IfEnvironmentIsNull()
        // {
        //     //****** GIVEN
        //     IServiceCollection services = new ServiceCollection();
        //     IConfiguration config = ConfigurationManager.InitConfigurationFromFile();

        //     //****** WHEN
        //     TestDelegate testCode = () => services.AddUmbrellaLogging(config, "");

        //     //****** ASSERT
        //     ArgumentNullException ex = (ArgumentNullException)Assert.Throws(typeof(ArgumentNullException), testCode);
        //     Assert.That(ex.ParamName, Is.EqualTo("environment"));
        //     Assert.Pass();
        // }

        // [Test]
        // public void AddUmbrellaLogging_InjectsTheLogger()
        // {
        //     //****** GIVEN
        //     IServiceCollection services = new ServiceCollection();
        //     IConfiguration config = ConfigurationManager.InitConfigurationFromFile();

        //     //****** WHEN
        //     var serilogLogger = services.AddUmbrellaLogging(config, "localhost");

        //     //****** ASSERT
        //     Assert.False(serilogLogger == null);

        //     var provider = services.BuildServiceProvider();
        //     var msftLoggerFactory = provider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
        //     Assert.False(msftLoggerFactory == null);
        //     var msftLogger = provider.GetService<Microsoft.Extensions.Logging.ILogger>();
        //     Assert.False(msftLogger == null);
        //     Assert.Pass();
        // }
    }
}