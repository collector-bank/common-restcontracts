// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseComponentTest.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts.ComponentTest
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using NUnit.Framework;

    /// <summary>
    /// A testing base class using a 'Setup - Act - Assert' pattern.
    /// Every method in the subclass marked with the [Test] attribute should only contain a single assert.
    /// </summary>
    public abstract class BaseComponentTest
    {
        protected abstract void Setup();

        /// <summary>
        /// The act method, where you should perform the actual action.
        /// </summary>
        protected abstract void Act();

        /// <summary>
        /// Prepare for assertions by reading from the database or disk.
        /// </summary>
        protected virtual void AssertionPreparation()
        {
        }

        /// <summary>
        /// A teardown method that will be executed at the end of every test.
        /// </summary>
        protected virtual void TearDown()
        {
        }

        protected virtual void SetupAdditionalDefaultData()
        {
        }

        [TestFixtureSetUp]
        protected void TestInitializer()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");

                Trace.WriteLine("Setting up additional default data...");
                SetupAdditionalDefaultData();

                Trace.WriteLine("Running setup...");
                Setup();

                Trace.WriteLine("Running act...");
                Act();

                Trace.WriteLine("Running assertion preparation...");
                AssertionPreparation();
            }
            catch (Exception)
            {
                TestFinalizer();
                throw;
            }
        }

        [TestFixtureTearDown]
        protected void TestFinalizer()
        {
            TearDown();
        }
    }
}