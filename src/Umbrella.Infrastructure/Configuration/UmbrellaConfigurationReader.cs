using Microsoft.Extensions.Configuration;

namespace Umbrella.Infrastructure.Configuration
{
    /// <summary>
    /// Implementation of config reader
    /// </summary>
    public class UmbrellaConfigurationReader : IUmbrellaConfigurationReader
    {
        #region Fields

        private readonly Dictionary<Type, string> _SettingsName;

        #endregion

        #region Properties

        public IConfiguration Configuration { get; private set; }

        public EnvironmentSettings Environment { get { return this.GetSettings<EnvironmentSettings>(); } }

        #endregion

        /// <summary>
        /// Default Cosntr
        /// </summary>
        /// <param name="config"></param>
        public UmbrellaConfigurationReader(IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            this.Configuration = config;
            this._SettingsName = new Dictionary<Type, string>();
            this._SettingsName.Add(typeof(EnvironmentSettings), "Environment");
        }
        /// <summary>
        /// REgisters a new section with its Data type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterSection<T>(string name) where T : class
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (this._SettingsName.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"Section {typeof(T)} already exists");

            this._SettingsName.Add(typeof(T), name);
        }
        /// <summary>
        /// TRUE if this component can read this kinf og settings, FALSE otherwhise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsTypeKnown<T>()
        {
            return this._SettingsName.ContainsKey(typeof(T));
        }
        /// <summary>
        /// REads the settings
        /// </summary>
        /// <typeparam name="T">type of settings to read</typeparam>
        /// <returns>the populated settings, taken from appsettings.json file</returns>
        public T GetSettings<T>()
        {
            if (!this.IsTypeKnown<T>())
                throw new ApplicationException($"Settings not found for type {typeof(T).FullName}");
            var settingsName = this._SettingsName[typeof(T)];

            var settings = Activator.CreateInstance<T>();
            this.Configuration.GetSection(settingsName).Bind(settings);
            return settings;
        }
    }
}