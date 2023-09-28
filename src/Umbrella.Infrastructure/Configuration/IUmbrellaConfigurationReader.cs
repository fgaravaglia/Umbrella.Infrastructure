using Microsoft.Extensions.Configuration;

namespace Umbrella.Infrastructure.Configuration
{
    public interface IUmbrellaConfigurationReader
    {
        /// <summary>
        /// COnfiguration Reader from MSFT
        /// </summary>
        /// <value></value>
        IConfiguration Configuration{get;}
        /// <summary>
        /// read environment name
        /// </summary>
        /// <value></value>
        EnvironmentSettings Environment{get;}
        /// <summary>
        /// code of current application
        /// </summary> 
        string ApplicationCode {get;}
        /// <summary>
        /// REgisters a new section with its Data type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RegisterSection<T>(string name) where T : class;
        /// <summary>
        /// TRUE if this component can read this kinf og settings, FALSE otherwhise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool IsTypeKnown<T>();
        /// <summary>
        /// REads the settings
        /// </summary>
        /// <typeparam name="T">type of settings to read</typeparam>
        /// <returns>the populated settings, taken from appsettings.json file</returns>
        T GetSettings<T>();
    }
}