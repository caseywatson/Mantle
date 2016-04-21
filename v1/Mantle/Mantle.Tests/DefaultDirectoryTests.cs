using System;
using Mantle.Interfaces;
using Moq;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class DefaultDirectoryTests
    {
        [Test]
        public void Should_return_named_instance_if_found()
        {
            const string testObjName = "Test";

            var testObj = new object();
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            var directory = new DefaultDirectory<object>(mockDependencyResolver.Object);

            mockDependencyResolver.Setup(dr => dr.Get<object>(testObjName)).Returns(testObj);

            var obj = directory[testObjName];

            Assert.IsNotNull(obj);
            Assert.AreSame(testObj, obj);
        }

        [Test]
        public void Should_return_null_if_named_instance_is_not_found()
        {
            const string testObjName = "Test";

            var mockDependencyResolver = new Mock<IDependencyResolver>();
            var directory = new DefaultDirectory<object>(mockDependencyResolver.Object);

            mockDependencyResolver.Setup(dr => dr.Get<object>(testObjName)).Returns(null);

            var obj = directory[testObjName];

            Assert.IsNull(obj);
        }

        [Test]
        public void Should_throw_ArgumentException_if_provided_name_is_empty()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            var directory = new DefaultDirectory<object>(mockDependencyResolver.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                var obj = directory[string.Empty];
            });
        }

        [Test]
        public void Should_throw_ArgumentException_if_provided_name_is_null()
        {
            var mockDependencyResolver = new Mock<IDependencyResolver>();
            var directory = new DefaultDirectory<object>(mockDependencyResolver.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                var obj = directory[null];
            });
        }
    }
}