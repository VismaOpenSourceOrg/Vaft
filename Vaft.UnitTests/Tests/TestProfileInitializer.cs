using System;
using Vaft.Framework.Core;

namespace Vaft.UnitTests.Tests
{
    /// <summary>
    /// The <see cref="TestProfileInitializer"/> class implements a profile initializer for testing
    /// </summary>
    public class TestProfileInitializer : IProfileInitializer
    {
        /// <summary>
        /// Initializes the specified profile.
        /// </summary>
        /// <param name="profile">The profile to initialize.</param>
        /// <remarks>
        /// The type of the profile depends on the driver. Implementors should take into account.
        /// </remarks>
        public void InitializeProfile(object profile)
        {
            // throwing an exception in order to avoid creating the driver
            var ex = new ApplicationException("Profile Initialized");
            ex.Data.Add("profile", profile.ToString());

            throw ex;
        }
    }
}
