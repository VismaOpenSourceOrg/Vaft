namespace Vaft.Framework.Core
{
    /// <summary>
    /// The <see cref="IProfileInitializer"/> interface defines a service for initializing driver profiles for a driver
    /// </summary>
    public interface IProfileInitializer
    {
        /// <summary>
        /// Initializes the specified profile.
        /// </summary>
        /// <param name="profile">The profile to initialize.</param>
        /// <remarks>The type of the profile depends on the driver. Implementors should take into account.</remarks>
        /// <example>
        /// <para>The following example shows a custom profile initializer implementation for the FireFox browser.</para>
        /// <code>
        /// public class MyProfileInitializer : IProfileInitializer
        /// {
        ///     public void InitializeProfile(object profile)
        ///     {
        ///         if (profile == null)
        ///         {
        ///             throw new ArgumentNullException("profile");
        ///         }
        ///         
        ///         var ffp = profile as FirefoxProfile;
        ///         if (ffp == null)
        ///         {
        ///             return;
        ///         }
        ///         
        ///         ffp.SetPreference("network.negotiate-auth.trusted-uris", "localhost:1234");
        ///     }
        /// }
        /// </code>
        /// <para>And add the initializer type to App.config</para>
        /// <code>
        ///  <configuration>
        ///   <appSettings>
        ///    <add key="DriverInitializationType" value="MyNamespace.MyProfileInitializer, MyAssemblyName" />
        ///   </appSettings>
        ///  </configuration>
        /// </code>
        /// </example>
        void InitializeProfile(object profile);
    }
}
