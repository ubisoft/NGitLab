using System;
using NUnit.Framework;

namespace NGitLab.Tests {
    public class Sha1Tests {
        [Test]
        public void WhenSha1WithLowerCase_ThenParsedCorrectly() {
            const string value = "2695effb5807a22ff3d138d593fd856244e155e7";
            Assert.AreEqual(value.ToUpper(), new Sha1(value).ToString().ToUpper());
        }

        [Test]
        public void WhenSha1WithLeadingZero_ThenParsedCorrectly() {
            const string value = "59529D73E3E6E2B7015F05D197E12C43B13BA033";
            Assert.AreEqual(value.ToUpper(), new Sha1(value).ToString().ToUpper());
        }

        [Test]
        public void WhenSha1WithUpperCase_ThenParsedCorrectly() {
            const string value = "2695EFFB5807A22FF3D138D593FD856244E155E7";
            Assert.AreEqual(value, new Sha1(value).ToString().ToUpper());
        }

        [Test]
        public void WhenNotEnoughtChars_ThenErrorThrown() {
            const string value = "2695EFFB5807A22FF3D138D593FD856244";
            Assert.Throws<ArgumentException>(() => { new Sha1(value); });
        }

        [Test]
        public void WhenToManyChars_ThenErrorThrown() {
            const string value = "2695EFFB5807A22FF3D138D593FD856244234234234324";
            Assert.Throws<ArgumentException>(() => { new Sha1(value); });
        }
    }
}